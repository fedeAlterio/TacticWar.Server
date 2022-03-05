using System.ComponentModel;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Core.Abstractions
{
    public interface ITurnInfo : INotifyPropertyChanged
    {
        IPlayer? CurrentTurnPlayer { get; }
        IPlayer? CurrentActionPlayer { get; }
        GamePhase CurrentPhase { get; }
        int ArmiesToPlace { get; }
        bool WaitingForArmiesPlacementAfterAttack { get; }
    }
}
