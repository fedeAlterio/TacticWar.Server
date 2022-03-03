using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Pipeline.Middlewares
{
    public class SingleTaskMiddlewareFromAction : SingleTaskMiddleware
    {
        // Private fields
        private readonly Func<Task> _action;



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
