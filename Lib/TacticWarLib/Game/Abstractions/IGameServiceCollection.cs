namespace TacticWar.Lib.Game.Abstractions
{
    public interface IGameServiceCollection
    {
        void AddSingleton<T>(Action<IGameManager, T>? onGameCreated = null) where T : class;
        void AddSingleton<T, V>(Action<IGameManager, V>? onGameCreated = null) where V : class, T where T : class;
    }
}
