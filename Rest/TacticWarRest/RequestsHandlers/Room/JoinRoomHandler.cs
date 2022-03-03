using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests;
using TacticWar.Rest.Requests.Room;
using TacticWar.Rest.ViewModels.Rooms;

namespace TacticWar.Rest.RequestsHandlers.Room
{
    public record JoinRoomHandler : IRequestHandler<JoinRoomRequest, RoomSnapshot>
    {
        // Private fields
        private readonly IRoomsManager _roomsManager;



        // Initialization
        public JoinRoomHandler(IRoomsManager roomsManager)
        {
            _roomsManager = roomsManager;
        }



        // Core
        public async Task<RoomSnapshot> Handle(JoinRoomRequest request, CancellationToken cancellationToken)
        {
            var room = await _roomsManager.FindById(request.RoomId);
            if (!room.GameStarted)
                room.AddPlayer(request.PlayerName!, request.SecretCode, request.IsBot);
            else
                await room.Authenticate(request.PlayerName!, request.SecretCode);

            var ret = new RoomSnapshot(room);
            return ret;
        }
    }
}
