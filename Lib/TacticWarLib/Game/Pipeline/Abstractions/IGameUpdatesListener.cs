using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Pipeline.Abstractions
{
    public interface IGameUpdatesListener
    {
        DateTime LastUpdateDate { get; }

        event Action<IPlayer> GameUpdated;
    }
}
