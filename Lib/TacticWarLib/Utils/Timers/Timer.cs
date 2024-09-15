using System.Timers;
using TacticWar.Lib.Utils.Timers.Abstractions;
using ITimer = TacticWar.Lib.Utils.Timers.Abstractions.ITimer;

namespace TacticWar.Lib.Utils.Timers
{
    public class Timer : ITimer
    {
        // Events
        public event Action Elapsed;



        // Private fields
        readonly System.Timers.Timer _timer;



        // Initialization
        public Timer(int interval)
        {
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += Timer_Elapsed;
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Elapsed?.Invoke();
        }



        // Public Methods
        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();  
    }
}
