using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests;
using TacticWar.Rest.Requests.Room;
using TacticWar.Rest.ViewModels.Rooms;

namespace TacticWar.Rest.RequestsHandlers.Room
{
    public record KeepAliveHandler : IRequestHandler<KeepAliveRequest, RoomSnapshot>
    {
        // Private fields
        private readonly IRoomsManager _roomsManager;



        // Initialization
        public KeepAliveHandler(IRoomsManager roomsManager)
        {
            _roomsManager = roomsManager;
        }



        // Core
        public async Task<RoomSnapshot> Handle(KeepAliveRequest request, CancellationToken cancellationToken)
        {
            var room = await _roomsManager.FindById(request.RoomId);
            room.KeepAlive(request.Color);
            var ret = new RoomSnapshot(room);
            return ret;
        }
    }
}
