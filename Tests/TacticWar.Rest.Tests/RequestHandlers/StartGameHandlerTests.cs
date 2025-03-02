using FluentAssertions;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Builders;
using TacticWar.Lib.Game.Rooms;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;
using TacticWar.Rest.Requests.Room;
using TacticWar.Rest.RequestsHandlers.Room;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Test.TacticWar.Rest.Tests.RequestHandlers
{
    public class StartGameHandlerTests
    {
        [FactFor(nameof(StartGameHandler.Handle))]
        public async Task Should_CorrectlyStartAGame()
        {
            var services = new ServiceCollection();
            services.AddFakeLogging();
            // Setup
            var playersInfo = ObjectsBuilder.NewPlayersInfoCollection();
            var gameConfigurator = new GameBuilder(services.BuildServiceProvider()).NewGame(new(playersInfo, 1));
            IGameManager? gameManager = null;
            var roomMock = new Mock<IRoom>();
            roomMock.Setup(x => x.BuildGame()).Returns(() => gameConfigurator);
            roomMock.Setup(x => x.StartGame()).Returns(async () => gameManager = await gameConfigurator.StartGame());
            roomMock.Setup(x => x.Players).Returns(playersInfo.Info.Select(x => new WaitingPlayer { Color = x.Color, Name = x.Name, IsBot = true }).ToList());

            var roomsManagerMock = new Mock<IRoomsManager>();
            roomsManagerMock.Setup(x => x.FindById(default)).Returns(() => Task.FromResult(roomMock.Object));

            var viewModelsLocator = new ViewModelsLocator();
            var startGameHandler = new StartGameHandler(roomsManagerMock.Object, viewModelsLocator);
            var startGameRequest = new StartGameRequest();
            await startGameHandler.Handle(startGameRequest, default);
            var viewModelService = await viewModelsLocator.FromGameManager(gameManager!);

            // Assertions
            viewModelService.Should().NotBeNull();
        }
    }
}
