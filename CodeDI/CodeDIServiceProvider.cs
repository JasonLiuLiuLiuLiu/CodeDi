using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDI
{
    public class CodeDIServiceProvider : ICodeDIServiceProvider
    {
        private IServiceCollection _serviceCollection;
        public CodeDIServiceProvider(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public T GetServiceByImplementationType<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public List<T> GetServiceByServiceType<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
