using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Deck.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Builders.Abstractions;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Pipeline.Middlewares;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Data;
using TacticWar.Lib.Game.Pipeline.Middlewares.Phases;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table;

namespace TacticWar.Lib.Tests.Game.Pipeline.TestsUtils
{
    public static class ObjectsBuilder
    {
        public static GameTable NewGameTable(IDiceRoller? diceRoller = null, IDeck<IObjective>? objectivesDeck = null)
        {
            diceRoller ??= NewDiceRoller();
            var gameMap = new MapBuilder().BuildNew();
            var playersInfoCollection = NewPlayersInfoCollection();
            var objectiveDeck = objectivesDeck ?? NewObjectiveDeck();
            var territoyCardsDeck = NewTerritoryCardsDeck(gameMap);
            var gameTable = new GameTable(gameMap, playersInfoCollection, objectiveDeck, diceRoller, territoyCardsDeck);
            return gameTable;
        }

        public static PlayersInfoCollection NewPlayersInfoCollection()
        {
            var playersInfo = new PlayerInfo[]
            {
                new("A", PlayerColor.Red),
                new("B", PlayerColor.Green),
                new("C", PlayerColor.Blue),
                new("D", PlayerColor.Purple),
            };
            var playersInfoCollection = new PlayersInfoCollection(playersInfo);
            return playersInfoCollection;
        }

        public static IDeck<IObjective> NewObjectiveDeck()
        {
            var objectiveDeckMock = new Mock<IDeck<IObjective>>();
            var objectiveMock = new Mock<IObjective>();
            var objective = objectiveMock.Object;
            objectiveDeckMock.Setup(d => d.Draw(out objective));
            return objectiveDeckMock.Object;
        }

        public static IDeck<TerritoryCard> NewTerritoryCardsDeck(GameMap gameMap)
        {
            var builder = new TerritoryDeckBuilder();
            return builder.NewDeck(gameMap);
        }

        public static GameMap NewMap () => new MapBuilder().BuildNew();

        public static ArmiesPlacementPhaseHandler NewArmiesPlacementPhaseMiddleware()
        {
            var gameTable = NewGameTable();
            var currentTurnInfo = NewCurrentTurnInfo(gameTable);
            var trisManager = NewTrisManager(gameTable, currentTurnInfo);
            var middleware = new ArmiesPlacementPhaseHandler(gameTable, currentTurnInfo, trisManager);
            return middleware;
        }

        public static TurnInfo NewCurrentTurnInfo(GameTable gameTable)
        {
            var player = gameTable.Players[0];
            return new TurnInfo { CurrentActionPlayer = player, CurrentTurnPlayer = player };
        }

        public static CardsManager NewTrisManager(GameTable? gameTable = null, TurnInfo? turnInfo = null)
        {
            return new CardsManager(gameTable ?? NewGameTable());
        }

        public static IDiceRoller NewDiceRoller()
        {
            var diceRollerMock = new Mock<IDiceRoller>();
            diceRollerMock.Setup(x => x.RollDice()).Returns(1);
            return diceRollerMock.Object;
        }

        public static IGameConfiguration NewGameConfiguration()
        {
            var gameConfigurationMock = new Mock<IGameConfiguration>();
            gameConfigurationMock.Setup(x => x.DelayAfterTerritoryConqueredMs).Returns(0);
            return gameConfigurationMock.Object;
        }
    }
}
