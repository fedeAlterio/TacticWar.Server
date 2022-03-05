using TacticWar.Lib.Game.Rooms.Abstractions;
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
