using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Players
{
    public class PlayersInfoCollection
    {
        // Initialization
        public PlayersInfoCollection(IEnumerable<PlayerInfo> info)
        {
            Info = info.ToList();
        }



        // Properties
        public IReadOnlyList<PlayerInfo> Info { get; }
    }
}
