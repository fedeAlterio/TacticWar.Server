using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions
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
