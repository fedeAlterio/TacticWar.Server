using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Rooms
{
    public class RoomConfiguration
    {
        public int KeepAliveInterval { get; init; } = 10000;
        public int DestroyRoomOnRoomDeathDelay { get; init; } = 60;
    }
}
