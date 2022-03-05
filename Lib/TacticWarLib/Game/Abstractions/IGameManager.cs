using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Lib.Game.Abstractions
{
    public interface IGameManager
    {
        IGameApi GameApi { get; }
        INewTurnManager TurnManager { get; }
        IGameTable GameTable { get; }
        IServiceProvider GameServiceProvider { get; }
    }
}
