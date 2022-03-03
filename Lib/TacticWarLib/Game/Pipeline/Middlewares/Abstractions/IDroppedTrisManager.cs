using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions
{
    public interface IDroppedTrisManager
    {
        event Action<int>? TrisDropped;
        public IReadOnlyList<IReadOnlyList<TerritoryCard>> DroppedCards { get; }
    }
}
