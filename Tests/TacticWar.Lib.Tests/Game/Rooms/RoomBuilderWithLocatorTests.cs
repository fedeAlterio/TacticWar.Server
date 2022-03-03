using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Rooms;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Lib.Tests.Attributes;
using Xunit;

namespace TacticWar.Lib.Tests.Game.Rooms
{
    public class RoomBuilderWithLocatorTests
    {
        [FactFor(nameof(RoomBuilderWithLocator))]
        public void Should_ProvideLocatorRoomBuilder()
        {
            var servicesCollection = new ServiceCollection();

            var room = NewRoom();
            servicesCollection.AddSingleton(_ => room);
            var serviceProvider = servicesCollection.BuildServiceProvider();
            var roomBuilder = new RoomBuilderWithLocator(serviceProvider);
            roomBuilder.NewRoom().Should().Be(room);
        }



        // Utils
        private Room NewRoom()
        {
            var gameBuilderMock = new Mock<INewGameBuilder>();
            var configuration = new RoomConfiguration();
            RoomTimerBuilder timerBuilder = () => new RoomTimer(configuration);
            return new Room(gameBuilderMock.Object, timerBuilder, configuration);
        }
    }
}
