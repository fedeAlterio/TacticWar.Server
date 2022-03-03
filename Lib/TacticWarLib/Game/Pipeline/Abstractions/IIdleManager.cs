using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Pipeline.Abstractions
{
    public interface IIdleManager
    {
        public IReadOnlySet<IPlayer> IdlePlayers { get; }
        public bool IsGameIdle { get; }
        public int IdleTimeoutPeriodMs { get; }
    }
}
