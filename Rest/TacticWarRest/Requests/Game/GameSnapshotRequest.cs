using TacticWar.Rest.ViewModels;

namespace TacticWar.Rest.Requests.Game
{
    public record GameSnapshotRequest : AuthenticatedRoomRequest<GameSnapshot>
    {
        public int VersionId { get; init; }
    }
}
