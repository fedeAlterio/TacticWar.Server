using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Builders.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives.Builders
{
    public class KillColorObjectivesBuilder : IObjectivesBuilder
    {
        // Private fields
        private readonly IGameStatistics _gameStatistics;
        private readonly IGameTable _gameTable;



        // Initialization
        public KillColorObjectivesBuilder(IGameStatistics gameStatistics, IGameTable gameTable)
        {
            _gameStatistics = gameStatistics;
            _gameTable = gameTable;
        }



        // Core
        public IEnumerable<IObjective> BuildObjectvies()
        {
            var killObjectives = Enum
                .GetValues(typeof(PlayerColor))
                .Cast<PlayerColor>()
                .Select(color => KillColor(color));

            return killObjectives;
        }



        // Utils
        private KillColorObjective KillColor(PlayerColor color) => new(color, 24, _gameStatistics, _gameTable);
    }
}
