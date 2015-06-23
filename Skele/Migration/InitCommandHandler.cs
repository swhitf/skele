using Skele.Core;
using Skele.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    class InitCommandHandler : CommandHandlerBase<InitCommand>
    {
        private DatabaseDriver driver;

        public InitCommandHandler(DatabaseDriver driver)
        {
            this.driver = driver;
        }

        public override void Execute(InitCommand input)
        {
            var session = GetOrCreateSession("Katana");
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
        }

        private DatabaseSession GetOrCreateSession(string name)
        {
            if (!driver.Exists(name))
            {
                Log.WriteLine("Database '{0}' not found, creating...", name);
                return driver.Create(name);
            }
            else
            {
                return driver.Open(name);
            }
        }
    }
}
