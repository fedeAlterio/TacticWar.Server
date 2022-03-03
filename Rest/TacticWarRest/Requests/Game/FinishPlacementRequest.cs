using MediatR;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Rest.Requests.Game
{
    public record FinishPlacementRequest : AuthenticatedRoomRequest
    {
        public PlayerColor PlayerColor { get; init; }
    }
}
