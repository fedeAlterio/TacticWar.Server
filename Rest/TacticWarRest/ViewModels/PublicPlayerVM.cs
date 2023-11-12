using TacticWar.Lib.Game.Players;

namespace TacticWar.Rest.ViewModels
{
    public class PublicPlayerVM :  PlayerVM
    {
        public PublicPlayerVM(Player player) : base(player)
        {
            Cards = player.Cards.Select(card => new CardSnapshot(card)).ToList();
        }

        public List<CardSnapshot> Cards { get; init; }
    }
}
