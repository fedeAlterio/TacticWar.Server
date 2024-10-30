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
        readonly IViewModelsLocator _viewModelsLocator;


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
            (ViewModelService, GameManager, PlayerColor) = await AuthenticationEx.Authenticate(_roomsManager, _viewModelsLocator, request.RoomId, request.PlayerName!, request.PlayerSecret);
        }
    }

    public static class AuthenticationEx
    {
        public static async Task<(IViewModelService viewModelService,IGameManager gameManager,  PlayerColor playerColor)> Authenticate(IRoomsManager roomsManager, 
                                                                                                                                                IViewModelsLocator viewModelsLocator,
                                                                                                                                                int roomId, string playerName, int playerSecret)
        {
            var room = await roomsManager.FindById(roomId);
            var gameManager = room.GameManager!;
            var playerColor = await room.Authenticate(playerName!, playerSecret);
            var viewModelService = await viewModelsLocator.FromGameManager(gameManager);
            return (viewModelService, gameManager, playerColor);
        }
    }
}
