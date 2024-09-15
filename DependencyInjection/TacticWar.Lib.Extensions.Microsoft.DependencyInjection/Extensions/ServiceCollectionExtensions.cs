using Microsoft.Extensions.DependencyInjection;
using TacticWar.Lib.Extensions.Microsoft.DependencyInjection.Installers;
using TacticWar.Lib.Game.Builders;
using TacticWar.Lib.Game.Builders.Abstractios;

namespace TacticWar.Lib.Extensions.Microsoft.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        // Extensions
        public static void AddTacticWar(this IServiceCollection @this)
        {
            @this.AddSingleton<IGameBuilder, GameBuilder>();
            InstallServices(@this);
        }



        // Utils
        static void InstallServices(IServiceCollection services)
        {
            var installers = typeof(ServiceCollectionExtensions).Assembly.GetTypes()
                .Where(t => typeof(IServicesInstaller).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .Select(t => (IServicesInstaller)Activator.CreateInstance(t));
            foreach (var installer in installers)
                installer.InstallServices(services);
        }
    }
}
