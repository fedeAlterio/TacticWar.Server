using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Map
{
    public class MapBuilder
    {
        // Initialization
        static MapBuilder()
        {
            InitializeNeighbors();
        }


        public GameMap BuildNew()
        {
            var territories = new List<Territory>();
            var continents = new[] { NorthAmerica(), SouthAmerica(), Europe(), Africa(), Asia(), Australia() };
            foreach (var continent in continents)
                territories.AddRange(continent);

            var gameMap = new GameMap(territories);
            return gameMap;
        }


        // Continents
        #region Continents
        private static IEnumerable<Territory> NorthAmerica() => new[]
        {
            Alaska, NorthWestTerritory, Alberta,
            Ontario, Quebec, Greenland,
            WesternUnitedStates, EasternUnitedStates, CentralAmerica
        };

        private static IEnumerable<Territory> SouthAmerica() => new[]
        {
            Venezuela, Brazil, Peru,
            Argentina
        };

        private static IEnumerable<Territory> Africa() => new[]
        {
            NorthAfrica, Egypt, EastAfrica,
            Congo, SouthAfrica, Madagascar
        };

        private static IEnumerable<Territory> Europe() => new[]
        {
            Iceland, GreatBritain, Scandinavia,
            NorthenEurope, WesternEurope, SouthernEurope,
            Ukraine
        };

        private static IEnumerable<Territory> Asia() => new[]
        {
            MiddleEast, Afghanistan, Ural,
            Siberia, Yakutsk, Irkutsk,
            Kamchatka, Mongolia, Japan,
            China, India, Siam
        };

        private static IEnumerable<Territory> Australia() => new[]
        {
            Indonesia, NewGuinea, WesternAustralia,
            EasternAustralia
        };
        #endregion


        // States
        #region North America

        public static Territory Alaska { get; } = new()
        {
            Name = "Alaska",
            Continent = Continent.NorthAmerica,
            Id = 1
        };


        public static Territory NorthWestTerritory { get; } = new()
        {
            Name = "North West Territory",
            Continent = Continent.NorthAmerica,
            Id = 6
        };


        public static Territory Alberta { get; } = new()
        {
            Name = "Alberta",
            Continent = Continent.NorthAmerica,
            Id = 2
        };


        public static Territory Ontario { get; } = new()
        {
            Name = "Ontario",
            Continent = Continent.NorthAmerica,
            Id = 7
        };


        public static Territory Greenland { get; } = new()
        {
            Name = "Greenland",
            Continent = Continent.NorthAmerica,
            Id = 5
        };

        public static Territory Quebec { get; } = new()
        {
            Name = "Quebec",
            Continent = Continent.NorthAmerica,
            Id = 8
        };


        public static Territory WesternUnitedStates { get; } = new()
        {
            Name = "Western United States",
            Continent = Continent.NorthAmerica,
            Id = 9
        };


        public static Territory EasternUnitedStates { get; } = new()
        {
            Name = "Eastern United States",
            Continent = Continent.NorthAmerica,
            Id = 4
        };


        public static Territory CentralAmerica { get; } = new()
        {
            Name = "Central America",
            Continent = Continent.NorthAmerica,
            Id = 3
        };

        #endregion

        #region South America

        public static Territory Venezuela { get; } = new()
        {
            Name = "Venezuela",
            Continent = Continent.SouthAmerica,
            Id = 13
        };


        public static Territory Brazil { get; } = new()
        {
            Name = "Brazil",
            Continent = Continent.SouthAmerica,
            Id = 11
        };

        public static Territory Peru { get; } = new()
        {
            Name = "Peru",
            Continent = Continent.SouthAmerica,
            Id = 12
        };


        public static Territory Argentina { get; } = new()
        {
            Name = "Argentina",
            Continent = Continent.SouthAmerica,
            Id = 10

        };
        #endregion

        #region Africa

        public static Territory NorthAfrica { get; } = new()
        {
            Name = "North Africa",
            Continent = Continent.Africa,
            Id = 18
        };


        public static Territory Egypt { get; } = new()
        {
            Name = "Egypt",
            Continent = Continent.Africa,
            Id = 16
        };


        public static Territory EastAfrica { get; } = new()
        {
            Name = "East Africa",
            Continent = Continent.Africa,
            Id = 15
        };


        public static Territory Congo { get; } = new()
        {
            Name = "Congo",
            Continent = Continent.Africa,
            Id = 14
        };

        public static Territory SouthAfrica { get; } = new()
        {
            Name = "South Africa",
            Continent = Continent.Africa,
            Id = 19
        };


        public static Territory Madagascar { get; } = new()
        {
            Name = "Madagascar",
            Continent = Continent.Africa,
            Id = 17
        };
        #endregion

        #region Europe

        public static Territory Iceland { get; } = new()
        {
            Name = "Iceland",
            Continent = Continent.Europe,
            Id = 21
        };


        public static Territory GreatBritain { get; } = new()
        {
            Name = "Great Britain",
            Continent = Continent.Europe,
            Id = 20
        };


        public static Territory Scandinavia { get; } = new()
        {
            Name = "Scandinavia",
            Continent = Continent.Europe,
            Id = 23
        };


        public static Territory NorthenEurope { get; } = new()
        {
            Name = "Northen Europe",
            Continent = Continent.Europe,
            Id = 22
        };


        public static Territory WesternEurope { get; } = new()
        {
            Name = "Western Europe",
            Continent = Continent.Europe,
            Id = 26
        };


        public static Territory SouthernEurope { get; } = new()
        {
            Name = "Southern Europe",
            Continent = Continent.Europe,
            Id = 24

        };


        public static Territory Ukraine { get; } = new()
        {
            Name = "Ukraine",
            Continent = Continent.Europe,
            Id = 25
        };
        #endregion

        #region Asia

        public static Territory MiddleEast { get; } = new()
        {
            Name = "Middle East",
            Continent = Continent.Asia,
            Id = 33
        };


        public static Territory Afghanistan { get; } = new()
        {
            Name = "Afghanistan",
            Continent = Continent.Asia,
            Id = 27
        };


        public static Territory Ural { get; } = new()
        {
            Name = "Ural",
            Continent = Continent.Asia,
            Id = 37
        };


        public static Territory Siberia { get; } = new()
        {
            Name = "Siberia",
            Continent = Continent.Asia,
            Id = 36
        };


        public static Territory Yakutsk { get; } = new()
        {
            Name = "Yakutsk",
            Continent = Continent.Asia,
            Id = 38
        };


        public static Territory Kamchatka { get; } = new()
        {
            Name = "Kamchatka",
            Continent = Continent.Asia,
            Id = 32
        };


        public static Territory Irkutsk { get; } = new()
        {
            Name = "Irkutsk",
            Continent = Continent.Asia,
            Id = 30
        };


        public static Territory Japan { get; } = new()
        {
            Name = "Japan",
            Continent = Continent.Asia,
            Id = 31
        };


        public static Territory Mongolia { get; } = new()
        {
            Name = "Mongolia",
            Continent = Continent.Asia,
            Id = 34
        };


        public static Territory China { get; } = new()
        {
            Name = "China",
            Continent = Continent.Asia,
            Id = 28
        };


        public static Territory India { get; } = new()
        {
            Name = "India",
            Continent = Continent.Asia,
            Id = 29
        };


        public static Territory Siam { get; } = new()
        {
            Name = "Siam",
            Continent = Continent.Asia,
            Id = 35
        };
        #endregion

        #region Australia
        public static Territory Indonesia { get; } = new()
        {
            Name = "Indonesia",
            Continent = Continent.Australia,
            Id = 40
        };


        public static Territory NewGuinea { get; } = new()
        {
            Name = "New Guinea",
            Continent = Continent.Australia,
            Id = 41
        };


        public static Territory WesternAustralia { get; } = new()
        {
            Name = "Western Australia",
            Continent = Continent.Australia,
            Id = 42
        };


        public static Territory EasternAustralia { get; } = new()
        {
            Name = "Eastern Australia",
            Continent = Continent.Australia,
            Id = 39
        };
        #endregion


        // Utils
        private static void InitializeNeighbors()
        {
            // North America
            Alaska.Neighbors = new List<Territory>
            {
                NorthWestTerritory, Alberta,
                Kamchatka
            };

            NorthWestTerritory.Neighbors = new List<Territory>
            {
                Alaska, Alberta, Greenland,
                Ontario
            };

            Alberta.Neighbors = new List<Territory>
            {
                Alaska, NorthWestTerritory, Ontario,
                WesternUnitedStates
            };

            Ontario.Neighbors = new List<Territory>
            {
                NorthWestTerritory, Greenland, Quebec,
                EasternUnitedStates, WesternUnitedStates,
                Alberta
            };

            Greenland.Neighbors = new List<Territory>
            {
                NorthWestTerritory, Ontario, Quebec,
                Iceland
            };

            Quebec.Neighbors = new List<Territory>
            {
                Greenland, Ontario, EasternUnitedStates
            };

            WesternUnitedStates.Neighbors = new List<Territory>
            {
                Alberta, Ontario, EasternUnitedStates,
                CentralAmerica
            };

            EasternUnitedStates.Neighbors = new List<Territory>
            {
                CentralAmerica, WesternUnitedStates, Ontario,
                Quebec
            };

            CentralAmerica.Neighbors = new List<Territory>
            {
                WesternUnitedStates, EasternUnitedStates, Venezuela,
            };


            // South America
            Venezuela.Neighbors = new List<Territory>
            {
                CentralAmerica, Peru, Brazil,
            };

            Brazil.Neighbors = new List<Territory>
            {
                Venezuela, Peru, Argentina,
                NorthAfrica
            };

            Peru.Neighbors = new List<Territory>
            {
                Venezuela, Brazil, Argentina,
            };

            Argentina.Neighbors = new List<Territory>
            {
                Peru, Brazil,
            };

            // Africa
            NorthAfrica.Neighbors = new List<Territory>
            {
                Brazil, WesternEurope, SouthernEurope,
                Egypt, EastAfrica, Congo
            };

            Egypt.Neighbors = new List<Territory>
            {
                SouthernEurope, NorthAfrica, EastAfrica,
                MiddleEast
            };

            EastAfrica.Neighbors = new List<Territory>
            {
                Egypt, MiddleEast, NorthAfrica,
                Congo, SouthAfrica, Madagascar
            };

            Congo.Neighbors = new List<Territory>
            {
                NorthAfrica, EastAfrica, SouthAfrica,
            };

            SouthAfrica.Neighbors = new List<Territory>
            {
                Congo, EastAfrica, Madagascar,
            };

            Madagascar.Neighbors = new List<Territory>
            {
                EastAfrica, SouthAfrica,
            };


            // Europe
            Iceland.Neighbors = new List<Territory>
            {
                Greenland, GreatBritain, Scandinavia,
            };

            GreatBritain.Neighbors = new List<Territory>
            {
                Iceland, Scandinavia, NorthenEurope,
                WesternEurope
            };

            Scandinavia.Neighbors = new List<Territory>
            {
                Iceland, GreatBritain, NorthenEurope,
                Ukraine
            };

            NorthenEurope.Neighbors = new List<Territory>
            {
                GreatBritain, Scandinavia, Ukraine,
                SouthernEurope, WesternEurope
            };

            WesternEurope.Neighbors = new List<Territory>
            {
                GreatBritain, NorthenEurope, SouthernEurope,
                NorthAfrica
            };

            SouthernEurope.Neighbors = new List<Territory>
            {
                WesternEurope, NorthenEurope, Ukraine,
                MiddleEast, Egypt, NorthAfrica
            };

            Ukraine.Neighbors = new List<Territory>
            {
                Scandinavia, NorthenEurope, SouthernEurope,
                MiddleEast, Afghanistan, Ural
            };


            // Asia
            MiddleEast.Neighbors = new List<Territory>
            {
                SouthernEurope, Egypt, EastAfrica,
                India, Afghanistan, Ukraine
            };

            Afghanistan.Neighbors = new List<Territory>
            {
                Ukraine, MiddleEast, India,
                China, Ural
            };

            Ural.Neighbors = new List<Territory>
            {
                Siberia, Ukraine, Afghanistan, China
            };

            Siberia.Neighbors = new List<Territory>
            {
                Ural, China, Mongolia, Yakutsk,
                Irkutsk
            };

            Yakutsk.Neighbors = new List<Territory>
            {
                Siberia, Irkutsk, Kamchatka
            };

            Kamchatka.Neighbors = new List<Territory>
            {
                Yakutsk, Irkutsk, Mongolia, Japan,
                Alaska
            };

            Irkutsk.Neighbors = new List<Territory>
            {
                Siberia, Yakutsk, Kamchatka,
                Mongolia,
            };

            Japan.Neighbors = new List<Territory>
            {
                Mongolia, Kamchatka
            };

            Mongolia.Neighbors = new List<Territory>
            {
                Irkutsk, Siberia, China,
                Japan, Kamchatka
            };

            China.Neighbors = new List<Territory>
            {
                Mongolia, Siberia, Ural,
                Afghanistan, India, Siam
            };

            India.Neighbors = new List<Territory>
            {
                Siam, China, Afghanistan,
                MiddleEast,
            };

            Siam.Neighbors = new List<Territory>
            {
                China, India, Indonesia
            };


            // Australia
            Indonesia.Neighbors = new List<Territory>
            {
                Siam, NewGuinea, WesternAustralia
            };

            NewGuinea.Neighbors = new List<Territory>
            {
                Indonesia, EasternAustralia, WesternAustralia
            };

            WesternAustralia.Neighbors = new List<Territory>
            {
                Indonesia, EasternAustralia, NewGuinea
            };

            EasternAustralia.Neighbors = new List<Territory>
            {
                WesternAustralia, NewGuinea
            };
        }
    }
}
