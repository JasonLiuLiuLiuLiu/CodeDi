using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDI
{
    public static class CodeDI
    {
        public static IServiceCollection AddCoreDI(this IServiceCollection services)
        {
            AddService(services);
            return services;
        }

        public static IServiceCollection AddCoreDI(this IServiceCollection services, CoderDIOptions options)
        {
            AddService(services, options);
            return services;
        }

        private static void AddService(IServiceCollection services, CoderDIOptions options = null)
        {
            if (options == null)
            {
                options = CoderDIOptions.GetOptionsWithDefaultValue();

                var assemblies = AssemblyLoader.LoadAssembly(options);

                var allInterfaces = assemblies.SelectMany(u => u.GetTypes()).Where(u => u.IsInterface);


            }
        }
    }
}
