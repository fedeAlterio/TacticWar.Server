﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Extensions
{
    public static class IListExtensions 
    {
        private static Random _random = new();
        public static void Shuffle<T>(this IList<T> @this)
        {
            int n = @this.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = @this[k];
                @this[k] = @this[n];
                @this[n] = value;
            }
        }    
    }
}
