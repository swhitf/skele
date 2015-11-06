using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    public class ConsoleAdapter
    {
        [ScriptExport("log")]
        public void Log(IScriptContext context, string output)
        {
            Console.WriteLine(output);
        }
    }
}
