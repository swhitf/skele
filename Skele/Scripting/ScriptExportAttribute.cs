using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ScriptExportAttribute : Attribute
    {
        // This is a positional argument
        public ScriptExportAttribute(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }
    }
}
