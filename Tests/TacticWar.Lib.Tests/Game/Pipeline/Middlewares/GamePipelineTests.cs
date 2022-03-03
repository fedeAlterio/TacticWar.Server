using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Pipeline;
using TacticWar.Lib.Game.Pipeline.Middlewares;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.Extensions;

namespace TacticWar.Lib.Tests.Game.Pipeline.Middlewares
{
    public class GamePipelineTests
    {
        [FactFor(nameof(GamePipeline.New))]
        public async Task Should_CreateCorrectPipeline()
        {
            int counter = 0;
            Action callBack = () => counter++;
            var middlware1 = new SingleTaskMiddlewareFromAction(callBack);
            var middlware2 = new SingleTaskMiddlewareFromAction(callBack);
            var pipeline = GamePipeline
                .New()
                .Add(middlware1)
                .Add(middlware2)
                .Build();

            await pipeline.AssertForEachMethodThat(setupBeforeCallingNext: () => counter = 0, assertion: () => counter == 2);
        }
    }
}
