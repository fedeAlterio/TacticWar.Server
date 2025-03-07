﻿using TacticWar.Lib.Game.Players;

namespace TacticWar.Lib.Game.Rooms
{
    public class WaitingPlayer
    {
        public string Name { get; init; }
        public PlayerColor Color { get; init; }
        public int SecretCode { get; init; }
        public bool IsBot { get; init; }
    }
}
