using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Deck.Builders;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Tests.Attributes;

namespace TacticWar.Lib.Tests.Game.Deck
{
    public class TerritoryDeckBuilderTests
    {
        // Tests
        [FactFor(nameof(TerritoryDeckBuilder.NewDeck))]
        public void Should_BuildADeckWithAllTheArmyTypes()
        {
            var cards = BuildAllCards();

            foreach (ArmyType armyType in Enum.GetValues(typeof(ArmyType)))
                cards.Any(x => x.ArmyType == armyType).Should().BeTrue();

            var cardsByType = cards.GroupBy(c => c.ArmyType).Select(x => new { ArmyType =x.Key, Cards = x.ToList() });
            var notJokers = cardsByType.Where(x => x.ArmyType != ArmyType.Joker);
            notJokers.Min(x => x.Cards.Count).Should().BeCloseTo(notJokers.Max(x => x.Cards.Count), 1);     
        }


        [FactFor(nameof(TerritoryDeckBuilder.NewDeck))]
        public void Should_BuildADeckWithNotJokersCardsUniformByType()
        {
            var cards = BuildAllCards();
            var cardsByType = cards.GroupBy(c => c.ArmyType).Select(x => new { ArmyType = x.Key, Cards = x.ToList() });
            var notJokers = cardsByType.Where(x => x.ArmyType != ArmyType.Joker);
            notJokers.Min(x => x.Cards.Count).Should().BeCloseTo(notJokers.Max(x => x.Cards.Count), 1);
        }


        [FactFor(nameof(TerritoryDeckBuilder.NewDeck))]
        public void Should_BuildADeckWithExactly2Jokers()
        {
            var cards = BuildAllCards();
            var cardsByType = cards.GroupBy(c => c.ArmyType).Select(x => new { ArmyType = x.Key, Cards = x.ToList() });
            var jokers = cardsByType.Where(x => x.ArmyType == ArmyType.Joker).First();
            jokers.Cards.Count.Should().Be(2);
        }



        // Utils
        List<TerritoryCard> BuildAllCards()
        {
            var deckBuilder = new TerritoryDeckBuilder();
            var deck = deckBuilder.NewDeck(NewGameMap());

            // Draw All Types
            var cards = new List<TerritoryCard>();
            while (deck.Draw(out var card))
                cards.Add(card);
            return cards;
        }

        GameMap NewGameMap()
        {
            return new MapBuilder().BuildNew();
        }
    }
}
