using TacticWar.Lib.Game.Core.Pipeline.Middlewares.Data;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Table;

namespace TacticWar.Lib.Game.Core.GamePhaseHandlers
{
    public class PlacementAfterAttackPhaseHandler
    {
        // Events
        public event Action? ArmiesPlaced;
        public event Action<AttackInfo>? PlacementAfterAttack;



        // Private fields
        readonly GameTable _gameTable;
        readonly TurnInfo _turnInfo;
        AttackInfo? _attackInfo;



        // Initialization
        public PlacementAfterAttackPhaseHandler(GameTable gameTable, TurnInfo turnInfo)
        {
            _gameTable = gameTable;
            _turnInfo = turnInfo;
        }



        // Core
        public void StartPlacementAfterAttackPhase(AttackInfo attackInfo)
        {
            _attackInfo = attackInfo;
            InvokePlacementAfterAttack(attackInfo);
        }



        // IGameApi
        public void PlaceArmiesAfterAttack(int armies)
        {
            _gameTable.Move(_turnInfo.CurrentTurnPlayer!, _attackInfo!.AttackFrom.Territory, _attackInfo.AttackTo.Territory, armies);
            ArmiesPlaced?.Invoke();
        }



        // Utils
        void InvokePlacementAfterAttack(AttackInfo attackInfo)
        {
            _turnInfo.WaitingForArmiesPlacementAfterAttack = false;
            PlacementAfterAttack?.Invoke(attackInfo);
        }
    }
}
