using TacticWar.Lib.Game.Abstractions;

namespace TacticWar.Rest.ViewModels.Services
{
    public class ViewModelsLocator : IViewModelsLocator
    {
        // Private fields
        readonly Dictionary<IGameManager, IViewModelService> _viewModelsByGameManagers = new();



        // Public Methods
        public async Task<IViewModelService> FromGameManager(IGameManager gameManager)
        {
            return await Task.FromResult(_viewModelsByGameManagers[gameManager]);
        }

        public async Task RegisterViewModel(IGameManager gameManager, IViewModelService gameServiceCollection)
        {
            _viewModelsByGameManagers.Add(gameManager, gameServiceCollection);
            await Task.CompletedTask;
        }
    }
}
