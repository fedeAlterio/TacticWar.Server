using TacticWar.Lib.Utils;

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
