using TacticWar.Lib.Game.Deck.Abstractions;
using TacticWar.Lib.Game.Deck.Objectives.Abstractions;
using TacticWar.Lib.Game.Players.Abstractions;

namespace TacticWar.Lib.Game.Deck.Objectives.Decks
{
    public class AlwaysWinObjectiveDeckBuilder
    {
        public IDeck<IObjective> NewDeck()
        {
            var objectives = Enumerable.Range(1, 10).Select(_ => new AlwaysWinObjective());
            return new Deck<IObjective>(objectives);
        }



        private class AlwaysWinObjective : IObjective
        {
            public string Description => "Easy Win";

            public bool IsCompleted(IPlayer player)
            {
                return true;
            }
        }
    }
}
