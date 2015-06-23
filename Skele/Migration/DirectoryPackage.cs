using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    class DirectoryPackage : StructuredPackage
    {
        private string directoryPath;

        public DirectoryPackage(string directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public override Uri Location
        {
            get { return Location; }
        }

        protected override StructuredPackage.Node GetRoot()
        {
            return new DirectoryNode(new DirectoryInfo(directoryPath));
        }

        protected override Resource CreateResource(StructuredPackage.Node node)
        {
            throw new NotImplementedException();
        }

        class DirectoryNode : Node
        {
            private DirectoryInfo info;

            public DirectoryNode(DirectoryInfo info)
            {
                this.info = info;
            }

            public override string Name
            {
                get { return info.Name; }
            }

            public override IEnumerable<Node> GetChildren()
            {
                return info.EnumerateDirectories()
                    .Select(x => new DirectoryNode(x));
            }

            public override IEnumerable<Resource> GetResources()
            {
                var resources = new List<Resource>();
                WalkDown(info, new string[0], resources);

                return resources;
            }

            private void WalkDown(DirectoryInfo level, IEnumerable<string> path, List<Resource> results)
            {
                foreach (var fi in level.EnumerateFiles())
                {
                    results.Add(new FileResource(string.Join(".", path), fi));
                }

                foreach (var di in level.EnumerateDirectories())
                {
                    WalkDown(di, path.Concat(new[] { di.Name }), results);
                }
            }
        }

        class FileResource : Resource
        {
            private FileInfo info;
            private string name;
            private string type;

            public FileResource(string logicalPath, FileInfo info)
            {
                this.info = info;

                var fileName = Path.GetFileNameWithoutExtension(info.Name);

                this.name = logicalPath.Length == 0 ? fileName : logicalPath + "." + fileName;
                this.type = Path.GetExtension(info.Name).ToLower();
            }

            public override string Name
            {
                get { return name; }
            }

            public override string Type
            {
                get { return type; }
            }

            public override Stream Open()
            {
                return info.OpenRead();
            }
        }

    }
}
