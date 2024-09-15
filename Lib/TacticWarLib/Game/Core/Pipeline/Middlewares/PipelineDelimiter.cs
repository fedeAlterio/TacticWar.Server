using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Core.Pipeline.Abstractions;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
{
    public class PipelineDelimiter : SingleTaskMiddleware, IPipelineDelimiter
    {
        // Private fields
        TaskCompletionSource<object?> _pipelineFreeTcs = new();
        readonly SemaphoreSlim _ss = new(1);



        // Properties
        public bool IsPipelineRunning { get; set; }
        public DateTime StartTurnDate { get; private set; }



        // Core
        protected override async Task DoAction()
        {
            using var _ = await _ss.WaitAsyncScoped();
            IsPipelineRunning = true;
            var next = Next;
            await next!();
            IsPipelineRunning = false;
            StartTurnDate = DateTime.Now;

            var tcs = _pipelineFreeTcs;
            _pipelineFreeTcs = new();
            tcs.SetResult(null);
        }

        public async Task WaitForPipelineFree()
        {
            if (!IsPipelineRunning)
                return;

            await _pipelineFreeTcs.Task;
        }
    }
}
