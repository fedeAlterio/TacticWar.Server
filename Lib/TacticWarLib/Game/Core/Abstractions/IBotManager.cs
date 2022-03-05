using TacticWar.Lib.Game.Bot.Abstractions;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Core.Abstractions
{
    public interface IBotManager
    {
        // Properties
        public bool IsBotPlaying { get; }



        // Methods
        public void AddBot(PlayerColor color, Action<IBotSettings>? configuration = null);
        public void AddBotForOneTurn(PlayerColor color, Action<IBotSettings>? configuration = null);
        public void RemoveBot(PlayerColor color);
    }
}
