using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    public class ProjectSerializer
    {
        public ProjectSerializer()
        {

        }

        public Project Deserialize(string filePath)
        {
            using (var tr = new StreamReader(File.OpenRead(filePath)))
            {
                var project = Deserialize(tr);
                project.Location = Path.GetFullPath(Path.GetDirectoryName(filePath));

                return project;
            }
        }

        public Project Deserialize(TextReader reader)
        {
            var jss = new JsonSerializer();
            jss.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jss.Formatting = Formatting.Indented;

            var wrapper = (ProjectWrapper)jss.Deserialize(reader, typeof(ProjectWrapper));
            var project = wrapper.Project.ToProject();

            return project;
        }

        public void Serialize(Project project, string filePath)
        {
            using (var tw = new StreamWriter(File.OpenWrite(filePath)))
            {
                Serialize(project, tw);
            }
        }

        public void Serialize(Project project, TextWriter writer)
        {
            var wrapper = new ProjectWrapper
            {
                Version = 1.0,
                Project = new ProjectMomento(project),
            };
            
            var jss = new JsonSerializer();
            jss.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jss.Formatting = Formatting.Indented;
            jss.Serialize(writer, wrapper, typeof(ProjectWrapper));
        }

        private class ProjectWrapper
        {
            public double Version;
            public ProjectMomento Project;
        }

        private class ProjectMomento
        {
            public ProjectMomento()
            {

            }

            public ProjectMomento(Project project)
            {
                Name = project.Name;
                MigrationsPath = project.MigrationsPath;
                SnapshotsPath = project.SnapshotsPath;
                Targets = project.Targets.ToDictionary(
                    x => x.Name, x => x.ConnectionString);
            }

            public string Name;
            public string MigrationsPath;
            public string SnapshotsPath;
            public Dictionary<string, string> Targets;

            public Project ToProject()
            {
                var proj = new Project
                {
                    Name = Name,
                    MigrationsPath = MigrationsPath,
                    SnapshotsPath = SnapshotsPath,
                };

                foreach (var entry in Targets)
                {
                    proj.Targets.Add(new ProjectTarget(entry.Key, entry.Value));
                }

                return proj;
            }
        }
    }
}
