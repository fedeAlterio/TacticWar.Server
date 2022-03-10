using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Core.Abstractions;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Core.Pipeline.Middlewares
{
    public class GameUpdatesListener : SingleTaskMiddleware, IGameUpdatesListener
    {
        // Events
        public event Action<IPlayer>? GameUpdated;



        // Private fields
        private readonly SemaphoreSlim _ss = new(1);
        private readonly ITurnInfo _turnInfo;
        private readonly INewTurnManager _turnManager;
        private bool _pipelineRunning;        


        // Initialization
        public GameUpdatesListener(ITurnInfo turnInfo, INewTurnManager turnManager)
        {
            _turnInfo = turnInfo;
            _turnManager = turnManager;
            _turnInfo.PropertyChanged += (_, _) => InvokeGameUpdated();
            turnManager.TurnStateUpdated += () => InvokeGameUpdated();
        }



        // Properties
        public DateTime LastUpdateDate { get; private set; }



        // Pipeline Action
        protected override async Task DoAction()
        {
            if (_ss.CurrentCount == 0)
                throw new InvalidOperationException($"Already doing a move");

            using var _ = await _ss.WaitAsyncScoped();
            try
            {
                _pipelineRunning = true;
                var next = Next;
                await next!();
            }
            finally
            {
                _pipelineRunning = false;
                InvokeGameUpdated();
            }
        }



        // Utils

        private void InvokeGameUpdated()
        {
            if (_pipelineRunning)
                return;

            LastUpdateDate = DateTime.Now;
            GameUpdated?.Invoke(_turnInfo.CurrentActionPlayer!);
        }
    }
}
