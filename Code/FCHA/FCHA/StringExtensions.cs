using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FCHA
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNull(this string str)
        {
            return null == str;
        }

        public static bool IsEmpty(this string str)
        {
            return string.Empty == str;
        }
    }
}
