using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TacticWar.Lib.Game.Players;
using Xunit;

namespace TacticWar.Lib.Tests.Attributes
{
    public class FactForAttribute : FactAttribute
    {
        public FactForAttribute(string subjectName = "Constructor", [CallerMemberName] string testMethodName = "")
            => DisplayName = $"{subjectName}_{testMethodName}";
    }
}
