using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Configuration.Abstractions;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Core.GamePhaseHandlers;
using TacticWar.Lib.Game.Core.Pipeline.Middlewares.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline.Middlewares.Data;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Table;
using TacticWar.Lib.Utils;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
{
    public class TurnManager : GamePipelineMiddleware, INewTurnManager
    {
        // Events
        public event Action? TurnStateUpdated;
        public event Action<ArmiesPlacementInfo>? ArmiesPlacementPhase
        {
            add => _placementPhaseHandler.ArmiesPlacementPhase += value;
            remove => _placementPhaseHandler.ArmiesPlacementPhase -= value;
        }
        public event Action<AttackPhaseInfo>? AttackPhase
        {
            add => _attackPhaseHandler.AttackPhase += value;
            remove => _attackPhaseHandler.AttackPhase -= value;
        }
        public event Action<FreeMovePhaseInfo>? FreeMovePhase
        {
            add => _freeMovePhaseHandler.FreeMovePhase += value;
            remove => _freeMovePhaseHandler.FreeMovePhase -= value;
        }
        public event Action<AttackInfo>? PlacementAfterAttackPhase
        {
            add => _placementAfterAttackPhaseHandler.PlacementAfterAttack += value;
            remove => _placementAfterAttackPhaseHandler.PlacementAfterAttack -= value;
        }
        public event Action<AttackInfo>? PlayerDefeated
        {
            add => _attackPhaseHandler.PlayerDefeated += value;
            remove => _attackPhaseHandler.PlayerDefeated -= value;
        }

        public event Action? StartPhaseEnded;
        public event Action? TurnEnded;
        public event Action? GameStarted;



        // Private fields
        readonly ArmiesPlacementPhaseHandler _placementPhaseHandler;
        readonly AttackPhaseHandler _attackPhaseHandler;
        readonly FreeMovePhaseHandler _freeMovePhaseHandler;
        readonly PlacementAfterAttackPhaseHandler _placementAfterAttackPhaseHandler;
        readonly CardsManager _cardsManager;
        readonly IGameConfiguration _gameConfiguration;
        readonly TurnInfo _turnInfo;
        readonly IEnumerator<Player> _turnOrdererdPlayers;
        Task _gameTask = Task.CompletedTask;
        Player? _playerWhoConqueredATerritoryLastTurn;



        // Initialization
        public TurnManager(GameTable gameTable, CardsManager trisManager, IGameConfiguration gameConfiguration, TurnInfo turnInfo)
        {
            GameTable = gameTable;
            _cardsManager = trisManager;
            _gameConfiguration = gameConfiguration;

            // Players
            _turnOrdererdPlayers = gameTable.Players.Cyclic().GetEnumerator();
            _turnOrdererdPlayers.MoveNext();

            // Turn info
            _turnInfo = turnInfo;
            BuildStartTurnInfo(turnInfo, _turnOrdererdPlayers.Current);

            // Phases Handlers
            _placementPhaseHandler = BuildNewArmiesPlacementPhaseHandler(gameTable, _turnInfo, trisManager);
            _attackPhaseHandler = BuildNewAttackPhaseHandler(gameTable, _turnInfo, gameConfiguration);
            _freeMovePhaseHandler = BuildNewFreeMovePhaseHandler(gameTable, _turnInfo);
            _placementAfterAttackPhaseHandler = BuildNewPlacementAfterAttackPhaseHandler(gameTable, _turnInfo);
            SetupDroppedTrisManager(trisManager);
            SetupEvents();
        }

        void SetupDroppedTrisManager(IDroppedTrisManager droppedTrisManager)
        {
            droppedTrisManager.TrisDropped += armiesCount => OnTrisDropped?.Invoke(armiesCount);
        }

        PlacementAfterAttackPhaseHandler BuildNewPlacementAfterAttackPhaseHandler(GameTable gameTable, TurnInfo turnInfo)
        {
            var ret = new PlacementAfterAttackPhaseHandler(gameTable, turnInfo);
            ret.PlacementAfterAttack += _ => turnInfo.CurrentPhase = GamePhase.PlacementAfterAttack;
            ret.ArmiesPlaced += () => _attackPhaseHandler.StartAttackPhase();
            return ret;
        }

        ArmiesPlacementPhaseHandler BuildNewArmiesPlacementPhaseHandler(GameTable gameTable, TurnInfo turnInfo, IDroppedTrisManager trisManager)
        {
            var ret = new ArmiesPlacementPhaseHandler(gameTable, turnInfo, trisManager);
            ret.ArmiesPlacementPhase += _ => turnInfo.CurrentPhase = GamePhase.ArmiesPlacement;
            return ret;
        }

        AttackPhaseHandler BuildNewAttackPhaseHandler(GameTable gameTable, TurnInfo turnInfo, IGameConfiguration gameConfiguration)
        {
            var ret = new AttackPhaseHandler(gameTable, turnInfo, gameConfiguration);
            ret.AttackPhase += _ => turnInfo.CurrentPhase = GamePhase.Attack;
            ret.TerritoryConquered += attackInfo =>
            {
                _playerWhoConqueredATerritoryLastTurn = _turnInfo.CurrentTurnPlayer;
                _placementAfterAttackPhaseHandler.StartPlacementAfterAttackPhase(attackInfo);
            };
            ret.PlayerDefeated += attackInfo => _cardsManager.TransferCards(attackInfo.Defender, attackInfo.Attacker);
            return ret;
        }

        FreeMovePhaseHandler BuildNewFreeMovePhaseHandler(GameTable gameTable, TurnInfo turnInfo)
        {
            var ret = new FreeMovePhaseHandler(gameTable, turnInfo);
            ret.FreeMovePhase += _ => turnInfo.CurrentPhase = GamePhase.FreeMove;
            return ret;
        }

        void SetupEvents()
        {
            TurnEnded += () => TurnNumber++;
            _placementPhaseHandler.ArmiesPlacementPhaseEnded += () => OnArmiesPlacementPhaseEnded?.Invoke();
            _attackPhaseHandler.AttackSkipped += () => OnAttackPhaseEnded?.Invoke();
            _freeMovePhaseHandler.FreeMovePhaseSkipped += () => OnFreeMovePhaseEnded?.Invoke();
        }

        void BuildStartTurnInfo(TurnInfo turnInfo, Player startPlayer)
        {
            turnInfo.CurrentTurnPlayer = turnInfo.CurrentActionPlayer = startPlayer;
            turnInfo.CurrentPhase = GamePhase.ArmiesPlacement;
        }



        // Properties
        public ITurnInfo TurnInfo => _turnInfo;
        public bool IsStartPhaseEnded { get; private set; }
        public bool IsGameStarted { get; private set; }
        public GameTable GameTable { get; }
        public int TurnNumber { get; private set; }



        // Turn Logic
        void PrepareStartPhase()
        {
            var startArmies = ArmiesPlacementInfo.MaxArmiesOnTableAtStart(GameTable.Players.Count);

            // Event Handlers setup
            void StartInitialPlacementPhase()
            {
                if (_turnInfo.CurrentActionPlayer!.ArmiesCount < startArmies)
                {
                    _placementPhaseHandler.StartInitialPlacementPhase();
                    InvokeTurnStateUpdated();
                }
                else
                {
                    InvokeStartPhaseEnded();
                    PreparePlayerTurns();
                }
            }

            void OnArmiesPlacementPhaseEnded()
            {
                MoveToNextPlayer();
                StartInitialPlacementPhase();
            }

            this.OnArmiesPlacementPhaseEnded = OnArmiesPlacementPhaseEnded;

            // Start
            GameStarted?.Invoke();
            StartInitialPlacementPhase();
        }

        void PreparePlayerTurns()
        {
            // Event Handlers setup
            void StartArmiesPlacementPhase()
            {
                if (_playerWhoConqueredATerritoryLastTurn is not null)
                {
                    _playerWhoConqueredATerritoryLastTurn.DrawCardFrom(GameTable.Deck);
                    _playerWhoConqueredATerritoryLastTurn = null;
                    _cardsManager.ResetDroppedTris();
                }

                _placementPhaseHandler.StartNormalTurnPlacementPhase();
                InvokeTurnStateUpdated();
            }

            void OnArmiesPlacementPhaseEnded()
            {
                _attackPhaseHandler.StartAttackPhase();
                InvokeTurnStateUpdated();
            }

            void OnAttackPhaseEnded()
            {
                _freeMovePhaseHandler.StartFreeMovePhase();
                InvokeTurnStateUpdated();
            }

            void OnFreeMovePhaseEnded()
            {
                TurnEnded?.Invoke();
                MoveToNextPlayer();
                StartArmiesPlacementPhase();
            }

            void OnTrisDropped(int armiesCount)
            {
                _placementPhaseHandler.NotifyTrisDropped(armiesCount);
                InvokeTurnStateUpdated();
            }

            this.OnArmiesPlacementPhaseEnded = OnArmiesPlacementPhaseEnded;
            this.OnAttackPhaseEnded = OnAttackPhaseEnded;
            this.OnFreeMovePhaseEnded = OnFreeMovePhaseEnded;
            this.OnTrisDropped = OnTrisDropped;

            // Start
            StartArmiesPlacementPhase();
        }



        // Event Handlers
        Action? OnArmiesPlacementPhaseEnded { get; set; }
        Action? OnAttackPhaseEnded { get; set; }
        Action? OnFreeMovePhaseEnded { get; set; }
        Action<int>? OnTrisDropped { get; set; }


        // Events to Task
        public async Task StartPhaseEndedAsync() => await this.TaskFromEvent(@this => @this.StartPhaseEnded);
        public async Task TurnEndedAsync() => await this.TaskFromEvent(@this => @this.TurnEnded);



        // Others
        public Task JoinGame() => IsGameStarted ? _gameTask : throw new InvalidOperationException($"Game not started yet, you can't join it");



        // Game Api
        public async override Task Start()
        {
            var next = Next;
            IsGameStarted = true;
            PrepareStartPhase();
            await next!();
        }

        public override async Task PlaceArmies(PlayerColor color, int armies, Territory territory)
        {
            var next = Next;
            _placementPhaseHandler.PlaceArmies(color, armies, territory);
            await next!();
        }

        public override async Task SkipPlacementPhase(PlayerColor playerColor)
        {
            var next = Next;
            _placementPhaseHandler.StartInitialPlacementPhase();
            await next!();
        }

        public override async Task Attack(PlayerColor _, Territory from, Territory to, int attackDice)
        {
            var next = Next;
            _attackPhaseHandler.Attack(from, to, attackDice);
            await next!();
        }

        public override async Task Defend(PlayerColor playerColor)
        {
            var next = Next;
            _attackPhaseHandler.Defend();
            await next!();
        }

        public override async Task PlaceArmiesAfterAttack(PlayerColor playerColor, int armies)
        {
            var next = Next;
            _placementAfterAttackPhaseHandler.PlaceArmiesAfterAttack(armies);
            await next!();
        }

        public override async Task SkipAttack(PlayerColor playerColor)
        {
            var next = Next;
            _attackPhaseHandler.SkipAttack();
            await next!();
        }

        public override async Task SkipFreeMove(PlayerColor playerColor)
        {
            var next = Next;
            _freeMovePhaseHandler.SkipFreeMove();
            await next!();
        }

        public override async Task Movement(PlayerColor playerColor, Territory from, Territory to, int armies)
        {
            var next = Next;
            _freeMovePhaseHandler.Movement(from, to, armies);
            await next!();
        }

        public async override Task PlayTris(PlayerColor color, IEnumerable<TerritoryCard> cards)
        {
            var next = Next;
            _cardsManager.PlayTris(_turnInfo.CurrentActionPlayer!, cards); ;
            await next!();
        }



        // Utils
        void InvokeStartPhaseEnded()
        {
            IsStartPhaseEnded = true;
            StartPhaseEnded?.Invoke();
        }

        void InvokeTurnStateUpdated(Action? doBefore = null)
        {
            doBefore?.Invoke();
            TurnStateUpdated?.Invoke();
        }

        void MoveToNextPlayer()
        {
            do
                _turnOrdererdPlayers.MoveNext();
            while (_turnOrdererdPlayers.Current.IsDead);
            _turnInfo.CurrentTurnPlayer = _turnOrdererdPlayers.Current;
            _turnInfo.CurrentActionPlayer = _turnOrdererdPlayers.Current;
        }
    }
}
