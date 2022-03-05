using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives.Extensions
{
    public static class IPlayerExtensions 
    {
        public static bool HasConqueredContinents(this IPlayer player, GameMap map, IEnumerable<Continent> continents)
        {
            return continents.All(continent => HasConqueredContinent(player, map, continent));
        }

        public static bool HasConqueredContinent(this IPlayer player, GameMap map, Continent continent)
        {
            foreach (var territory in map.GetContinentTeritories(continent))
                if (!player.Territories.Any(x => x.Territory.Id == territory.Id))
                    return false;
            return true;
        }
    }
}
