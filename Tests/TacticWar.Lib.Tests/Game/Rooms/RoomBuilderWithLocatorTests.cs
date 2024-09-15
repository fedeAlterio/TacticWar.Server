using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TacticWar.Lib.Game.Builders.Abstractios;
using TacticWar.Lib.Game.Rooms;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Lib.Tests.Attributes;

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
        Room NewRoom()
        {
            var gameBuilderMock = new Mock<IGameBuilder>();
            var configuration = new RoomConfiguration();
            RoomTimerBuilder timerBuilder = () => new RoomTimer(configuration);
            return new Room(gameBuilderMock.Object, timerBuilder, configuration);
        }
    }
}
