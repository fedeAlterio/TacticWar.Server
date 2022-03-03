using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Game.Deck.Objectives.Builders.Abstractions
{
    public interface IObjectivesBuilder
    {
        public IEnumerable<IObjective> BuildObjectvies();
    }
}
