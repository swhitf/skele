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
    class DefaultCommandContext : ICommandContext
    {
        public IDatabaseManagerFactory DatabaseManagerFactory
        {
            get;
            set;
        }

        public TextWriter Log
        {
            get;
            set;
        }

        public TextWriter Output
        {
            get;
            set;
        }

        public PackageFactory Packages
        {
            get;
            set;
        }

        public IPresenter Presenter
        {
            get;
            set;
        }

        public Project Project
        {
            get;
            set;
        }

        public string TargetName
        {
            get;
            set;
        }


        public ICommandDispatcher Dispatcher
        {
            get;
            set;
        }
    }
}
