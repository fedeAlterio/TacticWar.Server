using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TacticWar.Lib.Utils
{
    public struct DisposableLock : IDisposable
    {
        // Private fields
        private readonly object _locker;
        private bool _lockWasTaken;


        
        // Initialization
        public DisposableLock(object locker)
        {
            _locker = locker;            
            _lockWasTaken = false;
            Monitor.Enter(_locker, ref _lockWasTaken);                
        }

        

        // Public
        public void Dispose()
        {
            if (_lockWasTaken)
                Monitor.Exit(_locker);
        }
    }
}
