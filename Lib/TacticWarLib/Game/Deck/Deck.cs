using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Deck.Abstractions;

namespace TacticWar.Lib.Game.Deck
{
    public class Deck<T> : IDeck<T>
    {
        // Private fields
        private readonly List<T> _cards = new();



        // Initiliazation
        public Deck()
        {

        }

        public Deck(IEnumerable<T> cards)
        {
            AddCards(cards);
        }



        // Properties
        public int CardsCount => _cards.Count;



        // Public
        public bool Draw(out T territoryCard)
        {
            if(!_cards.Any())
            {
                territoryCard = default;
                return false;
            }

            var card = _cards[0];
            _cards.RemoveAt(0);
            territoryCard = card;
            return true;
        }

        public void AddCards(IEnumerable<T> cards)
        {
            var toAddCards = cards.ToList();
            var cardAlreadyInDeck = toAddCards.Any(_cards.Contains);
            if (cardAlreadyInDeck)
                throw new GameException($"Some cards are already in the deck");

            _cards.AddRange(toAddCards);
            _cards.Shuffle();
        }
    }
}
