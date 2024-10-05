using FluentAssertions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TacticWar.Lib.Game.Builders;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;

namespace TacticWar.Test.TacticWar.Lib.Tests.Game.Builders
{
    public class GameBuilderTests
    {
        [FactFor(nameof(GameBuilder))]
        public async Task Should_CorrectlyBuildAGame()
        {
            var services =new ServiceCollection();
            services.AddFakeLogging();
            var gameBuilder = new GameBuilder(services.BuildServiceProvider());
            var playersInfoCollection = ObjectsBuilder.NewPlayersInfoCollection();
            var gameManager = await gameBuilder.NewGame(new(playersInfoCollection, 1)).StartGame();
            gameManager.Should().NotBeNull();
            gameManager.TurnManager.Should().NotBeNull();
            gameManager.GameTable.Should().NotBeNull();
            gameManager.GameApi.Should().NotBeNull();
        }
    }
}
