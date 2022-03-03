using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Utils;

namespace TacticWar.Lib.Extensions
{
    public static class ObjectExtensions
    {
        public static DisposableLock Lock(this object @this)
        {
            return new DisposableLock(@this);
        }
    }
}
