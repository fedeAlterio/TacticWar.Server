using MediatR;

namespace TacticWar.Rest.Requests.Game
{
    public abstract record AuthenticatedRoomRequest : IRequest
    {
        public int RoomId { get; init; }
        public string? PlayerName { get; init; }
        public int PlayerSecret { get; init; }
    }

    public abstract record AuthenticatedRoomRequest<T> : AuthenticatedRoomRequest, IRequest<T>
    {

    }
}
