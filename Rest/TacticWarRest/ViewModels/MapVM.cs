using TacticWar.Lib.Game.GamePhases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TacticWar.Lib.Game.Abstractions;

namespace TacticWar.Rest.ViewModels
{
    public class MapVM
    {
        public MapVM(IGameTable gameTable)
        {
            _territories = (from player in gameTable.Players
                           from territory in player.Territories
                           select new TerritoryVM
                           {
                               Armies = territory.Armies,
                               Color = player.Color,
                               Id = territory.Territory.Id
                           })
                           .OrderBy(x => x.Id)
                           .ToList();
        }


        // Ordered by Id

        private List<TerritoryVM> _territories;
        public List<TerritoryVM> Territories
        {
            get => _territories;
            init => _territories = value.OrderBy(x => x.Id).ToList();
        }

        public TerritoryVM TerritoryById(int id) => Territories[id - 1];
    }
}
