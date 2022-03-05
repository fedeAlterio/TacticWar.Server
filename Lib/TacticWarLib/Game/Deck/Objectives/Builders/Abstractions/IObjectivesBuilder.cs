using TacticWar.Lib.Game.Deck.Objectives.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives.Builders.Abstractions
{
    public interface IObjectivesBuilder
    {
        public IEnumerable<IObjective> BuildObjectvies();
    }
}
