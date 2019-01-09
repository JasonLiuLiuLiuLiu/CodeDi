using System;
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

        public static IServiceCollection AddCoreDI(this IServiceCollection services,CoderDIOptions options)
        {
            AddService(services,options);
            return services;
        }

        private static void AddService(IServiceCollection services, CoderDIOptions options=null)
        {

        }

    }
}
