using System.Threading;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public record PlaceArmiesAfterAttackHandler : AuthenticatedRoomRequestHandler<PlaceArmiesAfterAttackRequest>
    {
        // Initialization
        public PlaceArmiesAfterAttackHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {
        }



        // Core
        protected override async Task HandleOnAuthenticated(PlaceArmiesAfterAttackRequest request, CancellationToken cancellationToken)
        {
            await GameManager.GameApi.PlaceArmiesAfterAttack(PlayerColor, request.Armies);
        }
    }
}
