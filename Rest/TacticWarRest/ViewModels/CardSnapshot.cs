using TacticWar.Lib.Game.Deck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TacticWar.Rest.ViewModels
{
    public class CardSnapshot
    {
        public CardSnapshot(TerritoryCard card)
        {
            TerritoryId = card.Id;
            ArmyType = card.ArmyType;
            TerritoryName = card.Territory?.Name;
            Id = card.Id;
        }

        public int Id { get; init; }
        public int TerritoryId { get; init; }
        public ArmyType ArmyType { get; init; }
        public string TerritoryName { get; init; }
    }
}
