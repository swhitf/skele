using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    public interface IScriptEngine
    {
        void Execute(string source);

        void Set(string name, Object obj);
    }
}
