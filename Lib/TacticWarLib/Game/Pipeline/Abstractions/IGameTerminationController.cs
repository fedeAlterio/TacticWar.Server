using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;

namespace TacticWar.Lib.Game.Pipeline.Abstractions
{
    public interface IGameTerminationController
    {
        event Action<VictoryPhaseInfo> Victory;
        event Action GameEnded;

        public bool IsGameEnded { get; }
    }
}
