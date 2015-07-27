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
        private INamedTypeFactory<IDatabaseDriver> driverFactory;

        public DefaultCommandContext(INamedTypeFactory<IDatabaseDriver> driverFactory)
        {
            this.driverFactory = driverFactory;

            Log = TextWriter.Null;
            Output = TextWriter.Null;
        }

        public Project Project
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

        public ProjectTarget ActiveTarget
        {
            get;
            private set;
        }

        public void SetTarget(string targetName)
        {
            if (string.IsNullOrEmpty(targetName))
            {
                ActiveTarget = Project.DefaultTarget;
            }
            else
            {
                if (Project.Targets.Contains(targetName))
                {
                    ActiveTarget = Project.Targets[targetName];
                }
                else
                {
                    throw new InvalidOperationException("Specified project target does not exist: " + targetName);
                }
            }
        }
        
        public IDatabaseSession GetDatabaseSession(ProjectTarget target = null)
        {
            if (target == null)
            {
                target = ActiveTarget;
            }

            var driver = driverFactory.Create(target.DriverName);
            var manager = driver.Connect(target.ConnectionString);
            var session = manager.Open(target.Database);

            return session;
        }

        public IDatabaseManager GetDatabaseManager(ProjectTarget target = null)
        {
            if (target == null)
            {
                target = ActiveTarget;
            }

            var driver = driverFactory.Create(target.DriverName);
            return driver.Connect(target.ConnectionString);
        }
    }
}
