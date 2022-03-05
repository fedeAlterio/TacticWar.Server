using System.Runtime.CompilerServices;
using Xunit;

namespace TacticWar.Lib.Tests.Attributes
{
    public class TheoryForAttribute : TheoryAttribute
    {
        public TheoryForAttribute(string methodName = "Constructor", [CallerMemberName] string testMethodName = "")
            => DisplayName = $"{methodName}_{testMethodName}";
    }
}
