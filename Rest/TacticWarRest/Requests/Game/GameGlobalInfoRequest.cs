using MediatR;
using TacticWar.Rest.ViewModels;

namespace TacticWar.Rest.Requests.Game
{
    public record GameGlobalInfoRequest : AuthenticatedRoomRequest<GameGlobalInfo>
    {

    }
}
