using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Abstractions
{
    public interface IGameServiceCollection
    {
        IGameServiceCollection AddSingleton<T, V>(Action<IGameManager, V>? onGameCreated = null) where V : class, T where T : class;
    }

    public interface INewGameServiceCollection
    {
        void AddSingleton<T>(Action<INewGameManager, T>? onGameCreated = null) where T : class;
        void AddSingleton<T, V>(Action<INewGameManager, V>? onGameCreated = null) where V : class, T where T : class;
    }
}
