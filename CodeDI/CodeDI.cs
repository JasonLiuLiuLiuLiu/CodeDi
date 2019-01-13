using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDi
{
    public static class CodeDi
    {
        public static IServiceCollection AddCoreDi(this IServiceCollection services, CodeDiOptions options = null)
        {
            return new CodeDiService(services, options).AddService();
        }

        public static IServiceCollection AddCoreDi(this IServiceCollection services, Action<CodeDiOptions> actionOptions)
        {
            CodeDiOptions options = new CodeDiOptions();
            actionOptions(options);
            return new CodeDiService(services, options).AddService();
        }
    }
}
