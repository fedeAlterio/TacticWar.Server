using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Deck.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Abstractions;

namespace TacticWar.Lib.Game.Players
{
    public class Player : IPlayer
    {
        // Private fields
        readonly Dictionary<Territory, PlayerTerritory> _territories = new();
        readonly List<TerritoryCard> _cards = new();



        // Properties
        public string Name { get; init; }
        public PlayerColor Color { get; init; }
        public IObjective Objective { get; init; }
        public IReadOnlyDictionary<Territory, PlayerTerritory> Territories => _territories;
        public int ArmiesCount => Territories.Sum(x => x.Value.Armies);
        public IReadOnlyList<TerritoryCard> Cards => _cards;
        IReadOnlyList<IPlayerTerritory> IPlayer.Territories => Territories.Values.ToList();
        public bool HasWon => Objective.IsCompleted(this);
        public bool IsDead => !Territories.Any();


        // Public
        public void RemoveTerritory(Territory territory)
        {
            if (!_territories.ContainsKey(territory))
                throw new GameException($"Player {Name} does not have territory {territory.Name}!");

            _territories.Remove(territory);
        }

        public void AddTerritory(Territory territory)
        {
            if (_territories.ContainsKey(territory))
                throw new GameException($"Player {Name} already has {territory.Name}");

            var playerTerritory = new PlayerTerritory
            {
                Territory = territory,
                Armies = 0
            };

            _territories.Add(territory, playerTerritory);
        }

        public void AddTerritories(IEnumerable<Territory> territories)
        {
            foreach (var territory in territories)
                AddTerritory(territory);
        }

        public bool DrawCardFrom(IDeck<TerritoryCard> deck)
        {
            var cardDrawn = deck.Draw(out var card);
            if (cardDrawn)
                _cards.Add(card);
            return cardDrawn;
        }

        public void RemoveCards(IEnumerable<TerritoryCard> cards, IDeck<TerritoryCard> deck)
        {
            var toRemoveCards = cards.ToList();
            foreach (var card in toRemoveCards)
                _cards.Remove(card);
            deck.AddCards(toRemoveCards);
        }

        public void TransferCards(IList<TerritoryCard> cards, Player player)
        {           
            cards = cards.ToList();
            var otherPlayerDoesnotHaveAnyCard = !cards.Any(player._cards.Contains);
            if (!otherPlayerDoesnotHaveAnyCard)
                throw new GameException($"Player {player.Name} already has some of the cards you want to transfer");

            var thisPlayerOwnsAllTheCards = cards.All(_cards.Contains);
            if (!thisPlayerOwnsAllTheCards)
                throw new GameException($"Player {Name} does not have all these cards");

            foreach (var card in cards)
                _cards.Remove(card);
            player._cards.AddRange(cards);
        }
    }
}
