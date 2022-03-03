using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Bot.Abstractions;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;

namespace TacticWar.Lib.Game.Bot
{
    public abstract class BotBase : IBot, IBotSettings
    {
        // Protected fields
        protected readonly IGameApi _gameApi;
        private readonly IGameTable _gameTable;
        protected readonly ITurnInfo _turnInfo;
        private readonly IGameTerminationController _gameTerminationController;
        private readonly Dictionary<GamePhase, Func<Task>> _commandsByPhase;
        


        // Initialization
        public BotBase(IGameApi gameApi, IGameTable gameTable, ITurnInfo turnInfo, IGameTerminationController gameTerminationController)
        {
            _gameApi = gameApi;
            _gameTable = gameTable;
            _turnInfo = turnInfo;
            _gameTerminationController = gameTerminationController;
            _commandsByPhase = BuildCommands();
        }

        private Dictionary<GamePhase, Func<Task>> BuildCommands() => new()
        {
            { GamePhase.ArmiesPlacement, Try(OnPlacementPhase) },
            { GamePhase.Attack, Try(OnAttackPhase) },
            { GamePhase.PlacementAfterAttack, Try(OnPlacementAfterAttackPhase) },
            { GamePhase.FreeMove, Try(OnFreeMovePhase) }
        };



        // Properties
        public int ThinkTimeMs { get; set; } = 400;
        public bool IsPlaying { get; private set; }

        

        // Events
        private Task OnAttackPhase()
        {
            return _gameTable.WaitingForDefence 
                ? OnDefence()
                : OnAttack();
        }



        // Public
        public async Task TryPlayOneStep()
        {
            await Task.Delay(ThinkTimeMs);

            var player = _turnInfo.CurrentTurnPlayer;
            if (_commandsByPhase.TryGetValue(_turnInfo.CurrentPhase, out var command))
            {
                await command?.Invoke()!;
            }
        }

        public async Task PlayTurn()
        {
            var currentPlayer = _turnInfo.CurrentActionPlayer;
            do
                await TryPlayOneStep();
            while (_turnInfo.CurrentActionPlayer == currentPlayer || _gameTerminationController.IsGameEnded);
        }



        // Commands        
        protected abstract Task OnPlacementPhase();
        protected abstract Task OnAttack();
        protected abstract Task OnDefence();
        protected abstract Task OnPlacementAfterAttackPhase();
        protected abstract Task OnFreeMovePhase();


        // Utils
        private Func<Task> Try(Func<Task> action) => async () =>
        {
            if (IsPlaying)
                throw new InvalidOperationException($"Bot already Playing");

            try
            {
                IsPlaying = true;
                await action?.Invoke()!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                IsPlaying = false;
            }
        };
    }
}
