using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Core
{
    public class ProjectTargetCollection : KeyedCollection<string, ProjectTarget>
    {
        public ProjectTargetCollection()
        {

        }

        public ProjectTargetCollection(IEnumerable<ProjectTarget> values)
        {
            foreach (var item in values)
            {
                Add(item);
            }
        }

        protected override string GetKeyForItem(ProjectTarget item)
        {
            return item.Name;            
        }
    }
}
