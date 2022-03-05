using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Players
{
    public class PlayerTerritory : IPlayerTerritory
    {
        public Territory Territory { get; init; }
        public int Armies { get; set; }
    }
}
