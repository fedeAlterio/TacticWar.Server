using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Pipeline.Abstractions;

namespace TacticWar.Lib.Game.Pipeline.Middlewares
{
    public class ShortCircuitIfGameEnded : SingleTaskMiddleware
    {
        // Private fields
        private IGameTerminationController _gameTerminationController;



        // Initialization
        public ShortCircuitIfGameEnded(IGameTerminationController gameTerminationController)
        {
            _gameTerminationController = gameTerminationController;
        }



        // Core
        protected override async Task DoAction()
        {
            var next = Next;
            if (_gameTerminationController.IsGameEnded)
                return;
            await next!();
        }
    }
}
