using System.Collections.Generic;

namespace TacticWar.Rest.Requests.Game
{
    public record PlaceArmiesRequest : AuthenticatedRoomRequest
    {
        public IReadOnlyList<PlacementInfo>? Placements { get; init; }
    }
    public record PlacementInfo(int TerritoryId, int Armies);
}
