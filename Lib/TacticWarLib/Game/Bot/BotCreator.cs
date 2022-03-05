using Microsoft.Extensions.DependencyInjection;
using TacticWar.Lib.Game.Bot.Abstractions;

namespace TacticWar.Lib.Game.Bot
{
    public class BotCreator : IBotCreator
    {
        // Private fields
        private readonly IServiceProvider _provider;



        // Initialization
        public BotCreator(IServiceProvider provider)
        {
            _provider = provider;
        }



        // Core

        public NoActionBot NewNoActionBot()
        {
            return ActivatorUtilities.CreateInstance<NoActionBot>(_provider);
        }
    }
}
