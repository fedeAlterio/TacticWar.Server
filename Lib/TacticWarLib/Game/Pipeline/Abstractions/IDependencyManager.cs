using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Pipeline.Abstractions
{
    public interface IDependencyManager
    {
        T Get<T>();
    }
}
