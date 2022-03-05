namespace TacticWar.Lib.Game.Rooms
{
    public class RoomConfiguration
    {
        public int KeepAliveInterval { get; init; } = 10000;
        public int DestroyRoomOnRoomDeathDelay { get; init; } = 60 * 1000;
    }
}
