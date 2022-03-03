using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Abstractions
{
    public interface IGameBuilder
    {
        IGameConfigurator NewGame(PlayersInfoCollection playersInfo);
    }

    public interface INewGameBuilder
    {
        INewGameConfigurator NewGame(PlayersInfoCollection playersInfo);
    }

    public interface IGameConfigurator : IGameServiceCollection
    {
        IGameManager StartGame();
    }

    public interface INewGameConfigurator : INewGameServiceCollection
    { 
        INewGameManager StartGame();    
    }

}
