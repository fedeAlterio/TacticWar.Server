using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Utils.Timers
{
    public interface ITimer
    {
        event Action Elapsed;        
        void Start();
        void Stop();
    }
}
