using MediatR;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests.Room;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Room
{
    public record StartGameHandler : IRequestHandler<StartGameRequest>
    {
        // Private fields
        private readonly IRoomsManager _roomsManager;
        private readonly IViewModelsLocator _viewModelsLocator;



        // Initialization
        public StartGameHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator)
        {
            _roomsManager = roomsManager;
            _viewModelsLocator = viewModelsLocator;
        }



        // Core
        public async Task<Unit> Handle(StartGameRequest request, CancellationToken cancellationToken)
        {
            var room = await _roomsManager.FindById(request.RoomId);
            var gameServices = room.BuildGame();
            RegisterGameServices(gameServices);
            AddBot(gameServices, room.Players.Skip(1).First().Color);
            await room.StartGame();
            return new Unit();
        }



        // Utils
        private void RegisterGameServices(IGameServiceCollection gameServices)
        {
            gameServices.AddSingleton<GameViewModelsBuilder>();
            gameServices.AddSingleton<IViewModelService, ViewModelService>((gameManager, vm) => _viewModelsLocator.RegisterViewModel(gameManager, vm));
            
        }

        private void AddBot(IGameServiceCollection gameServices, PlayerColor color)
        {
            
        }
    }
}
