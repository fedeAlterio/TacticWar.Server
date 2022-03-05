using Microsoft.Extensions.DependencyInjection;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Bot;
using TacticWar.Lib.Game.Bot.Abstractions;
using TacticWar.Lib.Game.Builders.Abstractios;
using TacticWar.Lib.Game.Configuration;
using TacticWar.Lib.Game.Configuration.Abstractions;
using TacticWar.Lib.Game.Core;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline.Middlewares;
using TacticWar.Lib.Game.Core.Pipeline.Middlewares.Data;
using TacticWar.Lib.Game.Deck.Builders;
using TacticWar.Lib.Game.Deck.Objectives.Decks;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Lib.Game.Builders
{
    public class GameBuilder : IGameBuilder
    {
        public IGameConfigurator NewGame(PlayersInfoCollection playersInfo)
        {
            return new GameConfigurator(playersInfo);
        }

        private class GameConfigurator : IGameConfigurator
        {
            // Private fields
            private readonly Dictionary<Type, Action<IGameManager, object>> _gameObserversTypes = new();
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
            public void AddSingleton<T>(Action<IGameManager, T>? onGameCreated = null) where T : class
            {
                AddSingleton<T, T>();
            }

            public void AddSingleton<T, V>(Action<IGameManager, V>? onGameCreated = null) where V : class, T where T : class
            {
                _services.AddSingleton<T, V>();
                _gameObserversTypes[typeof(T)] = (gameManager, v) => onGameCreated?.Invoke(gameManager, (V)v);
            }

            public async Task<IGameManager> StartGame()
            {
                _serviceProvider = _services.BuildServiceProvider();
                var gameManager = _serviceProvider.GetService<IGameManager>()!;
                NotifyObservers(gameManager);
                await gameManager.GameApi.Start();

                return gameManager;
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
                AddBoth<INewTurnManager, TurnManager>(services);
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
                services.AddSingleton<IGameManager, NewGameManager>();
            }



            // Utils
            private void NotifyObservers(IGameManager gameManager)
            {
                foreach (var (type, onCreated) in _gameObserversTypes)
                {
                    var observer = _serviceProvider!.GetService(type) ?? throw new InvalidOperationException($"Service {type} not registered");
                    onCreated?.Invoke(gameManager, observer);
                }
            }

            private void AddBoth<T, V>(IServiceCollection services) where V : class, T where T : class
            {
                services.AddSingleton<V>();
                services.AddSingleton<T>(provider => provider.GetService<V>()!);
            }
        }
    }
}
