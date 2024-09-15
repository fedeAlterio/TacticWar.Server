using TacticWar.Lib.Utils.Timers.Abstractions;
using ITimer = TacticWar.Lib.Utils.Timers.Abstractions.ITimer;

namespace TacticWar.Lib.Game.Rooms.Abstractions
{
    public delegate IRoomTimer RoomTimerBuilder();
    public interface IRoomTimer : ITimer
    {

    }
}
