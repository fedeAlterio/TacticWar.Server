namespace TacticWar.Rest.Requests.Game
{
    public record MoveArmiesRequest : AuthenticatedRoomRequest
    {
        public int FromId { get; init; }
        public int ToId { get; init; }
        public int Armies { get; init; }
    }
}
