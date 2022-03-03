using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Bot;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Pipeline;
using TacticWar.Lib.Game.Pipeline.Middlewares;
using TacticWar.Lib.Game.Map;

namespace TacticWar.Lib.Game
{
    public class GameManager
    {
        // Events
        //public event Action<IPlayer> GameUpdated;



        //// Private fields
        //private TurnManager _turnManager;
        //private GameStatistics _gameStatistics;
        //private GameTerminationController _gameTerminationController;
        //private GameValidation _validation;
        //private IdleManager _idleManager;
        //private GameUpdatesListener _gameUpdatesListener;
        //private GameTable _gameTable;
        //private GamePipeline.GamePipeline _gamePipeline;
        //private BotManager _botManager;



        //// Initialization
        //public void Initialize(IList<(string Name, PlayerColor Color)> playersInfo)
        //{
        //    var objectives = new ObjectivesDeckBuilder(this).NewDeck();
        //    var map = new MapBuilder().BuildMap();
        //    _gameTable = new GameTable(map, playersInfo, objectives);
        //    _gamePipeline = BuildGamePipeline();
        //    _gameUpdatesListener.GameUpdated += OnPlayerAction;
        //    SetupEvents();
        //}

        //private GamePipeline.GamePipeline BuildGamePipeline()
        //{            
        //    _turnManager = new(_gameTable);
        //    _gameStatistics = new();
        //    _validation = new();
        //    _idleManager = new(idlePeriod: 100_000);
        //    _gameTerminationController = new();
        //    _gameUpdatesListener = new();


        //    // Build Pipeline
        //    var gamePipeline = GamePipeline.GamePipeline.New()
        //        .Add<ShortCircuitIfGameEnded>()
        //        .Add(_validation)
        //        .Add(_turnManager)
        //        .Add(_gameStatistics)
        //        .Add(_gameTerminationController)
        //        .Add(_idleManager)
        //        .Add(_gameUpdatesListener)                
        //        .Build();

        //    _botManager = new(gamePipeline);


        //    // Register Services
        //    gamePipeline.Register<IGameTable>(_gameTable);
        //    gamePipeline.Register<ITurnManager>(_turnManager);
        //    gamePipeline.Register<IGameStatistics>(_gameStatistics);
        //    gamePipeline.Register<IGameTerminationController>(_gameTerminationController);
        //    gamePipeline.Register<IIdleManager>(_idleManager);
        //    gamePipeline.Register<IBotManager>(_botManager);
        //    gamePipeline.Register<IGameApi>(gamePipeline);
        //    gamePipeline.Register<IGameUpdatesListener>(_gameUpdatesListener);            
        //    return gamePipeline;
        //}

        //private void SetupEvents()
        //{            
        //    _idleManager.PlayerIdle += OnPlayerIdle;
        //    _turnManager.PlacementAftrAttack += _ => NotifyGameUpdated();
        //    _gameTerminationController.GameEnded += async () => await TerminateGame();            
        //}

        //public async void Start()
        //{
        //    _gamePipeline.Initialize();
        //    _botManager.Initialize();
        //    await _gamePipeline.Start();            
        //}



        //// Properties
        //public IGameApi GameApi => _gamePipeline;
        //public ITurnManager TurnManager => _turnManager;
        //public IGameTerminationController GameTerminationController => _gameTerminationController;
        //public IGameStatistics GameStatistics => _gameStatistics;
        //public IIdleManager IdleManager => _idleManager;
        //public IBotManager BotManager => _botManager;
        //public DateTime TurnStartDate { get; private set; }



        //// Events
        //private async void OnPlayerIdle(IPlayer player)
        //{
        //    await _botManager.SkipTurn();
        //}

        //private void OnPlayerAction(IPlayer player)
        //{
        //    TurnStartDate = DateTime.Now;
        //    NotifyGameUpdated();
        //}



        //// Utils
        //private async Task TerminateGame()
        //{
        //    await _gamePipeline.TerminateGame();
        //}

        //private void NotifyGameUpdated() => GameUpdated?.Invoke(_turnManager.CurrentActionPlayer);
    }
}
