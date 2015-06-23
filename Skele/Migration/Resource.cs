using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    public abstract class Resource
    {
        public abstract string Name { get; }

        public abstract string Type { get; }

        public abstract Stream Open();

        public string ReadFull()
        {
            using (var s = new StreamReader(Open()))
            {
                return s.ReadToEnd();
            }
        }
    }
}
