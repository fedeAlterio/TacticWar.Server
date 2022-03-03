using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck.Objectives;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;

namespace TacticWar.Test.TacticWar.Lib.Tests.Game.Deck.Objectives
{
    public class ConquerContinentsPlusOneObjectiveTests
    {
        [FactFor(nameof(ConquerContinentsPlusOneObjective.IsCompleted))]
        public void Should_ReturnTrueWhenPlayerHasAllTerritoriesOfTheContinentsPlusOne()
        {
            // Setup
            var gameMap = ObjectsBuilder.NewMap();
            var continents = new[] { Continent.Australia, Continent.Africa };
            var playerMock = new Mock<IPlayer>();
            var objective = new ConquerContinentsPlusOneObjective(continents, gameMap);


            // Assertions
            var wrongTerritories = gameMap.Australia.Union(gameMap.Africa)
                .Select(t => new PlayerTerritory { Armies = 0, Territory = t }).ToList();
            playerMock.Setup(x => x.Territories).Returns(wrongTerritories);
            objective.IsCompleted(playerMock.Object).Should().BeFalse();


            var rightTerritories = gameMap.Australia.Union(gameMap.Africa).Union(gameMap.Asia)
                .Select(t => new PlayerTerritory { Armies = 0, Territory = t }).ToList();
            playerMock.Setup(x => x.Territories).Returns(rightTerritories);
            objective.IsCompleted(playerMock.Object).Should().BeTrue();
        }
    }
}
