using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDI
{
    public class CoderDIOptions
    {
        public string[] AssembleNames { get; set; }

        public static CoderDIOptions GetOptionsWithDefaultValue()
        {
            return new CoderDIOptions()
            {
                AssembleNames = new[] { "*" }
            };
        }
    }
}
