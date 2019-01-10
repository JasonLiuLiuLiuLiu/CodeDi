using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDI
{
    interface ICodeDIServiceProvider
    {
        T GetServiceByImplementationType<T>() where T : class;
        List<T> GetServiceByServiceType<T>() where T : class;
    }
}
