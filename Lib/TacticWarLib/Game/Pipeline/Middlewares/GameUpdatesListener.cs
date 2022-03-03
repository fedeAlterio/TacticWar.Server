using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.GamePhases;
using TacticWar.Lib.Game.Pipeline.Abstractions;
using TacticWar.Lib.Game.Pipeline.Middlewares.Abstractions;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Pipeline.Middlewares
{
    public class GameUpdatesListener : SingleTaskMiddleware, IGameUpdatesListener
    {
        // Events
        public event Action<IPlayer>? GameUpdated;



        // Private fields
        private readonly SemaphoreSlim _ss = new(1);
        private readonly ITurnInfo _turnInfo;
        private readonly INewTurnManager _turnManager;
        private bool _pipelineRunning = false;



        // Initialization
        public GameUpdatesListener(ITurnInfo turnInfo, INewTurnManager turnManager)
        {
            _turnInfo = turnInfo;
            _turnManager = turnManager;
            _turnInfo.PropertyChanged += async (_,_) => await QueueGameUpdatNotifcation();
            turnManager.TurnStateUpdated += async () => await QueueGameUpdatNotifcation();
        }



        // Properties
        public DateTime LastUpdateDate { get; private set; }



        // Pipeline Action
        protected override async Task DoAction()
        {
            if (_ss.CurrentCount == 0)
               throw new InvalidOperationException($"Already doing a move");

            using (var _ = await _ss.WaitAsyncScoped())
            {
                _pipelineRunning = true;
            }

            try
            {
                var next = Next;
                await next!();
            }
            finally
            {
                
                using (var _ = await _ss.WaitAsyncScoped())
                {
                    _pipelineRunning = false;
                }

                await QueueGameUpdatNotifcation();
            }
        }



        // Utils
        private async Task QueueGameUpdatNotifcation()
        {
            using var _ = await _ss.WaitAsyncScoped();
            if (!_pipelineRunning)
                InvokeGameUpdated();
        }


        private void InvokeGameUpdated()
        {
            LastUpdateDate = DateTime.Now;
            GameUpdated?.Invoke(_turnInfo.CurrentActionPlayer!);
        }
    }
}
