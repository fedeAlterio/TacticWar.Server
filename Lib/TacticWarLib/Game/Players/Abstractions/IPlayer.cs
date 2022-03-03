using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Map;

namespace TacticWar.Lib.Game.Players.Abstractions
{
    public interface IPlayer
    {
        string Name { get; }
        PlayerColor Color { get; }
        IReadOnlyList<IPlayerTerritory> Territories { get; }
        IReadOnlyList<TerritoryCard> Cards { get; }
        IObjective Objective { get; }
        bool HasWon { get; }
        bool IsDead {get;}
    }
}
