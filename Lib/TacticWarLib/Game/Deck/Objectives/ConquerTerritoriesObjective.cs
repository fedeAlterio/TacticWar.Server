using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives
{
    public class ConquerTerritoriesObjective : IObjective
    {
        // Private fields
        readonly int _territoriesToConquer;



        // Initialization
        public ConquerTerritoriesObjective(int territoriesToConquer)
        {
            _territoriesToConquer = territoriesToConquer;
        }



        // Properties
        public string Description { get; }



        // Core
        public bool IsCompleted(IPlayer player)
        {
            return player.Territories.Count >= _territoriesToConquer;
        }
    }
}
