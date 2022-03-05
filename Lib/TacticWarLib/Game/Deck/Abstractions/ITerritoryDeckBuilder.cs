using TacticWar.Lib.Game.Map;

namespace TacticWar.Lib.Game.Deck.Abstractions
{
    public interface ITerritoryDeckBuilder
    {
        IDeck<TerritoryCard> NewDeck(GameMap map);
    }
}
