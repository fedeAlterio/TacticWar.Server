using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.GamePhases.PhaseInfo
{
    public class ArmiesPlacementInfo
    {
        // Initialization
        public static int GameStartArmies(int totPlayers, int totArmiesOnTable)
        {            
            int maxArmies = MaxArmiesOnTableAtStart(totPlayers);
            if (totArmiesOnTable > maxArmies)
                throw new GameException($"There are too many ({maxArmies}) on the game table!");

            var armiesToPlace = Math.Min(3, maxArmies - totArmiesOnTable);
            return armiesToPlace;
        }

        public static int NormalTurnArmies (GameMap gameMap, Player player)
        {
            var armiesToPlace = player.Territories.Count / 3;
            var continents = new[] { gameMap.Africa, gameMap.Australia, gameMap.Europe, gameMap.Asia, gameMap.NorthAmerica, gameMap.SouthAmerica };
            
            void AddArmiesIfHasContinent(IEnumerable<Territory> continentTerritories)
            {
                if (HasAllContinentTerritories(continentTerritories, player))
                    armiesToPlace += continentTerritories.First().Continent.Armies;
            }
            
            foreach (var continent in continents)
                AddArmiesIfHasContinent(continent);
            return armiesToPlace;
        }


        // Properties
        public int ArmiesToPlace { get; init; }
        public IReadOnlyList<IReadOnlyList<TerritoryCard>> DroppedCards { get; init; }


        // Public
        public static int MaxArmiesOnTableAtStart(int totPlayers) => 20 + (6 - totPlayers) * 5;        


        // Utils
        private static bool HasAllContinentTerritories(IEnumerable<Territory> continentTerritories, Player player)
        {
            foreach (var territory in continentTerritories)
                if (!player.Territories.ContainsKey(territory))
                    return false;
            return true;
        }
    }
}
