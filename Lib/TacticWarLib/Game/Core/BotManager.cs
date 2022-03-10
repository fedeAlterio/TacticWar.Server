using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Bot;
using TacticWar.Lib.Game.Bot.Abstractions;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Core
{
    public class BotManager : IBotManager
    {
        // Private fields
        private readonly Dictionary<PlayerColor, IBot> _botsByPlayer = new();
        private readonly IBotCreator _botCreator;
        private readonly INewTurnManager _turnManager;
        private readonly PlayersInfoCollection _playersInfoCollection;
        private readonly SemaphoreSlim _ss = new(1);



        // Initialization
        public BotManager(IBotCreator botCreator, INewTurnManager turnManager, PlayersInfoCollection playersInfoCollection, IGameUpdatesListener gameUpdatesListener)
        {
            _botCreator = botCreator;
            _turnManager = turnManager;
            _playersInfoCollection = playersInfoCollection;
            gameUpdatesListener.GameUpdated += OnGameUpdated;
            turnManager.GameStarted += () => AddBots(playersInfoCollection);
            turnManager.TurnEnded += () => OnTurnEnded?.Invoke();
        }

        private void AddBots(PlayersInfoCollection playersInfoCollection)
        {
            foreach (var playerInfo in playersInfoCollection.Info)
                if (playerInfo.isBot)
                    AddBot(playerInfo.Color);
        }



        // Properties
        public IReadOnlyList<PlayerColor> Bots => _botsByPlayer.Keys.ToList();
        public bool IsBotPlaying { get; private set; }



        // Events
        protected void OnGameUpdated(IPlayer player)
        {
            Task.Run(PlayIfShould);
        }

        private Action? OnTurnEnded { get; set; }



        // Public Methods
        public void RemoveBot(PlayerColor color)
        {
            _botsByPlayer.Remove(color);
        }

        public void AddBot(PlayerColor color, Action<IBotSettings>? configuration = null)
        {
            var bot = _botCreator.NewNoActionBot();
            configuration?.Invoke(bot);
            if (!_botsByPlayer.ContainsKey(color))
                _botsByPlayer.Add(color, bot);
            Task.Run(PlayIfShould);
        }


        public void AddBotForOneTurn(PlayerColor color, Action<IBotSettings>? configuration = null)
        {
            void OnTurnEnded()
            {
                _botsByPlayer.Remove(color);
                this.OnTurnEnded -= OnTurnEnded;
            }
            this.OnTurnEnded += OnTurnEnded;
            AddBot(color);
        }

        public async Task SkipTurn()
        {
            var bot = _botCreator.NewNoActionBot();
            await bot.PlayTurn();
        }




        // Utils
        private async Task PlayIfShould()
        {
            using var _ = await _ss.WaitAsyncScoped();

            if (!_turnManager.IsGameStarted)
                return;

            if (IsBotPlaying)
                return;


            var turnInfo = _turnManager.TurnInfo;
            while (ShouldPlayABot(turnInfo.CurrentActionPlayer!.Color, out var bot))
            {
                IsBotPlaying = true;
                await PlayOneStepWith(bot!);
                IsBotPlaying = false;
            }
        }

        private bool ShouldPlayABot(PlayerColor color, out IBot? bot)
        {
            return _botsByPlayer.TryGetValue(color, out bot);
        }


        private async Task PlayOneStepWith(IBot bot)
        {
            try
            {
                await bot.TryPlayOneStep();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
