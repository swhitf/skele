using Skele.Interop.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    class DefaultVersionStrategy : IVersionStrategy
    {
        private ISqlDialect dialect;

        public DefaultVersionStrategy(ISqlDialect dialect)
        {
            this.dialect = dialect;
        }

        public void PostMigration(TextWriter output, MigrationStep migration)
        {
            var sql = new SqlBuilder(dialect)
                .Insert("__Version")
                .Data(new
                {
                    Major = migration.Target.Major,
                    Minor = migration.Target.Minor,
                    Patch = migration.Target.Revision,
                    InstallDate = DateTime.UtcNow,
                })
                .ToSql();

            output.WriteLine(sql);
            output.WriteLine("GO");
        }

        public void PostScript(TextWriter output)
        {

        }

        public void PreMigration(TextWriter output, MigrationStep migration)
        {

        }

        public void PreScript(TextWriter output)
        {
            var sql = new SqlBuilder(dialect)
                .CreateTable("__Version")
                .Column("Major", "int", nullable: false)
                .Column("Minor", "int", nullable: false)
                .Column("Patch", "int", nullable: false)
                .Column("InstallDate", "datetime", nullable: false)
                .ToSql();

            output.WriteLine(sql);
            output.WriteLine("GO");
        }
    }
}
