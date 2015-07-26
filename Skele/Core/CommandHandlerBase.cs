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
    abstract class CommandHandlerBase<T> : ICommandHandler<T>
        where T : ICommand
    {
        public CommandHandlerBase(ICommandContext context)
        {
            Context = context;

            DatabaseManagerFactory = context.DatabaseManagerFactory;
            Dispatcher = context.Dispatcher;
            Log = context.Log;
            Output = context.Output;
            Presenter = context.Presenter;
            Project = context.Project;
        }

        protected ICommandContext Context
        {
            get;
            private set;
        }

        protected ICommandDispatcher Dispatcher
        {
            get;
            private set;
        }

        protected IDatabaseManagerFactory DatabaseManagerFactory
        {
            get;
            private set;
        }

        protected TextWriter Log
        {
            get;
            private set;
        }

        protected TextWriter Output
        {
            get;
            private set;
        }

        protected IPresenter Presenter
        {
            get;
            private set;
        }

        protected Project Project
        {
            get;
            private set;
        }

        protected ProjectTarget GetTarget()
        {
            var tn = Context.TargetName;

            if (string.IsNullOrEmpty(tn))
            {
                return Project.DefaultTarget;
            }
            else
            {
                if (Project.Targets.Contains(tn))
                {
                    return Project.Targets[tn];
                }
                else
                {
                    throw new InvalidOperationException("Specified project target does not exist: " + tn);
                }
            }
        }

        protected IDatabaseSession GetDatabaseSession(ProjectTarget target = null)
        {
            if (target == null)
            {
                target = GetTarget();
            }

            var manager = DatabaseManagerFactory.Create(target.ConnectionString);
            var session = manager.Open(target.Database);

            return session;
        }

        protected IDatabaseManager GetDatabaseManager(ProjectTarget target = null)
        {
            if (target == null)
            {
                target = GetTarget();
            }

            return DatabaseManagerFactory.Create(target.ConnectionString);
        }

        public abstract int Execute(T input);

        protected virtual int CancelledResult(string message = null)
        {
            if (message != null)
            {
                Console.WriteLine(message);
            }

            return 1;
        }

        protected virtual int FailureResult(string message = null)
        {
            if (message != null)
            {
                Console.WriteLine(message);
            }

            return 2;
        }

        protected virtual int SuccessResult(string message = null)
        {
            if (message != null)
            {
                Console.WriteLine(message);
            }

            return 0;
        }
    }
}
