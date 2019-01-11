using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDI
{
    interface ICodeDiServiceProvider
    {
        T GetServiceByImplementationType<T>() where T : class;
        List<T> GetServiceByServiceType<T>() where T : class;
    }
}
