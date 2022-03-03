using FluentAssertions;
using FluentAssertions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Tests.Extensions
{
    public static class GenericColletionAssertionsExtensions
    {
        public static void HaveSameReferencesOf<T>(this GenericCollectionAssertions<T> thisShould, IEnumerable<T> other)
        {
            foreach(var item in thisShould.Subject)
            {
                var existsInOther = other.Any(x => ReferenceEquals(item, x));
                if (!existsInOther)
                    throw new InvalidOperationException($"{item} does not exists in {other.Select(x => $"{x}").Aggregate((acc, x) => $"{acc}, {x}")}");
                existsInOther.Should().BeTrue();
            }
            thisShould.HaveCount(other.Count());
        }

        public static void BeAllDistinct<T>(this GenericCollectionAssertions<T> thisShould)
        {
            thisShould.Subject.Count().Should().Be(thisShould.Subject.Distinct().Count());
        }
    }
}
