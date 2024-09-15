using TacticWar.Lib.Game.Base;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares.Data
{
    public class TurnInfo : ViewModelBase, ITurnInfo
    {
        // Private fields
        Player? _currentTurnPlayer;
        Player? _currentActionPlayer;
        GamePhase _currentPhase;
        AttackInfo? _currentAttackInfo;
        int _armiesToPlace;
        bool _waitingForArmiesPlacementAfterAttack;



        // Properties
        public Player? CurrentTurnPlayer { get => _currentTurnPlayer; set => SetProperty(ref _currentTurnPlayer, value); }
        public Player? CurrentActionPlayer { get => _currentActionPlayer; set => SetProperty(ref _currentActionPlayer, value); }
        public GamePhase CurrentPhase { get => _currentPhase; set => SetProperty(ref _currentPhase, value); }
        public int ArmiesToPlace { get => _armiesToPlace; set => SetProperty(ref _armiesToPlace, value); }
        public bool WaitingForArmiesPlacementAfterAttack { get => _waitingForArmiesPlacementAfterAttack; set => SetProperty(ref _waitingForArmiesPlacementAfterAttack, value); }


        #region Covariance on interfaces workaround
        IPlayer? ITurnInfo.CurrentTurnPlayer => CurrentTurnPlayer;
        IPlayer? ITurnInfo.CurrentActionPlayer => CurrentActionPlayer;

        #endregion
    }
}
