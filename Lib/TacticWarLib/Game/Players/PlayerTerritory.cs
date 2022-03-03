using TacticWar.Lib.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;

namespace TacticWar.Lib.Game.Players
{
    public class PlayerTerritory : IPlayerTerritory
    {
        public Territory Territory { get; init; }
        public int Armies { get; set; }
    }
}
