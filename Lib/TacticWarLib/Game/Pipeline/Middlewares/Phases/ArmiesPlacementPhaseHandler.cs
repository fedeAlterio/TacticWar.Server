using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Data;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table;
using TacticWar.Lib.Utils;

namespace TacticWar.Lib.Game.Pipeline.Middlewares.Phases
{
    public class ArmiesPlacementPhaseHandler
    {
        // Events
        public event Action<ArmiesPlacementInfo>? ArmiesPlacementPhase;        
        public event Action? ArmiesPlacementPhaseEnded;



        // Private fields
        private readonly GameTable _gameTable;
        private readonly TurnInfo _currentTurnInfo;
        private readonly IDroppedTrisManager _trisManager;



        // Initialization
        public ArmiesPlacementPhaseHandler(GameTable gameTable, TurnInfo currentTurnInfo, IDroppedTrisManager trisManager)
        {
            _gameTable = gameTable;
            _currentTurnInfo = currentTurnInfo;
            _trisManager = trisManager;
        }



        // Properties
        public int ArmiesToPlace { get => _currentTurnInfo.ArmiesToPlace; private set => _currentTurnInfo.ArmiesToPlace = value; }        


        // Commands
        public void StartInitialPlacementPhase()
        {
            ArmiesToPlace = ArmiesPlacementInfo.GameStartArmies(_gameTable.Players.Count, _currentTurnInfo.CurrentActionPlayer!.ArmiesCount);
            if (ArmiesToPlace > 0)
                InvokeArmiesPlacementPhase();
        }

        public void PlaceArmies(PlayerColor color, int armies, Territory territory)
        {
            _gameTable.PlaceArmies(_currentTurnInfo.CurrentActionPlayer!, territory, armies);
            ArmiesToPlace -= armies;

            if (ArmiesToPlace == 0)
                SkipPlacement();
            else
                InvokeArmiesPlacementPhase();
        }

        public void StartNormalTurnPlacementPhase()
        {
            ArmiesToPlace = ArmiesPlacementInfo.NormalTurnArmies(_gameTable.Map, _currentTurnInfo.CurrentActionPlayer!);
            InvokeArmiesPlacementPhase();
        }

        public void NotifyTrisDropped(int armiesCount)
        {
            ArmiesToPlace += armiesCount;
            InvokeArmiesPlacementPhase();
        }

        public void SkipPlacementPhase(PlayerColor playerColor)
        {
            SkipPlacement();
        }


        // Events to Task
        public async Task ArmiesPlacementPhaseEndedAsync() => await this.TaskFromEvent(@this => @this.ArmiesPlacementPhaseEnded);



        // Utils
        private void SkipPlacement()
        {
            ArmiesToPlace = 0;
            ArmiesPlacementPhaseEnded?.Invoke();
        }

        private void InvokeArmiesPlacementPhase()
        {            
            var info = new ArmiesPlacementInfo { ArmiesToPlace = ArmiesToPlace, DroppedCards = _trisManager.DroppedCards };
            ArmiesPlacementPhase?.Invoke(info);
        }
    }
}
