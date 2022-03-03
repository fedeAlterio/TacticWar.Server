using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Extensions.Microsoft.DependencyInjection.Installers
{
    internal interface IServicesInstaller
    {
        void InstallServices(IServiceCollection serviceCollection);
    }
}
