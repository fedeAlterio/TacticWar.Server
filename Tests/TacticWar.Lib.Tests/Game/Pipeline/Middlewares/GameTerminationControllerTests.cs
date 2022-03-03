using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;
using TacticWar.Test.TacticWar.Lib.Tests.Utils;

namespace TacticWar.Test.TacticWar.Lib.Tests.Game.Pipeline.Middlewares
{
    public class GameTerminationControllerTests
    {
        [FactFor(nameof(GameTerminationController))]
        public async void Should_InvokeVictoryEventWhenPlayerHasWon()
        {
            await using var exceptionCatcher = new ExceptionCatcher();
            var objectiveMock = new Mock<IObjective>();
            objectiveMock.Setup(o => o.IsCompleted(It.IsAny<IPlayer>())).Returns(true);
            var objectives = Enumerable.Range(0, 10).Select(_ => objectiveMock.Object);
            var objectivesDeck = new Deck<IObjective>(objectives);

            var idleManagerMock = new Mock<IIdleManager>();
            idleManagerMock.Setup(x => x.IsGameIdle).Returns(false);

            var gameTable = ObjectsBuilder.NewGameTable(objectivesDeck: objectivesDeck);
            var gameTerminationController = new GameTerminationController(gameTable,idleManagerMock.Object);

            var tcs = new TaskCompletionSource<object?>();
            gameTerminationController.Victory += exceptionCatcher.Action<VictoryPhaseInfo>(async victoryInfo =>
            {
                victoryInfo.Winner.Should().NotBeNull();                
                tcs.SetResult(null);
            });

            await gameTerminationController.Start();
            await tcs.Task;
        }
    }
}
