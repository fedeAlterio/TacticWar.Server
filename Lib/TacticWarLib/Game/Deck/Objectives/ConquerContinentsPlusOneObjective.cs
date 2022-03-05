using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Extensions;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives
{
    public class ConquerContinentsPlusOneObjective : IObjective
    {
        // Private fields
        private readonly GameMap _gameMap;
        private readonly IReadOnlyList<Continent> _continents;



        // Initialization
        public ConquerContinentsPlusOneObjective(IEnumerable<Continent> continents, GameMap gameMap)
        {
            _continents = continents.ToList();
            _gameMap = gameMap;
            Description = $"You have to conquer {continents.Select(x => x.Name).Aggregate((x, cur) => $"{cur}, {x}")} plus another continent of your choice";
        }



        // Properties
        public string Description { get; }



        // Core
        public bool IsCompleted(IPlayer player)
        {
            if (player.Territories.Count == _gameMap.Territories.Count)
                return true;

            if (! player.HasConqueredContinents(_gameMap, _continents))
                return false;

            foreach (var continent in Continent.Continents)
                if (!_continents.Contains(continent))
                    if (player.HasConqueredContinent(_gameMap, continent))
                        return true;
            return false;
        }
    }
}
