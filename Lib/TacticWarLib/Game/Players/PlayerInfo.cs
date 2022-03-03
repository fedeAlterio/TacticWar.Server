using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Players
{
    public record PlayerInfo(string Name, PlayerColor Color, bool isBot = false);
}
