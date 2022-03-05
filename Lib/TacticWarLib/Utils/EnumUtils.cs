namespace TacticWar.Lib.Utils
{
    public static class EnumUtils
    {
        public static IEnumerable<T> GetValues<T>() where T : Enum
            => Enum.GetValues(typeof(T)).Cast<T>();
    }
}
