using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives
{
    public class ConquerTerritoriesWithArmiesObjective : IObjective
    {
        // Private fields
        private readonly int _territoriesToConquer;
        private readonly int _armiesForTerritory;



        // Initialization
        public ConquerTerritoriesWithArmiesObjective(int territoriesToConquer, int armiesForTerritory)
        {
            _territoriesToConquer = territoriesToConquer;
            _armiesForTerritory = armiesForTerritory;
            Description = $"You have to conquer {territoriesToConquer} territories, placing at least {armiesForTerritory} armies in each of them";
        }



        // Properties
        public string Description { get; }



        // Core
        public bool IsCompleted(IPlayer player)
        {
            return player.Territories.Where(x => x.Armies >= _armiesForTerritory).Count() >= _territoriesToConquer;
        }
    }
}
