using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares;
using TacticWar.Lib.Game.Deck.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Data;

namespace TacticWar.Lib.Game
{
    public class NewGameManager : INewGameManager
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
