using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public record FinishPlacementHandler : AuthenticatedRoomRequestHandler<FinishPlacementRequest>
    {
        // initialization
        public FinishPlacementHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {
        }



        // Core 
        protected override async Task HandleOnAuthenticated(FinishPlacementRequest request, CancellationToken cancellationToken)
        {
            await GameManager.GameApi.SkipPlacementPhase(PlayerColor);
        }
    }
}
