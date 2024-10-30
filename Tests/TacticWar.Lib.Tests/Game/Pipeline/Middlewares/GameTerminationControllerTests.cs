using FluentAssertions;
using Moq;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Testing;
using TacticWar.Lib.Game;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline.Middlewares;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
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
            idleManagerMock.Setup(x => x.GameEnded)
                           .Returns(new Subject<Unit>());

            var gameTable = ObjectsBuilder.NewGameTable(objectivesDeck: objectivesDeck);
            var gameTerminationController = new GameTerminationController(gameTable, idleManagerMock.Object, new GameStartupInformation(new([]), 1), new FakeLogger<GameTerminationController>());

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
