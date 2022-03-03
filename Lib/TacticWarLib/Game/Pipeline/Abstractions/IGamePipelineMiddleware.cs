using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Pipeline.Abstractions
{
    public delegate Task NextPipelineStepDelegate();
    public interface IGamePipelineMiddleware : IGameApi
    {
        NextPipelineStepDelegate? Next { get; set; }
        void Initialize();
    }
}
