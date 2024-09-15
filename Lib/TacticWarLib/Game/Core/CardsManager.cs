using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table;

namespace TacticWar.Lib.Game.Core
{
    public class CardsManager : IDroppedTrisManager
    {
        // Events
        public event Action<int>? TrisDropped;



        // Private fields
        readonly GameTable _gameTable;



        // Initialization
        public CardsManager(GameTable gameTable)
        {
            _gameTable = gameTable;
        }



        // Properties
        public List<List<TerritoryCard>> DroppedCards { get; set; } = new List<List<TerritoryCard>>();
        IReadOnlyList<IReadOnlyList<TerritoryCard>> IDroppedTrisManager.DroppedCards => DroppedCards;



        // Core
        public void PlayTris(Player player, IEnumerable<TerritoryCard> cards)
        {
            var dropped = cards.ToList();
            var armiesCount = _gameTable.PlayTris(player, dropped);
            DroppedCards.Add(dropped);
            InvokeTrisDropped(armiesCount);
        }

        public void ResetDroppedTris()
        {
            DroppedCards.Clear();
        }

        public void TransferCards(Player from, Player to)
        {
            var cards = from.Cards;
            from.TransferCards(cards.ToList(), to);
        }


        // Utils
        void InvokeTrisDropped(int armiesCount)
        {
            TrisDropped?.Invoke(armiesCount);
        }
    }
}
