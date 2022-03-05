using TacticWar.Lib.Game.Rooms.Abstractions;

namespace TacticWar.Lib.Game.Rooms
{
    public class RoomBuilderWithLocator : IRoomBuilder
    {
        // Private fields
        private readonly IServiceProvider _serviceProvider;



        // Initialization
        public RoomBuilderWithLocator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }



        // Core
        public Room NewRoom()
        {
            return (Room) _serviceProvider.GetService(typeof(Room));
        }
    }
}
