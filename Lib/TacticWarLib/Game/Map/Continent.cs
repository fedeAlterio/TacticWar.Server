using System.Runtime.CompilerServices;

namespace TacticWar.Lib.Game.Map
{
    public class Continent
    {
        // Static
        public static Continent NorthAmerica { get; } = new() { Armies = 5 };
        public static Continent SouthAmerica { get; } = new() { Armies = 2 };
        public static Continent Africa { get; } = new() { Armies = 3 };
        public static Continent Europe { get; } = new() { Armies = 5 };
        public static Continent Asia { get; } = new() { Armies = 7 };
        public static Continent Australia { get; } = new() { Armies = 2 };
        public static IReadOnlyList<Continent> Continents => new[] { NorthAmerica, SouthAmerica, Africa, Europe, Asia, Australia };



        // Initialization
        Continent([CallerMemberName] string name = "")
        {
            Name = name;
        }


        // Properties
        public int Armies { get; init; }
        public string Name { get; init; }



        // Public 
        public override string ToString()
        {
            return Name;
        }
    }
}
