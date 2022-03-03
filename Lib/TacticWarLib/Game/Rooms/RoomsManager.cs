using TacticWar.Lib.Game.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Rooms.Abstractions;

namespace TacticWar.Lib.Game.Rooms
{
    public class RoomsManager : IRoomsManager
    {
        // Private fields
        private static readonly object _locker = new();
        private readonly IRoomBuilder _roomBuilder;
        private List<IRoom> _rooms = new();



        // Initialization
        public RoomsManager(IRoomBuilder roomBuilder)
        {
            _roomBuilder = roomBuilder;
        }



        // Properties
        public IReadOnlyList<IRoom> Rooms => _rooms;



        // Events
        private async void OnRoomDeath(IRoom room)
        {
            await Task.Delay(3000);
            using var _ = _locker.Lock();
            _rooms.Remove(room);
        }



        // Public Methods
        public Task<IRoom> NewRoom()
        {
            using var _ = _locker.Lock();

            var room = _roomBuilder.NewRoom();
            room.DeadRoom += OnRoomDeath;
            _rooms.Add(room);
            return Task.FromResult<IRoom>(room);
        }

        public Task DeleteRoom(int roomId)
        {
            using var _ = _locker.Lock();

            var room = _rooms.FirstOrDefault(r => r.Id == roomId)
                    ?? throw new GameException($"There is no room with that id");

            _rooms.Remove(room);
            return Task.CompletedTask;
        }


        public Task<IRoom> FindById(int id)
        {
            return Task.FromResult(FirstOrDefault(room => room.Id == id)
                ?? throw new GameException($"There is not room with Id {id}"));
        }



        // Utils
        private IRoom? FirstOrDefault(Func<IRoom, bool> func)
        {
            using var _ = _locker.Lock();
            return _rooms.FirstOrDefault(func);
        }
    }
}
