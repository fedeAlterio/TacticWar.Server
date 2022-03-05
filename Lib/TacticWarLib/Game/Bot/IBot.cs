namespace TacticWar.Lib.Game.Bot
{
    public interface IBot
    {
        // Properties
        bool IsPlaying { get; }



        // Commands
        Task TryPlayOneStep();
        Task PlayTurn();
    }
}
