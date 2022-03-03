using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TacticWar.Lib.Utils
{
    public struct ScopedSemaphoreSlim : IDisposable
    {
        private SemaphoreSlim _ss;
        
        
        public static async Task<ScopedSemaphoreSlim> WaitAsync(SemaphoreSlim ss)
        {
            await ss.WaitAsync();
            return new ScopedSemaphoreSlim { _ss = ss };
        }

        public void Dispose()
        {
            _ss.Release();
        }
    }
}
