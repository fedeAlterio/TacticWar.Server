using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Table.Abstractions;
using Timer = System.Timers.Timer;


namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
{
    public class IdleManager : SingleTaskMiddleware, IIdleManager
    {
        readonly Subject<IPlayer?> _playerDidSomething = new();
        readonly BehaviorSubject<bool> _isGameEnded = new(false);
        readonly Subject<Unit> _gameEnded = new();
        // Private fields
        readonly HashSet<IPlayer> _idlePlayers = new();
        readonly IBotManager _botManager;
        readonly ITurnInfo _turnInfo;
        readonly IGameTable _gameTable;

        public IObservable<Unit> GameEnded => _gameEnded;



        // Initialization
        public IdleManager(IBotManager botManager,ITurnInfo turnInfo, IGameTable gameTable)
        {
            IdleTimeoutPeriodMs = 20 * 1000;
            _botManager = botManager;
            _turnInfo = turnInfo;
            _gameTable = gameTable;
        }


        // Properties        
        public IReadOnlySet<IPlayer> IdlePlayers => _idlePlayers;
        public bool IsGameIdle => _idlePlayers.Count == _gameTable.Players.Count;
        public int IdleTimeoutPeriodMs { get; }


        // Game middleware
        public override async Task Start()
        {
            _playerDidSomething
                .StartWith(_turnInfo.CurrentTurnPlayer)
                .Do(player =>
                {
                    if (!_botManager.IsBotPlaying && player is not null)
                    {
                        _idlePlayers.Remove(player);
                    }
                })
                .Select(player => player switch
                {
                    null => Observable.Empty<IPlayer>(),
                    _ => Observable.Return(player)
                                   .Delay(TimeSpan.FromMilliseconds(IdleTimeoutPeriodMs))
                })
                .Switch()
                .Do(OnPlayerIdle)
                .TakeUntil(_isGameEnded.Where(static isEnded => isEnded))
                .Subscribe();

            await base.Start();
        }

        public override async Task TerminateGame()
        {
            _isGameEnded.OnNext(true);
            await base.TerminateGame();
        }


        void OnPlayerIdle(IPlayer player)
        {
            _idlePlayers.Add(player);
            if (IsGameIdle)
            {
                _gameEnded.OnNext(Unit.Default);
                return;
            }

            if (_turnInfo.CurrentActionPlayer != player)
                return;

            _botManager.AddBotForOneTurn(player.Color);
        }

        protected override async Task DoAction()
        {
            var next = Next;

            var currentPlayer = _turnInfo.CurrentActionPlayer;
            _playerDidSomething.OnNext(currentPlayer);

            await next!();
        }
    }
}
