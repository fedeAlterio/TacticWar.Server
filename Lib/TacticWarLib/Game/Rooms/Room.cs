using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Utils;
using TacticWar.Lib.Extensions;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Builders.Abstractios;

namespace TacticWar.Lib.Game.Rooms
{
    public class Room : IRoom
    {
        // Events        
        public event Action<IRoom>? DeadRoom;



        // Private fields
        private static Random _random = new();
        private static readonly object _locker = new();
        private Dictionary<PlayerColor, IRoomTimer> _timers = new();
        private static Dictionary<int, bool> _roomIds = new();        
        private readonly List<WaitingPlayer> _players = new();
        private readonly IGameBuilder _gameBuilder;
        private readonly RoomTimerBuilder _roomTimerBuilder;
        private IGameConfigurator? _gameConfigurator;



        // Initialization
        public Room(IGameBuilder gameBuilder, RoomTimerBuilder roomTimerBuilder, RoomConfiguration roomConfiguration)
        {
            lock (_locker)
            {
                Id = NewId();
            }
            Configuration = roomConfiguration;
            _gameBuilder = gameBuilder;
            _roomTimerBuilder = roomTimerBuilder;
        }

  

        private static int NewId()
        {
            int ret;            
            do
                ret = _random.Next(0, 100000);
            while (!_roomIds.TryAdd(ret, true));
            
            return ret;
        }



        // Properties
        public RoomConfiguration Configuration { get; }
        public int Id { get; }
        public bool GameStarted { get; private set; }
        public IReadOnlyList<WaitingPlayer> Players => _players;
        public IGameManager? GameManager { get; private set; }        
        public WaitingPlayer? Host => _players?.FirstOrDefault();



        // Events
        private void OnPlayerDisconnected(PlayerColor color)
        {
            if (GameStarted)
                return;

            using var _ = _locker.Lock();

            RemovePlayer(color);
            if (!_players.Any())
                OnRoomDeath();
        }

        private async void OnRoomDeath()
        {
            await Task.Delay(Configuration.DestroyRoomOnRoomDeathDelay);
            DeadRoom?.Invoke(this);
            using var _ = _locker.Lock();
            _roomIds.Remove(Id);            
        }



        // Public Methods
        public async Task<PlayerColor> Authenticate(string name, int secretCode)
        {
            var player = _players.FirstOrDefault(x => x.Name == name && x.SecretCode == secretCode);
            return player is not null 
                ? await Task.FromResult(player.Color) 
                : throw new GameException($"Authentication Failed. Maybe you have inserted the wrong secret code?");
        }


        public WaitingPlayer AddPlayer(string name, int secretCode, bool isBot = false)
        {
            using var _ = _locker.Lock();

            // Validation
            var possibleColors = EnumUtils.GetValues<PlayerColor>().Except(_players.Select(x => x.Color));
            if (!possibleColors.Any())
                throw new GameException($"Room is full");

            var color = possibleColors.First();
            var alreadyJoinedSameName = _players.Where(x => x.Name == name).Any();
            if (alreadyJoinedSameName)
                throw new GameException($"A player with same name already joined");

            // Logic
            var waitingPlayer = new WaitingPlayer { Name = name, Color = color, SecretCode = secretCode, IsBot = isBot };
            _players.Add(waitingPlayer);
            AddTimer(color);

            return waitingPlayer;
        }


        public void RemovePlayer(PlayerColor color)
        {
            using var _ = _locker.Lock();
            var player = _players.FirstOrDefault(x => x.Color == color)
                       ?? throw new GameException($"Player of color {color} has not joined the room");

            _players.Remove(player);
            _timers[color].Stop();            
            _timers.Remove(color);
        }


        public IGameServiceCollection BuildGame()
        {
            using var _ = _locker.Lock();

            foreach (var timer in _timers.Values)
                timer.Stop();

            var players = _players.Select(x => new PlayerInfo(x.Name, x.Color, x.IsBot)).ToList();
            var playersInfoCollection = new PlayersInfoCollection(players);
            _gameConfigurator = _gameBuilder.NewGame(playersInfoCollection);

            return _gameConfigurator;
        }

        public async Task StartGame()
        {
            Task<IGameManager> gameManagerTask;
            using (var _ = _locker.Lock())
            {
                if (GameStarted)
                    throw new GameException($"Game already started!");

                if (Players.Count < 2)
                    throw new GameException($"There are not enough players to start!");

                GameStarted = true;
                gameManagerTask = _gameConfigurator!.StartGame();
            }

            GameManager = await gameManagerTask;
            //GameManager.GameTerminationController.GameEnded += OnRoomDeath;
        }


        public void KeepAlive(PlayerColor color)
        {
            if(_timers.TryGetValue(color, out var timer))
            {
                // Reset the timer
                timer.Stop();
                timer.Start();
            }
        }



        // Utils
        private void AddTimer(PlayerColor playerColor)
        {
            var timer = _roomTimerBuilder!.Invoke();
            timer.Elapsed += () =>  OnPlayerDisconnected(playerColor);
            _timers.Add(playerColor, timer);
            timer.Start();
        }
    }
}
