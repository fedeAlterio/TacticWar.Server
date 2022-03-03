using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Tests.Attributes;
using TacticWar.Lib.Tests.Extensions;
using Xunit;

namespace TacticWar.Lib.Tests.Game.Map
{
    public class MapBuilderTests
    {
        [FactFor(nameof(MapBuilder.BuildNew))]
        public void Should_BuildAllNorthAmericaTerritories()
        {
            var map = new MapBuilder().BuildNew();
            var territories = new[]
            {
                MapBuilder.Alaska, MapBuilder.NorthWestTerritory, MapBuilder.Alberta,
                MapBuilder.Ontario, MapBuilder.Quebec, MapBuilder.Greenland,
                MapBuilder.WesternUnitedStates, MapBuilder.EasternUnitedStates, MapBuilder.CentralAmerica
            };
            territories.Should().HaveSameReferencesOf(map.NorthAmerica);
        }


        [FactFor(nameof(MapBuilder.BuildNew))]
        public void Should_BuilAllSouthAmericaTerritories()
        {
            var map = new MapBuilder().BuildNew();
            var territories = new[]
            {
                MapBuilder.Venezuela, MapBuilder.Brazil, MapBuilder.Peru,
                MapBuilder.Argentina
            };
            territories.Should().HaveSameReferencesOf(map.SouthAmerica);
        }


        [FactFor(nameof(MapBuilder.BuildNew))]
        public void Should_BuilAllEuropeTerritories()
        {
            var map = new MapBuilder().BuildNew();
            var territories = new[]
            {
                MapBuilder.Iceland, MapBuilder.GreatBritain, MapBuilder.Scandinavia,
                MapBuilder.NorthenEurope, MapBuilder.WesternEurope, MapBuilder.SouthernEurope,
                MapBuilder.Ukraine
            };
            territories.Should().HaveSameReferencesOf(map.Europe);
        }


        [FactFor(nameof(MapBuilder.BuildNew))]
        public void Should_BuilAllAfricaTerritories()
        {
            var map = new MapBuilder().BuildNew();
            var territories = new[]
            {
                MapBuilder.NorthAfrica, MapBuilder.Egypt, MapBuilder.EastAfrica,
                MapBuilder.Congo, MapBuilder.SouthAfrica, MapBuilder.Madagascar
            };
            territories.Should().HaveSameReferencesOf(map.Africa);
        }


        [FactFor(nameof(MapBuilder.BuildNew))]
        public void MapBuilder_Should_BuilAllAsiaTerritories()
        {
            var map = new MapBuilder().BuildNew();
            var territories = new[]
            {
                MapBuilder.MiddleEast, MapBuilder.Afghanistan, MapBuilder.Ural,
                MapBuilder.Siberia, MapBuilder.Yakutsk, MapBuilder.Irkutsk,
                MapBuilder.Kamchatka, MapBuilder.Mongolia, MapBuilder.Japan,
                MapBuilder.China, MapBuilder.India, MapBuilder.Siam
            };
            territories.Should().HaveSameReferencesOf(map.Asia);
        }


        [FactFor(nameof(MapBuilder.BuildNew))]
        public void Should_BuilAllAustraliaTerritories()
        {
            var map = new MapBuilder().BuildNew();
            var territories = new[]
            {
                MapBuilder.Indonesia, MapBuilder.NewGuinea, MapBuilder.WesternAustralia,
                MapBuilder.EasternAustralia
            };
            territories.Should().HaveSameReferencesOf(map.Australia);
        }



