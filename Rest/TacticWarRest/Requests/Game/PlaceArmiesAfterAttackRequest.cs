namespace TacticWar.Rest.Requests.Game
{
    public record PlaceArmiesAfterAttackRequest : AuthenticatedRoomRequest
    {
        public int Armies { get; init; }
    }
}
