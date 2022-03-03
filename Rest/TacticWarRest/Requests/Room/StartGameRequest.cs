using MediatR;

namespace TacticWar.Rest.Requests.Room
{
    public record StartGameRequest : IRequest
    {
        public int RoomId { get; init; }
    }
}
