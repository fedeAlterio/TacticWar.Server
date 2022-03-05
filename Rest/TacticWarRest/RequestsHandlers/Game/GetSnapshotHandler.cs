using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public record GetSnapshotHandler : AuthenticatedRoomRequestHandler<GameSnapshotRequest, GameSnapshot>
    {
        // Initialization
        public GetSnapshotHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {
        }



        // Core
        protected override async Task<GameSnapshot> HandleOnAuthenticated(GameSnapshotRequest request, CancellationToken cancellationToken)
        {
            var gameSnapshot = await ViewModelService.GetGameSnapshot(PlayerColor, request.VersionId);
            return gameSnapshot;
        }
    }
}
