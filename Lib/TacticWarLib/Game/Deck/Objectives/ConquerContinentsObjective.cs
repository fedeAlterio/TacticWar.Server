using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Extensions;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives
{
    public class ConquerContinentsObjective : IObjective
    {
        // Private fields
        readonly IReadOnlyList<Continent> _continents;
        readonly GameMap _gameMap;



        // Initialization
        public ConquerContinentsObjective(IEnumerable<Continent> continents, GameMap gameMap)
        {
            _continents = continents.ToList();
            Description = $"You have to conquer " + _continents.Count switch
            {
                > 2 => $"{continents.Select(x => x.Name).Aggregate((x, cur) => $"{cur}, {x}")}",
                2 => $"{_continents[0].Name} and {_continents[1].Name}",
                1 => _continents[0].Name,
                _ => throw new GameException($"Impossible to have {_continents.Count} continents to conquer")
            };
            _gameMap = gameMap;
        }



        // Properties
        public string Description { get; }



        // Core
        public bool IsCompleted(IPlayer player)
        {
            return player.HasConqueredContinents(_gameMap, _continents);
        }
    }
}
