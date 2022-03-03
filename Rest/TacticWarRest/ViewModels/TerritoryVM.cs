using System;
using System.Collections.Generic;
using System.Text;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Rest.ViewModels
{
    public class TerritoryVM
    {
        public PlayerColor Color { get; set; }
        public int Id { get; set; }
        public int Armies { get; set; }
    }
}
