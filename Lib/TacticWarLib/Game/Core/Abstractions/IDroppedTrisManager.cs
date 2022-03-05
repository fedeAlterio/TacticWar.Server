using TacticWar.Lib.Game.Deck;

namespace TacticWar.Lib.Game.Core.Abstractions
{
    public interface IDroppedTrisManager
    {
        event Action<int>? TrisDropped;
        public IReadOnlyList<IReadOnlyList<TerritoryCard>> DroppedCards { get; }
    }
}
