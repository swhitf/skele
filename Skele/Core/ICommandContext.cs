using Skele.Interop;
using Skele.Migration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    public interface ICommandContext
    {
        TextWriter Log
        {
            get;
        }

        TextWriter Output
        {
            get;
        }

        Project Project
        {
            get;
        }

        ProjectTarget ActiveTarget
        {
            get;
        }

        IDatabaseSession GetDatabaseSession(ProjectTarget target = null);

        IDatabaseManager GetDatabaseManager(ProjectTarget target = null);
    }
}
