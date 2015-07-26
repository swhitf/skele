using Skele.Core;
using Skele.Interop;
using Skele.Interop.MetaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    class InitCommandHandler : CommandHandlerBase<InitCommand>
    {
        public InitCommandHandler(ICommandContext context)
            : base(context)
        {
        }

        public override int Execute(InitCommand input)
        {
            var session = GetOrCreateSession();
            var metadata = session.Describe();

            if (!metadata.Tables.Contains("__Version"))
            {
                Log.WriteLine("Version table not found, creating...");

                var table = new TableDescriptor();
                table.Name = "__Version";
                table.Columns.Add(new ColumnDescriptor { Name = "Major", DataType = typeof(int) });
                table.Columns.Add(new ColumnDescriptor { Name = "Minor", DataType = typeof(int) });
                table.Columns.Add(new ColumnDescriptor { Name = "Patch", DataType = typeof(int) });
                table.Columns.Add(new ColumnDescriptor { Name = "InstallDate", DataType = typeof(DateTime) });

                session.CreateTable(table);
            }

            Log.WriteLine("Database initialized.");
            return SuccessResult();
        }

        private IDatabaseSession GetOrCreateSession()
        {
            var target = Context.ActiveTarget;
            var manager = Context.GetDatabaseManager(target);

            if (!manager.Exists(target.Database))
            {
                Log.WriteLine("Database '{0}' not found, creating...", target.Database);
                return manager.Create(target.Database);
            }
            else
            {
                return manager.Open(target.Database);
            }
        }
    }
}
