using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Bot.Abstractions
{
    public interface IBotSettings
    {
        int ThinkTimeMs { get; set; }
    }
}
