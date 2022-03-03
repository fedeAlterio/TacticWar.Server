using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Pipeline;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.Extensions;

namespace TacticWar.Lib.Tests.Game.Pipeline.Middlewares
{
    public class ShortCircuitIfGameEndedTests
    {
        [FactFor(nameof(ShortCircuitIfGameEnded))]
        public async void Should_NotCallNextMiddlwareIfGameEnded()
        {
            var isGameEnded = false;
            var secondMiddlewareCalled = false;
            var gameTerminationController = GetGameTerminationController(() => isGameEnded);
            var shortCircuitMiddleware = new ShortCircuitIfGameEnded(gameTerminationController);
            var flagMiddleware = new SingleTaskMiddlewareFromAction(() => secondMiddlewareCalled = true);
            var gamePipeline = GamePipeline.New()
                .Add(shortCircuitMiddleware)
                .Add(flagMiddleware)
                .Build();

            await gamePipeline.AssertForEachMethodThat(setupBeforeCallingNext: () => secondMiddlewareCalled = false, assertion: () => secondMiddlewareCalled);
            isGameEnded = true;
            await gamePipeline.AssertForEachMethodThat(setupBeforeCallingNext: () => secondMiddlewareCalled = false, assertion: () => !secondMiddlewareCalled);
        }



        // Utils
        private IGameTerminationController GetGameTerminationController(Func<bool> gameEndedGetter)
        {
            var gameTerminationController = new Mock<IGameTerminationController>();
            gameTerminationController.Setup(x => x.IsGameEnded).Returns(gameEndedGetter);
            return gameTerminationController.Object;
        }
    }
}
