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
        public static IServiceCollection AddCoreDI(this IServiceCollection services, CoderDIOptions options = null)
        {
            return new CodeDIService(services, options).AddService();
        }

    }
}
