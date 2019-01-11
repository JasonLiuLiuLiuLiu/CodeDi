using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDI
{
    public interface ICodeDiServiceProvider
    {
        T GetService<T>(string name) where T : class;
    }
}
