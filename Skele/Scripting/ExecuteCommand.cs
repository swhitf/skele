using Skele.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Scripting
{
    class ExecuteCommand : ICommand
    {
        public ExecuteCommand()
        {
            FilePath = "test.js";
        }

        public string FilePath
        {
            get;
            set;
        }
    }
}
