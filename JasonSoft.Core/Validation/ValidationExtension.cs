using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JasonSoft;

namespace JasonSoft.Validation
{
    public static class ValidationExtension
    {
        /// <summary>
        /// Determines whether the specified eval string contains only alpha characters.
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <returns>
        ///     <c>true</c> if the specified eval string is alpha; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlpha(this string evalString)
        {
            return !Regex.IsMatch(evalString, RegexPattern.ALPHA);
        }

        /// <summary>
        /// Determines whether the specified eval string contains only alphanumeric characters
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <returns>
        ///     <c>true</c> if the string is alphanumeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphaNumeric(this string evalString)
        {
            return !Regex.IsMatch(evalString, RegexPattern.ALPHA_NUMERIC);
        }

        /// <summary>
        /// Determines whether the specified eval string contains only alphanumeric characters
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <param name="allowSpaces">if set to <c>true</c> [allow spaces].</param>
        /// <returns>
        ///     <c>true</c> if the string is alphanumeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAlphaNumeric(this string evalString, bool allowSpaces)
        {
            if (allowSpaces)
                return !Regex.IsMatch(evalString, RegexPattern.ALPHA_NUMERIC_SPACE);
            return IsAlphaNumeric(evalString);
        }


        /// <summary>
        /// Determines whether the specified eval string contains only numeric characters
        /// </summary>
        /// <param name="evalString">The eval string.</param>
        /// <returns>
        ///     <c>true</c> if the string is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(this string evalString)
        {
            return !Regex.IsMatch(evalString, RegexPattern.NUMERIC);
        }


        /// <summary>
        /// Determines whether the specified email address string is valid based on regular expression evaluation.
        /// </summary>
        /// <param name="emailAddressString">The email address string.</param>
        /// <returns>
        ///     <c>true</c> if the specified email address is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmail(this string emailAddressString)
        {
            return Regex.IsMatch(emailAddressString, RegexPattern.EMAIL);
        }

        /// <summary>
        /// Determines whether the specified string is lower case.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>
        ///     <c>true</c> if the specified string is lower case; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLowerCase(this string inputString)
        {
            return Regex.IsMatch(inputString, RegexPattern.LOWER_CASE);
        }

        /// <summary>
        /// Determines whether the specified string is upper case.
        /// </summary>
        /// <param name="inputString">The input string.</param>
        /// <returns>
        ///     <c>true</c> if the specified string is upper case; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUpperCase(this string inputString)
        {
            return Regex.IsMatch(inputString, RegexPattern.UPPER_CASE);
        }

        /// <summary>
        /// Determines whether the specified string is a valid GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns>
        ///     <c>true</c> if the specified string is a valid GUID; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGuid(this string guid)
        {
            return Regex.IsMatch(guid, RegexPattern.GUID);
        }

        /// <summary>
        /// Determines whether the specified string is a valid IP address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>
        ///     <c>true</c> if valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIPAddress(this string ipAddress)
        {
            return Regex.IsMatch(ipAddress, RegexPattern.IP_ADDRESS);
        }

        /// <summary>
        /// Determines whether the specified string is a valid URL string using the referenced regex string.
        /// </summary>
        /// <param name="url">The URL string.</param>
        /// <returns>
        ///     <c>true</c> if valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsURL(this string url)
        {
            return Regex.IsMatch(url, RegexPattern.URL);
        }


        /// <summary>
        /// Determines whether the specified string is consider a strong password based on the supplied string.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        ///     <c>true</c> if strong; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStrongPassword(this string password)
        {
            return Regex.IsMatch(password, RegexPattern.STRONG_PASSWORD);
        }

        public static Boolean IsDateTime(this String source)
        {
            DateTime result;
            return DateTime.TryParse(source, out result);
        }
    }
}
