using FluentAssertions;
using Moq;
using System;
using System.Linq;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Deck.Abstractions;
using TacticWar.Lib.Game.Deck.Builders;
using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;

namespace TacticWar.Lib.Tests.Game.Table
{
    public class GameTableTests
    {
        [FactFor]
        public void Should_CorrectlyInjectFromParameters()
        {
            var gameTable = NewGameTable();
            gameTable.Deck.Should().NotBeNull();
            gameTable.Map.Should().NotBeNull();
            var playersInfoCollection = NewPlayersInfoCollection();
            gameTable.Players.Select(x => new PlayerInfo(x.Name, x.Color)).Should().BeEquivalentTo(playersInfoCollection.Info);
        }


        [FactFor]
        public void Should_CreatePlayersWithOnlyOneArmyInEachTerritory()
        {
            var gameTable = NewGameTable();
            var players = gameTable.Players;
            foreach (var player in players)
                foreach (var (_, playerTerritory) in player.Territories)
                    playerTerritory.Armies.Should().Be(1);
        }


        [FactFor]
        public void Should_CreatePlayersWithRightNumberOfTerritories()
        {
            var gameTable = NewGameTable();
            var players = gameTable.Players;
            var minTerritories = players.Min(p => p.Territories.Count);
            var maxTerritories = players.Max(p => p.Territories.Count);
            minTerritories.Should().BeCloseTo(maxTerritories, 1);
        }


        [FactFor]
        public void Should_ThrowIf2PlayersHaveTheSameName()
        {
            var gameMap = new MapBuilder().BuildNew();
            var playersInfo = new PlayerInfo[]
            {
                new("A", PlayerColor.Blue),
                new("A", PlayerColor.Blue),
            };
            var playersInfoCollection = new PlayersInfoCollection(playersInfo);
            var objectiveDeck = NewObjectiveDeck();
            var territoyCardsDeck = NewTerritoryCardsDeck(gameMap);
            Action createGameTable = () => new GameTable(gameMap, playersInfoCollection, objectiveDeck, NewDIceRoller(), territoyCardsDeck);
            createGameTable.Should().Throw<GameException>();
        }



        [FactFor] 
        public void Should_BuildInstanceWithCorrectStartingFlags()
        {
            var gameTable = NewGameTable();
            gameTable.WaitingForArmiesMovement.Should().BeFalse();
            gameTable.WaitingForDefence.Should().BeFalse();
        }

        


        // Utils
        private GameTable NewGameTable()
        {
            var gameMap = new MapBuilder().BuildNew();
            var playersInfoCollection = NewPlayersInfoCollection();
            var objectiveDeck = NewObjectiveDeck();
            var territoyCardsDeck = NewTerritoryCardsDeck(gameMap);
            var gameTable = new GameTable(gameMap, playersInfoCollection, objectiveDeck, NewDIceRoller(), territoyCardsDeck);;
            return gameTable;
        }

        private PlayersInfoCollection NewPlayersInfoCollection()
        {
            var playersInfo = new PlayerInfo[]
            {
                new("A", PlayerColor.Red),
                new("B", PlayerColor.Green),
                new("C", PlayerColor.Blue),
                new("D", PlayerColor.Green),
            };
            var playersInfoCollection = new PlayersInfoCollection(playersInfo);
            return playersInfoCollection;
        }

        private IDeck<IObjective> NewObjectiveDeck()
        {
            var objectiveDeckMock = new Mock<IDeck<IObjective>>();
            var objectiveMock = new Mock<IObjective>();
            var objective = objectiveMock.Object;
            objectiveDeckMock.Setup(d => d.Draw(out objective));
            return objectiveDeckMock.Object;
        }

        private IDeck<TerritoryCard> NewTerritoryCardsDeck(GameMap gameMap)
        {
            var builder = new TerritoryDeckBuilder();
            return builder.NewDeck(gameMap);
        }

        private IDiceRoller NewDIceRoller() => ObjectsBuilder.NewDiceRoller();
    }
}
