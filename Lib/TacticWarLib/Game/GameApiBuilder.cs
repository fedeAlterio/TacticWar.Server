﻿using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline;
using TacticWar.Lib.Game.Core.Pipeline.Middlewares;

namespace TacticWar.Lib.Game
{
    public class GameApiBuilder
    {
        // Private fields
        readonly IGamePipelineBuilder _gamePipelineBuilder;



        // Initialization
        public GameApiBuilder(TurnManager turnManager, GameUpdatesListener gameUpdatesListener, GameTerminationController gameTerminationController,
                              ShortCircuitIfGameEnded shortCircuitIfGameEnded, GameStatistics gameStatistics, GameValidation gameValidation,
                              IdleManager idleManager, PipelineDelimiter pipelineDelimiter, GameLogging gameLogging)
        {
            _gamePipelineBuilder = GamePipeline.New()
                                               .Add(gameLogging)
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
