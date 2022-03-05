using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Players;
using TacticWar.Rest.Utils.LongPolling;
using TacticWar.Lib.Game.Table.Abstractions;

namespace TacticWar.Rest.ViewModels.Services
{
    public class ViewModelService : IViewModelService
    {
        // Private fields
        private readonly GameViewModelsBuilder _viewModelsBuilder;
        private readonly Dictionary<PlayerColor, UpdateQueue<GameSnapshot>> _snapshotUpdates = new();



        // Initialization
        public ViewModelService(GameViewModelsBuilder viewModelsBuilder, IGameTable gameTable)
        {
            _viewModelsBuilder = viewModelsBuilder;
            foreach (var player in gameTable.Players)
                _snapshotUpdates.Add(player.Color, new());
            _viewModelsBuilder.GameUpdated += OnGameUpdated;
        }



        // Events handlers
        private void OnGameUpdated()
        {
            foreach (var (playerColor, queue) in _snapshotUpdates)
                queue.NotifyNew(_viewModelsBuilder.GetGameSnapshot(playerColor));
        }



        // Public
        public async Task<GameSnapshot> GetGameSnapshot(PlayerColor playerColor, int versionId)
        {
            if (!_snapshotUpdates.TryGetValue(playerColor, out var queue))
                throw new GameException($"Impossible find player of color {playerColor}");

            var ret = versionId < queue.VersionFetched
                ? _viewModelsBuilder.GetGameSnapshot(playerColor)
                : await queue.Get();
            ret.VersionId = queue.VersionFetched;
            return ret;
        }

        public Task<GameGlobalInfo> GetGameGlobalInfo(PlayerColor color)
        {
            var ret = _viewModelsBuilder.GetGameGlobalInfo(color);
            return Task.FromResult(ret);
        }
    }
}
