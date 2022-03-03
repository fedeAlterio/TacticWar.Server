using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Table;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Pipeline.Exceptions;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;

namespace TacticWar.Lib.Game.Pipeline.Middlewares
{
    public class TurnManager : GamePipelineMiddleware, ITurnManager
    {
        // events        
        public event Action<ArmiesPlacementInfo> ArmiesPlacementPhase;
        public event Action<AttackPhaseInfo> AttackPhase;
        public event Action<MovementPhaseInfo> FreeMove;
        public event Action<AttackInfo> PlacementAftrAttack;
        public event Action<GamePhase> PhaseChanged;



        // Private fields        
        private TaskCompletionSource<int> _armiesPlaced = new();
        private TaskCompletionSource<bool> _attackDone = new();
        private TaskCompletionSource<bool> _freeMoveDone = new();
        private AttackInfo _currentAttackInfo;
        private readonly IEnumerator<Player> _players;
        private Func<Player> _currentPlayer;
        private readonly List<List<TerritoryCard>> _trisDropped = new();
        private bool _territoryConquered;



        // Initailzation
        public TurnManager(GameTable gametable)
        {
            GameTable = gametable;
            CurrentPhase = GamePhase.ArmiesPlacement;
            _currentPlayer = () => CurrentTurnPlayer;
            SetupEvents();
            _players = GameTable.Players.Cyclic().GetEnumerator();
            _players.MoveNext();
        }

        private void SetupEvents()
        {
            ArmiesPlacementPhase += OnArmiesPlacementPhase;
            PlacementAftrAttack += OnPlacingAfterAttack;
        }



        // Properties
        public Player CurrentActionPlayer => _currentPlayer();
        public Player CurrentTurnPlayer => _players.Current;
        public GameTable GameTable { get; private set; }
        private bool IsGameEnded { get; set; }
        public bool GameStarted { get; private set; }
        public Player Winner { get; private set; }
        public int ArmiesToPlace { get; private set; }
        public bool WaitingForArmiesPlacementAfterAttack { get; private set; }

        private GamePhase _currentPhase;
        public GamePhase CurrentPhase
        {
            get => _currentPhase;
            set
            {
                if (value == _currentPhase)
                    return;

                _currentPhase = value;
                OnPhaseChanged();
            }
        }

        #region ITurnManager covariance workaround
        IGameTable ITurnManager.GameTable => GameTable;
        IPlayer ITurnManager.Winner => Winner;
        IPlayer ITurnManager.CurrentActionPlayer => CurrentActionPlayer;
        IPlayer ITurnManager.CurrentTurnPlayer => CurrentTurnPlayer;
        #endregion



        // Events        
        private void OnPhaseChanged()
        {
            _trisDropped.Clear();
            PhaseChanged?.Invoke(CurrentPhase);
        }

        private void OnArmiesPlacementPhase(ArmiesPlacementInfo info)
        {
            _territoryConquered = false;
            ArmiesToPlace = info?.ArmiesToPlace ?? 0;
        }

        private void OnPlacingAfterAttack(AttackInfo info)
        {
            _currentAttackInfo = info;
        }




        // Turn Logic
        public async Task Start()
        {
            GameStarted = true;

            var tcs = new TaskCompletionSource<bool>();
            void WaitForStartHandler(ArmiesPlacementInfo obj) => tcs.SetResult(true);
            ArmiesPlacementPhase += WaitForStartHandler;

            GameRoutine();

            // Wait for firs placement phase
            await tcs.Task;
            ArmiesPlacementPhase -= WaitForStartHandler;

            // Continue
            await Next();
        }

        private async void GameRoutine()
        {
            try
            {
                await DoStartPhase();
                while (!IsGameEnded)
                {
                    await DoPlayerTurn();
                    NextPlayer();
                }
            }
            catch (GameEndedException)
            {

            }

            IsGameEnded = true;
        }


        private async Task DoPlayerTurn()
        {
            if (!_players.Current.Territories.Any())
                return;

            ArmiesToPlace = ArmiesPlacementInfo.NormalTurnArmies(GameTable.Map, CurrentActionPlayer);

            var armiesPlacedTask = _armiesPlaced.Task;
            InvokeArmiesPlacementPhase();
            await armiesPlacedTask;

            var attackDoneTask = _attackDone.Task;
            InvokeAttackPhase(new(default, default, default, default));
            await attackDoneTask;

            var freeMoveTask = _freeMoveDone.Task;
            InvokeFreeMove(MovementPhaseInfo.NormalTurn());
            await _freeMoveDone.Task;
        }

        private async Task DoStartPhase()
        {
            var startArmies = ArmiesPlacementInfo.MaxArmiesOnTableAtStart(GameTable.Players.Count);
            while (CurrentActionPlayer.ArmiesCount < startArmies)
                foreach (var _ in GameTable.Players)
                {
                    await DoStartPlacementPahse();
                    NextPlayer();
                }
        }

        private async Task DoStartPlacementPahse()
        {
            var armiesPlacedTask = _armiesPlaced.Task;
            ArmiesToPlace = ArmiesPlacementInfo.GameStartArmies(GameTable.Players.Count, CurrentActionPlayer.ArmiesCount);
            if (ArmiesToPlace > 0)
            {
                InvokeArmiesPlacementPhase();
                await armiesPlacedTask;
            }
        }

        public async override Task TerminateGame()
        {
            CancelTurn(new GameEndedException());
            await Next();
        }



