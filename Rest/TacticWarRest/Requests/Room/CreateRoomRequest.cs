using MediatR;
using System.ComponentModel.DataAnnotations;
using TacticWar.Rest.ViewModels.Rooms;

namespace TacticWar.Rest.Requests.Room
{
    public record CreateRoomRequest : IRequest<RoomSnapshot>
    {
        public int RoomId { get; init; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        [RegularExpression(@"[a-zA-Z]+")]
        public string PlayerName { get; init; }

        public int SecretCode { get; init; }
    }
}
