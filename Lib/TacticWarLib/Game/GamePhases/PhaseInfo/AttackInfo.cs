using TacticWar.Lib.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.GamePhases.PhaseInfo
{
    public class AttackInfo
    {
        public Player Attacker { get; init; }
        public Player Defender { get; init; }
        public PlayerTerritory AttackFrom { get; init; }
        public PlayerTerritory AttackTo { get; init; }
        public AttackResult Result { get; init; }
    }
}
