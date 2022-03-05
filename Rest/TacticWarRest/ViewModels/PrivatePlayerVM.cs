using TacticWar.Lib.Game.Players.Abstractions;

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
