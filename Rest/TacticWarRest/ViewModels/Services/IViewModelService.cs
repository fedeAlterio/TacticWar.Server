using TacticWar.Lib.Game.GamePhases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Players;

namespace TacticWar.Rest.ViewModels.Services
{
    public interface IViewModelService
    {
        Task<GameSnapshot> GetGameSnapshot(PlayerColor playerColor, int versionId);
        Task<GameGlobalInfo> GetGameGlobalInfo(PlayerColor playerColor);

    }
}
