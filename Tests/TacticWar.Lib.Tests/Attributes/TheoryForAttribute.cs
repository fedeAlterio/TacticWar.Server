using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TacticWar.Lib.Tests.Attributes
{
    public class TheoryForAttribute : TheoryAttribute
    {
        public TheoryForAttribute(string methodName = "Constructor", [CallerMemberName] string testMethodName = "")
            => DisplayName = $"{methodName}_{testMethodName}";
    }
}
