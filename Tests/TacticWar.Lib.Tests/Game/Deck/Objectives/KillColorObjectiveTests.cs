using FluentAssertions;
using Moq;
using System.Linq;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;

namespace TacticWar.Test.TacticWar.Lib.Tests.Game.Deck.Objectives
{
    public class KillColorObjectiveTests
    {
        [FactFor(nameof(KillColorObjective.IsCompleted))]
        public void Should_ReturnTrueIfPlayerKilledCorrectPlayerOrOnTerritoriesCountFallback()
        {
            var gameTable = ObjectsBuilder.NewGameTable();
            var toKillPlayer = gameTable.Players.First();            
            var territoriesCount = 24;

            var playerMock = new Mock<IPlayer>();
            var player = playerMock.Object;
            var gameStatisticsMock = new Mock<IGameStatistics>();
            gameStatisticsMock.Setup(x => x.TryGetKiller(toKillPlayer, out player)).Returns(true);

            var objective = new KillColorObjective(toKillPlayer.Color, territoriesCount, gameStatisticsMock.Object, gameTable);
            objective.IsCompleted(player).Should().BeTrue();
        }

        [FactFor(nameof(KillColorObjective.IsCompleted))]
        public void Should_ReturnFalseIfPlayerNotKilledCorrectPlayerAndNotEnoughTerritories()
        {
            var gameTable = ObjectsBuilder.NewGameTable();
            var toKillPlayer = gameTable.Players.First();
            var territoriesCount = 24;

            var playerMock = new Mock<IPlayer>();
            var territories = gameTable.Map.Territories.Take(territoriesCount - 1).Select(t => new PlayerTerritory { Territory = t }).ToList();
            playerMock.Setup(x => x.Territories).Returns(territories.ToList());

            IPlayer player = gameTable.Players.ElementAt(1);
            var gameStatisticsMock = new Mock<IGameStatistics>();
            gameStatisticsMock.Setup(x => x.TryGetKiller(toKillPlayer, out player)).Returns(true);

            var objective = new KillColorObjective(toKillPlayer.Color, territoriesCount, gameStatisticsMock.Object, gameTable);
            objective.IsCompleted(playerMock.Object).Should().BeFalse();
        }

        [FactFor(nameof(KillColorObjective.IsCompleted))]
        public void Should_ReturnTrueIfPlayerNotKilledCorrectPlayerAndEnoughTerritories()
        {
            var gameTable = ObjectsBuilder.NewGameTable();
            var toKillPlayer = gameTable.Players.First();
            var territoriesCount = 24;

            var playerMock = new Mock<IPlayer>();
            var territories = gameTable.Map.Territories.Take(territoriesCount).Select(t => new PlayerTerritory { Territory = t }).ToList();
            playerMock.Setup(x => x.Territories).Returns(territories.ToList());

            IPlayer player = gameTable.Players.ElementAt(1);
            var gameStatisticsMock = new Mock<IGameStatistics>();
            gameStatisticsMock.Setup(x => x.TryGetKiller(toKillPlayer, out player)).Returns(true);

            var objective = new KillColorObjective(toKillPlayer.Color, territoriesCount, gameStatisticsMock.Object, gameTable);
            objective.IsCompleted(playerMock.Object).Should().BeTrue();
        }
    }
}
