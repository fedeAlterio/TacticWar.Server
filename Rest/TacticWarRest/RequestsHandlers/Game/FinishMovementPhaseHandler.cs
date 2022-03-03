using System.Threading;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public record FinishMovementPhaseHandler : AuthenticatedRoomRequestHandler<FinishMovementPhaseRequest>
    {
        // Initialization
        public FinishMovementPhaseHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {
        }



        // Core
        protected override async Task HandleOnAuthenticated(FinishMovementPhaseRequest request, CancellationToken cancellationToken)
        {
            await GameManager.GameApi.SkipFreeMove(PlayerColor);
        }
    }
}
