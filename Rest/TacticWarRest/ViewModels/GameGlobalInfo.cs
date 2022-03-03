using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Game;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Rest.ViewModels
{
    public class GameGlobalInfo
    {
        public GameGlobalInfo(IPlayer player, GameMap gameMap)
        {
            NeighborhoodInfo = new(gameMap);
            Objective = new(player.Objective);
        }


        // Properties
        public NeighborhoodInfo NeighborhoodInfo { get; init; }
        public ObjectiveVM Objective { get; init; }
    }
}
