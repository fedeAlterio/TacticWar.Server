using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Builders.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives.Builders
{
    public class ConquerTerritoriesObjectivesBuilder : IObjectivesBuilder
    {
        // COre
        public IEnumerable<IObjective> BuildObjectvies()
        {
            yield return new ConquerTerritoriesObjective(24);
        }
    }
}
