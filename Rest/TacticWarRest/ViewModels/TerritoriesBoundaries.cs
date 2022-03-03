using TacticWar.Lib.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TacticWar.Rest.ViewModels
{
    public class NeighborhoodInfo
    {
        public NeighborhoodInfo(GameMap map)
        {
            TerritoriesNeighbours = map.Territories.Select(territory => new TerritoryNeighbors
            {
                TerritoryId = territory.Id,
                Neighbors = territory.Neighbors.Select(x => x.Id).ToList()
            }).ToList();
        }

        public List<TerritoryNeighbors> TerritoriesNeighbours { get; init; }
    }

    public class TerritoryNeighbors
    {
        public int TerritoryId { get; init; }
        public List<int> Neighbors { get; init; }
    }
}
