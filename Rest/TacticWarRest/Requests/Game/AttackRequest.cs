namespace TacticWar.Rest.Requests.Game
{
    public record AttackRequest : AuthenticatedRoomRequest
    {
        public int AttackId { get; init; }
        public int DefenceId { get; init; }
        public int AttackDice { get; init; }
    }
}
