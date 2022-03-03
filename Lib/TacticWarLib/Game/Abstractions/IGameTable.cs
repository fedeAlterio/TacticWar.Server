using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Abstractions
{
    public interface IGameTable
    {
        GameMap Map { get; }
        IReadOnlyList<IPlayer> Players { get; }
        bool WaitingForDefence { get; }
        bool WaitingForArmiesMovement { get; }
    }
}
