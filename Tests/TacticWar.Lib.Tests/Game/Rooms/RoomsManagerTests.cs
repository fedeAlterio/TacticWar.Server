using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Builders.Abstractios;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Rooms;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Lib.Tests.Attributes;

namespace TacticWar.Lib.Tests.Game.Rooms
{
    public class RoomsManagerTests
    {
        // Tets
        [FactFor(nameof(RoomsManager.NewRoom))]
        public async Task Should_CreateARoom()
        {
            var roomsManager = NewRoomsManager();
            var room = await roomsManager.NewRoom();
            roomsManager.Rooms.Should().Contain(room);
        }


        [FactFor(nameof(RoomsManager.DeleteRoom))]
        public async Task RoomsManager_Should_RemoveARoom()
        {
            var roomsManager = NewRoomsManager();
            var room = await roomsManager.NewRoom();
            await roomsManager.DeleteRoom(room.Id);
            roomsManager.Rooms.Should().NotContain(room);
        }


        [FactFor(nameof(RoomsManager.DeleteRoom))]
        public async Task Should_ThrowIfTryingToDeleteANotExistentRoom()
        {
            var roomsManager = NewRoomsManager();
            Action deleteRoom = () => roomsManager.DeleteRoom(1);
            deleteRoom.Should().Throw<GameException>();
            var room = await roomsManager.NewRoom();
            deleteRoom = () => roomsManager.DeleteRoom(room.Id == 0 ? 1 : 0);
            deleteRoom.Should().Throw<GameException>();
        }


        [FactFor(nameof(RoomsManager.FindById))]
        public async Task Should_FindRoomById()
        {
            var roomsManager = NewRoomsManager();
            var roomA = await roomsManager.NewRoom();
            var roomB = await roomsManager.NewRoom();
            (await roomsManager.FindById(roomA.Id)).Should().Be(roomA);
            (await roomsManager.FindById(roomB.Id)).Should().Be(roomB);
        }


        [FactFor(nameof(RoomsManager.FindById))]
        public async Task Should_ThrowIfTryingToFindANotExistentRoom()
        {
            var roomsManager = NewRoomsManager();
            var roomA = await roomsManager.NewRoom();
            Func<Task> findRoomAction = async () => await roomsManager.FindById(roomA.Id == 0 ? 1 : 0);
            await findRoomAction.Should().ThrowAsync<GameException>();
        }



        // Utils
        RoomsManager NewRoomsManager()
        {
            var roomBuilderMock = new Mock<IRoomBuilder>();
            roomBuilderMock.Setup(r => r.NewRoom()).Returns(NewRoom);
            return new RoomsManager(roomBuilderMock.Object);
        }

        Room NewRoom()
        {
            var gameBuilder = NewGameBuilder();
            var configuration = new RoomConfiguration { KeepAliveInterval = 100_000 }; // Infinite timer delay
            var room = new Room(gameBuilder, () => new RoomTimer(configuration), configuration);
            return room;
        }

        IGameBuilder NewGameBuilder()
        {
            var builderMock = new Mock<IGameBuilder>();
            return builderMock.Object;
        }
    }
}
