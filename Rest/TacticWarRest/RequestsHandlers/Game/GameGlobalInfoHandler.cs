using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public record GameGlobalInfoHandler : AuthenticatedRoomRequestHandler<GameGlobalInfoRequest, GameGlobalInfo>
    {
        // Initialization
        public GameGlobalInfoHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {
        }



        // Core
        protected override async Task<GameGlobalInfo> HandleOnAuthenticated(GameGlobalInfoRequest request, CancellationToken cancellationToken)
        {
            var globalInfo = await ViewModelService.GetGameGlobalInfo(PlayerColor);
            return globalInfo;
        }
    }
}
