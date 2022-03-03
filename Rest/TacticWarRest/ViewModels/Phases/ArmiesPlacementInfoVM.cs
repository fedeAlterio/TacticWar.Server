using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;

namespace TacticWar.Rest.ViewModels.Phases
{
    public class ArmiesPlacementInfoVM
    {
        public ArmiesPlacementInfoVM(ArmiesPlacementInfo info)
        {
            ArmiesToPlace = info.ArmiesToPlace;
            DroppedCards = info.DroppedCards?
                        .Select(tris =>
                                    tris?.Select(card => new CardSnapshot(card)).ToList())
                        .ToList();
        }

        public int ArmiesToPlace { get; set; }
        public IReadOnlyList<IReadOnlyList<CardSnapshot>> DroppedCards { get; set; }
    }
}
