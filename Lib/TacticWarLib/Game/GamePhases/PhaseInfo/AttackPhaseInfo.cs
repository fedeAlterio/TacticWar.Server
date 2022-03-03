using TacticWar.Lib.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.GamePhases.PhaseInfo
{
    public class AttackPhaseInfo
    {
        // Initialization
        public AttackPhaseInfo(PlayerTerritory? attackTerritory = default, PlayerTerritory? defenceTerritory = default, IEnumerable<int>? attackDice = default, IEnumerable<int>? defenceDice = default)
        {
            AttackTerritory = attackTerritory;
            DefenceTerritory = defenceTerritory;
            AttackDice = attackDice?.ToList();
            DefenceDice = defenceDice?.ToList();
        }

        public static AttackPhaseInfo FromAttackinfo(AttackInfo info) => new
        (
            attackTerritory: info.AttackFrom,
            defenceTerritory: info.AttackTo,
            attackDice: info.Result?.AttackDice?.ToList(),
            defenceDice: info.Result?.DefenceDice?.ToList()
        );



        // Properties
        public PlayerTerritory? AttackTerritory { get; init; }
        public PlayerTerritory? DefenceTerritory { get; init; }
        public IReadOnlyList<int>? AttackDice { get; init; }
        public IReadOnlyList<int>? DefenceDice { get; init; }
    }
}
