using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeDI
{
    class AssemblyLoader
    {
        public static IList<Assembly> LoadAssembly(CoderDiOptions options)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            if (!options.IncludeSystemAssemblies)
            {
                assemblies = assemblies.Where(assembly => assembly.ManifestModule.Name != "<In Memory Module>"
                                                          && !assembly.FullName.StartsWith("System")
                                                          && !assembly.FullName.StartsWith("Microsoft")
                                                          && assembly.Location.IndexOf("App_Web", StringComparison.Ordinal) == -1
                                                          && assembly.Location.IndexOf("App_global", StringComparison.Ordinal) == -1
                                                          && assembly.FullName.IndexOf("CppCodeProvider", StringComparison.Ordinal) == -1
                                                          && assembly.FullName.IndexOf("WebMatrix", StringComparison.Ordinal) == -1
                                                          && assembly.FullName.IndexOf("SMDiagnostics", StringComparison.Ordinal) == -1
                                                          && assembly.FullName.IndexOf("Newtonsoft", StringComparison.Ordinal) == -1
                                                          && !string.IsNullOrEmpty(assembly.Location)).ToList();
            }
            assemblies.AddRange(LoadFromPaths(options.AssemblyPaths).Where(toAdd=>assemblies.All(u=>u.FullName!=toAdd.FullName)));
            return assemblies.Where(u => options.AssemblyNames.Any( name => Regex.IsMatch(u.FullName, name)))
                .Where(u => options.IgnoreAssemblies == null || options.IgnoreAssemblies.All(ignore => Regex.IsMatch(u.FullName, ignore) == false))
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
