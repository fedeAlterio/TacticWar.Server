using TacticWar.Lib.Game.Players.Abstractions;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Rest.ViewModels
{
    public class PlayerVM
    {
        public PlayerVM(IPlayer player)
        {
            Name = player.Name;
            Color = player.Color;
        }


        public string Name { get; init; }
        public PlayerColor Color { get; init; }
    }
}
