using TacticWar.Lib.Game.Abstractions;

namespace TacticWar.Rest.ViewModels.Services
{
    public interface IViewModelsLocator
    {
        Task<IViewModelService> FromGameManager(IGameManager gameManager);
        Task RegisterViewModel(IGameManager gameManager, IViewModelService gameServiceCollection);
    }
}
