using Microsoft.AspNetCore.Mvc;
using MediatR;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels;

namespace TacticWar.Rest.Controllers
{
    [ApiController]
    [Route("[Controller]/{RoomId}")]
    public class GameController : Controller
    {
        // Private fields
        private readonly IMediator _mediator;



        // Initialization
        public GameController(IMediator mediator)
        {
            _mediator = mediator;
        }



        // Properties
        [FromRoute] public int RoomId { get; set; }
        [FromHeader] public int PlayerSecret { get; set; }
        [FromHeader] public string? PlayerName { get; set; }



        // Actions
        [HttpPost]
        [Route("[Action]")]
        public async Task<GameSnapshot> GetSnapshot(GetSnapshotBody body)
        {
            var request = NewAuthenticatedRequest<GameSnapshotRequest, GameSnapshot>() with { VersionId = body.VersionId };
            return await SendAuthenticatedRequest(request);
        }
        public record GetSnapshotBody(int VersionId);


        [HttpPost]
        [Route("[Action]")]
        public async Task<GameGlobalInfo> GetGameGlobalInfo(GameGlobalInfoBody body)
        {
            var request = NewAuthenticatedRequest<GameGlobalInfoRequest, GameGlobalInfo>();
            return await SendAuthenticatedRequest(request);
        }
        public record GameGlobalInfoBody();


        [HttpPost]
        [Route("[Action]")]
        public async Task FinishPlacement(FinishPlacementBody body)
        {
            var request = NewAuthenticatedRequest<FinishPlacementRequest>();
            await SendAuthenticatedRequest(request);
        }
        public record FinishPlacementBody();



        [HttpPost]
        [Route("[Action]")]
        public async Task Attack(AttackBody body)
        {
            var request = NewAuthenticatedRequest<AttackRequest>() with { AttackId = body.AttackId, DefenceId = body.DefenceId, AttackDice = body.AttackDice };
            await SendAuthenticatedRequest(request);
        }
        public record AttackBody(int AttackId, int DefenceId, int AttackDice);


        [HttpPost]
        [Route("[Action]")]
        public async Task Defend(DefendBody body)
        {
            var request = NewAuthenticatedRequest<DefendRequest>();
            await SendAuthenticatedRequest(request);
        }
        public record DefendBody();



        [HttpPost]
        [Route("[Action]")]
        public async Task PlaceArmies(PlaceArmiesBody body)
        {
            var request = NewAuthenticatedRequest<PlaceArmiesRequest>()
                          with
            { Placements = body.PlacementInfo.Select(x => new Requests.Game.PlacementInfo(x.TerritoryId, x.Armies)).ToList() };
            await SendAuthenticatedRequest(request);
        }
        public record PlaceArmiesBody(IList<PlacementInfo> PlacementInfo);
        public record PlacementInfo(int TerritoryId, int Armies);


        [HttpPost]
        [Route("[Action]")]
        public async Task PlaceArmiesAfterAttack(PlaceArmiesAfterAttackInfo body)
        {
            var request = NewAuthenticatedRequest<PlaceArmiesAfterAttackRequest>() with { Armies = body.Armies };
            await SendAuthenticatedRequest(request);
        }
        public record PlaceArmiesAfterAttackInfo(int Armies);


        [HttpPost]
        [Route("[Action]")]
        public async Task FinishAttackPhase(FinishAttackPhaseBody body)
        {
            var response = NewAuthenticatedRequest<FinishAttackPhaseRequest>();
            await SendAuthenticatedRequest(response);
        }
        public record FinishAttackPhaseBody();


        [HttpPost]
        [Route("[Action]")]
        public async Task MoveArmies(MoveArmiesBody body)
        {
            var request = NewAuthenticatedRequest<MoveArmiesRequest>() with { FromId = body.FromId, ToId = body.ToId, Armies = body.Armies };
            await SendAuthenticatedRequest(request);
        }
        public record MoveArmiesBody(int FromId, int ToId, int Armies);


        [HttpPost]
        [Route("[Action]")]
        public async Task FinishMovementPhase(FinishMovementPhaseBody body)
        {
            var request = NewAuthenticatedRequest<FinishMovementPhaseRequest>();
            await SendAuthenticatedRequest(request);
        }
        public record FinishMovementPhaseBody();


        [HttpPost]
        [Route("[Action]")]
        public async Task DropTris(DropTrisBody body)
        {
            var request = NewAuthenticatedRequest<DropTrisRequest>() with { CardsIds = body.CardIds };
            await SendAuthenticatedRequest(request);
        }
        public record DropTrisBody(List<int> CardIds);



        // Utils
        private T NewAuthenticatedRequest<T>() where T : AuthenticatedRoomRequest, new()
            => new() { PlayerName = PlayerName, PlayerSecret = PlayerSecret, RoomId = RoomId };

        private T NewAuthenticatedRequest<T, V>() where T : AuthenticatedRoomRequest<V>, new()
            => NewAuthenticatedRequest<T>();

        private Task<TResponse> SendAuthenticatedRequest<TResponse>(AuthenticatedRoomRequest<TResponse> request)
            => _mediator.Send<TResponse>(request);

        private Task SendAuthenticatedRequest(AuthenticatedRoomRequest request)
            => _mediator.Send(request);
    }
}
