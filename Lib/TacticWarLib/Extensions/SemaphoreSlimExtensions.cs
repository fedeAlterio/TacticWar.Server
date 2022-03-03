using TacticWar.Lib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TacticWar.Lib.Extensions
{
    public static class SemaphoreSlimExtensions
    {
        public static Task<ScopedSemaphoreSlim> WaitAsyncScoped(this SemaphoreSlim @this)
        {
            return ScopedSemaphoreSlim.WaitAsync(@this);
        }
    }
}
