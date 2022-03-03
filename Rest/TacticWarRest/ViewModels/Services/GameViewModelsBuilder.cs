using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Rest.ViewModels.Phases;
using TacticWar.Lib.Game.Abstractions;

namespace TacticWar.Rest.ViewModels.Services
{
    public class GameViewModelsBuilder
    {
        // Events
        public Action? GameUpdated;



        // Private fields        
        private readonly object _locker = new();
        private readonly INewTurnManager _turnManager;
        private readonly IGameTable _gameTable;
        private readonly IGameTerminationController _gameTerminationController;
        private readonly IGameUpdatesListener _gameUpdatesListener;
        private readonly IIdleManager _idleManager;
        private ArmiesPlacementInfoVM? _armiesPlacementInfo;
        private AttackPhaseInfoVm? _attackPhaseInfo;
        private PlacementAfterAttackInfoVM? _placementAfterAttackInfo;
        private MovementPhaseInfoVM? _movementPhaseInfo;
        private VictoryPhaseVM? _victoryPhaseInfo;



        // Initialization
        public GameViewModelsBuilder(INewTurnManager turnManager, IGameTable gameTable,
            IGameTerminationController gameTerminationController, IGameUpdatesListener gameUpdatesListener,
            IIdleManager idleManager)
        {
            _turnManager = turnManager;
            _gameTable = gameTable;
            _gameTerminationController = gameTerminationController;
            _gameUpdatesListener = gameUpdatesListener;
            _idleManager = idleManager;
            SetupEvents();
        }

        private void SetupEvents()
        {
            var turnManager = _turnManager;
            turnManager.ArmiesPlacementPhase += OnArmiesPlacementPhase;
            turnManager.AttackPhase += OnAttackPhase;
            turnManager.PlacementAfterAttackPhase += OnPlacementAfterAttack;
            turnManager.FreeMovePhase += OnFreeMove;
            _gameUpdatesListener.GameUpdated += OnGameUpdated;
            _gameTerminationController.Victory += OnVictory;
        }

        private void OnGameUpdated(IPlayer player)
        {
            GameUpdated?.Invoke();
        }




        // Events
        private void OnVictory(VictoryPhaseInfo info) => Update(() =>
        {
            _victoryPhaseInfo = new(info);
            GameUpdated?.Invoke();
        });

        private void OnArmiesPlacementPhase(ArmiesPlacementInfo info) => Update(() => _armiesPlacementInfo = new(info));
        private void OnAttackPhase(AttackPhaseInfo info) => Update(() => _attackPhaseInfo = new(info));
        private void OnPlacementAfterAttack(AttackInfo info) => Update(() => _placementAfterAttackInfo = new(info));
        private void OnFreeMove(FreeMovePhaseInfo info) => Update(() => _movementPhaseInfo = new());
        private void Update(Action? action = null)
        {
            Reset();
            action?.Invoke();
        }



        // Public Methods
        public GameSnapshot GetGameSnapshot(PlayerColor playerColor)
        {
            using var _ = _locker.Lock();

            var ret = new GameSnapshot()
            {
                ArmiesPlacementInfo = _armiesPlacementInfo,
                AttackPhaseInfo = _attackPhaseInfo,
                PlacementAfterAttackInfo = _placementAfterAttackInfo,
                MovementPhaseInfo = _movementPhaseInfo,
                VictoryPhaseInfo = _victoryPhaseInfo,
                CurrentPlayerColor = _turnManager.TurnInfo.CurrentTurnPlayer!.Color,
                Players = _gameTable.Players
                        .Select(p => new PrivatePlayerVM(p))
                        .ToList(),
                Map = new(_gameTable),
                Cards = _gameTable.Players.First(p => p.Color == playerColor)
                                                     .Cards
                                                     .Select(c => new CardSnapshot(c))
                                                     .ToList(),
                CurrentPlayerTurnStartOffsetMs = (DateTime.Now - _gameUpdatesListener.LastUpdateDate).TotalMilliseconds,
                CurrentTurnLengthMs = _idleManager.IdleTimeoutPeriodMs,
                //CurrentTurnLengthMs = GameManager.IdleManager.IdleTimeoutPeriod,
            };
            return ret;
        }

        public GameGlobalInfo GetGameGlobalInfo(PlayerColor color)
        {
            using var _ = _locker.Lock();

            var player = _gameTable.Players.FirstOrDefault(x => x.Color == color)
                ?? throw new GameException($"There is no player with color {color}");
            return new(player, _gameTable.Map);
        }



        // Utils
        private void Reset()
        {
            _armiesPlacementInfo = default;
            _attackPhaseInfo = default;
            _movementPhaseInfo = default;
            _placementAfterAttackInfo = default;
        }
    }
}
