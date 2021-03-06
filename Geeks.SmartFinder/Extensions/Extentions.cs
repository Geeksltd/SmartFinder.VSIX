namespace System
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Geeks.VSIX.SmartFinder.FileFinder;

    public static class Extensions
    {
        public static string ZebblifyFileName(this string fileName) => fileName + ".zbl";

        public static string WrapInQuatation(this string path) => "\"" + path + "\"";

        public static string FormatWith(this string format, object arg, params object[] additionalArgs)
        {
            if (additionalArgs == null || additionalArgs.Length == 0)
            {
                return string.Format(format, arg);
            }
            else
            {
                return string.Format(format, new object[] { arg }.Concat(additionalArgs).ToArray());
            }
        }

        public static bool HasValue(this string text) => !string.IsNullOrEmpty(text);

        public static string Or(this string text, string ifEmpty) => text.HasValue() ? text : ifEmpty;

        public static string TrimStart(this string text, string textToTrim, bool ignoreCase = false)
        {
            if (text.StartsWith(textToTrim, ignoreCase, Globalization.CultureInfo.InvariantCulture))
            {
                return text.Substring(textToTrim.Length);
            }
            else
            {
                return text;
            }
        }

        public static string TrimEnd(this string text, string textToTrim)
        {
            if (text.EndsWith(textToTrim))
            {
                return text.TrimEnd(textToTrim.Length);
            }
            else
            {
                return text;
            }
        }

        public static string TrimEnd(this string text, int numberOfCharacters)
        {
            if (numberOfCharacters < 0)
                throw new ArgumentException("numberOfCharacters must be greater than 0.");

            if (numberOfCharacters == 0) return text;

            if (text.IsEmpty() || text.Length <= numberOfCharacters)
                return string.Empty;

            return text.Substring(0, text.Length - numberOfCharacters);
        }

        public static bool IsEmpty(this string text) => string.IsNullOrEmpty(text);

        public static string Remove(this string text, params string[] substringsToExclude)
        {
            if (text.IsEmpty()) return text;

            var result = text;

            foreach (var sub in substringsToExclude)
                result = result.Replace(sub, "");

            return result;
        }

        public static string ToString<T>(this IEnumerable<T> list, string seperator)
        {
            return ToString(list, seperator, seperator);
        }

        public static string ToString<T>(this IEnumerable<T> list, string seperator, string lastSeperator)
        {
            if (list == null || list.Any() == false) return string.Empty;

            var castedItems = list as IEnumerable<object>;
            if (castedItems == null || castedItems.Any() == false) return string.Empty;

            var items = castedItems.ToArray();

            var result = new StringBuilder();
            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];

                if (item == null) result.Append("{NULL}");
                else result.Append(item.ToString());

                if (i < items.Length - 2)
                    result.Append(seperator);

                if (i == items.Length - 2)
                    result.Append(lastSeperator);
            }

            return result.ToString();
        }

        internal static bool IsMSharp(this Item item)
        {
            if (item.FileName.Contains("UI") || item.FileName.Contains("Model"))
                if (File.Exists(item.BasePath + @"\#Model.csproj") || File.Exists(item.BasePath + @"\#UI.csproj"))
                    return true;

            return false;
        }
        internal static bool IsMSharpUI(this Item item)
        {
            if (item.FileName.Contains("UI") || item.FileName.Contains("Model"))
                if (File.Exists(item.BasePath + @"\#UI.csproj"))
                    return true;

            return false;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> list) => list == null || list.Count() == 0;

        public static bool EndsWithAny(this string input, params string[] listOfEndings)
        {
            foreach (var option in listOfEndings)
                if (input.EndsWith(option)) return true;

            return false;
        }

        public static bool Contains(this string text, string subString, bool caseSensitive)
        {
            if (text == null && subString == null)
                return true;

            if (text == null) return false;

            if (subString.IsEmpty()) return true;

            if (caseSensitive)
            {
                return text.Contains(subString);
            }
            else
            {
                return text.ToUpper().Contains(subString.Get(s => s.ToUpper()));
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static K Get<T, K>(this T item, Func<T, K> selector)
        {
            if (object.ReferenceEquals(item, null))
                return default(K);
            else
            {
                try
                {
                    return selector(item);
                }
                catch (NullReferenceException)
                {
                    return default(K);
                }
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static K? Get<T, K>(this T item, Func<T, K?> selector) where K : struct
        {
            if (item == null) return null;

            try
            {
                return selector(item);
            }
            catch (NullReferenceException)
            {
                return default(K);
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static Guid? Get<T>(this T item, Func<T, Guid> selector) where T : class
        {
            if (item == null) return null;

            try
            {
                return selector(item);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static int? Get<T>(this T item, Func<T, int> selector) where T : class
        {
            if (item == null) return null;

            try
            {
                return selector(item);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static double? Get<T>(this T item, Func<T, double> selector) where T : class
        {
            if (item == null) return null;

            try
            {
                return selector(item);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static decimal? Get<T>(this T item, Func<T, decimal> selector) where T : class
        {
            if (item == null) return null;

            try
            {
                return selector(item);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static bool? Get<T>(this T item, Func<T, bool> selector) where T : class
        {
            if (item == null) return null;

            try
            {
                return selector(item);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static string Get(this DateTime? item, Func<DateTime?, string> selector)
        {
            if (item == null) return null;

            try
            {
                return selector(item);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static byte? Get<T>(this T item, Func<T, byte> selector) where T : class
        {
            if (item == null) return null;

            try
            {
                return selector(item);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static DateTime? Get<T>(this T item, Func<T, DateTime> selector) where T : class
        {
            if (item == null) return null;

            try
            {
                return selector(item);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static DateTime? Get<T>(this T item, Func<T, DateTime?> selector) where T : class
        {
            if (item == null) return null;

            try
            {
                return selector(item);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a specified member of this object. If this is null, null will be returned. Otherwise the specified expression will be returned.
        /// </summary>
        public static T Get<T>(this DateTime? item, Func<DateTime?, T> selector) where T : struct
        {
            if (item == null) return default(T);

            try
            {
                return selector(item);
            }
            catch (NullReferenceException)
            {
                return default(T);
            }
        }

        public static string Replace(this string str, string oldValue, string newValue, bool caseSensitive)
        {
            if (caseSensitive)
                return str.Replace(oldValue, newValue);

            var prevPos = 0;
            var retval = str;
            var pos = retval.IndexOf(oldValue, StringComparison.InvariantCultureIgnoreCase);

            while (pos > -1)
            {
                retval = str.Remove(pos, oldValue.Length);
                retval = retval.Insert(pos, newValue);
                prevPos = pos + newValue.Length;
                pos = retval.IndexOf(oldValue, prevPos, StringComparison.InvariantCultureIgnoreCase);
            }

            return retval;
        }

        const string SINGLE_QUOTE = "'", DOUBLE_QUOTE = "\"";

        public static string StripQuotation(this string str)
        {
            if (str.StartsWith(SINGLE_QUOTE))
                return str
                    .TrimStart(SINGLE_QUOTE)
                    .TrimEnd(SINGLE_QUOTE);

            if (str.StartsWith(DOUBLE_QUOTE))
                return str
                    .TrimStart(DOUBLE_QUOTE)
                    .TrimEnd(DOUBLE_QUOTE);

            return str;
        }
    }
}