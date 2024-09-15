using Microsoft.AspNetCore.Mvc;
using MediatR;
using TacticWar.Rest.ViewModels.Rooms;
using TacticWar.Rest.Requests.Room;

namespace TacticWar.Rest.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        // Private fields
        readonly IMediator _mediator;



        // Initialization
        public RoomsController(IMediator mediator)
        {
            _mediator = mediator;
        }



        // Actions
        [HttpPost]
        [Route("[Action]")]
        public async Task<RoomSnapshot> CreateRoom(CreateRoomRequest request)
        {
            return await _mediator.Send(request);
        }



        [HttpPost]
        [Route("[Action]")]
        public async Task<RoomSnapshot> JoinRoom(JoinRoomRequest request)
        {
            return await _mediator.Send(request);
        }


        [HttpPost]
        [Route("[Action]")]
        public async Task StartGame(StartGameRequest request)
        {
            await _mediator.Send(request);
        }


        [HttpPost]
        [Route("[Action]")]
        public async Task<RoomSnapshot> KeepAlive(KeepAliveRequest request)
        {
            return await _mediator.Send(request);
        }
    }
}
