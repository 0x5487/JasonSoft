using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JasonSoft
{
    public static partial class CoreExtension
    {
        public static Boolean IsNullOrEmpty(this String source)
        {
            return String.IsNullOrEmpty(source);
        }

        public static Boolean IsNullOrWhiteSpace(this String source)
        {
            if (source == null)
                return true;

            return (source.Trim() == string.Empty);
        }

        public static String Substring(this String source, String startingAt, String endAt)
        {
            Int32 startIndex = source.IndexOf(startingAt, 0);

            if (startIndex > -1)
            {
                Int32 endIndex = source.IndexOf(endAt, startIndex + startingAt.Length);

                if (endIndex > -1)
                {
                    return source.Substring(startIndex + startingAt.Length, endIndex - (startIndex + startingAt.Length));
                }
            }

            return null;
        }

        public static String[] Split(this String source, String regexPattern)
        {
            if (source.IsNullOrEmpty() || regexPattern.IsNullOrEmpty()) throw new ArgumentNullException("source");

            List<String> results = new List<string>();

            Match match = null;

            while ((match = Regex.Match(source, regexPattern)).Success)
            {
                results.Add(source.Substring(0, match.Index));
                source = source.Substring(match.Index + match.Length);
            }

            if (!results.IsNullOrEmpty() && !source.IsNullOrEmpty())
            {
                results.Add(source);
                return results.ToArray();
            }

            return null;
        }

        public static Dictionary<String, String> SplitByKey(this String source, String keyRegexPattern, Boolean overwrite)
        {
            if (source.IsNullOrEmpty() || keyRegexPattern.IsNullOrEmpty()) throw new ArgumentNullException("source");

            Dictionary<String, String> results = new Dictionary<string, string>();

            Match match = null;
            Match lastMatch = null;

            while ((match = Regex.Match(source, keyRegexPattern)).Success)
            {
                if (lastMatch != null)
                {
                    Boolean flag = results.ContainsKey(lastMatch.Value);
                    if (flag && overwrite)
                    {
                        results[lastMatch.Value] = source.Substring(0, match.Index);
                    }
                    else if (flag == false)
                    {
                        results.Add(lastMatch.Value, source.Substring(0, match.Index));
                    }
                }

                lastMatch = match;
                source = source.Substring(match.Index + match.Length);
            }

            if (lastMatch != null)
            {
                Boolean flag = results.ContainsKey(lastMatch.Value);
                if (flag && overwrite)
                {
                    results[lastMatch.Value] = source.Substring(0, match.Index);
                }
                else if (flag == false)
                {
                    results.Add(lastMatch.Value, source.Substring(0));
                }
            }

            if (!results.IsNullOrEmpty())
                return results;
            else
                return null;
        }


        public static String Combine(this IEnumerable source, String sign)
        {
            if (source == null) throw new ArgumentNullException();
            if (sign == null) throw new ArgumentNullException();
            if (source.IsNullOrEmpty()) return String.Empty;

            String result = String.Empty;

            IEnumerator list = source.GetEnumerator();
            Boolean running = list.MoveNext();
            while (running)
            {
                result += list.Current;
                running = list.MoveNext();

                if (running)
                    result += sign;
            }

            return result.Trim();
        }

        /// <summary>
        /// Strips the last specified chars from a string.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="removeFromEnd">The remove from end.</param>
        /// <returns></returns>
        public static string Chop(this string sourceString, int removeFromEnd)
        {
            string result = sourceString;
            if ((removeFromEnd > 0) && (sourceString.Length > removeFromEnd - 1))
                result = result.Remove(sourceString.Length - removeFromEnd, removeFromEnd);
            return result;
        }

        /// <summary>
        /// Strips the last specified chars from a string.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="backDownTo">The back down to.</param>
        /// <returns></returns>
        public static string Chop(this string sourceString, string backDownTo)
        {
            int removeDownTo = sourceString.LastIndexOf(backDownTo);
            int removeFromEnd = 0;
            if (removeDownTo > 0)
                removeFromEnd = sourceString.Length - removeDownTo;

            string result = sourceString;

            if (sourceString.Length > removeFromEnd - 1)
                result = result.Remove(removeDownTo, removeFromEnd);

            return result;
        }

        /// <summary>
        /// Removes the specified chars from the beginning of a string.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="removeFromBeginning">The remove from beginning.</param>
        /// <returns></returns>
        public static string Clip(this string sourceString, int removeFromBeginning)
        {
            string result = sourceString;
            if (sourceString.Length > removeFromBeginning)
                result = result.Remove(0, removeFromBeginning);
            return result;
        }

        /// <summary>
        /// Removes chars from the beginning of a string, up to the specified string
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <param name="removeUpTo">The remove up to.</param>
        /// <returns></returns>
        public static string Clip(this string sourceString, string removeUpTo)
        {
            int removeFromBeginning = sourceString.IndexOf(removeUpTo);
            string result = sourceString;

            if (sourceString.Length > removeFromBeginning && removeFromBeginning > 0)
                result = result.Remove(0, removeFromBeginning);

            return result;
        }

        /// <summary>
        /// Strips the last char from a a string.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public static string Chop(this string sourceString)
        {
            return Chop(sourceString, 1);
        }

        /// <summary>
        /// Strips the last char from a a string.
        /// </summary>
        /// <param name="sourceString">The source string.</param>
        /// <returns></returns>
        public static string Clip(this string sourceString)
        {
            return Clip(sourceString, 1);
        }


        /// <summary>
        /// Strips all HTML tags from a string
        /// </summary>
        /// <param name="htmlString">The HTML string.</param>
        /// <returns></returns>
        public static string StripHTML(this string htmlString)
        {
            return StripHTML(htmlString, String.Empty);
        }

        /// <summary>
        /// Strips all HTML tags from a string and replaces the tags with the specified replacement
        /// </summary>
        /// <param name="htmlString">The HTML string.</param>
        /// <param name="htmlPlaceHolder">The HTML place holder.</param>
        /// <returns></returns>
        public static string StripHTML(this string htmlString, string htmlPlaceHolder)
        {
            const string pattern = @"<(.|\n)*?>";
            string sOut = Regex.Replace(htmlString, pattern, htmlPlaceHolder);
            sOut = sOut.Replace(" ", String.Empty);
            sOut = sOut.Replace("&amp;", "&");
            sOut = sOut.Replace("&gt;", ">");
            sOut = sOut.Replace("&lt;", "<");
            return sOut;
        }

        /// <summary>
        /// Converts a generic List collection to a single comma-delimitted string.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static string ToDelimitedList(this IEnumerable<string> list)
        {
            return ToDelimitedList(list, ",");
        }

        /// <summary>
        /// Converts a generic List collection to a single string using the specified delimitter.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns></returns>
        public static string ToDelimitedList(this IEnumerable<string> list, string delimiter)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in list)
                sb.Append(String.Concat(s, delimiter));
            string result = sb.ToString();
            result = Chop(result);
            return result;
        }

        public static Boolean Contain(this String source, String value)
        {
            return source.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) > -1 ? true : false;
        }


    }
}
