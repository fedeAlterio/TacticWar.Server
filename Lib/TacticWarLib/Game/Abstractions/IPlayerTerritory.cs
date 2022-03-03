using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Map;

namespace TacticWar.Lib.Game.Abstractions
{
    public interface IPlayerTerritory
    {
        Territory Territory { get; }
        int Armies { get; }
    }
}
