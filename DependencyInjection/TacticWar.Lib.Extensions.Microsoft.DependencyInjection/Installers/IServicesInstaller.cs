using Microsoft.Extensions.DependencyInjection;

namespace TacticWar.Lib.Extensions.Microsoft.DependencyInjection.Installers
{
    internal interface IServicesInstaller
    {
        void InstallServices(IServiceCollection serviceCollection);
    }
}
