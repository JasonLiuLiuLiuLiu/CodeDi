using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDI
{
    public class CodeDiServiceProvider : ICodeDiServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;
        public CodeDiServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetService<T>() where T : class
        {
            return ((IEnumerable<T>)_serviceProvider.GetService(typeof(IEnumerable<T>))).FirstOrDefault();
        }

        public T GetService<T>(string name) where T : class
        {
            return ((IEnumerable<T>)_serviceProvider.GetService(typeof(IEnumerable<T>))).FirstOrDefault(u => Regex.IsMatch(u.GetType().Name, name));
        }
    }
}
