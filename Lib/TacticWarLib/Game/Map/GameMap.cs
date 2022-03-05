namespace TacticWar.Lib.Game.Map
{
    public class GameMap
    {
        // Private fields
        private readonly List<Territory> _territories;



        // Initialization
        internal GameMap(IEnumerable<Territory> territories)
        {
            _territories = territories.ToList();


            // Setup continents
            TerritoriesByContinent = _territories
                .GroupBy(x => x.Continent)
                .ToDictionary(g => g.Key, g => (IReadOnlyList<Territory>) g.ToList());

            foreach (var x in TerritoriesByContinent)
            {
                var xTerritories = x.Value;
                if (x.Key == Continent.Europe)
                    Europe = xTerritories;
                else if (x.Key == Continent.Australia)
                    Australia = xTerritories;
                else if (x.Key == Continent.NorthAmerica)
                    NorthAmerica = xTerritories;
                else if (x.Key == Continent.Africa)
                    Africa = xTerritories;
                else if (x.Key == Continent.Asia)
                    Asia = xTerritories;
                else if (x.Key == Continent.SouthAmerica)
                    SouthAmerica = xTerritories;
            }            
        }



        // Properties
        public IReadOnlyList<Territory> Territories => _territories;
        public IReadOnlyList<Territory> NorthAmerica { get; }
        public IReadOnlyList<Territory> SouthAmerica { get; }
        public IReadOnlyList<Territory> Africa { get; }
        public IReadOnlyList<Territory> Europe { get; }
        public IReadOnlyList<Territory> Asia { get; }
        public IReadOnlyList<Territory> Australia { get; }
        private IReadOnlyDictionary<Continent, IReadOnlyList<Territory>> TerritoriesByContinent { get; }



        // Public
        public Territory TerritoryById(int id)
        {
            return Territories.First(x => x.Id == id);
        }

        public IReadOnlyList<Territory> GetContinentTeritories(Continent continent)
        {
            return TerritoriesByContinent[continent];
        }
    }
}
