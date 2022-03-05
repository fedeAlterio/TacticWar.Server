using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Lib.Game.GamePhases.Abstractions
{
    public interface ITurnManager
    {
        // Events
        event Action<ArmiesPlacementInfo> ArmiesPlacementPhase;
        event Action<AttackPhaseInfo> AttackPhase;
        event Action<MovementPhaseInfo> FreeMove;
        event Action<AttackInfo> PlacementAftrAttack;
        event Action<GamePhase> PhaseChanged;


        // Properties
        public IPlayer CurrentActionPlayer { get; }
        public IPlayer CurrentTurnPlayer { get; }
        public IGameTable GameTable { get; }
        public bool GameStarted { get; }
        public IPlayer Winner { get; }
        public int ArmiesToPlace { get; }
        public bool WaitingForArmiesPlacementAfterAttack { get; }
        public GamePhase CurrentPhase { get; }
    }
}
