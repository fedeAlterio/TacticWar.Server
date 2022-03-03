using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.GamePhases;

namespace TacticWar.Lib.Game.Abstractions
{
    public interface IGameManager
    {
        IGameApi GameApi { get; }
        ITurnManager TurnManager { get; }
    }
}
