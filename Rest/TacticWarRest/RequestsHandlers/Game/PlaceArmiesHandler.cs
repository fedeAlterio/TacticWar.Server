using System.Threading;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public record PlaceArmiesHandler : AuthenticatedRoomRequestHandler<PlaceArmiesRequest>
    {
        // Initialization
        public PlaceArmiesHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {
        }



        // Core
        protected override async Task HandleOnAuthenticated(PlaceArmiesRequest request, CancellationToken cancellationToken)
        {
            foreach (var placement in request.Placements)
            {
                var territory = GameManager.GameTable.Map.TerritoryById(placement.TerritoryId);
                await GameManager.GameApi.PlaceArmies(PlayerColor, placement.Armies, territory);
            }
        }
    }
}
