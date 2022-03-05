using TacticWar.Lib.Game.Map;

namespace TacticWar.Lib.Game.Players.Abstractions
{
    public interface IPlayerTerritory
    {
        Territory Territory { get; }
        int Armies { get; }
    }
}
