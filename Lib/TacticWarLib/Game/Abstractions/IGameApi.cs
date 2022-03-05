using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Abstractions
{

    public interface IGameApi
    {
        Task Start();
        Task PlaceArmies(PlayerColor playerColor, int armies, Territory territory);
        Task PlayTris(PlayerColor playerColor, IEnumerable<TerritoryCard> cards);
        Task SkipPlacementPhase(PlayerColor playerColor);
        Task PlaceArmiesAfterAttack(PlayerColor playerColor, int armies);
        Task Attack(PlayerColor color, Territory from, Territory to, int attackDice);
        Task Defend(PlayerColor playerColor);
        Task SkipAttack(PlayerColor playerColor);
        Task Movement(PlayerColor playerColor, Territory from, Territory to, int armies);
        Task SkipFreeMove(PlayerColor playerColor);
        Task TerminateGame();
    }
}
