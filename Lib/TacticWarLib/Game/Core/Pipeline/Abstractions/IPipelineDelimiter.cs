namespace TacticWar.Lib.Game.Core.Pipeline.Abstractions
{
    public interface IPipelineDelimiter
    {
        // Properties
        public bool IsPipelineRunning { get; }



        // Core
        Task WaitForPipelineFree();
    }
}
