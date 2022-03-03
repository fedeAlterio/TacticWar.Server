using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Map;

namespace TacticWar.Lib.Game.Deck.Abstractions
{
    public interface ITerritoryDeckBuilder
    {
        IDeck<TerritoryCard> NewDeck(GameMap map);
    }
}
