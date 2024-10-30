using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TacticWar.Lib.Game.Players;
using TacticWar.Lib.Game.Rooms.Abstractions;
using TacticWar.Rest.RequestsHandlers.Game;
using TacticWar.Rest.ViewModels;
using TacticWar.Rest.ViewModels.Services;

namespace TacticWar.Rest.Hubs;

public class GameHub : Hub
{
    public async IAsyncEnumerable<GameSnapshot> GetSnapshots(string playerName,
                                                             int roomId,
                                                             int playerSecret,
                                                             [FromServices] IRoomsManager roomsManager,
                                                             [FromServices] IViewModelsLocator viewModelsLocator,
                                                             [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var x = await AuthenticationEx.Authenticate(roomsManager, viewModelsLocator, roomId, playerName, playerSecret);
        await foreach (var snapshot in x.viewModelService.GetGameSnapshot(x.playerColor)
                                                       .ToAsyncEnumerable()
                                                       .WithCancellation(cancellationToken))
        {
            yield return snapshot;
        }
    }
}
