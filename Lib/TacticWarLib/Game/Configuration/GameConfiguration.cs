using TacticWar.Lib.Game.Configuration.Abstractions;

namespace TacticWar.Lib.Game.Configuration
{
    public class GameConfiguration : IGameConfiguration
    {
        public int DelayAfterTerritoryConqueredMs { get; } = 3000;
    }
}
