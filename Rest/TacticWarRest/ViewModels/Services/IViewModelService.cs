using TacticWar.Lib.Game.Players;

namespace TacticWar.Rest.ViewModels.Services
{
    public interface IViewModelService
    {
        IObservable<GameSnapshot> GetGameSnapshot(PlayerColor playerColor);
        Task<GameGlobalInfo> GetGameGlobalInfo(PlayerColor playerColor);
    }
}
