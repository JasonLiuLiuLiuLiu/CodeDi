using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDi
{
    public class CoderDiOptions
    {
        public ServiceLifetime DefaultServiceLifetime { get; set; } = ServiceLifetime.Scoped;

        private string[] _assemblyNames;
        public string[] AssemblyNames
        {
            get => _assemblyNames ?? new[] { "*" };
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
        public Dictionary<string, ServiceLifetime> ServiceLifeTimeMappings { get; set; }
    }
}
