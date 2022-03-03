using MediatR;
using TacticWar.Lib.Game.Players;
using TacticWar.Rest.ViewModels.Rooms;

namespace TacticWar.Rest.Requests.Room
{
    public record KeepAliveRequest : IRequest<RoomSnapshot>
    {
        public int RoomId { get; init; }
        public PlayerColor Color { get; init; }
    }
}
