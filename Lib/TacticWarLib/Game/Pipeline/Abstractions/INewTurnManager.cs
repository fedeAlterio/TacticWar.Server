using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;

namespace TacticWar.Lib.Game.Pipeline.Abstractions
{
    public interface INewTurnManager
    {
        // Events                
        event Action? TurnStateUpdated;
        event Action<ArmiesPlacementInfo>? ArmiesPlacementPhase;
        event Action<AttackPhaseInfo>? AttackPhase;
        event Action<FreeMovePhaseInfo>? FreeMovePhase;
        event Action? StartPhaseEnded;
        event Action? GameStarted;
        event Action<AttackInfo>? PlacementAfterAttackPhase;
        event Action? TurnEnded;



        // Events to Task
        Task StartPhaseEndedAsync();
        Task TurnEndedAsync();      



        // Properties
        ITurnInfo TurnInfo { get; }
        bool IsGameStarted { get; }        
    }
}
