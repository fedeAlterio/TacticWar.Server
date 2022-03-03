using System;
using System.Collections.Generic;
using System.Text;
using TacticWar.Lib.Game.Players;
using TacticWar.Rest.ViewModels.Phases;

namespace TacticWar.Rest.ViewModels
{
    public class GameSnapshot
    {
        public MapVM? Map { get; init; }
        public List<PrivatePlayerVM>? Players { get; init; }
        public PlayerColor CurrentPlayerColor { get; init; }
        public double CurrentPlayerTurnStartOffsetMs { get; init; }
        public int CurrentTurnLengthMs { get; init; }
        public ArmiesPlacementInfoVM? ArmiesPlacementInfo { get; init; }
        public AttackPhaseInfoVm? AttackPhaseInfo { get; init; }
        public MovementPhaseInfoVM? MovementPhaseInfo { get; init; }
        public PlacementAfterAttackInfoVM? PlacementAfterAttackInfo { get; init; }
        public VictoryPhaseVM? VictoryPhaseInfo { get; init; }
        public int VersionId { get; set; }
        public List<CardSnapshot>? Cards { get; init; }
    }
}
