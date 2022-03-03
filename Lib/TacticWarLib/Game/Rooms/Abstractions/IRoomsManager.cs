using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Rooms.Abstractions
{
    public interface IRoomsManager
    {
        Task<IRoom> NewRoom();
        Task DeleteRoom(int roomId);
        Task<IRoom> FindById(int id);
    }
}
