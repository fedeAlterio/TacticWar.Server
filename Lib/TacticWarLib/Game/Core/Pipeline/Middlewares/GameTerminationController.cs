using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
{
    public partial class GameTerminationController : SingleTaskMiddleware, IGameTerminationController
    {
        // Events
        public event Action<VictoryPhaseInfo>? Victory;
        public event Action? GameEnded;



        // Private fields-
        IGameTable _gameTable;
        readonly IIdleManager _idleManager;
        readonly GameStartupInformation _gameStartupInformation;
        readonly ILogger<GameTerminationController> _logger;

        // Initialization
        public GameTerminationController(IGameTable gameTable, IIdleManager idleManager, 
                                         GameStartupInformation gameStartupInformation,
                                         ILogger<GameTerminationController> logger)
        {
            _gameTable = gameTable;
            _idleManager = idleManager;
            _gameStartupInformation = gameStartupInformation;
            _logger = logger;
            idleManager.GameEnded
                       .Take(1)
                       .Subscribe(_ => InvokeGameTerminated());
        }



        // Properties
        public bool IsGameEnded { get; private set; }



        // Public
        public async override Task TerminateGame()
        {
            var next = Next;
            InvokeGameTerminated();
            await next!();
        }



        // Core
        Task CheckVictory()
        {
            if (!TryGetWinner(out var winner, out bool shouldWait))
                return Task.CompletedTask;

            InvokeVictory(winner!, shouldWait);
            return Task.CompletedTask;
        }




        // Invoke Events
        async Task InvokeGameTerminated()
        {
            if (IsGameEnded)
                return;

            LogTerminatingGame(_logger, _gameStartupInformation.RoomId);
            IsGameEnded = true;
            await Task.Delay(3000);
            GameEnded?.Invoke();
            LogGameTerminated(_logger, _gameStartupInformation.RoomId);
        }

        async Task InvokeVictory(IPlayer winner, bool shouldWait)
        {
            IsGameEnded = true;

            if (shouldWait)
                await Task.Delay(3000);

            var victoryInfo = new VictoryPhaseInfo { Winner = winner };
            Victory?.Invoke(victoryInfo);
            await InvokeGameTerminated();
        }

        protected override async Task DoAction()
        {
            var next = Next;
            await CheckVictory();
            await next!();
        }


        // Utils
        bool TryGetWinner(out IPlayer? winner, out bool shouldWait)
        {
            winner = _idleManager.IsGameIdle
                ? _gameTable.Players.First()
                : _gameTable.Players.FirstOrDefault(p => p.HasWon);
            shouldWait = !_idleManager.IsGameIdle;
            return winner != null;
        }

        [LoggerMessage(LogLevel.Information, "Terminating game.  Room Id {roomId}")]
        static partial void LogTerminatingGame(ILogger logger, int roomId);


        [LoggerMessage(LogLevel.Information, "Game terminated.  Room Id {roomId}")]
        static partial void LogGameTerminated(ILogger logger, int roomId);
    }
}
