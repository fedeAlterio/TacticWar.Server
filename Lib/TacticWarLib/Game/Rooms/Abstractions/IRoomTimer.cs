using TacticWar.Lib.Utils.Timers.Abstractions;

namespace TacticWar.Lib.Game.Rooms.Abstractions
{
    public delegate IRoomTimer RoomTimerBuilder();
    public interface IRoomTimer : ITimer
    {

    }
}
