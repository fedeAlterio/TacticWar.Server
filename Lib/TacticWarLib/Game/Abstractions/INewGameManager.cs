using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Pipeline.Abstractions;

namespace TacticWar.Lib.Game.Abstractions
{
    public interface INewGameManager
    {
        IGameApi GameApi { get; }
        INewTurnManager TurnManager { get; }
        IGameTable GameTable { get; }
        IServiceProvider GameServiceProvider { get; }
    }
}
