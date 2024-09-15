using FluentAssertions;
using Moq;
using System;
using System.Linq;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Deck.Abstractions;
using TacticWar.Lib.Game.Deck.Builders;
using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Tests.Attributes;

namespace TacticWar.Lib.Tests.Game.Players
{
    public class PlayerTests
    {        
        // Constructor
        [FactFor]
        public void Should_CorrectlyInitializePlayer()
        {
            var player = NewPlayer();
            player.Name.Should().Be("A");
            player.Color.Should().Be(PlayerColor.Black);
            player.Objective.Should().NotBeNull();
            player.Territories.Should().BeEmpty();
            player.ArmiesCount.Should().Be(0);
            player.Cards.Should().BeEmpty();
            (player as IPlayer).Territories.Should().BeEmpty();
        }



        [FactFor(nameof(Player.AddTerritory))]
        public void Should_AddATerritory()
        {
            var player = NewPlayer();
            var territory = NewTerritory();
            player.AddTerritory(territory);
            player.Territories.Should().ContainKey(territory);
            player.Territories.Count.Should().Be(1);
            var playerTerritory = player.Territories[territory];
            playerTerritory.Territory.Should().Be(territory);
        }


        [FactFor(nameof(Player.AddTerritory))]
        public void ShouldThrow_IfPlayerAlreadyHasThatTerritory()
        {
            var player = NewPlayer();
            var territory = NewTerritory();
            player.AddTerritory(territory);
            Action addTerritory = () => player.AddTerritory(territory);
            addTerritory.Should().Throw<GameException>();
        }


        [FactFor(nameof(Player.AddTerritories))]
        public void Should_AddMultipleTerritories()
        {
            var player = NewPlayer();
            var territories = new[] { NewTerritory(), NewTerritory(), NewTerritory() };
            player.AddTerritories(territories);
            player.Territories.Count.Should().Be(territories.Length);
            foreach (var territory in territories)
            {
                player.Territories.Should().ContainKey(territory);
                var playerTerritory = player.Territories[territory];
                playerTerritory.Territory.Should().Be(territory);
            }
        }


        [FactFor(nameof(Player.AddTerritories))]
        public void AddTerritories_ShouldThrow_IfTryingToAddAterritoriesThatAlreadyHas()
        {
            var player = NewPlayer();
            var territories = new[] { NewTerritory(), NewTerritory(), NewTerritory() };
            player.AddTerritories(territories);
            territories = new[] { NewTerritory(), territories[0], NewTerritory() };
            Action addTerritories = () => player.AddTerritories(territories);
            addTerritories.Should().Throw<GameException>();
        }



        [FactFor(nameof(Player.RemoveTerritory))]
        public void Should_RemoveATerritory()
        {
            var player = NewPlayer();
            var territory = NewTerritory();
            player.AddTerritory(territory);
            player.RemoveTerritory(territory);
            player.Territories.Count.Should().Be(0);
        }


        [FactFor(nameof(Player.AddTerritories))]
        public void ShouldThrow_IfTryingToRemoveATerritoryThatDoesNotExists()
        {
            var player = NewPlayer();
            var territory = NewTerritory();
            player.AddTerritory(territory);
            Action removeTerritory = () => player.RemoveTerritory(NewTerritory());
            removeTerritory.Should().Throw<GameException>();
        }


        [FactFor(nameof(Player.DrawCardFrom))]
        public void Should_DrawATerritoryCardFromATerritoryCardDeck()
        {
            var player = NewPlayer();
            var deck = NewTerritoryDeck();
            int totCards = 0;
            while (player.DrawCardFrom(deck))
                totCards ++;
            deck.Draw(out var _).Should().BeFalse();
            player.Cards.Count.Should().Be(totCards);
        }


        [FactFor(nameof(Player.RemoveCards))]
        public void Should_RemoveCardsAndPutThemInADeck()
        {
            var player = NewPlayer();
            var deck = NewTerritoryDeck();
            var deckCardsCount = deck.CardsCount;
            player.DrawCardFrom(deck);
            player.DrawCardFrom(deck);
            player.Cards.Count.Should().Be(2);
            deck.CardsCount.Should().Be(deckCardsCount - 2);
            var cards = player.Cards;
            player.RemoveCards(cards, deck);
            player.Cards.Count.Should().Be(0);
            deck.CardsCount.Should().Be(deckCardsCount);
        }


        [FactFor(nameof(Player.TransferCards))]
        public void Player_Should_TransferCardsToAnotherPlayer()
        {
            var p1 = NewPlayer();
            var p2 = NewPlayer();
            var deck = NewTerritoryDeck();
            p1.DrawCardFrom(deck);
            p1.DrawCardFrom(deck);
            var p1Cards = p1.Cards.ToList();
            p1.TransferCards(p1Cards, p2);
            p1.Cards.Should().BeEmpty();
            p2.Cards.ToList().Should().BeEquivalentTo(p1Cards);
        }


        [FactFor(nameof(Player.TransferCards))]
        public void Should_ThrowIfTryingToTransferCardsHeDoesNotHave()
        {
            var player = new Player();
            var p1 = NewPlayer();
            var p2 = NewPlayer();

            var deck = NewTerritoryDeck();
            p1.DrawCardFrom(deck);            
            p2.DrawCardFrom(deck);
            var cards = player.Cards.Union(p2.Cards).ToList();
            Action transferCards = () => p1.TransferCards(cards, p2);
            transferCards.Should().Throw<GameException>();
        }


        [FactFor(nameof(Player.TransferCards))]
        public void Should_ThrowIfOherPlayersAlreadyHasTheCardsHeIsTryingToTransfer()
        {
            var player = new Player();
            var p1 = NewPlayer();
            var p2 = NewPlayer();

            var deck = NewTerritoryDeck();
            p1.DrawCardFrom(deck);
            p2.DrawCardFrom(deck);
            var cards = p2.Cards.ToList();

            Action transferCards = () => p1.TransferCards(cards, p2);
            transferCards.Should().Throw<GameException>();
        }



        // Utils
        IDeck<TerritoryCard> NewTerritoryDeck()
        {
            return new TerritoryDeckBuilder().NewDeck(new MapBuilder().BuildNew());
        }

        Territory NewTerritory()
        {
            var territory = new Territory
            {
                Name = new Guid().ToString(),
                Continent = Continent.Australia,
                Id = 0,
            };
            return territory;
        }

        Player NewPlayer()
        {
            var objectiveMock = new Mock<IObjective>();

            var player = new Player
            {
                Name = "A",
                Color = PlayerColor.Black,
                Objective = objectiveMock.Object,
            };
            return player;
        }
    }
}
