using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeDi
{
    class AssemblyLoader
    {
        public static IList<Assembly> LoadAssembly(CoderDiOptions options)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            if (!options.IncludeSystemAssemblies)
            {
                assemblies = assemblies.Where(assembly => assembly.ManifestModule.Name != "<In Memory Module>"
                                                          && !assembly.GetAssemblyName().StartsWith("System")
                                                          && !assembly.GetAssemblyName().StartsWith("Microsoft")
                                                          && assembly.Location.IndexOf("App_Web", StringComparison.Ordinal) == -1
                                                          && assembly.Location.IndexOf("App_global", StringComparison.Ordinal) == -1
                                                          && assembly.GetAssemblyName().IndexOf("CppCodeProvider", StringComparison.Ordinal) == -1
                                                          && assembly.GetAssemblyName().IndexOf("WebMatrix", StringComparison.Ordinal) == -1
                                                          && assembly.GetAssemblyName().IndexOf("SMDiagnostics", StringComparison.Ordinal) == -1
                                                          && assembly.GetAssemblyName().IndexOf("Newtonsoft", StringComparison.Ordinal) == -1
                                                          && !string.IsNullOrEmpty(assembly.Location)).ToList();
            }
            assemblies.AddRange(LoadFromPaths(options.AssemblyPaths).Where(toAdd => assemblies.All(u => u.GetAssemblyName() != toAdd.GetAssemblyName())));
            return assemblies.Where(u => options.AssemblyNames.Any(name => u.GetAssemblyName().Matches(name)))
                .Where(u => options.IgnoreAssemblies == null || options.IgnoreAssemblies.All(ignore => u.GetAssemblyName().Matches(ignore) == false))
                .Distinct().ToList();

        }

        private static IEnumerable<Assembly> LoadFromPaths(string[] paths)
        {
            var dllPath = paths.Where(u => !string.IsNullOrEmpty(u)).SelectMany(u => Directory.GetFiles(u, "*.dll"));
            foreach (var path in dllPath)
            {
                yield return Assembly.LoadFile(path);
            }
        }
    }
}