        // Tris
        public async override Task PlayTris(PlayerColor color, IEnumerable<TerritoryCard> cards)
        {
            var dropped = cards.ToList();
            var armies = GameTable.PlayTris(CurrentActionPlayer, dropped);
            _trisDropped.Add(dropped);
            ArmiesToPlace += armies;
            InvokeArmiesPlacementPhase();
            await Next();
        }




        // Place armies
        public async override Task PlaceArmies(PlayerColor color, int armies, Territory territory)
        {
            GameTable.PlaceArmies(CurrentActionPlayer, territory, armies);
            ArmiesToPlace -= armies;

            if (ArmiesToPlace == 0)
                SkipPlacement();
            else
                InvokeArmiesPlacementPhase();
            await Next();
        }

        public async override Task SkipPlacementPhase(PlayerColor playerColor)
        {
            SkipPlacement();
            await Next();
        }


        public async override Task PlaceArmiesAfterAttack(PlayerColor playerColor, int armies)
        {
            GameTable.Move(CurrentActionPlayer, _currentAttackInfo.AttackFrom.Territory, _currentAttackInfo.AttackTo.Territory, armies);
            WaitingForArmiesPlacementAfterAttack = false;
            InvokeAttackPhase(new(default, default, default, default));
            await Next();
        }



        // Attack-Defend
        public async override Task Attack(PlayerColor color, Territory from, Territory to, int attackDice)
        {
            var dice = GameTable.Attack(CurrentActionPlayer, from, to, attackDice);
            var (pFrom, pTo) = (FromTerritory(from), FromTerritory(to));
            var info = new AttackPhaseInfo
            (
                attackDice: dice,
                attackTerritory: pFrom,
                defenceTerritory: pTo
            );

            var defender = PlayerFromTerritory(to);
            _currentPlayer = () => defender;
            InvokeAttackPhase(info);
            await Next();
        }

        public async override Task Defend(PlayerColor playerColor)
        {
            var attackInfo = GameTable.Defend();
            var info = AttackPhaseInfo.FromAttackinfo(attackInfo);

            _currentPlayer = () => CurrentTurnPlayer;

            InvokeAttackPhase(info);
            if (info.DefenceTerritory.Armies == 0)
            {
                WaitingForArmiesPlacementAfterAttack = true;
                PlacementAfterAttack(attackInfo);
            }
            await Next();
        }

        private async void PlacementAfterAttack(AttackInfo attackInfo)
        {
            if (!_territoryConquered)
            {
                _territoryConquered = true;
                DoAt(GamePhase.ArmiesPlacement, () => attackInfo.Attacker.DrawCardFrom(GameTable.Deck));
            }

            await Task.Delay(3000);
            CheckForPlayerDefeated(attackInfo);
            InvokePlacementAfterAttack(attackInfo);
        }

        public async Task SkipAttack(PlayerColor playerColor)
        {
            _attackDone.SetResult(true);
            _attackDone = new();
            await Next();
        }



        // Movement
        public async override Task Movement(PlayerColor playerColor, Territory from, Territory to, int armies)
        {
            GameTable.Move(CurrentActionPlayer, from, to, armies);

            _freeMoveDone.SetResult(true);
            _freeMoveDone = new();
            await Next();
        }

        public async override Task SkipFreeMove(PlayerColor playerColor)
        {
            _freeMoveDone.SetResult(true);
            _freeMoveDone = new();
            await Next();
        }


        // Utils
        private Player PlayerFromTerritory(Territory territory)
        {
            return GameTable.Players.First(x => x.Territories.ContainsKey(territory));
        }

        private void SkipPlacement()
        {
            ArmiesToPlace = 0;
            var armiesPlaced = _armiesPlaced;
            _armiesPlaced = new();
            armiesPlaced.SetResult(0);
        }


        private void CancelTurn(Exception e)
        {
            _armiesPlaced?.SetException(e);
            _attackDone?.SetException(e);
            _freeMoveDone?.SetException(e);
        }

        private void CheckForPlayerDefeated(AttackInfo info)
        {
            var defender = info.Defender;
            if (defender.Territories.Count > 0)
                return;

            // Player Has Lost
            var attacker = info.Attacker;
            var cards = defender.Cards;
            defender.TransferCards(cards.ToList(), attacker);
        }


        private PlayerTerritory FromTerritory(Territory territory)
            => PlayerFromTerritory(territory).Territories[territory];

        private void NextPlayer() => _players.MoveNext();

        private void DoAt(GamePhase gamePhase, Action action)
        {
            void Handler(GamePhase phase)
            {
                if (gamePhase != phase)
                    return;
                action?.Invoke();
                PhaseChanged -= Handler;
            }
            PhaseChanged += Handler;
        }



        // Invoking Events      
        private void InvokePlacementAfterAttack(AttackInfo info)
        {
            CurrentPhase = GamePhase.PlacementAfterAttack;
            PlacementAftrAttack?.Invoke(info);
        }

        private void InvokeFreeMove(MovementPhaseInfo info)
        {
            CurrentPhase = GamePhase.FreeMove;
            FreeMove?.Invoke(info);
        }

        private void InvokeAttackPhase(AttackPhaseInfo info)
        {
            CurrentPhase = GamePhase.Attack;
            AttackPhase?.Invoke(info);
        }

        private void InvokeArmiesPlacementPhase()
        {
            CurrentPhase = GamePhase.ArmiesPlacement;
            var info = new ArmiesPlacementInfo { ArmiesToPlace = ArmiesToPlace, DroppedCards = _trisDropped };
            ArmiesPlacementPhase?.Invoke(info);
        }
    }
}
