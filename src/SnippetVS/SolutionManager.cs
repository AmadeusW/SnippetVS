using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetVS
{
    class SolutionManager
    {
        public static Workspace CurrentWorkspace { get; internal set; }

        public static Solution CurrentSolution
        {
            get { return CurrentWorkspace?.CurrentSolution; }
        }
    }
}
