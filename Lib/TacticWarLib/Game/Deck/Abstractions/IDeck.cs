namespace TacticWar.Lib.Game.Deck.Abstractions
{
    public interface IDeck<T>
    {
        int CardsCount { get; }
        bool Draw(out T territoryCard);
        void AddCards(IEnumerable<T> cards);
    }
}
