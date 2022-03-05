using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Test.TacticWar.Rest.Tests.Utils
{
    public class GameBuildInfo
    {
        // Initialization
        public GameBuildInfo(IGameManager gameManager, IViewModelsLocator viewModelsLocator, IRoomsManager roomsManager)
        {
            GameManager = gameManager;
            ViewModelsLocator = viewModelsLocator;
            RoomsManager = roomsManager;
        }



        // Properties
        public IGameManager GameManager { get; }
        public IViewModelsLocator ViewModelsLocator { get; }
        public IRoomsManager RoomsManager { get; }
    }
}
