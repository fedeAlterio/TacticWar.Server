using TacticWar.Lib.Game.Deck.Objectives.Abstractions;

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
