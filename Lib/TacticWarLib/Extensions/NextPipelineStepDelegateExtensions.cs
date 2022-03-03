using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Pipeline.Abstractions;

namespace TacticWar.Lib.Extensions
{
    public static class NextPipelineStepDelegateExtensions
    {
        //public static IAsyncDisposable ExecuteAtEndOfScope(this NextPipelineStepDelegate @this) => new NextAtEndOfScope(@this);


        // Private class
        private class NextAtEndOfScope : IAsyncDisposable
        {
            private NextPipelineStepDelegate _next;


            public NextAtEndOfScope(NextPipelineStepDelegate next)
            {
                _next = next;
            }

            public async ValueTask DisposeAsync()
            {
                await _next();
            }
        }
    }
}
