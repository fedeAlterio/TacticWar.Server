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
