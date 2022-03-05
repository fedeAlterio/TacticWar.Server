namespace TacticWar.Lib.Game.Players
{
    public class PlayersInfoCollection
    {
        // Initialization
        public PlayersInfoCollection(IEnumerable<PlayerInfo> info)
        {
            Info = info.ToList();
        }



        // Properties
        public IReadOnlyList<PlayerInfo> Info { get; }
    }
}
