using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Builders;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Rooms;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;
using TacticWar.Rest.Requests.Room;
using TacticWar.Rest.RequestsHandlers.Room;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Test.TacticWar.Rest.Tests.Utils
{
    public static class TestGameBuilder
    {   
        public static async Task<GameBuildInfo> BuildGame(PlayersInfoCollection playersInfo)
        {
            // Setup            
            var gameConfigurator = new GameBuilder().NewGame(playersInfo);
            INewGameManager? gameManager = null;
            var roomMock = new Mock<IRoom>();
            roomMock.Setup(x => x.BuildGame()).Returns(() => gameConfigurator);
            roomMock.Setup(x => x.StartGame()).Callback(() => gameManager = gameConfigurator.StartGame());
            roomMock.Setup(x => x.Players).Returns(playersInfo.Info.Select(x => new WaitingPlayer { Color = x.Color, Name = x.Name }).ToList());
            roomMock.Setup(x => x.GameManager).Returns(() => gameManager);

            var roomsManagerMock = new Mock<IRoomsManager>();
            roomsManagerMock.Setup(x => x.FindById(default)).Returns(() => Task.FromResult(roomMock.Object));            

            var viewModelsLocator = new ViewModelsLocator();
            var startGameHandler = new StartGameHandler(roomsManagerMock.Object, viewModelsLocator);
            var startGameRequest = new StartGameRequest();
            await startGameHandler.Handle(startGameRequest, default);
            var viewModelService = await viewModelsLocator.FromGameManager(gameManager!);

            return new(gameManager!, viewModelsLocator, roomsManagerMock.Object);
        }
    }
}
