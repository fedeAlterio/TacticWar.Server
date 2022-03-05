using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Rooms.Abstractions;

namespace TacticWar.Rest.ViewModels.Rooms
{
    public class RoomSnapshot
    {
        // Initialization
        public RoomSnapshot(IRoom room)
        {
            RoomId = room.Id;
            Players = room.Players.Select(p => new WaitingPlayerSnapshot(p)).ToList();
            HostColor = room.Host?.Color ?? default;
            GameStarted = room.GameStarted;
        }



        // Properties
        public int RoomId { get; }
        public IReadOnlyList<WaitingPlayerSnapshot> Players { get; }
        public PlayerColor HostColor { get; }
        public bool GameStarted { get; }
    }
}
