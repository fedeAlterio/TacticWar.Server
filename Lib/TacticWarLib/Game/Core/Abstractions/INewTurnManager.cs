using TacticWar.Lib.Game.GamePhases.PhaseInfo;

namespace TacticWar.Lib.Game.Core.Abstractions
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
