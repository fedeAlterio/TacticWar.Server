namespace TacticWar.Lib.Extensions
{
    public static class EnumerableExtensions
    {
        private static Random _random = new();
        public static IEnumerable<T> Cyclic<T>(this IEnumerable<T> @this)
        {
            while (true)
                foreach (var x in @this)
                    yield return x;
        }

        public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> @this)
        {
            return @this.OrderBy(x => _random.Next());
        }
        
        public static bool AllSame<T, V>(this IEnumerable<T> @this, Func<T, V> propertySelector, out V value)
        {            
            using var enumerator = @this.GetEnumerator();
            if(!enumerator.MoveNext())
            {
                value = default;
                return false;
            }

            var firsttValue = propertySelector(enumerator.Current);
            while(enumerator.MoveNext())
                if(!propertySelector(enumerator.Current).Equals(firsttValue))
                {
                    value = default;
                    return false;
                }
            value = firsttValue;
            return true;
        }

        public static IEnumerable<(T, V)> ChainedWith<T, V>(this IEnumerable<T> @this, IEnumerable<V> other)
        {
            var tEnumerator = @this.GetEnumerator();
            var vEnumerator = other.GetEnumerator();
            while (tEnumerator.MoveNext() && vEnumerator.MoveNext())
                yield return (tEnumerator.Current, vEnumerator.Current);
        }
    }
}
