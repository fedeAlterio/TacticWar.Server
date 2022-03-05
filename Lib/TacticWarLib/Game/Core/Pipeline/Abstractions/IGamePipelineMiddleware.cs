using TacticWar.Lib.Game.Abstractions;

namespace TacticWar.Lib.Game.Core.Pipeline.Abstractions
{
    public delegate Task NextPipelineStepDelegate();
    public interface IGamePipelineMiddleware : IGameApi
    {
        NextPipelineStepDelegate? Next { get; set; }
        void Initialize();
    }
}
