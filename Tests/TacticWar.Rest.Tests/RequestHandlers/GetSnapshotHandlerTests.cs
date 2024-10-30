using FluentAssertions;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;
using TacticWar.Rest.RequestsHandlers.Game;
using TacticWar.Test.TacticWar.Rest.Tests.Utils;

namespace TacticWar.Test.TacticWar.Rest.Tests.RequestHandlers
{
    public class GetSnapshotHandlerTests
    {
        [FactFor(nameof(GetSnapshotHandler.Handle))]
        public async Task Should_GiveASnapshotWhenGameIsStarted()
        {
            var playersInfo = ObjectsBuilder.NewPlayersInfoCollection();
            var gameBuildInfo = await TestGameBuilder.BuildGame(playersInfo);
            var viewModelService = await gameBuildInfo.ViewModelsLocator.FromGameManager(gameBuildInfo.GameManager);
            var snapshot = await viewModelService.GetGameSnapshot(playersInfo.Info.First().Color).Take(1);
            snapshot.Should().NotBeNull();
        }
    }
}
