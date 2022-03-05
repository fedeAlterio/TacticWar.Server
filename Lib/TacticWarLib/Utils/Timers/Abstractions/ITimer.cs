namespace TacticWar.Lib.Utils.Timers.Abstractions
{
    public interface ITimer
    {
        event Action Elapsed;
        void Start();
        void Stop();
    }
}
