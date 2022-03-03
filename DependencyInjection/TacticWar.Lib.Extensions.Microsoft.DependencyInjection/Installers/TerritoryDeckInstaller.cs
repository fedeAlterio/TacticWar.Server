using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Deck.Abstractions;

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
