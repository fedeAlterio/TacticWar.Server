using FluentAssertions;
using System.Linq;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Extensions;

namespace TacticWar.Lib.Tests.Game.Map
{
    public class GameMapTests
    {
        [FactFor(nameof(GameMap))]
        public void Should_BuildAllTerritories()
        {
            var gameMap = new MapBuilder().BuildNew();

            gameMap.NorthAmerica.Count.Should().Be(9);
            gameMap.SouthAmerica.Count.Should().Be(4);
            gameMap.Europe.Count.Should().Be(7);
            gameMap.Africa.Count.Should().Be(6);
            gameMap.Asia.Count.Should().Be(12);
            gameMap.Australia.Count.Should().Be(4);

            var allTerritories = gameMap.Australia
                .Union(gameMap.Africa)
                .Union(gameMap.SouthAmerica)
                .Union(gameMap.NorthAmerica)
                .Union(gameMap.Europe)
                .Union(gameMap.Asia);
            gameMap.Territories.Should().HaveSameReferencesOf(allTerritories);
        }


        [FactFor(nameof(GameMap))]
        public void Should_ProvideCorrectTerritoryById()
        {
            var gameMap = new MapBuilder().BuildNew();
            foreach(var territory in gameMap.Territories)   
                gameMap.TerritoryById(territory.Id).Should().Be(territory);
        }


        [FactFor(nameof(GameMap))]
        public void Should_ProvideCorrectTerritoriesByContinent()
        {
            var gameMap = new MapBuilder().BuildNew();
            foreach(var territory in gameMap.Territories)
                gameMap.GetContinentTeritories(territory.Continent).Should().Contain(territory);
        }


        [FactFor(nameof(GameMap))]
        public void Should_HaveConsistentNeighbors()
        {
            var gameMap = new MapBuilder().BuildNew();
            foreach (var territory in gameMap.Territories)
                foreach (var neighbor in territory.Neighbors)
                    neighbor.Neighbors.Should().Contain(territory);
        }
    }
}
