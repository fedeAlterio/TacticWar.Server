using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.Logging;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
{
    public partial class IdleManager : SingleTaskMiddleware, IIdleManager
    {
        readonly Subject<IPlayer?> _playerDidSomething = new();
        readonly BehaviorSubject<bool> _isGameEnded = new(false);
        readonly Subject<Unit> _gameEnded = new();
        // Private fields
        readonly HashSet<IPlayer> _idlePlayers = new();
        readonly IBotManager _botManager;
        readonly ITurnInfo _turnInfo;
        readonly IGameTable _gameTable;
        readonly GameStartupInformation _startupInformation;
        readonly ILogger<IdleManager> _logger;

        public IObservable<Unit> GameEnded => _gameEnded;



        // Initialization
        public IdleManager(IBotManager botManager,
                           ITurnInfo turnInfo,
                           IGameTable gameTable,
                           GameStartupInformation startupInformation,
                           ILogger<IdleManager> logger)
        {
            IdleTimeoutPeriodMs = 100 * 1000;
            _botManager = botManager;
            _turnInfo = turnInfo;
            _gameTable = gameTable;
            _startupInformation = startupInformation;
            _logger = logger;
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
                    if (player is null || _botManager.IsBotPlaying)
                        return;

                    if (_idlePlayers.Remove(player))
                    {
                        LogPlayerNotMoreIdled(_logger, _startupInformation.RoomId, player.Name);
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
            if (_idlePlayers.Add(player))
            {
                LogPlayerIdled(_logger, _startupInformation.RoomId, player.Name);
            }

            if (IsGameIdle)
            {
                LogGameIdled(_logger, _startupInformation.RoomId);
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


        [LoggerMessage(LogLevel.Information, "Player {playerName} is no more idle: Room Id {roomId}")]
        static partial void LogPlayerNotMoreIdled(ILogger logger, int roomId, string playerName);


        [LoggerMessage(LogLevel.Information, "Player {playerName} is idled. Room Id {roomId}")]
        static partial void LogPlayerIdled(ILogger logger, int roomId, string playerName);


        [LoggerMessage(LogLevel.Information, "Everyone is idled.  Room Id {roomId}")]
        static partial void LogGameIdled(ILogger logger, int roomId);
    }
}
