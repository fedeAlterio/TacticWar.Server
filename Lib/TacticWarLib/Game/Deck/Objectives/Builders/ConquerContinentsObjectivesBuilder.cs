using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck.Objectives.Builders.Abstractions;
using TacticWar.Lib.Game.Map;

namespace TacticWar.Lib.Game.Deck.Objectives.Builders
{
    public class ConquerContinentsObjectivesBuilder : IObjectivesBuilder
    {
        // Private fields
        private readonly GameMap _gameMap;



        // Initialization
        public ConquerContinentsObjectivesBuilder(GameMap gameMap)
        {
            _gameMap = gameMap;
        }



        // Core
        public IEnumerable<IObjective> BuildObjectvies()
        {
            yield return Conquer(Continent.NorthAmerica, Continent.Africa);
            yield return Conquer(Continent.NorthAmerica, Continent.Australia);
            yield return Conquer(Continent.Asia, Continent.SouthAmerica);
            yield return Conquer(Continent.Asia, Continent.Africa);
            yield return ConquerPlusOne(Continent.Europe, Continent.SouthAmerica);
            yield return ConquerPlusOne(Continent.Europe, Continent.Australia);
        }



        // Utils
        private ConquerContinentsObjective Conquer(params Continent[] continents) => new(continents, _gameMap);
        private ConquerContinentsPlusOneObjective ConquerPlusOne(params Continent[] continents) => new(continents, _gameMap);
    }
}