        // Neighbors
        [FactFor(nameof(MapBuilder.BuildNew))]
        public void Should_BuildNorthAmericaWithCorrectNeighbors()
        {
            MapBuilder.Alaska.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.NorthWestTerritory, MapBuilder.Alberta, MapBuilder.Kamchatka });
            MapBuilder.NorthWestTerritory.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Alaska, MapBuilder.Alberta, MapBuilder.Greenland, MapBuilder.Ontario });
            MapBuilder.Alberta.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Alaska, MapBuilder.NorthWestTerritory, MapBuilder.Ontario, MapBuilder.WesternUnitedStates });
            MapBuilder.Ontario.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.NorthWestTerritory, MapBuilder.Greenland, MapBuilder.Quebec, MapBuilder.EasternUnitedStates, MapBuilder.WesternUnitedStates, MapBuilder.Alberta });
            MapBuilder.Greenland.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.NorthWestTerritory, MapBuilder.Ontario, MapBuilder.Quebec, MapBuilder.Iceland });
            MapBuilder.Quebec.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Greenland, MapBuilder.Ontario, MapBuilder.EasternUnitedStates });
            MapBuilder.WesternUnitedStates.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Alberta, MapBuilder.Ontario, MapBuilder.EasternUnitedStates, MapBuilder.CentralAmerica });
            MapBuilder.EasternUnitedStates.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.CentralAmerica, MapBuilder.WesternUnitedStates, MapBuilder.Ontario, MapBuilder.Quebec });
            MapBuilder.CentralAmerica.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.WesternUnitedStates, MapBuilder.EasternUnitedStates, MapBuilder.Venezuela, });
        }


        [FactFor(nameof(MapBuilder.BuildNew))]
        public void Should_BuildSouthAmericaWithCorrectNeighbors()
        {
            MapBuilder.Venezuela.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.CentralAmerica, MapBuilder.Peru, MapBuilder.Brazil });
            MapBuilder.Brazil.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Venezuela, MapBuilder.Peru, MapBuilder.Argentina, MapBuilder.NorthAfrica });
            MapBuilder.Peru.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Venezuela, MapBuilder.Brazil, MapBuilder.Argentina });
            MapBuilder.Argentina.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Peru, MapBuilder.Brazil });
        }


        [FactFor(nameof(MapBuilder.BuildNew))]
        public void Should_BuilEuropeWithCorrectNeighbors()
        {
            MapBuilder.Iceland.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Greenland, MapBuilder.GreatBritain, MapBuilder.Scandinavia });
            MapBuilder.GreatBritain.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Iceland, MapBuilder.Scandinavia, MapBuilder.NorthenEurope, MapBuilder.WesternEurope });
            MapBuilder.Scandinavia.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Iceland, MapBuilder.GreatBritain, MapBuilder.NorthenEurope, MapBuilder.Ukraine });
            MapBuilder.NorthenEurope.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.GreatBritain, MapBuilder.Scandinavia, MapBuilder.Ukraine, MapBuilder.SouthernEurope, MapBuilder.WesternEurope });
            MapBuilder.WesternEurope.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.GreatBritain, MapBuilder.NorthenEurope, MapBuilder.SouthernEurope, MapBuilder.NorthAfrica });
            MapBuilder.SouthernEurope.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.WesternEurope, MapBuilder.NorthenEurope, MapBuilder.Ukraine, MapBuilder.MiddleEast, MapBuilder.Egypt, MapBuilder.NorthAfrica });
            MapBuilder.Ukraine.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Scandinavia, MapBuilder.NorthenEurope, MapBuilder.SouthernEurope, MapBuilder.MiddleEast, MapBuilder.Afghanistan, MapBuilder.Ural });
        }


        [FactFor(nameof(MapBuilder.BuildNew))]
        public void Should_BuilAfricaWithCorrectNeighbors()
        {
            MapBuilder.NorthAfrica.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Brazil, MapBuilder.WesternEurope, MapBuilder.SouthernEurope, MapBuilder.Egypt, MapBuilder.EastAfrica, MapBuilder.Congo });
            MapBuilder.Egypt.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.SouthernEurope, MapBuilder.NorthAfrica, MapBuilder.EastAfrica, MapBuilder.MiddleEast });
            MapBuilder.EastAfrica.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Egypt, MapBuilder.MiddleEast, MapBuilder.NorthAfrica, MapBuilder.Congo, MapBuilder.SouthAfrica, MapBuilder.Madagascar, });
            MapBuilder.Congo.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.NorthAfrica, MapBuilder.EastAfrica, MapBuilder.SouthAfrica });
            MapBuilder.SouthAfrica.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Congo, MapBuilder.EastAfrica, MapBuilder.Madagascar });
            MapBuilder.Madagascar.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.EastAfrica, MapBuilder.SouthAfrica });
        }


        [FactFor(nameof(MapBuilder.BuildNew))]
        public void Should_BuilAsiaWithCorrectNeighbors()
        {
            MapBuilder.MiddleEast.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.SouthernEurope, MapBuilder.Egypt, MapBuilder.EastAfrica, MapBuilder.India, MapBuilder.Afghanistan, MapBuilder.Ukraine });
            MapBuilder.Afghanistan.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Ukraine, MapBuilder.MiddleEast, MapBuilder.India, MapBuilder.China, MapBuilder.Ural });
            MapBuilder.Ural.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Siberia, MapBuilder.Ukraine, MapBuilder.Afghanistan, MapBuilder.China });
            MapBuilder.Siberia.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Ural, MapBuilder.China, MapBuilder.Mongolia, MapBuilder.Yakutsk, MapBuilder.Irkutsk });
            MapBuilder.Yakutsk.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Siberia, MapBuilder.Irkutsk, MapBuilder.Kamchatka });
            MapBuilder.Kamchatka.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Yakutsk, MapBuilder.Irkutsk, MapBuilder.Mongolia, MapBuilder.Japan, MapBuilder.Alaska });
            MapBuilder.Irkutsk.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Siberia, MapBuilder.Yakutsk, MapBuilder.Kamchatka, MapBuilder.Mongolia });
            MapBuilder.Japan.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Mongolia, MapBuilder.Kamchatka });
            MapBuilder.Mongolia.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Irkutsk, MapBuilder.Siberia, MapBuilder.China, MapBuilder.Japan, MapBuilder.Kamchatka });
            MapBuilder.China.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Mongolia, MapBuilder.Siberia, MapBuilder.Ural, MapBuilder.Afghanistan, MapBuilder.India, MapBuilder.Siam });
            MapBuilder.India.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Siam, MapBuilder.China, MapBuilder.Afghanistan, MapBuilder.MiddleEast });
            MapBuilder.Siam.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.China, MapBuilder.India, MapBuilder.Indonesia });
        }


        [FactFor(nameof(MapBuilder.BuildNew))]
        public void Should_BuildAustraliaWithCorrectNeighbors()
        {
            MapBuilder.Indonesia.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Siam, MapBuilder.NewGuinea, MapBuilder.WesternAustralia });
            MapBuilder.NewGuinea.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Indonesia, MapBuilder.EasternAustralia, MapBuilder.WesternAustralia });
            MapBuilder.WesternAustralia.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.Indonesia, MapBuilder.EasternAustralia, MapBuilder.NewGuinea });
            MapBuilder.EasternAustralia.Neighbors.Should().HaveSameReferencesOf(new[] { MapBuilder.WesternAustralia, MapBuilder.NewGuinea });
        }
    }
}
