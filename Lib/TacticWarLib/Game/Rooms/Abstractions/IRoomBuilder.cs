using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Rooms;

namespace TacticWar.Lib.Game.Rooms.Abstractions
{
    public interface IRoomBuilder
    {
        public Room NewRoom();
    }
}
