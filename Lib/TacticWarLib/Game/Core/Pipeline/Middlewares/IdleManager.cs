using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Table.Abstractions;
using Timer = System.Timers.Timer;


namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
{
    public class IdleManager : SingleTaskMiddleware, IIdleManager
    {
        // Events
        public event Action<IPlayer>? PlayerIdle;
        public event Action? IdleGame;



        // Private fields
        private readonly Dictionary<IPlayer, Timer> _timersByPlayer = new();
        private readonly HashSet<IPlayer> _idlePlayers = new();
        private readonly IBotManager _botManager;
        private readonly ITurnInfo _turnInfo;
        private readonly IGameTable _gameTable;



        // Initialization
        public IdleManager(IBotManager botManager, ITurnInfo turnInfo, IGameTable gameTable)
        {
            IdleTimeoutPeriodMs = 180 * 1000;
            _botManager = botManager;
            _turnInfo = turnInfo;
            _gameTable = gameTable;
        }



        private Timer BuildTimer(IPlayer player)
        {
            var timer = new Timer(IdleTimeoutPeriodMs);
            timer.Elapsed += (_, _) => OnPlayerIdle(player);
            return timer;
        }



        // Properties        
        public IReadOnlySet<IPlayer> IdlePlayers => _idlePlayers;
        public bool IsGameIdle => _idlePlayers.Count == _gameTable.Players.Count;
        public int IdleTimeoutPeriodMs { get; }




        // Game middleware
        public override async Task Start()
        {
            foreach (var player in _gameTable.Players)
                _timersByPlayer.Add(player, BuildTimer(player));
            OnPlayerAction(_turnInfo!.CurrentTurnPlayer!);
            await base.Start();
        }

        public override async Task TerminateGame()
        {
            foreach (var (_, timer) in _timersByPlayer)
                timer.Stop();
            await base.TerminateGame();
        }




        // Events handlers
        private Task OnGameStateChanged()
        {
            var currentPlayer = _turnInfo.CurrentActionPlayer;
            OnPlayerAction(currentPlayer!);
            return Task.CompletedTask;
        }


        private void OnPlayerAction(IPlayer actionPlayer)
        {
            if (!_botManager.IsBotPlaying)
            {
                _idlePlayers.Remove(actionPlayer);
            }

            foreach (var (player, timer) in _timersByPlayer)
                if (_turnInfo.CurrentActionPlayer != player)
                    timer.Stop();
                else
                {
                    timer.Stop();
                    timer.Start();
                }
        }

        private void OnPlayerIdle(IPlayer player)
        {
            if (_turnInfo.CurrentActionPlayer != player)
                return;

            _idlePlayers.Add(player);
            PlayerIdle?.Invoke(player);
            _botManager.AddBotForOneTurn(player.Color);
            if (IsGameIdle)
                IdleGame?.Invoke();
        }

        protected override async Task DoAction()
        {
            var next = Next;
            await OnGameStateChanged();
            await next!();
        }
    }
}
