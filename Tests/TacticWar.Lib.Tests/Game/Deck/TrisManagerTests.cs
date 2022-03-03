using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Tests.Attributes;
using Xunit;

namespace TacticWar.Lib.Tests.Game.Deck
{
    public class TrisManagerTests
    {
        // Private fields
        private int _id = 0;



        // Tests
        [TheoryFor(nameof(TrisManager.ArmiesFromTris))]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(10)]
        public void Should_ThrowIfPlayerHasNot3Cards(int totCards)
        {
            int i = 0;
            var territories = Enumerable.Range(0, totCards).Select(_ => new Territory { Name = "a" + i, Continent = Continent.Asia, Id = i++ });
            PlayTris(territories).Should().Throw<GameException>();
        }


        [TheoryFor(nameof(TrisManager.ArmiesFromTris))]
        [InlineData(ArmyType.Artillery, ArmyType.Artillery, ArmyType.Cavalry)]
        [InlineData(ArmyType.Cavalry, ArmyType.Cavalry, ArmyType.Artillery)]
        [InlineData(ArmyType.Cavalry, ArmyType.Artillery, ArmyType.Joker)]
        [InlineData(ArmyType.Joker, ArmyType.Joker, ArmyType.Artillery)]
        public void Should_ThrowOnInvalidTris(ArmyType a, ArmyType b, ArmyType c)
        {
            var cards = new[] { NewCard(a), NewCard(b), NewCard(c) };
            PlayTris(cards).Should().Throw<GameException>();
        }


        [TheoryFor(nameof(TrisManager.ArmiesFromTris))]
        [InlineData(ArmyType.Artillery, ArmyType.Artillery, ArmyType.Artillery, 4)]
        [InlineData(ArmyType.Infantry, ArmyType.Infantry, ArmyType.Infantry, 6)]
        [InlineData(ArmyType.Cavalry, ArmyType.Cavalry, ArmyType.Cavalry, 8)]
        [InlineData(ArmyType.Cavalry, ArmyType.Infantry, ArmyType.Artillery, 10)]
        [InlineData(ArmyType.Cavalry, ArmyType.Cavalry, ArmyType.Joker, 12)]
        [InlineData(ArmyType.Artillery, ArmyType.Artillery, ArmyType.Joker, 12)]
        [InlineData(ArmyType.Infantry, ArmyType.Infantry, ArmyType.Joker, 12)]
        public void Should_ReturnCorrectArmies(ArmyType a, ArmyType b, ArmyType c, int expectedArmies)
        {
            var cards = new[] { NewCard(a), NewCard(b), NewCard(c) };
            var armies = PlayTris(cards).Invoke();
            armies.Should().Be(expectedArmies);
        }


        [FactFor(nameof(TrisManager.ArmiesFromTris))]
        public void TrisManager_Should_Add2ArmiesToTrisIfPlayerHasTerritory()
        {
            var cards = new[] { NewCard(ArmyType.Artillery), NewCard(ArmyType.Infantry), NewCard(ArmyType.Cavalry) };
            var player = NewPlayer(new[] { cards[0].Territory, cards[1].Territory, new Territory { Continent = Continent.Asia, Name = "A", Id = _id++ } });
            var trisManager = new TrisManager();
            var armies = trisManager.ArmiesFromTris(player, cards);
            armies.Should().Be(14);
        }



        // Utils
        private TerritoryCard NewCard(ArmyType armyType)
        {
            var territory = new Territory { Continent = Continent.Asia, Id = _id, Name = "A" + _id++ };
            var card = new TerritoryCard { ArmyType = armyType, Territory = territory};
            return card;
        }

        private Func<int> PlayTris(IEnumerable<Territory> territories)
        {
            var player = NewPlayer(new List<Territory>());
            var trisManager = new TrisManager();
            var cards = CardsFromTerritories(territories);
            var getArmies = () => trisManager.ArmiesFromTris(player, cards);
            return getArmies;
        }

        private Func<int> PlayTris(IEnumerable<TerritoryCard> cards)
        {
            var player = NewPlayer(new List<Territory>());
            var trisManager = new TrisManager();
            var getArmies = () => trisManager.ArmiesFromTris(player, cards);
            return getArmies;
        }

        private IEnumerable<TerritoryCard> CardsFromTerritories(IEnumerable<Territory> territories)
        {
            var id = 0;
            foreach (var territory in territories)
                yield return new TerritoryCard { Id = id++, ArmyType = ArmyType.Artillery, Territory = territory };
        }

        private IPlayer NewPlayer(IEnumerable<Territory> territories)
        {
            var playerTerritories = BuildPlayerTerritoriesFrom(territories);
            return NewPlayer(playerTerritories);
        }

        private IPlayer NewPlayer(IEnumerable<IPlayerTerritory> playerTerritories)
        {
            var playerMock = new Mock<IPlayer>();
            playerMock.Setup(x => x.Territories).Returns(playerTerritories.ToList());
            return playerMock.Object;
        }

        private IEnumerable<IPlayerTerritory> BuildPlayerTerritoriesFrom(IEnumerable<Territory> territories)
        {
            foreach (var territory in territories)
            {
                var playerTerritoryMock = new Mock<IPlayerTerritory>();
                playerTerritoryMock.Setup(x => x.Territory).Returns(territory);
                playerTerritoryMock.Setup(x => x.Armies).Returns(1);
                yield return playerTerritoryMock.Object;
            }
        }
    }
}
