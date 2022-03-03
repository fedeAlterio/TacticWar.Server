using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Bot
{
    public class NoActionBot : BotBase
    {
        // Initialization
        public NoActionBot(IGameApi gameApi, IGameTable gameTable, ITurnInfo turnInfo, IGameTerminationController gameTerminationController) 
            : base(gameApi, gameTable, turnInfo, gameTerminationController)
        {
        }


        private PlayerColor CurrentPlayer => _turnInfo.CurrentActionPlayer!.Color;


        // Commands
        protected override Task OnAttack()
        {
            return _gameApi.SkipAttack(CurrentPlayer);
        }

        protected override Task OnDefence()
        {
            return _gameApi.Defend(CurrentPlayer);
        }

        protected override Task OnFreeMovePhase()
        {
            return _gameApi.SkipFreeMove(CurrentPlayer);
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
                await _gameApi.PlaceArmies(CurrentPlayer, 1, territory.Territory);
            else
                await _gameApi.SkipPlacementPhase(CurrentPlayer);
        }
    }
}
