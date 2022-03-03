using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Table
{
    internal class GameTableValidator
    {
        // Private fields
        private readonly GameTable _gameTable;



        // Intiialization
        public GameTableValidator(GameTable gameTable)
        {
            _gameTable = gameTable;
        }



        // Validation
        public void PlayTris(Player player, IEnumerable<TerritoryCard> cards)
        {
            AssertIsOfPlayer(player, cards);
        }


        public void Move(Player player, Territory from, Territory to, int armies)
        {
            AssertPlayerIsInGame(player);
            AssertIsOfPlayer(player, from);
            AssertIsOfPlayer(player, to);
            AssertAreClose(from, to);

            var pFrom = player.Territories[from];
            AssertHasArmies(armies + 1, pFrom);
        }


        public void PlaceArmies(Player player, Territory territory, int armies)
        {
            AssertPlayerIsInGame(player);
            AssertIsOfPlayer(player, territory);
            AssertArmiesMakesSense(armies);
        }


        public void Attack(Player attacker, Territory from, Territory to, int attackDiceCount)
        {
            AssertPlayerIsInGame(attacker);
            AssertIsOfPlayer(attacker, from);
            AsserDiceCountMakeSense(attackDiceCount);

            var defender = _gameTable.Players.FirstOrDefault(x => x.Territories.ContainsKey(to));
            AssertNotSamePlayer(attacker, defender);

            var (pFrom, pTo) = (attacker.Territories[from], defender.Territories[to]);
            AssertHasArmies(attackDiceCount + 1, pFrom);
            AssertAreClose(from, to);
        }


        public void Defend()
        {
            AssertIsWaitingForDefence();
        }



        // Assertions
        public void AssertIsInAValidState()
        {
            AssertNotWaitingForDefence();
        }


        public void AssertIsWaitingForDefence()
        {
            if (!_gameTable.WaitingForDefence)
                throw new GameException($"There is nothing to defend from!");
        }


        public void AssertPlayerIsInGame(Player player)
        {
            if (!_gameTable.Players.Contains(player))
                throw new GameException($"Player {player?.Name} is not playing in this game");
        }


        public void AssertIsOfPlayer(Player player, IEnumerable<TerritoryCard> cards)
        {
            foreach (var card in cards)
                if (!player.Cards.Contains(card))
                    throw new GameException($"Player {player.Name} does not have the card {card.Territory}");
        }


        public void AssertIsOfPlayer(Player player, Territory territory)
        {
            if (!player.Territories.ContainsKey(territory))
                throw new GameException($"Player {player.Name} does not have the territory {territory.Name}");
        }


        public void AssertAreClose(Territory t1, Territory t2)
        {
            if (!t1.Neighbors.Contains(t2))
                throw new GameException($"{t1.Name} is not near to {t2.Name}");
        }


        public void AssertHasArmies(int armies, PlayerTerritory territory)
        {
            AssertArmiesMakesSense(armies);

            if (territory.Armies < armies)
                throw new GameException($"Too few armies in {territory.Territory.Name}");
        }


        public void AssertArmiesMakesSense(int armies)
        {
            if (armies <= 0)
                throw new GameException($"It's impossible to have {armies}!");
        }


        public void AssertNotSamePlayer(Player p1, Player p2)
        {
            if (p1 == p2)
                throw new GameException($"Players are the same!");
        }


        public void AsserDiceCountMakeSense(int diceCount)
        {
            if (diceCount <= 0 || diceCount > 3)
                throw new GameException($"Dice count must be between 1 and 3");
        }


        public void AssertNotWaitingForDefence()
        {
            if (_gameTable.WaitingForDefence)
                throw new GameException($"You need to wait the defence to roll before!");
        }
    }
}
