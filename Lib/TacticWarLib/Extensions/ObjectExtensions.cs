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
