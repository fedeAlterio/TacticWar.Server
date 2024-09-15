using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives
{
    public class KillColorObjective : IObjective
    {
        // Private fields
        readonly PlayerColor _playerColor;
        readonly int _totTerritoriesToConquerIfAlreadyKilled;
        readonly IGameStatistics _gameStatistics;
        readonly IGameTable _gameTable;



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
