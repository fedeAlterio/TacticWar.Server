using System.Reactive;
using System.Reactive.Linq;
using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Players;
using TacticWar.Rest.Utils.LongPolling;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Rest.ViewModels.Services
{
    public class ViewModelService : IViewModelService
    {
        // Private fields
        readonly GameViewModelsBuilder _viewModelsBuilder;

        // Initialization
        public ViewModelService(GameViewModelsBuilder viewModelsBuilder, IGameTable gameTable)
        {
            _viewModelsBuilder = viewModelsBuilder;
        }

        public IObservable<GameSnapshot> GetGameSnapshot(PlayerColor playerColor)
        {
            return _viewModelsBuilder.GameUpdated
                                     .StartWith(Unit.Default)
                                     .Select(_ => _viewModelsBuilder.GetGameSnapshot(playerColor));
        }

        public Task<GameGlobalInfo> GetGameGlobalInfo(PlayerColor color)
        {
            var ret = _viewModelsBuilder.GetGameGlobalInfo(color);
            return Task.FromResult(ret);
        }
    }
}
