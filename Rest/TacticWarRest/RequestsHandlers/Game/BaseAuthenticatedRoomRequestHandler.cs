using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public abstract record BaseAuthenticatedRoomRequestHandler<TAuthenticatedRequest> where TAuthenticatedRequest : AuthenticatedRoomRequest
    {
        // Fields
        protected readonly IRoomsManager _roomsManager;
        private readonly IViewModelsLocator _viewModelsLocator;


        // Initialization
        public BaseAuthenticatedRoomRequestHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator)
        {
            _roomsManager = roomsManager;
            _viewModelsLocator = viewModelsLocator;
        }



        // Properties
        protected IGameManager GameManager { get; private set; }
        protected PlayerColor PlayerColor { get; private set; }
        protected IViewModelService ViewModelService { get; private set; }



        // Authentication
        protected async Task Authenticate(TAuthenticatedRequest request)
        {
            var room = await _roomsManager.FindById(request.RoomId);
            GameManager = room.GameManager!;
            PlayerColor = await room.Authenticate(request.PlayerName!, request.PlayerSecret);
            ViewModelService = await _viewModelsLocator.FromGameManager(GameManager);
        }
    }
}
