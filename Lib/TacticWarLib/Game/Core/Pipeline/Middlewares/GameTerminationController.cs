using System.Reactive.Linq;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
{
    public class GameTerminationController : SingleTaskMiddleware, IGameTerminationController
    {
        // Events
        public event Action<VictoryPhaseInfo>? Victory;
        public event Action? GameEnded;



        // Private fields-
        IGameTable _gameTable;
        readonly IIdleManager _idleManager;



        // Initialization
        public GameTerminationController(IGameTable gameTable, IIdleManager idleManager)
        {
            _gameTable = gameTable;
            _idleManager = idleManager;
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
        async void InvokeGameTerminated()
        {
            if (IsGameEnded)
                return;

            IsGameEnded = true;
            await Task.Delay(3000);
            GameEnded?.Invoke();
        }

        async void InvokeVictory(IPlayer winner, bool shouldWait)
        {
            IsGameEnded = true;

            if (shouldWait)
                await Task.Delay(3000);

            var victoryInfo = new VictoryPhaseInfo { Winner = winner };
            Victory?.Invoke(victoryInfo);
            InvokeGameTerminated();
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
    }
}
