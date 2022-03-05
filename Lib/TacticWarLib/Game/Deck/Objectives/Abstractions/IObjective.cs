using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives.Abstractions
{
    public interface IObjective
    {
        public string Description { get; }
        bool IsCompleted(IPlayer player);
    }
}
