using Microsoft.Extensions.DependencyInjection;
using TacticWar.Lib.Game.Deck.Abstractions;
using TacticWar.Lib.Game.Deck.Builders;

namespace TacticWar.Lib.Extensions.Microsoft.DependencyInjection.Installers
{
    public class TerritoryDeckInstaller : IServicesInstaller
    {
        public void InstallServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ITerritoryDeckBuilder, TerritoryDeckBuilder>();
        }
    }
}
