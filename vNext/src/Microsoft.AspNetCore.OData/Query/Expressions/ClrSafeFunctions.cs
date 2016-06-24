using System;
using System.Diagnostics.Contracts;

namespace Microsoft.AspNetCore.OData.Query.Expressions
{
    /// <summary>
    /// This class contains safe equivalents of CLR functions that
    /// could throw exceptions at runtime.
    /// </summary>
    internal class ClrSafeFunctions
    {
        public static string SubstringStart(string str, int startIndex)
        {
            Contract.Assert(str != null);

            if (startIndex < 0)
            {
                startIndex = 0;
            }

            // String.Substring(int) accepts startIndex==length
            return startIndex <= str.Length
                    ? str.Substring(startIndex)
                    : string.Empty;
        }

        public static string SubstringStartAndLength(string str, int startIndex, int length)
        {
            Contract.Assert(str != null);

            if (startIndex < 0)
            {
                startIndex = 0;
            }

            int strLength = str.Length;

            // String.Substring(int, int) accepts startIndex==length
            if (startIndex > strLength)
            {
                return string.Empty;
            }

            length = Math.Min(length, strLength - startIndex);
            return length >= 0
                    ? str.Substring(startIndex, length)
                    : string.Empty;
        }
    }
}