﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CodeDi
{
    public static class StringExtensions
    {
        //https://github.com/dotnetcore/AspectCore-Framework/blob/master/core/src/AspectCore.Core/Configuration/Extensions/StringExtensions.cs
        public static unsafe bool Matches(this string input, string pattern)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentNullException(nameof(pattern));

            bool matched = false;

            fixed (char* p_wild = pattern)
            fixed (char* p_str = input)
            {
                char* wild = p_wild, str = p_str, cp = null, mp = null;

                while ((*str) != 0 && (*wild != '*'))
                {
                    if ((*wild != *str) && (*wild != '?')) return matched; wild++; str++;
                }

                while (*str != 0)
                {
                    if (*wild == '*') { if (0 == (*++wild)) return (matched = true); mp = wild; cp = str + 1; }
                    else if ((*wild == *str) || (*wild == '?')) { wild++; str++; } else { wild = mp; str = cp++; }
                }

                while (*wild == '*') wild++; return (*wild) == 0;
            }
        }

        public static string GetAssemblyName(this Assembly assembly)
        {
            return assembly.GetName().Name;
        }
    }
}
