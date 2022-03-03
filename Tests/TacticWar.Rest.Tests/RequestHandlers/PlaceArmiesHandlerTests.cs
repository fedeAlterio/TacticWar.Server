using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.RequestsHandlers.Game;
using TacticWar.Test.TacticWar.Rest.Tests.Utils;

namespace TacticWar.Test.TacticWar.Rest.Tests.RequestHandlers
{
    public class PlaceArmiesHandlerTests
    {
        [FactFor(nameof(PlaceArmiesHandler.Handle))]
        public async Task Should_PlaceArmies()
        {
            var gameInfo = await TestGameBuilder.BuildGame(ObjectsBuilder.NewPlayersInfoCollection());
            var placeArmiesHandler = new PlaceArmiesHandler(gameInfo.RoomsManager, gameInfo.ViewModelsLocator);
            var player = gameInfo.GameManager.TurnManager.TurnInfo.CurrentTurnPlayer!;
            var territory = player.Territories.First();
            var oldArmies = territory.Armies;
            var placeArmeisRequest = new PlaceArmiesRequest
            {
                RoomId = 0,
                Placements = new List<PlacementInfo>() { new(territory.Territory.Id, 1) }
            };
            await placeArmiesHandler.Handle(placeArmeisRequest, default);

            territory.Armies.Should().Be(oldArmies + 1);
        }
    }
}
