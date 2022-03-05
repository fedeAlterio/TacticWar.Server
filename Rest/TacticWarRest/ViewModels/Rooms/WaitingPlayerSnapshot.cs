using TacticWar.Lib.Game.Rooms;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Rest.ViewModels.Rooms
{
    public class WaitingPlayerSnapshot
    {
        // Initialization
        public WaitingPlayerSnapshot(WaitingPlayer waitingPlayer)
        {
            Color = waitingPlayer.Color;
            Name = waitingPlayer.Name;
        }



        // Propertise
        public PlayerColor Color { get; }
        public string Name { get; }
    }
}
