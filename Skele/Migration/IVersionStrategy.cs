using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    public interface IVersionStrategy
    {
        void PostMigration(TextWriter output, MigrationStep migration);

        void PostScript(TextWriter output);

        void PreMigration(TextWriter output, MigrationStep migration);

        void PreScript(TextWriter output);
    }
}
