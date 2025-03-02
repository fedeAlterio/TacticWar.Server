using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline;
using TacticWar.Lib.Game.Core.Pipeline.Middlewares;
using TacticWar.Lib.Game.Core.Pipeline.Middlewares.Data;
using TacticWar.Lib.Game.GamePhases.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.Extensions;

namespace TacticWar.Lib.Tests.Game.Pipeline.Middlewares
{
    public class GameUpdatesListenerTests
    {
        [FactFor(nameof(GameUpdatesListener))]
        public async Task Should_TriggerAnEventForEveryPipelineMethod()
        {
            bool eventHandlerCalled = false;
            var player = new Player();
            var turnManagerMock = new Mock<INewTurnManager>();           
            var middleware = new GameUpdatesListener(new TurnInfo { CurrentActionPlayer = player, CurrentTurnPlayer = player}, turnManagerMock.Object);
            middleware.GameUpdated += p =>
            {
                eventHandlerCalled = true;
                p.Should().Be(player);
            };
            var pipeline = GamePipeline.New().Add(middleware).Build();
            await pipeline.AssertForEachMethodThat(setupBeforeCallingNext: () => eventHandlerCalled = false, assertion: () => eventHandlerCalled);
            eventHandlerCalled = false;            
        }



        // Utils
        ITurnManager GetTurnManager()
        {
            var mock = new Mock<ITurnManager>();
            var playerMock = new Mock<IPlayer>();
            mock.Setup(x => x.CurrentActionPlayer).Returns(playerMock.Object);
            return mock.Object;
        }
    }
}
