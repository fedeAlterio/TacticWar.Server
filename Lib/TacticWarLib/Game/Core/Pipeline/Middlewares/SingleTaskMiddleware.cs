using TacticWar.Lib.Game.Core.Pipeline.Middlewares.Abstractions;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
{
    public abstract class SingleTaskMiddleware : GamePipelineMiddleware
    {
        // Game API
        public async override Task Start()
        {
            await DoAction();
        }

        public async override Task PlayTris(PlayerColor color, IEnumerable<TerritoryCard> cards)
        {
            await DoAction();
        }

        public async override Task PlaceArmies(PlayerColor color, int armies, Territory territory)
        {
            await DoAction();
        }

        public async override Task SkipPlacementPhase(PlayerColor playerColor)
        {
            await DoAction();
        }

        public async override Task PlaceArmiesAfterAttack(PlayerColor playerColor, int armies)
        {
            await DoAction();
        }

        public async override Task Attack(PlayerColor color, Territory from, Territory to, int attackDice)
        {
            await DoAction();
        }

        public async override Task Defend(PlayerColor playerColor)
        {
            await DoAction();
        }

        public async override Task SkipAttack(PlayerColor playerColor)
        {
            await DoAction();
        }

        public async override Task Movement(PlayerColor playerColor, Territory from, Territory to, int armies)
        {
            await DoAction();
        }

        public async override Task SkipFreeMove(PlayerColor playerColor)
        {
            await DoAction();
        }

        public async override Task TerminateGame()
        {
            await DoAction();
        }


        // Core        
        protected abstract Task DoAction();
    }
}
