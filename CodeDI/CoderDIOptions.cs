using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDI
{
    public class CoderDiOptions
    {
        private int? _defaultServiceLifetime;
        public int DefaultServiceLifetime
        {
            get => _defaultServiceLifetime ?? 1;
            set => _defaultServiceLifetime = value;
        }

        private string[] _assemblyNames;
        public string[] AssemblyNames
        {
            get => _assemblyNames ?? new[] { ".*" };
            set => _assemblyNames = value;
        }

        private string[] _assemblyPaths;
        public string[] AssemblyPaths
        {
            get => _assemblyPaths ?? new[] { AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath, AppDomain.CurrentDomain.DynamicDirectory };
            set => _assemblyPaths = value;
        }
        public string[] IgnoreAssemblies { get; set; }
        public bool IncludeSystemAssemblies { get; set; }

        public string[] IgnoreInterface { get; set; }

        public Dictionary<string, string> InterfaceMappings { get; set; }
        public Dictionary<string, int> ServiceLifeTimeMappings { get; set; }
    }
}
