using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game;
using TacticWar.Lib.Game.Rooms;
using TacticWar.Lib.Game.Rooms.Abstractions;

namespace TacticWar.Lib.Extensions.Microsoft.DependencyInjection.Installers
{
    internal class RoomServicesInstaller : IServicesInstaller
    {
        public void InstallServices(IServiceCollection services)
        {
            services.AddSingleton<IRoomBuilder, RoomBuilderWithLocator>();
            services.AddSingleton<IRoomsManager, RoomsManager>();
            services.AddSingleton<RoomConfiguration>();
            services.AddSingleton<IRoomBuilder, RoomBuilderWithLocator>();
            services.AddTransient<Room>();
            services.AddTransient<RoomTimer>();
            services.AddSingleton<RoomTimerBuilder>(p => () => p.GetService<RoomTimer>());
        }
    }
}
