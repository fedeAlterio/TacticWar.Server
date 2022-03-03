using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Rooms;
using TacticWar.Lib.Tests.Attributes;
using Xunit;

namespace TacticWar.Lib.Tests.Game.Players
{
    public class PlayersInfoCollectionTests
    {
        [FactFor(nameof(PlayersInfoCollection))]
        public void Should_HaveCorrectInfo()
        {
            var info = new PlayerInfo[]
            {
                new("A", PlayerColor.Red),
                new("B", PlayerColor.Green),
                new("C", PlayerColor.Blue),
                new("D", PlayerColor.Red),
            };
            var playersInfo = new PlayersInfoCollection(info);
            playersInfo.Info.Should().BeEquivalentTo(info);
        }
    }
}
