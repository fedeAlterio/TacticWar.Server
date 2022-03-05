using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public record FinishAttackPhaseHandler : AuthenticatedRoomRequestHandler<FinishAttackPhaseRequest>
    {
        // Initialization
        public FinishAttackPhaseHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {
        }



        // Core
        protected override async Task HandleOnAuthenticated(FinishAttackPhaseRequest request, CancellationToken cancellationToken)
        {
            await GameManager.GameApi.SkipAttack(PlayerColor);
        }
    }
}
