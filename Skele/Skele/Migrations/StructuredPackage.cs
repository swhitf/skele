using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migrations
{
    public abstract partial class StructuredPackage : Package
    {
        protected abstract Node GetRoot();

        protected abstract Resource CreateResource(Node node);

        public override IReadOnlyList<Migration> GetMigrations()
        {
            var migsRoot = TryGetSection("Migrations");
            var results = new List<Migration>();

            foreach (var migrNode in migsRoot)
            {
                Version version;
                if (!Version.TryParse(migrNode.Name, out version))
                    continue;

                var migration = new Migration(version);
                migration.AddRange(migrNode.GetResources());

                results.Add(migration);
            }

            return results;
        }

        private Node TryGetSection(string name, bool defaultToRoot = true)
        {
            var root = GetRoot();
            var target = root.FindChild(name);

            if (target != null)
                return target;

            if (defaultToRoot)
                return root;

            return null;            
        }

        protected abstract class Node : IEnumerable<Node>
        {
            public abstract string Name { get; }

            public abstract IEnumerable<Node> GetChildren();

            public abstract IEnumerable<Resource> GetResources();

            public Node FindChild(string name, bool fullSearch = true)
            {
                var children = GetChildren();

                var node = children.FirstOrDefault(x =>
                    string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase)
                );

                if (fullSearch && node == null)
                {
                    foreach (var child in children)
                    {
                        node = child.FindChild(name, fullSearch);

                        if (node != null)
                            break;
                    }
                }

                return node;
            }

            public IEnumerator<Node> GetEnumerator()
            {
                return GetChildren().GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
