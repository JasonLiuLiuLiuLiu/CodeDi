using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDI
{
    public class CoderDIOptions
    {
        public string[] AssemblyNames { get; set; }
        public string[] AssemblyPath { get; set; }
        public bool IncludeSystemAssembly { get; set; }


        public static CoderDIOptions GetOptionsWithDefaultValue()
        {
            return new CoderDIOptions()
            {
                //This means Anything https://stackoverflow.com/questions/1798285/parsing-quantifier-x-y-following-nothing
                AssemblyNames = new[] { ".*" },

                AssemblyPath = new []{AppDomain.CurrentDomain.BaseDirectory,AppDomain.CurrentDomain.RelativeSearchPath,AppDomain.CurrentDomain.DynamicDirectory}
                
            };
        }
    }
}
