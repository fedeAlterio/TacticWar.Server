using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline.Abstractions;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares.Abstractions
{
    public abstract class GamePipelineMiddleware : IGameApi, IGamePipelineMiddleware
    {
        // Private fields
        NextPipelineStepDelegate? _next;



        // Properties
        public NextPipelineStepDelegate? Next { get => _next ?? (() => Task.CompletedTask); set => _next = value; }


        public virtual void Initialize() { }



        // IGameAPi
        public virtual Task Start()
        {
            return Next();
        }

        public virtual Task PlayTris(PlayerColor color, IEnumerable<TerritoryCard> cards)
        {
            return Next();
        }

        public virtual Task PlaceArmies(PlayerColor color, int armies, Territory territory)
        {
            return Next();
        }

        public virtual Task SkipPlacementPhase(PlayerColor playerColor)
        {
            return Next();
        }

        public virtual Task PlaceArmiesAfterAttack(PlayerColor playerColor, int armies)
        {
            return Next();
        }

        public virtual Task Attack(PlayerColor color, Territory from, Territory to, int attackDice)
        {
            return Next();
        }

        public virtual Task Defend(PlayerColor playerColor)
        {
            return Next();
        }

        public virtual Task SkipAttack(PlayerColor playerColor)
        {
            return Next();
        }

        public virtual Task Movement(PlayerColor playerColor, Territory from, Territory to, int armies)
        {
            return Next();
        }

        public virtual Task SkipFreeMove(PlayerColor playerColor)
        {
            return Next();
        }

        public virtual Task TerminateGame()
        {
            return Next();
        }
    }
}
