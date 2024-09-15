namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
{
    public class SingleTaskMiddlewareFromAction : SingleTaskMiddleware
    {
        // Private fields
        readonly Func<Task> _action;



        // Initialization
        public SingleTaskMiddlewareFromAction(Func<Task> action)
        {
            _action = action;
        }

        public SingleTaskMiddlewareFromAction(Action action)
        {
            _action = async () => action();
        }



        // Core
        protected override async Task DoAction()
        {
            var next = Next;
            await _action.Invoke();
            await next!();
        }
    }
}
