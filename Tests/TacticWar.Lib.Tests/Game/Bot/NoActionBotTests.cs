using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Bot;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Game.Pipeline.TestsUtils;
using TacticWar.Test.TacticWar.Rest.Tests.Utils;

namespace TacticWar.Test.TacticWar.Lib.Tests.Game.Bot
{
    public class NoActionBotTests
    {
        [FactFor(nameof(NoActionBot))]
        public async Task Should_BeAbleToPlayAgainstThemSelf()
        {
            var gameInfo = await TestGameBuilder.BuildGame(ObjectsBuilder.NewPlayersInfoCollection());
            var serviceProvider = gameInfo.GameManager.GameServiceProvider;
            var botManager = serviceProvider.GetService<IBotManager>()!;
            botManager.Should().NotBeNull();
            var gameTable = gameInfo.GameManager.GameTable;
            var turnManager = gameInfo.GameManager.TurnManager!;            

            async Task WaitForTurns(int turnsCount)
            {
                for (var i = 0; i < turnsCount; i++)
                    await turnManager.TurnEndedAsync();
            }

            var turnsTask = WaitForTurns(20);


            // Adding all bots
            var colors = gameTable.Players.Select(p => p.Color).ToList();            
            foreach (var color in colors)
                botManager.AddBot(color, bot => bot.ThinkTimeMs = 0);

            await turnsTask;
        }
    }
}
