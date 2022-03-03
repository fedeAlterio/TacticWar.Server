using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Pipeline.Middlewares
{
    public class GameTerminationController : SingleTaskMiddleware, IGameTerminationController
    {
        // Events
        public event Action<VictoryPhaseInfo>? Victory;
        public event Action? GameEnded;



        // Private fields-
        private IGameTable _gameTable;
        private readonly IIdleManager _idleManager;



        // Initialization
        public GameTerminationController(IGameTable gameTable, IIdleManager idleManager)
        {
            _gameTable = gameTable;
            _idleManager = idleManager;
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
        private Task CheckVictory()
        {
            if (!TryGetWinner(out var winner, out bool shouldWait))
                return Task.CompletedTask;

            InvokeVictory(winner!, shouldWait);
            return Task.CompletedTask;
        }




        // Invoke Events
        private async void InvokeGameTerminated()
        {
            if (IsGameEnded)
                return;

            IsGameEnded = true;
            await Task.Delay(3000);
            GameEnded?.Invoke();
        }

        private async void InvokeVictory(IPlayer winner, bool shouldWait)
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
        private bool TryGetWinner(out IPlayer? winner, out bool shouldWait)
        {
            winner = _idleManager.IsGameIdle 
                ? _gameTable.Players.First()
                : _gameTable.Players.FirstOrDefault(p => p.HasWon);
            shouldWait = !_idleManager.IsGameIdle;
            return winner != null;
        }
    }
}
