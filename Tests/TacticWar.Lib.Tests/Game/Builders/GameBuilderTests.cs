using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Builders;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;

namespace TacticWar.Test.TacticWar.Lib.Tests.Game.Builders
{
    public class GameBuilderTests
    {
        [FactFor(nameof(GameBuilder))]
        public void Should_CorrectlyBuildAGame()
        {
            var gameBuilder = new GameBuilder();
            var playersInfoCollection = ObjectsBuilder.NewPlayersInfoCollection();
            var gameManager = gameBuilder.NewGame(playersInfoCollection).StartGame();
            gameManager.Should().NotBeNull();
            gameManager.TurnManager.Should().NotBeNull();
            gameManager.GameTable.Should().NotBeNull();
            gameManager.GameApi.Should().NotBeNull();
        }
    }
}
