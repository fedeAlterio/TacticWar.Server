using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck.Objectives;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;

namespace TacticWar.Test.TacticWar.Lib.Tests.Game.Deck.Objectives
{
    public class ConquerTerritoriesWithArmiesObjectiveTests
    {
        [FactFor(nameof(ConquerTerritoriesWithArmiesObjective.IsCompleted))]
        public void Should_ReturnTrueIfEnoughTerritoriesAreOccupiedWithEnoughArmies()
        {
            var territoriesCount = 18;
            var armiesPerTerritory = 2;
            var objective = new ConquerTerritoriesWithArmiesObjective(territoriesCount, armiesPerTerritory);
            var gameMap = ObjectsBuilder.NewMap();
            var playerMock = new Mock<IPlayer>();

            var rightTerritories = gameMap.Territories.Take(territoriesCount);
            var wrongTerritories = gameMap.Territories.Take(territoriesCount - 1);

            // Right Path
            var playerTerritories = rightTerritories.Select(t => new PlayerTerritory { Armies = 2, Territory = t });
            playerMock.Setup(x => x.Territories).Returns(playerTerritories.ToList());
            objective.IsCompleted(playerMock.Object).Should().BeTrue();


            // Wrong Paths
            playerTerritories = rightTerritories.Select(t => new PlayerTerritory { Armies = 1, Territory = t });
            playerMock.Setup(x => x.Territories).Returns(playerTerritories.ToList());
            objective.IsCompleted(playerMock.Object).Should().BeFalse();

            playerTerritories = wrongTerritories.Select(t => new PlayerTerritory { Armies = 2, Territory = t });
            playerMock.Setup(x => x.Territories).Returns(playerTerritories.ToList());
            objective.IsCompleted(playerMock.Object).Should().BeFalse();
        }
    }
}
