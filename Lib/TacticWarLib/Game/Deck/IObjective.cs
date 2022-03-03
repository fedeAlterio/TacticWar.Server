using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Deck
{
    public interface IObjective
    {
        public string Description { get; }
        bool IsCompleted(IPlayer player);
    }
}
