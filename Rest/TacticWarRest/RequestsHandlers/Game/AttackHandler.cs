using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public record AttackHandler : AuthenticatedRoomRequestHandler<AttackRequest>
    {
        // Core
        public AttackHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {
        }



        // Core
        protected override async Task HandleOnAuthenticated(AttackRequest request, CancellationToken cancellationToken)
        {
            await GameManager.GameApi.Attack(PlayerColor, TerritoryById(request.AttackId), TerritoryById(request.DefenceId), request.AttackDice);
        }



        // Utils
        private Territory TerritoryById(int id) => GameManager.GameTable.Map.TerritoryById(id);
    }
}
