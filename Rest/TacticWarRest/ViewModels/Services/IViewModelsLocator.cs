using TacticWar.Lib.Game.GamePhases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Game;
using TacticWar.Lib.Game.Abstractions;

namespace TacticWar.Rest.ViewModels.Services
{
    public interface IViewModelsLocator
    {
        Task<IViewModelService> FromGameManager(INewGameManager gameManager);
        Task RegisterViewModel(INewGameManager gameManager, IViewModelService gameServiceCollection);
    }
}
