using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Tests.Attributes;

namespace TacticWar.Lib.Tests.Game.Deck
{
    public class DeckTests
    {
        [FactFor]
        public void Should_InjectCardsPassedFromConstructor()
        {
            var numbers = Enumerable.Range(0, 100);
            var deck = new Deck<int>(numbers);

            var drawnCards = new List<int>();
            foreach (var _ in numbers)
            {
                var containsACard = deck.Draw(out var card);
                containsACard.Should().BeTrue();
                drawnCards.Add(card);
            }
            drawnCards.Should().BeEquivalentTo(numbers);

            var cardDrawn = deck.Draw(out var _);
            cardDrawn.Should().BeFalse();
        }


        [FactFor(nameof(Deck<object>.AddCards))]
        public void Should_AddCardsCorrectly()
        {
            var deck = new Deck<int>();
            bool cardDrawn = deck.Draw(out var _);
            cardDrawn.Should().BeFalse();
            var cards = new[] { 1, 3, 5 };
            deck.AddCards(cards);
            var drawnCards = new List<int>();
            while (deck.Draw(out var card))
                drawnCards.Add(card);
            drawnCards.Should().BeEquivalentTo(cards);
        }


        [FactFor(nameof(Deck<object>.AddCards))]
        public void Should_ThrowWhenAddingACardAlreadyInTheDeck()
        {
            var deck = new Deck<int>();
            var cards = new[] { 1 };
            deck.AddCards(cards);
            Action addCards = () => deck.AddCards(cards);
            addCards.Should().Throw<GameException>();
        }
    }
}
