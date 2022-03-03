using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;

namespace TacticWar.Lib.Game.Configuration
{
    public class GameConfiguration : IGameConfiguration
    {
        public int DelayAfterTerritoryConqueredMs { get; } = 3000;
    }
}
