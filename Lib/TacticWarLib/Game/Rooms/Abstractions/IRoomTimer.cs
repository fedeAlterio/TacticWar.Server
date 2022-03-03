using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Utils.Timers;

namespace TacticWar.Lib.Game.Rooms.Abstractions
{
    public delegate IRoomTimer RoomTimerBuilder();
    public interface IRoomTimer : ITimer
    {

    }
}
