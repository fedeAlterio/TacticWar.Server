using MediatR;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests.Room;
using TacticWar.Rest.ViewModels.Rooms;

namespace TacticWar.Rest.RequestsHandlers.Room
{
    public record CreateRoomHandler : IRequestHandler<CreateRoomRequest, RoomSnapshot>
    {
        // Private fields
        readonly IRoomsManager _roomsManager;



        // Initialization
        public CreateRoomHandler(IRoomsManager roomsManager)
        {
            _roomsManager = roomsManager;
        }



        // Core
        public async Task<RoomSnapshot> Handle(CreateRoomRequest request, CancellationToken cancellationToken)
        {
            var room = await _roomsManager.NewRoom();
            room.AddPlayer(request.PlayerName, request.SecretCode);
            //room.AddPlayer(request.PlayerName + "b", request.SecretCode, true);
            //room.AddPlayer(body.PlayerName + "c", body.SecretCode);
            //room.AddPlayer(body.PlayerName + "d", body.SecretCode);
            //room.AddPlayer(body.PlayerName + "e", body.SecretCode);
            //room.AddPlayer(body.PlayerName + "f", body.SecretCode);

            var ret = new RoomSnapshot(room);
            return ret;
        }
    }
}
