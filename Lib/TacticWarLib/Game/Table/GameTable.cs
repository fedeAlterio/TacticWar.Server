using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.GamePhases.PhaseInfo;
using TacticWar.Lib.Game.Deck.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Abstractions;

namespace TacticWar.Lib.Game.Table
{
    public sealed class GameTable : IGameTable
    {
        // Events
        public event Action<GameTable> OnStateChanged;



        // Static
        private static readonly Random _random = new();



        // Private fields
        private readonly GameTableValidator _validate;
        private readonly IDiceRoller _diceRoller;
        private PlayerTerritory _attackFrom;
        private PlayerTerritory _attackTo;
        private IList<int> _attackDice;
        private Player _attacker;
        private Player _defender;



        // Initialization
        public GameTable(GameMap map, PlayersInfoCollection playersInfo, IDeck<IObjective> objectivesDeck, IDiceRoller diceRoller, IDeck<TerritoryCard> territoryDeck)
        {
            _ = map ?? throw new ArgumentNullException(nameof(map));
            _ = playersInfo ?? throw new ArgumentNullException(nameof(playersInfo));

            Map = map;
            Deck = territoryDeck;
            ObjectivesDeck = objectivesDeck;
            _diceRoller = diceRoller;
            Players = CreatePlayers(playersInfo.Info);
            _validate = new(this);
        }


        private List<Player> CreatePlayers(IReadOnlyList<PlayerInfo> playersInfo)
        {
            if (playersInfo.Distinct().Count() < playersInfo.Count)
                throw new GameException($"There are 2 players with the same name!");

            var players = playersInfo.Select(info => PlayerFromPlayerInfo(info.Name, info.Color)).ToList();
            AssignStartTerritories(players);
            AssignStartArmies(players);
            return players;
        }

        private Player PlayerFromPlayerInfo(string name, PlayerColor color) => new()
        {
            Name = name,
            Color = color,
            Objective = DrawObjective()
        };

        private void AssignStartTerritories(IList<Player> players)
        {
            var territories = Map.Territories.ToList();
            territories.Shuffle();            
            foreach (var (player, territory) in players.Cyclic().ChainedWith(territories))
                player.AddTerritory(territory);
        }

        private void AssignStartArmies(IEnumerable<Player> players)
        {
            foreach (var player in players)
                foreach (var territory in player.Territories.Values)
                    territory.Armies = 1;
        }



        // Properties
        public GameMap Map { get; }
        public IReadOnlyList<Player> Players { get; }
        public bool WaitingForDefence { get; private set; }
        public bool WaitingForArmiesMovement { get; private set; }
        public IDeck<TerritoryCard> Deck { get; private set; }
        private IDeck<IObjective> ObjectivesDeck { get; set; }
        IReadOnlyList<IPlayer> IGameTable.Players => Players;



        // Public Methods
        public int PlayTris(Player player, IEnumerable<TerritoryCard> cards)
        {
            // Validation
            _validate.PlayTris(player, cards);

            // Logic
            var trisManager = new TrisManager();
            var armies = trisManager.ArmiesFromTris(player, cards);
            player.RemoveCards(cards, Deck);

            OnGameStateChanged();
            return armies;
        }


        public void Move(Player player, Territory from, Territory to, int armies)
        {
            // Validation
            _validate.Move(player, from, to, armies);

            // Logic
            var(pFrom, pTo) = (player.Territories[from], player.Territories[to]);
            pFrom.Armies -= armies;
            pTo.Armies += armies;
            WaitingForArmiesMovement = false;
            OnGameStateChanged();
        }


        public void PlaceArmies(Player player, Territory territory, int armies)
        {
            // Validation
           _validate.PlaceArmies(player, territory, armies);

            // Logic
            player.Territories[territory].Armies += armies;
            OnGameStateChanged();
        }


        public IList<int> Attack(Player attacker, Territory from, Territory to, int attackDiceCount)
        {
            // Validation
            _validate.Attack(attacker, from, to, attackDiceCount);

            // Logic
            var defender = Players.FirstOrDefault(x => x.Territories.ContainsKey(to));
            var (pFrom, pTo) = (attacker.Territories[from], defender.Territories[to]);

            var attackDice = RollDice(attackDiceCount);
            (_attackFrom, _attackTo) = (pFrom, pTo);
            (_attacker, _defender) = (attacker, defender);
            _attackDice = attackDice;
            WaitingForDefence = true;

            OnGameStateChanged();
            return attackDice;
        }

        public AttackInfo Defend()
        {
            // Validation
            _validate.Defend();

            // Logic
            var defenceDiceCount = Math.Clamp(_attackTo.Armies, 1, 3);
            var defenceDice = RollDice(defenceDiceCount);
            var attackResult = new AttackResult(_attackDice, defenceDice);
            _attackTo.Armies -= attackResult.ArmiesLostByDefence;
            _attackFrom.Armies -= attackResult.ArmiesLostByAttack;

            if (_attackTo.Armies == 0)
            {
                _defender.RemoveTerritory(_attackTo.Territory);
                _attacker.AddTerritory(_attackTo.Territory);
                WaitingForArmiesMovement = true;
            }

            var attackInfo = new AttackInfo
            {
                Attacker = _attacker,
                Defender = _defender,
                AttackFrom = _attackFrom,
                AttackTo = _attackTo,
                Result = attackResult
            };

            WaitingForDefence = false;
            OnGameStateChanged();
            return attackInfo;
        }




        // Utils
        private IObjective DrawObjective()
        {
            ObjectivesDeck.Draw(out var ret);
            return ret;
        }    

        private IList<int> RollDice(int diceCount)
        {
            return Enumerable.Range(0, diceCount)
                .Select(_ =>_diceRoller.RollDice())
                .ToList();
        }



        // Invoking events
        private void OnGameStateChanged()
        {
            OnStateChanged?.Invoke(this);
        }
    }
}
