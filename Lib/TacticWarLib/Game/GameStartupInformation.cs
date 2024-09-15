using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game;

public record GameStartupInformation(PlayersInfoCollection PlayersInfo, int RoomId);
