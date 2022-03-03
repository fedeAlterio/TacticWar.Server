using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;

namespace TacticWar.Lib.Game.Pipeline.Middlewares
{
    public class TrisManagerMiddleware : GamePipelineMiddleware
    {
        // Properties
        public IReadOnlyList<IReadOnlyList<TerritoryCard>>? DroppedCards { get; set; }
    }
}
