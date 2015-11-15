using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    public class Project
    {
        public const string DEFAULT_FILE = "skele.json";

        public Project()
        {
            Targets = new ProjectTargetCollection();
            MigrationsPath = "Migrations";
            ScriptsPath = "Scripts";
            SnapshotsPath = "Snapshots";
        }

        public ProjectTarget DefaultTarget
        {
            get { return Targets["default"]; }
        }

        public string Location
        {
            get;
            set;
        }

        public string MigrationsPath
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string ScriptsPath
        {
            get;
            set;
        }

        public string SnapshotsPath
        {
            get;
            set;
        }

        public ProjectTargetCollection Targets
        {
            get;
            private set;
        }

        public static Project Load(string path)
        {
            var ps = new ProjectSerializer();
            return ps.Deserialize(Path.Combine(path, DEFAULT_FILE));
        }

        public static Project TryLoad(string inputPath)
        {
            try
            {
                var actualPath = Path.Combine(inputPath, DEFAULT_FILE);
                if (File.Exists(actualPath))
                {
                    var ps = new ProjectSerializer();
                    return ps.Deserialize(actualPath);
                }
            }
            catch
            {
            }

            return null;
        }
    }
}
