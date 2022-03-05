using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Rest.ViewModels.Phases
{
    public class VictoryPhaseVM
    {
        public VictoryPhaseVM(VictoryPhaseInfo info)
        {
            WinnerColor = info.Winner!.Color;
            WinnerObjective = new ObjectiveVM(info.Winner.Objective);
        }


        public PlayerColor WinnerColor { get; set; }
        public ObjectiveVM WinnerObjective { get; set; }
    }
}
