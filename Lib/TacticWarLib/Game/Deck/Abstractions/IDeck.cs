using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Deck.Abstractions
{
    public interface IDeck<T>
    {
        int CardsCount { get; }
        bool Draw(out T territoryCard);
        void AddCards(IEnumerable<T> cards);
    }
}
