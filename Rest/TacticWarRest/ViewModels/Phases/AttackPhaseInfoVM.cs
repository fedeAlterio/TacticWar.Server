using TacticWar.Lib.Game.GamePhases;
using System;
using System.Collections.Generic;
using System.Text;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;

namespace TacticWar.Rest.ViewModels.Phases
{
    public class AttackPhaseInfoVm
    {
        public AttackPhaseInfoVm(AttackPhaseInfo info)
        {
            AttackDice = info.AttackDice?.ToList();
            DefenceDice = info.DefenceDice?.ToList();
            AttackTerritroryId = info.AttackTerritory?.Territory.Id;
            DefenceTerritoryId = info.DefenceTerritory?.Territory.Id;
        }

        public int? AttackTerritroryId { get; set; }
        public int? DefenceTerritoryId { get; set; }
        public IList<int>? AttackDice { get; set; }
        public IList<int>? DefenceDice { get; set; }
    }
}
