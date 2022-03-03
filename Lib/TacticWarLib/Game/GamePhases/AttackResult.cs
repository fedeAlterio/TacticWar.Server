using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.GamePhases
{
    public class AttackResult
    {
        // Initialization
        public AttackResult (IList<int> attack, IList<int> defence)
        {
            defence = defence.OrderByDescending(x => x).ToList();
            attack = attack.OrderByDescending(x => x).ToList();
            var min = Math.Min(attack.Count, defence.Count);
            for (var i = 0; i < min; i++)
                if (defence[i] >= attack[i])
                    ArmiesLostByAttack++;
                else
                    ArmiesLostByDefence++;
            AttackDice = attack.ToList();
            DefenceDice = defence.ToList();
        }


        // Properties

        public IReadOnlyList<int> DefenceDice { get;} 
        public IReadOnlyList<int> AttackDice { get; } 
        public int ArmiesLostByDefence { get; }
        public int ArmiesLostByAttack { get; }
    }
}
