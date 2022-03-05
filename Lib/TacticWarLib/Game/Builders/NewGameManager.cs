using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Table.Abstractions;
using TacticWar.Lib.Game.Core.Abstractions;

namespace TacticWar.Lib.Game.Builders
{
    public class NewGameManager : IGameManager
    {
        // Initialization
        public NewGameManager(IGameApi gameApi, INewTurnManager turnManager, IGameTable gameTable, IServiceProvider gameServiceProvider)
        {
            GameApi = gameApi;
            TurnManager = turnManager;
            GameTable = gameTable;
            GameServiceProvider = gameServiceProvider;
        }



        // Properties
        public IGameApi GameApi { get; }
        public INewTurnManager TurnManager { get; }
        public IGameTable GameTable { get; }
        public IServiceProvider GameServiceProvider { get; }
    }
}
