using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    public class PackageFactory
    {
        public virtual Package Create(string path)
        {
            if (Directory.Exists(path))
            {
                return new DirectoryPackage(path);
            }

            throw new InvalidOperationException();
        }

        //public virtual Package Create(Uri uri)
        //{
            
        //}
    }
}
