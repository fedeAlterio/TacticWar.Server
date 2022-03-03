using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions
{
    public interface IPipelineDelimiter
    {
        // Properties
        public bool IsPipelineRunning { get; }



        // Core
        Task WaitForPipelineFree();
    }
}
