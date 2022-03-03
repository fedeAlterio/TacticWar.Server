using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Bot
{
    public interface IBot
    {
        // Properties
        bool IsPlaying { get; }



        // Commands
        Task TryPlayOneStep();
        Task PlayTurn();
    }
}
