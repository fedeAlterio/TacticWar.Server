using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Test.TacticWar.Rest.Tests.Utils
{
    public class GameBuildInfo
    {
        // Initialization
        public GameBuildInfo(INewGameManager gameManager, IViewModelsLocator viewModelsLocator, IRoomsManager roomsManager)
        {
            GameManager = gameManager;
            ViewModelsLocator = viewModelsLocator;
            RoomsManager = roomsManager;
        }



        // Properties
        public INewGameManager GameManager { get; }
        public IViewModelsLocator ViewModelsLocator { get; }
        public IRoomsManager RoomsManager { get; }
    }
}
