using FluentAssertions;
using System.Linq;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Extensions;

namespace TacticWar.Lib.Tests.Game.Map
{
    public class ContinentTests
    {
        [FactFor(nameof(Continent))]
        public void Should_HaveCorrectArmiesCount()
        {
            Continent.NorthAmerica.Armies.Should().Be(5);
            Continent.Asia.Armies.Should().Be(7);
            Continent.Africa.Armies.Should().Be(3);
            Continent.SouthAmerica.Armies.Should().Be(2);
            Continent.Australia.Armies.Should().Be(2);
            Continent.Europe.Armies.Should().Be(5);
        }


        [FactFor(nameof(Continent))]
        public void Should_HaveCorrectNumberOfContinents()
        {
            const int continentCount = 6;
            Continent.Continents.Distinct().Count().Should().Be(continentCount);
            Continent.Continents.Should().BeAllDistinct();
        }


        [FactFor(nameof(Continent))]
        public void Should_HaveContinentsAsSingleton()
        {
            var continentsA = Continent.Continents.ToList();
            var continentsB = Continent.Continents.ToList();
            continentsA.Count.Should().Be(continentsB.Count);
            foreach(var continent in continentsA)
            {
                var existsSameInB = continentsB.Any(c => c == continent);
                existsSameInB.Should().BeTrue();
            }    
        }


        [FactFor(nameof(Continent))]
        public void Should_HaveCorrectName()
        {
            Continent.NorthAmerica.Name.Should().Be("NorthAmerica");
            Continent.SouthAmerica.Name.Should().Be("SouthAmerica");
            Continent.Asia.Name.Should().Be("Asia");
            Continent.Europe.Name.Should().Be("Europe");
            Continent.Africa.Name.Should().Be("Africa");
            Continent.Australia.Name.Should().Be("Australia");

            foreach(var continent in Continent.Continents)
                continent.ToString().Should().Be(continent.Name);
        }
    }
}
