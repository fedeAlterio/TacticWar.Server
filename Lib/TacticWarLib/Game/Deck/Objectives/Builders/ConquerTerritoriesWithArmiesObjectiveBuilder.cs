using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck.Objectives.Builders.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives.Builders
{
    public class ConquerTerritoriesWithArmiesObjectiveBuilder : IObjectivesBuilder
    {
        // Core
        public IEnumerable<IObjective> BuildObjectvies()
        {
            yield return new ConquerTerritoriesWithArmiesObjective(18, 2);
        }
    }
}
