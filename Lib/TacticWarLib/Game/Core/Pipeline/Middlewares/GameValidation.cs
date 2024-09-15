using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline.Middlewares.Abstractions;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
{
    public class GameValidation : GamePipelineMiddleware
    {
        // Private fields
        readonly INewTurnManager _turnManager;
        readonly IGameTable _gameTable;
        readonly ITurnInfo _turnInfo;



        // Initialization
        public GameValidation(INewTurnManager turnManager, IGameTable gameTable)
        {
            _turnManager = turnManager;
            _gameTable = gameTable;
            _turnInfo = turnManager.TurnInfo;
        }



        // GameAPI
        public async override Task Attack(PlayerColor playerColor, Territory from, Territory to, int attackDice)
        {
            var next = Next;
            AssertIsPlayerTurn(playerColor);
            AssertIsAttackPhase();
            AssertNotWaitingAfterAttackPlacement();
            await next!();
        }

        public async override Task Defend(PlayerColor playerColor)
        {
            var next = Next;
            AssertIsPlayerTurn(playerColor);
            AssertIsAttackPhase();
            AssertNotWaitingAfterAttackPlacement();
            await next!();
        }

        public async override Task Movement(PlayerColor playerColor, Territory from, Territory to, int armies)
        {
            var next = Next;
            AssertIsPlayerTurn(playerColor);
            AssertIsMovementPhase();
            await next!();
        }

        public async override Task PlaceArmies(PlayerColor playerColor, int armies, Territory territory)
        {
            var next = Next;
            AssertIsPlayerTurn(playerColor);
            AssertIsPlacementPhase();
            var armiesToPlace = _turnManager.TurnInfo.ArmiesToPlace;
            if (armiesToPlace <= 0)
                throw new GameException($"There are no more army to place");

            if (armies > armiesToPlace)
                throw new GameException($"You don't have so many armies to place");

            await next!();
        }

        public async override Task PlaceArmiesAfterAttack(PlayerColor playerColor, int armies)
        {
            var next = Next;
            AssertIsPlayerTurn(playerColor);
            AssertIsPlacingAfterAttackPhase();
            await next!();
        }

        public async override Task PlayTris(PlayerColor playerColor, IEnumerable<TerritoryCard> cards)
        {
            var next = Next;
            AssertIsPlayerTurn(playerColor);
            AssertIsPlacementPhase();
            await next!();
        }

        public async override Task SkipAttack(PlayerColor playerColor)
        {
            var next = Next;
            AssertIsPlayerTurn(playerColor);
            AssertNotWaitingAfterAttackPlacement();
            AssertIsAttackPhase();
            AssertNotWaitingForDefence();
            await next!();
        }

        public async override Task SkipFreeMove(PlayerColor playerColor)
        {
            var next = Next;
            AssertIsPlayerTurn(playerColor);
            AssertIsMovementPhase();
            await next!();
        }

        public async override Task SkipPlacementPhase(PlayerColor playerColor)
        {
            var next = Next;
            AssertIsPlayerTurn(playerColor);
            AssertIsPlacementPhase();
            AssertZeroArmiesToPlace();
            await next!();
        }




        // Assertions
        void AssertIsPlayerTurn(PlayerColor color)
        {
            if (_turnInfo!.CurrentActionPlayer!.Color != color)
                throw new GameException($"It's not turn of {color}");
        }

        void AssertNotWaitingAfterAttackPlacement()
        {
            if (_turnInfo.WaitingForArmiesPlacementAfterAttack)
                throw new GameException($"You have to place your armies before");
        }

        void AssertNotWaitingForDefence()
        {
            if (_gameTable.WaitingForDefence)
                throw new GameException($"You need to wait the defence to roll before!");
        }

        void AssertZeroArmiesToPlace()
        {
            if (_turnInfo.ArmiesToPlace > 0)
                throw new GameException($"You still have armies to place");
        }

        void AssertIsPlacementPhase()
        {
            if (_turnInfo.CurrentPhase != GamePhase.ArmiesPlacement)
                throw new GameException($"We are not in Placement phase");
        }

        void AssertIsAttackPhase()
        {
            if (_turnInfo.CurrentPhase != GamePhase.Attack)
                throw new GameException($"We are not in Attacking phase");
        }

        void AssertIsMovementPhase()
        {
            if (_turnInfo.CurrentPhase != GamePhase.FreeMove)
                throw new GameException($"We are not in Free move phase");
        }

        void AssertIsPlacingAfterAttackPhase()
        {
            if (_turnInfo.CurrentPhase != GamePhase.PlacementAfterAttack)
                throw new GameException($"We are not in Place after attack phase!");
        }
    }
}
