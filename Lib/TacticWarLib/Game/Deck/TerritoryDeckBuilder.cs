using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck.Abstractions;

namespace TacticWar.Lib.Game.Deck
{
    public class TerritoryDeckBuilder : ITerritoryDeckBuilder
    {
        public IDeck<TerritoryCard> NewDeck(GameMap map)
        {
            var armyTypes = new[] { ArmyType.Artillery, ArmyType.Cavalry, ArmyType.Infantry }.Cyclic().GetEnumerator();
            ArmyType NextType()
            {
                armyTypes.MoveNext();
                return armyTypes.Current;
            }

            int id = 0;
            var cards = map.Territories
                .Select(territory => new TerritoryCard { ArmyType = NextType(), Territory = territory, Id = id++})
                .ToList();
            cards.Add(new TerritoryCard { ArmyType = ArmyType.Joker, Id = id++ });
            cards.Add(new TerritoryCard { ArmyType = ArmyType.Joker, Id = id++ });

            return new Deck<TerritoryCard> (cards);
        }
    }
}
