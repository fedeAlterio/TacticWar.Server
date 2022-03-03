using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TacticWar.Lib.Game;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public abstract record AuthenticatedRoomRequestHandler<TAuthenticatedRequest>
        : BaseAuthenticatedRoomRequestHandler<TAuthenticatedRequest>, IRequestHandler<TAuthenticatedRequest>
        where TAuthenticatedRequest : AuthenticatedRoomRequest
    {
        // Initialization
        protected AuthenticatedRoomRequestHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator)
            : base(roomsManager, viewModelsLocator)
        {
        }



        // Core
        public async Task<Unit> Handle(TAuthenticatedRequest request, CancellationToken cancellationToken)
        {
            await Authenticate(request);
            await HandleOnAuthenticated(request, cancellationToken);
            return new();
        }

        protected abstract Task HandleOnAuthenticated(TAuthenticatedRequest request, CancellationToken cancellationToken);
    }

    public abstract record AuthenticatedRoomRequestHandler<TAuthenticatedRequest, TResponse>
        : BaseAuthenticatedRoomRequestHandler<TAuthenticatedRequest>, IRequestHandler<TAuthenticatedRequest, TResponse>
        where TAuthenticatedRequest : AuthenticatedRoomRequest<TResponse>
    {
        // Initialization
        public AuthenticatedRoomRequestHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {

        }



        // Request handling
        public async Task<TResponse> Handle(TAuthenticatedRequest request, CancellationToken cancellationToken)
        {
            await Authenticate(request);
            return await HandleOnAuthenticated(request, cancellationToken);
        }

        protected abstract Task<TResponse> HandleOnAuthenticated(TAuthenticatedRequest request, CancellationToken cancellationToken);
    }
}
