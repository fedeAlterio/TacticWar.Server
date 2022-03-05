using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public record MoveArmiesHandler : AuthenticatedRoomRequestHandler<MoveArmiesRequest>
    {
        // Initialization
        public MoveArmiesHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {
        }



        // Core
        protected override async Task HandleOnAuthenticated(MoveArmiesRequest request, CancellationToken cancellationToken)
        {
            Territory TerritoryById(int id) => GameManager.GameTable.Map.TerritoryById(id);
            var (from, to) = (TerritoryById(request.FromId), TerritoryById(request.ToId));
            await GameManager.GameApi.Movement(PlayerColor, from, to, request.Armies);
        }
    }
}
