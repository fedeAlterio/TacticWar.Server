using System.Threading;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public record DefendHandler : AuthenticatedRoomRequestHandler<DefendRequest>
    {
        // Initialization
        public DefendHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {
        }



        // Core
        protected override async Task HandleOnAuthenticated(DefendRequest request, CancellationToken cancellationToken)
        {
            await GameManager.GameApi.Defend(PlayerColor);
        }
    }
}
