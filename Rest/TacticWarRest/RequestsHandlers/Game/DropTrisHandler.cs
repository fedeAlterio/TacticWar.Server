using TacticWar.Lib.Game.Exceptions;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.Requests.Game;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.RequestsHandlers.Game
{
    public record DropTrisHandler : AuthenticatedRoomRequestHandler<DropTrisRequest>
    {
        // Initialization
        public DropTrisHandler(IRoomsManager roomsManager, IViewModelsLocator viewModelsLocator) : base(roomsManager, viewModelsLocator)
        {
        }



        // Core
        protected override async Task HandleOnAuthenticated(DropTrisRequest request, CancellationToken cancellationToken)
        {
            var cards = GameManager.TurnManager.TurnInfo.CurrentActionPlayer.Cards;
            var cardsToAdd = cards.Where(card => request.CardsIds.Contains(card.Id)).ToList();
            if (cardsToAdd.Count != request.CardsIds.Count)
                throw new GameException($"Current player does not have all these cards");

            await GameManager.GameApi.PlayTris(PlayerColor, cardsToAdd);
        }
    }
}
