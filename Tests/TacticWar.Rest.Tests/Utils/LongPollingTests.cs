using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Rest.Utils.LongPolling;

namespace TacticWar.Test.TacticWar.Rest.Tests.Utils
{
    public class LongPollingTests
    {
        [FactFor(nameof(UpdateQueue<int>.Get))]
        public async Task Should_ProvideConsistentUpdates()
        {
            var queue = new UpdateQueue<int>();
            var numTask1 = queue.Get();
            var numTask2 = queue.Get();
            numTask1.IsCompleted.Should().BeFalse();
            numTask2.IsCompleted.Should().BeFalse();
            queue.NotifyNew(1);
            queue.NotifyNew(2);
            var num1 = await numTask1;
            var num2 = await numTask1;
            num1.Should().Be(1);
            num2.Should().Be(1);
        }
    }
}
