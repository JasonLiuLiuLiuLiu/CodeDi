using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDi
{
    public interface ICodeDiServiceProvider
    {
        T GetService<T>() where T : class;
        T GetService<T>(string name) where T : class;
    }
}
