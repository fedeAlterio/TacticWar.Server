namespace TacticWar.Lib.Game.Core.Abstractions
{
    public interface IDependencyManager
    {
        T Get<T>();
    }
}
