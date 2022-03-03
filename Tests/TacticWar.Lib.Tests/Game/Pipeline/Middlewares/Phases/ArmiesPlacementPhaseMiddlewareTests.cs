using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Data;
using TacticWar.Lib.Game.Pipeline.Middlewares.Phases;
using TacticWar.Lib.Game.Table;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;

namespace TacticWar.Lib.Tests.Game.Pipeline.Middlewares.Phases
{
    public class ArmiesPlacementPhaseMiddlewareTests
    {
        [FactFor(nameof(ArmiesPlacementPhaseHandler.StartInitialPlacementPhase))]
        public void Should_CorrectlyStartWithPlacement()
        {
            // Setup
            var gameTable = ObjectsBuilder.NewGameTable();
            var currentTurnInfo = NewCurrentTurnInfo(gameTable);
            var trisManager = NewTrisManager();
            var middleware = new ArmiesPlacementPhaseHandler(gameTable, currentTurnInfo, trisManager);            
            ArmiesPlacementInfo? eventArgs = null;
            middleware.ArmiesPlacementPhase += args => eventArgs = args;

            // Assertions
            middleware.ArmiesToPlace.Should().Be(0);
            middleware.StartInitialPlacementPhase();
            eventArgs.Should().NotBeNull();

            var expectedArmies = ArmiesPlacementInfo.GameStartArmies(gameTable.Players.Count, currentTurnInfo.CurrentActionPlayer.ArmiesCount);
            eventArgs.Should().NotBeNull();
            eventArgs!.ArmiesToPlace.Should().Be(expectedArmies);
            eventArgs.DroppedCards.Should().BeEquivalentTo(trisManager.DroppedCards);
        }

        

        // Utils
        private ArmiesPlacementPhaseHandler NewArmiesPlacementPhaseMiddleware()
        {
            var gameTable = ObjectsBuilder.NewGameTable();
            var currentTurnInfo = NewCurrentTurnInfo(gameTable);
            var trisManager = NewTrisManager();
            var middleware = new ArmiesPlacementPhaseHandler(gameTable, currentTurnInfo, trisManager);           
            return middleware;
        }

        private TurnInfo NewCurrentTurnInfo(GameTable gameTable)
        {
            var player = gameTable.Players[0];
            return new TurnInfo { CurrentTurnPlayer = player, CurrentActionPlayer = player };
        }

        private IDroppedTrisManager NewTrisManager()
        {
            var trisManagerMock = new Mock<IDroppedTrisManager>();
            var droppedCards = new List<List<TerritoryCard>> { new List<TerritoryCard> { new TerritoryCard() } };
            trisManagerMock.Setup(x => x.DroppedCards).Returns(droppedCards);
            var trisManager = trisManagerMock.Object;
            return trisManager;
        }
    }
}
