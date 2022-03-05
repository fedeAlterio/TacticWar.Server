using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Deck.Objectives.Abstractions;

namespace TacticWar.Lib.Game.Players.Abstractions
{
    public interface IPlayer
    {
        string Name { get; }
        PlayerColor Color { get; }
        IReadOnlyList<IPlayerTerritory> Territories { get; }
        IReadOnlyList<TerritoryCard> Cards { get; }
        IObjective Objective { get; }
        bool HasWon { get; }
        bool IsDead {get;}
    }
}
