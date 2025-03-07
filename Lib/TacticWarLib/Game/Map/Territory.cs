﻿namespace TacticWar.Lib.Game.Map
{
    public class Territory
    {
        // Private fields
        List<Territory> _neighbors;
        
        
        
        // Properties
        public string Name { get; init; }
        public Continent Continent { get; init; }
        public int Id { get; init; }
        public IReadOnlyList<Territory> Neighbors
        {
            get => _neighbors;
            internal set => _neighbors = _neighbors == null
                ? value.ToList()
                : throw new InvalidOperationException($"Neighbors already set");
        }



        // Public
        public override string ToString()
        {
            return Name;
        }
    }
}
