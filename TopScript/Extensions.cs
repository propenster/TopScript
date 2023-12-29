using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopScript
{
    public static class Extensions
    {
        public static string ToDelimitedString<T>(this IEnumerable<T> values)
        {
            return values.ToDelimitedString(x => x.ToString(),
            CultureInfo.CurrentCulture.TextInfo.ListSeparator);
        }
        public static string ToDelimitedString<T>(
        this IEnumerable<T> source, Func<T, string> converter)
        {
            return source.ToDelimitedString(converter,
                CultureInfo.CurrentCulture.TextInfo.ListSeparator);
        }


        public static string ToDelimitedString<T>(this IEnumerable<T> source,
            Func<T, string> converter, string separator)
        {
            return string.Join(separator, source.Select(converter).ToArray());
        }


    }
}
