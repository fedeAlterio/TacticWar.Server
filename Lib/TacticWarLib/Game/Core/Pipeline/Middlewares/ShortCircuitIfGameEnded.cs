using TacticWar.Lib.Game.Core.Abstractions;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
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
