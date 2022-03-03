using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Bot;
using TacticWar.Lib.Game.Configuration;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Deck.Objectives.Decks;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Data;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table;

namespace TacticWar.Lib.Game.Builders
{
    public class GameBuilder : INewGameBuilder
    {
        public INewGameConfigurator NewGame(PlayersInfoCollection playersInfo)
        {
            return new GameConfigurator(playersInfo);
        }

        private class GameConfigurator : INewGameConfigurator
        {
            // Private fields
            private readonly Dictionary<Type, Action<INewGameManager, object>> _gameObserversTypes = new();
            private readonly PlayersInfoCollection _playersInfoCollection;
            private readonly IServiceCollection _services;
            private IServiceProvider? _serviceProvider;



            // IGameBuilder
            public GameConfigurator(PlayersInfoCollection playersInfo)
            {
                _playersInfoCollection = playersInfo;
                _services = CreateServiceCollection();
                AddBot();
            }

            private void AddBot()
            {
                _services.AddSingleton<IBotCreator, BotCreator>();
                AddSingleton<IBotManager, BotManager>();
            }



            // IGameConfiguration
            public void AddSingleton<T>(Action<INewGameManager, T>? onGameCreated = null) where T : class
            {
                AddSingleton<T, T>();
            }

            public void AddSingleton<T, V>(Action<INewGameManager, V>? onGameCreated = null) where V : class, T where T : class
            {
                _services.AddSingleton<T, V>();
                _gameObserversTypes[typeof(T)] = (gameManager, v) => onGameCreated?.Invoke(gameManager, (V)v);
            }

            public INewGameManager StartGame()
            {
                _serviceProvider = _services.BuildServiceProvider();
                var gameManager = _serviceProvider.GetService<INewGameManager>()!;
                NotifyObservers(gameManager);
                gameManager.GameApi.Start();

                return gameManager;
            }



            // Private
            private void NotifyObservers(INewGameManager gameManager)
            {
                foreach (var (type, onCreated) in _gameObserversTypes)
                {
                    var observer = _serviceProvider!.GetService(type) ?? throw new InvalidOperationException($"Service {type} not registered");
                    onCreated?.Invoke(gameManager, observer);
                }
            }


            // Services Registration
            private IServiceCollection CreateServiceCollection()
            {
                var services = new ServiceCollection();
                InjectGameDependencies(services);
                RegisterObservers(services);
                RegisterPipelineDependencies(services);
                InjectGameApi(services);
                InjectGameManager(services);
                return services;
            }

       

            private void InjectGameDependencies(IServiceCollection services)
            {
                var gameMap = new MapBuilder().BuildNew();
                services.AddSingleton(gameMap);
                services.AddSingleton(new TerritoryDeckBuilder().NewDeck(gameMap));
                services.AddSingleton(_playersInfoCollection!);
                services.AddSingleton(provider => new ObjectivesDeckBuilder(provider).NewDeck());
                //services.AddSingleton(provider => new AlwaysWinObjectiveDeckBuilder().NewDeck());
                services.AddSingleton<IDiceRoller, DiceRoller>();
                services.AddSingleton<IGameConfiguration, GameConfiguration>();
                AddBoth<IDroppedTrisManager, CardsManager>(services);
                AddBoth<IGameTable, GameTable>(services);
                AddBoth<ITurnInfo, TurnInfo>(services);
            }

            private void InjectGameApi(IServiceCollection services)
            {
                services.AddSingleton<GameApiBuilder>();
                services.AddSingleton<IGameApi>(provider => provider.GetService<GameApiBuilder>()!.BuildGameApi());
            }

            private void RegisterObservers(IServiceCollection services)
            {
                foreach (var (type, _) in _gameObserversTypes)
                    services.AddSingleton(type);
            }

            private void RegisterPipelineDependencies(IServiceCollection services)
            {
                AddBoth<INewTurnManager, NewTurnManager>(services);
                AddBoth<IGameUpdatesListener, GameUpdatesListener>(services);
                AddBoth<IGameTerminationController, GameTerminationController>(services);
                AddBoth<IGameStatistics, GameStatistics>(services);
                AddBoth<IIdleManager, IdleManager>(services);
                AddBoth<IPipelineDelimiter, PipelineDelimiter>(services);
                services.AddSingleton<ShortCircuitIfGameEnded>();
                services.AddSingleton<GameValidation>();

            }

            private void InjectGameManager(ServiceCollection services)
            {
                services.AddSingleton<INewGameManager, NewGameManager>();
            }


            // Utils

            private void AddBoth<T, V>(IServiceCollection services) where V : class, T where T : class
            {
                services.AddSingleton<V>();
                services.AddSingleton<T>(provider => provider.GetService<V>()!);
            }
        }
    }
}
