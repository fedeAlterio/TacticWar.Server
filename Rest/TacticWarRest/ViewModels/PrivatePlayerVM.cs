using TacticWar.Lib.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Pipeline.Abstractions;

namespace TacticWar.Rest.ViewModels
{
    public class PrivatePlayerVM : PlayerVM
    {
        public PrivatePlayerVM(IPlayer player/*, IIdleManager idleManager*/) : base(player)
        {
            Name = player.Name;
            CardsCount = player.Cards.Count;
            //IsIdle = idleManager.IdlePlayers.Contains(player);
        }

        public int CardsCount { get; init; }
        public bool IsIdle { get; init; }
    }
}
