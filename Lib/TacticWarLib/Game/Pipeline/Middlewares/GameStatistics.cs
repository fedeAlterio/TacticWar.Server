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
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Table;

namespace TacticWar.Lib.Game.Pipeline.Middlewares
{
    public class GameStatistics : GamePipelineMiddleware, IGameStatistics
    {
        // Private fields
        private readonly Dictionary<IPlayer, IPlayer> _kills = new();
        private readonly IGameTable _gameTable;
        private readonly ITurnInfo _turnInfo;
        private HashSet<IPlayer> _alivePlayers = new();
        private HashSet<IPlayer> _deadPlayers = new();



        // Initialization
        public GameStatistics(IGameTable gameTable, ITurnInfo turnInfo)
        {
            _gameTable = gameTable;
            _turnInfo = turnInfo;
        }



        // Properties
        public IReadOnlyDictionary<IPlayer, IPlayer> Kills => _kills; // dead to killer
        public IReadOnlySet<IPlayer> AlivePlayers => _alivePlayers;
        public IReadOnlySet<IPlayer> DeadPlayers => _deadPlayers;



        // IGameStatistics
        public bool TryGetKiller(IPlayer victim, out IPlayer? killer) => _kills.TryGetValue(victim, out killer);
        public bool IsDead(IPlayer player) => _deadPlayers.Contains(player);
        public bool IsAlive(IPlayer player) => _alivePlayers.Contains(player);



        // Pipeline Methods
        public async override Task Defend(PlayerColor playerColor)
        {
            var next = Next;
            UpdateKillsAndDeaths();
            await next!();
        }



        // Utils
        private void UpdateKillsAndDeaths()
        {
            var oldDeads = _deadPlayers;
            UpdateDeaths();
            var newDeads = _deadPlayers?.Except(oldDeads ?? Enumerable.Empty<IPlayer>()) ?? Enumerable.Empty<IPlayer>();

            foreach (var dead in newDeads)
                _kills[dead] = _turnInfo.CurrentActionPlayer!;
        }

        private void UpdateDeaths()
        {
            var deads = _gameTable.Players.Where(x => x.Territories.Count == 0);
            _deadPlayers = deads.ToHashSet();
            _alivePlayers = _gameTable.Players.Except(_deadPlayers).ToHashSet();
        }
    }
}
