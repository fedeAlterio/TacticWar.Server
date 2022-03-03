using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TacticWar.Lib.Services.Translation
{
    public interface ITranslationService
    {
        string Get(string str);
    }
}
