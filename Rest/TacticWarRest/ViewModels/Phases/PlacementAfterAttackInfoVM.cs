using System;
using System.Collections.Generic;
using System.Text;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Rest.ViewModels.Phases
{
    public class PlacementAfterAttackInfoVM
    {
        public PlacementAfterAttackInfoVM(AttackInfo info)
        {
            Attacker = info.Attacker.Color;
            Defender = info.Defender.Color;
            AttackFromId = info.AttackFrom.Territory.Id;
            AttackToId = info.AttackTo.Territory.Id;
        }

        public PlayerColor Attacker { get; set; }
        public PlayerColor Defender { get; set; }
        public int AttackFromId { get; set; }
        public int AttackToId { get; set; }
    }
}
