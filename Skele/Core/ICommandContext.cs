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
    interface ICommandContext
    {
        IDatabaseManagerFactory DatabaseManagerFactory
        {
            get;
        }

        ICommandDispatcher Dispatcher
        {
            get;
        }

        TextWriter Log
        {
            get;
        }

        TextWriter Output
        {
            get;
        }

        PackageFactory Packages
        {
            get;
        }

        IPresenter Presenter
        {
            get;
        }

        Project Project
        {
            get;
        }

        string TargetName
        {
            get;
        }
    }
}
