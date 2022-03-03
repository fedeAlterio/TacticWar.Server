using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives
{
    public class KillColorObjective : IObjective
    {
        // Private fields
        private readonly PlayerColor _playerColor;
        private readonly int _totTerritoriesToConquerIfAlreadyKilled;
        private readonly IGameStatistics _gameStatistics;
        private readonly IGameTable _gameTable;



        // Initialization
        public KillColorObjective(PlayerColor playerColor, int totTerritoriesToConquerIfAlreadyKilled, IGameStatistics gameStatistics, IGameTable gameTable)
        {
            _playerColor = playerColor;
            _totTerritoriesToConquerIfAlreadyKilled = totTerritoriesToConquerIfAlreadyKilled;
            _gameStatistics = gameStatistics;
            _gameTable = gameTable;
            Description = $"You have to kill {playerColor} or conquer {totTerritoriesToConquerIfAlreadyKilled} territories if {playerColor} is no more in the game";
        }



        // Properties
        public string Description { get; }



        // Core
        public bool IsCompleted(IPlayer player)
        {   
            var statistics = _gameStatistics;
            var tokillPlayer = _gameTable.Players.FirstOrDefault(x => x.Color == _playerColor);
            if (tokillPlayer is null || tokillPlayer == player)
                return player.Territories.Count >= _totTerritoriesToConquerIfAlreadyKilled;

            return statistics.TryGetKiller(tokillPlayer, out var killer)
                  && (killer == player || player.Territories.Count >= _totTerritoriesToConquerIfAlreadyKilled);
        }
    }
}
