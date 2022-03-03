using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck;

namespace TacticWar.Rest.ViewModels
{
    public class ObjectiveVM
    {
        // Initialization
        public ObjectiveVM(IObjective objective)
        {
            Description = objective.Description;
        }



        // Properties
        public string Description { get; init; }
    }
}
