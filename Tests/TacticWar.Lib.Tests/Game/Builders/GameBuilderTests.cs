using FluentAssertions;
using System.Threading.Tasks;
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
            var gameBuilder = new GameBuilder();
            var playersInfoCollection = ObjectsBuilder.NewPlayersInfoCollection();
            var gameManager = await gameBuilder.NewGame(playersInfoCollection).StartGame();
            gameManager.Should().NotBeNull();
            gameManager.TurnManager.Should().NotBeNull();
            gameManager.GameTable.Should().NotBeNull();
            gameManager.GameApi.Should().NotBeNull();
        }
    }
}
