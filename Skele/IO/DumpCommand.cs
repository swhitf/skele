using Skele.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.IO
{
    class DumpCommand : ICommand
    {
        public string OutputPath
        {
            get;
            set;
        }

        public string[] TableList
        {
            get;
            set;
        }

        public string TargetFormat
        {
            get;
            set;
        }
    }
}
