using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Extensions.Microsoft.DependencyInjection.Installers;
using TacticWar.Lib.Game;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Builders;
using TacticWar.Lib.Game.Rooms;
using TacticWar.Lib.Game.Rooms.Abstractions;

namespace TacticWar.Lib.Extensions.Microsoft.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // Extensions
        public static void AddTacticWar(this IServiceCollection @this)
        {
            @this.AddSingleton<INewGameBuilder, GameBuilder>();
            InstallServices(@this);
        }



        // Utils
        private static void InstallServices(IServiceCollection services)
        {
            var installers = typeof(ServiceCollectionExtensions).Assembly.GetTypes()
                .Where(t => typeof(IServicesInstaller).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .Select(t => (IServicesInstaller)Activator.CreateInstance(t));
            foreach (var installer in installers)
                installer.InstallServices(services);
        }
    }
}
