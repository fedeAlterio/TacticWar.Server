using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Core.Pipeline.Abstractions;
using TacticWar.Lib.Game.Deck;
using TacticWar.Lib.Game.Map;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Core.Pipeline
{
    public class GamePipeline : IGameApi, IGamePipelineBuilder
    {
        // Private fields
        readonly List<Func<IGamePipelineMiddleware>> _pipelineBuilders = new();
        readonly List<IGamePipelineMiddleware> _pipeline = new();



        // Initialization
        GamePipeline() { }
        public static IGamePipelineBuilder New() => new GamePipeline();



        // Core
        public IGamePipelineBuilder Add<T>(Func<T> builder) where T : class, IGamePipelineMiddleware
        {
            _pipelineBuilders.Add(builder);
            return this;
        }

        public IGamePipelineBuilder Add<T>() where T : class, IGamePipelineMiddleware, new()
        {
            var index = _pipelineBuilders.Count;
            return Add(() => new T());
        }

        public IGamePipelineBuilder Add<T>(T instance) where T : class, IGamePipelineMiddleware
        {
            if (instance is null)
                throw new ArgumentNullException(nameof(instance));

            return Add(() => instance);
        }

        public IGameApi Build()
        {
            for (var i = 0; i < _pipelineBuilders.Count; i++)
            {
                var builder = _pipelineBuilders[i];
                var gameStep = builder();
                _pipeline.Add(gameStep);
            }
            return this;
        }



        // Game API
        public Task Start()
        {
            return ForEach(api => api.Start());
        }

        public Task Attack(PlayerColor playerColor, Territory from, Territory to, int attackDice)
        {
            return ForEach(api => api.Attack(playerColor, from, to, attackDice));
        }

        public Task Defend(PlayerColor playerColor)
        {
            return ForEach(api => api.Defend(playerColor));
        }

        public Task Movement(PlayerColor playerColor, Territory from, Territory to, int armies)
        {
            return ForEach(api => api.Movement(playerColor, from, to, armies));
        }

        public Task PlaceArmies(PlayerColor playerColor, int armies, Territory territory)
        {
            return ForEach(api => api.PlaceArmies(playerColor, armies, territory));
        }

        public Task PlaceArmiesAfterAttack(PlayerColor playerColor, int armies)
        {
            return ForEach(api => api.PlaceArmiesAfterAttack(playerColor, armies));
        }

        public Task PlayTris(PlayerColor playerColor, IEnumerable<TerritoryCard> cards)
        {
            return ForEach(api => api.PlayTris(playerColor, cards));
        }

        public Task SkipAttack(PlayerColor playerColor)
        {
            return ForEach(api => api.SkipAttack(playerColor));
        }

        public Task SkipFreeMove(PlayerColor playerColor)
        {
            return ForEach(api => api.SkipFreeMove(playerColor));
        }

        public Task SkipPlacementPhase(PlayerColor playerColor)
        {
            return ForEach(api => api.SkipPlacementPhase(playerColor));
        }

        public Task TerminateGame()
        {
            return ForEach(api => api.TerminateGame());
        }

        public void Initialize()
        {
            foreach (var middleware in _pipeline)
                middleware.Initialize();
        }



        // Utils
        async Task ForEach(Func<IGamePipelineMiddleware, Task> action)
        {
            var pipeline = _pipeline.ToList();
            var i = 0;

            async Task NextStep()
            {
                var current = pipeline[i];
                current.Next = i < pipeline.Count - 1 ? NextStep : () => Task.CompletedTask;
                i++;
                await action.Invoke(current);
            }

            await NextStep();
        }
    }

    public interface IGamePipelineBuilder
    {
        IGamePipelineBuilder Add<T>(T instance) where T : class, IGamePipelineMiddleware;
        IGameApi Build();
    }
}
