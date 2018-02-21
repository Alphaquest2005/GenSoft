using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class StringExtensions
    {
        public static string TrimEnd(this string input, string suffixToRemove,
            StringComparison comparisonType = StringComparison.CurrentCulture)
        {

            if (input != null && suffixToRemove != null
                && input.EndsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }
            return input;
        }

        public static string TrimStart(this string input, string suffixToRemove,
            StringComparison comparisonType = StringComparison.CurrentCulture)
        {

            if (input != null && suffixToRemove != null
                && input.StartsWith(suffixToRemove, comparisonType))
            {
                return input.Substring(suffixToRemove.Length);
            }
            return input;
        }
    }
}
