using System.Runtime.CompilerServices;
using Xunit;

namespace TacticWar.Lib.Tests.Attributes
{
    public class FactForAttribute : FactAttribute
    {
        public FactForAttribute(string subjectName = "Constructor", [CallerMemberName] string testMethodName = "")
            => DisplayName = $"{subjectName}_{testMethodName}";
    }
}
