using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Lib.Utils.Timers;
using Timer = TacticWar.Lib.Utils.Timers.Timer;

namespace TacticWar.Lib.Game.Rooms
{
    public class RoomTimer : Timer, IRoomTimer
    {
        public RoomTimer(RoomConfiguration roomConfiguration) : base(roomConfiguration.KeepAliveInterval)
        {

        }
    }
}
