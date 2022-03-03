using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Pipeline.Abstractions
{
    public interface IGameStatistics
    {
        IReadOnlyDictionary<IPlayer, IPlayer> Kills { get; }
        IReadOnlySet<IPlayer> AlivePlayers { get; }
        IReadOnlySet<IPlayer> DeadPlayers { get; }
        bool TryGetKiller(IPlayer victim, out IPlayer killer);
        bool IsAlive(IPlayer player);
        bool IsDead(IPlayer player);
    }
}
