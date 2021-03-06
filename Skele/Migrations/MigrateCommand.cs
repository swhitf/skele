﻿using Skele.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migrations
{
    public class MigrateCommand : ICommand
    {
        public Package Package
        {
            get;
            set;
        }

        public Version SourceVersion
        {
            get;
            set;
        }

        public Version TargetVersion
        {
            get;
            set;
        }
    }
}
