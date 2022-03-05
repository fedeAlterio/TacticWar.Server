using TacticWar.Lib.Game.Map;

namespace TacticWar.Lib.Game.Deck
{
    public class TerritoryCard
    {
        public int Id { get; init; }
        public Territory Territory { get; init; }
        public ArmyType ArmyType { get; init; }
    }
}
