using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Deck
{
    public class TrisManager
    {
        // Core
        public int ArmiesFromTris(IPlayer player, IEnumerable<TerritoryCard> cards)
        {
            var armies = GetArmies(cards);
            foreach (var card in cards)
                if(card.Territory != null)
                    if (player.Territories.Any(x => x.Territory == card.Territory))
                        armies += 2;
            return armies;
        }



        // Utils 
        private int GetArmies(IEnumerable<TerritoryCard> cards)
        {
            var tris = cards.ToList();
            if (tris.Count != 3)
                throw new GameException($"You should drop exactly 3 cards");

            if (cards.AllSame(card => card.ArmyType, out var type))
                return type switch
                {
                    ArmyType.Infantry => 6,
                    ArmyType.Cavalry => 8,
                    ArmyType.Artillery => 4,
                    
                    _ => throw new GameException($"Tris not valid")
                };

            var joker = cards.FirstOrDefault(card => card.ArmyType == ArmyType.Joker);                    
            if(joker != null)
            {
                var notJoker = cards.Except(new[] { joker }).ToList(); ;
                return notJoker.AllSame(card => card.ArmyType, out _)
                    ? 12
                    : throw new GameException($"Tris not valid");
            }

            if (cards.Select(x => x.ArmyType).Distinct().Count() == 3)
                return 10;

            throw new GameException($"Tris not valid");
        }
    }
}
