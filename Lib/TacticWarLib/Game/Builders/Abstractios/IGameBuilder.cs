using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Builders.Abstractios
{
    public interface IGameBuilder
    {
        IGameConfigurator NewGame(GameStartupInformation gameStartupInformation);
    }

    public interface IGameConfigurator : IGameServiceCollection
    {
        Task<IGameManager> StartGame();
    }

}
