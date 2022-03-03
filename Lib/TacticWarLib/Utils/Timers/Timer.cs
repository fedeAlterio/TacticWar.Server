using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TacticWar.Lib.Utils.Timers
{
    public class Timer : ITimer
    {
        // Events
        public event Action Elapsed;



        // Private fields
        private readonly System.Timers.Timer _timer;



        // Initialization
        public Timer(int interval)
        {
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Elapsed?.Invoke();
        }



        // Public Methods
        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();  
    }
}
