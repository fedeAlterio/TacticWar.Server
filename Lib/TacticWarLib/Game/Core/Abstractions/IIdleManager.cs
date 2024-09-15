using System.Reactive;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Core.Abstractions
{
    public interface IIdleManager
    {
        public IReadOnlySet<IPlayer> IdlePlayers { get; }
        public bool IsGameIdle { get; }
        public int IdleTimeoutPeriodMs { get; }
        IObservable<Unit> GameEnded { get; }
    }
}
