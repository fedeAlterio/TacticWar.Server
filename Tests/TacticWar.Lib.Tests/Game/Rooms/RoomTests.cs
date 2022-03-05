using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Builders.Abstractios;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Rooms;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Extensions;

namespace TacticWar.Test.TacticWar.Lib.Tests.Game.Rooms
{
    public class RoomTests
    {
        [FactFor]
        private void Should_GenerateNewUniqueIdEveryTime()
        {
            var ids = Enumerable.Range(1, 1000).Select(x => NewRoom().Id).ToList();
            ids.Count.Should().Be(ids.Distinct().Count());
        }


        [FactFor]
        private void Should_CorrectlyInitializeARoom()
        {
            var room = NewRoom();
            room.GameStarted.Should().BeFalse();
            room.GameManager.Should().BeNull();
            room.Players.Should().BeEmpty();
            room.Host.Should().BeNull();
        }


        [FactFor(nameof(Room.AddPlayer))]
        private void Should_CorrectlyAddPlayerToPlayersList()
        {
            var info = new[]
             {
                new{Name = "A", SecretCode = 0, IsBot = false},
                new{Name = "B", SecretCode = 1, IsBot = true},
                new{Name = "C", SecretCode = 2, IsBot = false},
                new{Name = "D", SecretCode = 3, IsBot = true},
                new{Name = "E", SecretCode = 4,IsBot = false },
                new{Name = "F", SecretCode = 5 ,IsBot = true },
            };

            var players = new List<WaitingPlayer>();
            var room = NewRoom();
            foreach (var x in info)
            {
                var player = room.AddPlayer(x.Name, x.SecretCode, x.IsBot);
                players.Add(player);
            }

            room.Players.Should().HaveSameReferencesOf(players);
            room.Players.Select(p => new { p.Name, p.SecretCode, p.IsBot }).Should().BeEquivalentTo(info);
            room.Players.Select(p => p.Color).Should().BeAllDistinct();
        }


        [FactFor(nameof(Room.AddPlayer))]
        private void Should_ThrowOnSamePlayerName()
        {
            var room = NewRoom();
            var info = new { Name = "A", SecretCode = 0, IsBot = false };
            Action Add = () => room.AddPlayer(info.Name, info.SecretCode, info.IsBot);
            Add.Should().NotThrow();
            Add.Should().Throw<GameException>();
        }


        [FactFor(nameof(Room.AddPlayer))]
        private void Should_ThrowIfTooManyPlayersAreAdded()
        {
            var room = NewRoom();
            var info = new[]
            {
                new{Name = "A", SecretCode = 0, IsBot = false},
                new{Name = "B", SecretCode = 1, IsBot = true},
                new{Name = "C", SecretCode = 2, IsBot = false},
                new{Name = "D", SecretCode = 3, IsBot = true},
                new{Name = "E", SecretCode = 4,IsBot = false },
                new{Name = "F", SecretCode = 5 ,IsBot = true },
            };
            foreach (var x in info)
            {
                var addAction = () => room.AddPlayer(x.Name, x.SecretCode, x.IsBot);
                addAction.Should().NotThrow();
            }
            var exceptionPlayerInfo = new { Name = "G", SecretCode = 6, IsBot = false };
            var add = () => room.AddPlayer(exceptionPlayerInfo.Name, exceptionPlayerInfo.SecretCode, exceptionPlayerInfo.IsBot);
            add.Should().Throw<GameException>();
        }



        [FactFor(nameof(Room.RemovePlayer))]
        private void Should_RemovePlayerFromPlayersList()
        {
            var room = NewRoom();
            var player = room.AddPlayer("A", 1, true);
            room.Players.Count.Should().Be(1);
            room.RemovePlayer(player.Color);
            room.Players.Should().BeEmpty();
        }


        [FactFor(nameof(Room.RemovePlayer))]
        private void Should_ThrowIfRemovingNotExistentPlayer()
        {
            var room = NewRoom();
            var notExistentColor = PlayerColor.Red;
            var remove = () => room.RemovePlayer(notExistentColor);
            remove.Should().Throw<GameException>();
            var player = room.AddPlayer("A", 1, true);
            notExistentColor = player.Color == PlayerColor.Red ? PlayerColor.Blue : PlayerColor.Red;
            remove.Should().Throw<GameException>();
        }


        [FactFor(nameof(Room.AddPlayer))]
        private async Task Should_StartIdleTimerForJoinedPlayer()
        {
            var configuration = new RoomConfiguration { KeepAliveInterval = 500 };
            Room room = NewRoom(configuration);
            var player = room.AddPlayer("A", 1, true);
            room.Players.Count.Should().Be(1);
            await Task.Delay(2 * configuration.KeepAliveInterval);
            room.Players.Count.Should().Be(0);
        }


        [FactFor(nameof(Room.KeepAlive))]
        private async Task Should_KeepAlivePlayer()
        {
            var configuration = new RoomConfiguration { KeepAliveInterval = 500 };
            Room room = NewRoom(configuration);
            var player = room.AddPlayer("A", 1, true);
            var iterations = 5;
            for (var i = 0; i < iterations; i++)
            {
                room.KeepAlive(player.Color);
                room.Players.Count.Should().Be(1);
                await Task.Delay(configuration.KeepAliveInterval / 2);
            }

            await Task.Delay(configuration.KeepAliveInterval * 2);
            room.Players.Count.Should().Be(0);
        }



        // utils
        private Room NewRoom(RoomConfiguration? configuration = null)
        {
            var gameBuilder = NewGameBuilder();
            configuration ??= new RoomConfiguration { KeepAliveInterval = 100_000 }; // Infinite timer delay


            var room = new Room(gameBuilder, () => new RoomTimer(configuration), configuration);
            return room;
        }

        private IGameBuilder NewGameBuilder()
        {
            var builderMock = new Mock<IGameBuilder>();
            return builderMock.Object;
        }
    }
}
