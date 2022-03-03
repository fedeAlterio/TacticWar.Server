using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.GamePhases.PhaseInfo
{
    public class VictoryPhaseInfo
    {
        public IPlayer? Winner { get; init; }        
    }
}
