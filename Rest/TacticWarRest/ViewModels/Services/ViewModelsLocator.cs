using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Game;
using TacticWar.Lib.Game.Abstractions;

namespace TacticWar.Rest.ViewModels.Services
{
    public class ViewModelsLocator : IViewModelsLocator
    {
        // Private fields
        private readonly Dictionary<INewGameManager, IViewModelService> _viewModelsByGameManagers = new();



        // Public Methods
        public async Task<IViewModelService> FromGameManager(INewGameManager gameManager)
        {
            return await Task.FromResult(_viewModelsByGameManagers[gameManager]);
        }

        public async Task RegisterViewModel(INewGameManager gameManager, IViewModelService gameServiceCollection)
        {
            _viewModelsByGameManagers.Add(gameManager, gameServiceCollection);
            await Task.CompletedTask;
        }
    }
}
