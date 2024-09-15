using Microsoft.Extensions.DependencyInjection;
using TacticWar.Lib.Game.Deck.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Builders.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives.Decks
{
    public class ObjectivesDeckBuilder
    {
        // Private fields
        readonly IServiceProvider _serviceProvider;



        // Initialization
        public ObjectivesDeckBuilder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }



        // Core
        public IDeck<IObjective> NewDeck()
        {
            var buildersTypes = typeof(IObjectivesBuilder).Assembly.DefinedTypes
                .Where(t => !t.IsInterface && !t.IsAbstract && typeof(IObjectivesBuilder).IsAssignableFrom(t))
                .ToList();

            var builders = buildersTypes
                .Take(1)
                .Select(t => ActivatorUtilities.CreateInstance(_serviceProvider, t))
                .Cast<IObjectivesBuilder>()
                .ToList();
            ;

            var objectives = builders.SelectMany(builder => builder.BuildObjectvies()).ToList();
            var deck = new Deck<IObjective>(objectives);
            return deck;
        }
    }
}
