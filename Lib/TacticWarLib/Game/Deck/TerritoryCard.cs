using TacticWar.Lib.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Deck
{
    public class TerritoryCard
    {
        public int Id { get; init; }
        public Territory Territory { get; init; }
        public ArmyType ArmyType { get; init; }
    }
}
