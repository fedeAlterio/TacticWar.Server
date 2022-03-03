using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Bot;

namespace TacticWar.Lib.Game.Abstractions
{
    public interface IBotCreator
    {
        NoActionBot NewNoActionBot();
    }
}
