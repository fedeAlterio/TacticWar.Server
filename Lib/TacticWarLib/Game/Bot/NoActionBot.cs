using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Lib.Game.Bot
{
    public class NoActionBot : BotBase
    {
        // Initialization
        public NoActionBot(IGameApi gameApi, IGameTable gameTable, ITurnInfo turnInfo, IGameTerminationController gameTerminationController) 
            : base(gameApi, gameTable, turnInfo, gameTerminationController)
        {
        }



        // Properties
        private PlayerColor CurrentActionPlayer => _turnInfo.CurrentActionPlayer!.Color;



        // Commands
        protected override Task OnAttack()
        {
            return _gameApi.SkipAttack(CurrentActionPlayer);
        }

        protected override Task OnDefence()
        {
            return _gameApi.Defend(CurrentActionPlayer);
        }

        protected override Task OnFreeMovePhase()
        {
            return _gameApi.SkipFreeMove(CurrentActionPlayer);
        }

        protected override Task OnPlacementAfterAttackPhase()
        {
            var currentPlayer = _turnInfo.CurrentActionPlayer;
            return _gameApi.PlaceArmiesAfterAttack(currentPlayer!.Color, 1);
        }

        protected override async Task OnPlacementPhase()
        {
            var currentPlayer = _turnInfo.CurrentActionPlayer;
            var territory = currentPlayer!.Territories.Shuffled().FirstOrDefault();
            if (territory is not null && _turnInfo.ArmiesToPlace > 0)
                await _gameApi.PlaceArmies(CurrentActionPlayer, 1, territory.Territory);
            else
                await _gameApi.SkipPlacementPhase(CurrentActionPlayer);
        }
    }
}
