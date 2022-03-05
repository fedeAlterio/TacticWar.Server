using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Core.Abstractions
{
    public interface IGameUpdatesListener
    {
        DateTime LastUpdateDate { get; }

        event Action<IPlayer> GameUpdated;
    }
}
