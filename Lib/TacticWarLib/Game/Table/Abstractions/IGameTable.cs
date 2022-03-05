using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Table.Abstractions
{
    public interface IGameTable
    {
        GameMap Map { get; }
        IReadOnlyList<IPlayer> Players { get; }
        bool WaitingForDefence { get; }
        bool WaitingForArmiesMovement { get; }
    }
}
