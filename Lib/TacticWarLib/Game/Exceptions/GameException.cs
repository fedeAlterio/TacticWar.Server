using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Exceptions
{
    public class GameException : Exception
    {
        public GameException(string message) : base(message)
        {
        }

        public GameException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
