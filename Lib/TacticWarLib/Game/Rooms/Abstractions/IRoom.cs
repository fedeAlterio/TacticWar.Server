using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Abstractions;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Rooms.Abstractions
{
    public interface IRoom
    {
        // Events
        public event Action<IRoom> DeadRoom;



        // Properties
        public RoomConfiguration Configuration { get; }
        public int Id { get; }
        public bool GameStarted { get; }
        public IReadOnlyList<WaitingPlayer> Players { get; }
        public INewGameManager? GameManager { get; }
        public WaitingPlayer? Host { get; }



        // Methods
        Task<PlayerColor> Authenticate(string name, int secretCode);
        WaitingPlayer AddPlayer(string name, int secretCode, bool isBot = false);
        void RemovePlayer(PlayerColor color);
        INewGameServiceCollection BuildGame();
        void StartGame();
        void KeepAlive(PlayerColor color);
    }
}
