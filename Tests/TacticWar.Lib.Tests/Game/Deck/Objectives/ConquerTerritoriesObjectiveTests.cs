using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck.Objectives;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;

namespace TacticWar.Test.TacticWar.Lib.Tests.Game.Deck.Objectives
{
    public class ConquerTerritoriesObjectiveTests
    {
        [FactFor(nameof(ConquerTerritoriesObjective.IsCompleted))]
        public void Should_ReturnTrueIfPlayerHasAllRequiredTerritoriesNumber()
        {            
            var territoriesCount = 24;
            var objective = new ConquerTerritoriesObjective(territoriesCount);
            var playerMock = new Mock<IPlayer>();
            var gameMap = ObjectsBuilder.NewMap();
          
            
            var wrongTerritories = gameMap.Territories.Take(territoriesCount - 1)
                .Select(t => new PlayerTerritory { Armies = 0, Territory = t })
                .ToList();
            playerMock.Setup(x => x.Territories).Returns(wrongTerritories);
            objective.IsCompleted(playerMock.Object).Should().BeFalse();


            var rightTerritories = gameMap.Territories.Take(territoriesCount)
                .Select(t => new PlayerTerritory { Armies = 0, Territory = t })
                .ToList();
            playerMock.Setup(x => x.Territories).Returns(rightTerritories);
            objective.IsCompleted(playerMock.Object).Should().BeTrue();
        }
    }
}
