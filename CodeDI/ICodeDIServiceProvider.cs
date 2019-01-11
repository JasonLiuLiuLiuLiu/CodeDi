using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDI
{
    public interface ICodeDiServiceProvider
    {
        T GetService<T>() where T : class;
        T GetService<T>(string name) where T : class;
    }
}
