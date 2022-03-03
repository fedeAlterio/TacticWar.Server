using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Data;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table;

namespace TacticWar.Lib.Game.Pipeline.Middlewares.Phases
{
    public class PlacementAfterAttackPhaseHandler
    {
        // Events
        public event Action? ArmiesPlaced;
        public event Action<AttackInfo>? PlacementAfterAttack;



        // Private fields
        private readonly GameTable _gameTable;
        private readonly TurnInfo _turnInfo;
        private AttackInfo? _attackInfo;



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
            _gameTable.Move(_turnInfo.CurrentActionPlayer!, _attackInfo!.AttackFrom.Territory, _attackInfo.AttackTo.Territory, armies);
            ArmiesPlaced?.Invoke();
        }



        // Utils
        private void InvokePlacementAfterAttack(AttackInfo attackInfo)
        {
            _turnInfo.WaitingForArmiesPlacementAfterAttack = false;
            PlacementAfterAttack?.Invoke(attackInfo);
        }
    }
}
