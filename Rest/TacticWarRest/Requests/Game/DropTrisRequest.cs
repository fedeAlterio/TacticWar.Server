using System.Collections.Generic;

namespace TacticWar.Rest.Requests.Game
{
    public record DropTrisRequest : AuthenticatedRoomRequest
    {
        public IReadOnlyList<int> CardsIds { get; init; }
    }
}
