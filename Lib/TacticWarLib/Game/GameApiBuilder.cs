using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Pipeline;
using TacticWar.Lib.Game.Pipeline.Middlewares;

namespace TacticWar.Lib.Game
{
    public class GameApiBuilder
    {
        // Private fields
        private readonly IGamePipelineBuilder _gamePipelineBuilder;



        // Initialization
        public GameApiBuilder(NewTurnManager turnManager, GameUpdatesListener gameUpdatesListener, GameTerminationController gameTerminationController,
                              ShortCircuitIfGameEnded shortCircuitIfGameEnded, GameStatistics gameStatistics, GameValidation gameValidation,
                              IdleManager idleManager, PipelineDelimiter pipelineDelimiter)
        {
            _gamePipelineBuilder = GamePipeline.New()
                 .Add(gameUpdatesListener)
                 .Add(pipelineDelimiter)
                 .Add(shortCircuitIfGameEnded)
                 .Add(gameValidation)
                 .Add(turnManager)
                 .Add(gameStatistics)
                 .Add(gameTerminationController)
                 .Add(idleManager)
                 ;
        }



        // Core
        public IGameApi BuildGameApi()
        {
            return _gamePipelineBuilder.Build();
        }
    }
}
