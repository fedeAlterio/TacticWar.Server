using System.Reactive;
using System.Reactive.Subjects;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Rest.ViewModels.Phases;
using TacticWar.Lib.Game.Table.Abstractions;
using TacticWar.Lib.Game.Core.Abstractions;

namespace TacticWar.Rest.ViewModels.Services
{
    public class GameViewModelsBuilder
    {
        // Events
        readonly ISubject<Unit> _gameUpdated = Subject.Synchronize(new Subject<Unit>());
        public IObservable<Unit> GameUpdated => _gameUpdated;



        // Private fields        
        readonly object _locker = new();
        readonly INewTurnManager _turnManager;
        readonly IGameTable _gameTable;
        readonly IGameTerminationController _gameTerminationController;
        readonly IGameUpdatesListener _gameUpdatesListener;
        readonly IIdleManager _idleManager;
        ArmiesPlacementInfoVM? _armiesPlacementInfo;
        AttackPhaseInfoVm? _attackPhaseInfo;
        PlacementAfterAttackInfoVM? _placementAfterAttackInfo;
        MovementPhaseInfoVM? _movementPhaseInfo;
        VictoryPhaseVM? _victoryPhaseInfo;



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

        void SetupEvents()
        {
            var turnManager = _turnManager;
            turnManager.ArmiesPlacementPhase += OnArmiesPlacementPhase;
            turnManager.AttackPhase += OnAttackPhase;
            turnManager.PlacementAfterAttackPhase += OnPlacementAfterAttack;
            turnManager.FreeMovePhase += OnFreeMove;
            _gameUpdatesListener.GameUpdated += OnGameUpdated;
            _gameTerminationController.Victory += OnVictory;
        }

        void OnGameUpdated(IPlayer player)
        {
            _gameUpdated.OnNext(Unit.Default);
        }




        // Events
        void OnVictory(VictoryPhaseInfo info) => Update(() =>
        {
            _victoryPhaseInfo = new(info);
            _gameUpdated.OnNext(Unit.Default);
        });

        void OnArmiesPlacementPhase(ArmiesPlacementInfo info) => Update(() => _armiesPlacementInfo = new(info));
        void OnAttackPhase(AttackPhaseInfo info) => Update(() => _attackPhaseInfo = new(info));
        void OnPlacementAfterAttack(AttackInfo info) => Update(() => _placementAfterAttackInfo = new(info));
        void OnFreeMove(FreeMovePhaseInfo info) => Update(() => _movementPhaseInfo = new());

        void Update(Action? action = null)
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
        void Reset()
        {
            _armiesPlacementInfo = default;
            _attackPhaseInfo = default;
            _movementPhaseInfo = default;
            _placementAfterAttackInfo = default;
        }
    }
}
