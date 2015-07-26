using Skele.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    class PrepareCommand : ICommand
    {
        public string DirectoryPath { get; set; }
    }
}
