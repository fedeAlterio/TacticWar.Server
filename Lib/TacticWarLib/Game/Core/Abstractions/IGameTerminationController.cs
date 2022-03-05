using TacticWar.Lib.Game.GamePhases.PhaseInfo;

namespace TacticWar.Lib.Game.Core.Abstractions
{
    public interface IGameTerminationController
    {
        event Action<VictoryPhaseInfo> Victory;
        event Action GameEnded;

        public bool IsGameEnded { get; }
    }
}
