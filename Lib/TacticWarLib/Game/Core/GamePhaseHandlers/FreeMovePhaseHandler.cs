using TacticWar.Lib.Game.Core.Pipeline.Middlewares.Data;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Table;
using TacticWar.Lib.Utils;

namespace TacticWar.Lib.Game.Core.GamePhaseHandlers
{
    public class FreeMovePhaseHandler
    {
        // Events
        public event Action<FreeMovePhaseInfo>? FreeMovePhase;
        public event Action? FreeMovePhaseSkipped;



        // Private fields
        private readonly GameTable _gameTable;
        private readonly TurnInfo _turnInfo;



        // Initialization
        public FreeMovePhaseHandler(GameTable gameTable, TurnInfo turnInfo)
        {
            _gameTable = gameTable;
            _turnInfo = turnInfo;
        }



        // Core
        public void StartFreeMovePhase()
        {
            InvokeFreeMovePhase();
        }

        public void Movement(Territory from, Territory to, int armies)
        {
            _gameTable.Move(_turnInfo.CurrentActionPlayer!, from, to, armies);
            InvokeFreeMoveSkipped();
        }

        public void SkipFreeMove()
        {
            InvokeFreeMoveSkipped();
        }



        // Events to Task
        public async Task FreeMovePhaseSkippedAsync() => await this.TaskFromEvent(@this => @this.FreeMovePhaseSkipped);



        // Utils
        private void InvokeFreeMoveSkipped()
        {
            FreeMovePhaseSkipped?.Invoke();
        }

        private void InvokeFreeMovePhase()
        {
            var info = new FreeMovePhaseInfo();
            FreeMovePhase?.Invoke(info);
        }
    }
}
