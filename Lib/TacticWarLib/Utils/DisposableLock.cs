namespace TacticWar.Lib.Utils
{
    public struct DisposableLock : IDisposable
    {
        // Private fields
        readonly object _locker;
        bool _lockWasTaken;


        
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
