using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Halo.Utilities
{
    public class Validation
    {
        /// <summary>
        /// Validates the email.
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified email is email; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool IsEmail(string stringToCheck)
        {
            return IsMatch(stringToCheck, @"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$");
        }

        /// <summary>
        /// Validates the phone.
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <returns>
        /// True/False
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool IsPhone(string stringToCheck)
        {
            return IsMatch(stringToCheck, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");
        }

        /// <summary>
        /// Determines whether the specified link is valid URL.  Works for Relative and Absolute URLS.
        /// </summary>
        /// <param name="stringToCheck">The link.</param>
        /// <returns>
        ///   <c>true</c> if the specified link is URL; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool IsURL(string stringToCheck)
        {
            return IsURL(stringToCheck, UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// Determines whether the specified link is URL.
        /// </summary>
        /// <param name="stringToCheck">The link.</param>
        /// <param name="relativeOrAbsolute">Validate for Relative or Absolute.</param>
        /// <returns>
        ///   <c>true</c> if the specified link is URL; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool IsURL(string stringToCheck, UriKind relativeOrAbsolute)
        {
            Uri url = null;

            if (!Uri.TryCreate(stringToCheck, relativeOrAbsolute, out url))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether [is alpha numeric] [the specified string to check].
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <returns>
        ///   <c>true</c> if [is alpha numeric] [the specified string to check]; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool IsAlphaNumeric(string stringToCheck)
        {
            return IsMatch(stringToCheck, @"^[a-zA-Z0-9\s,]*$");
        }

        /// <summary>
        /// Determines whether the specified string to check is numeric.
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified string to check is numeric; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool IsNumeric(string stringToCheck)
        {
            return stringToCheck.ToCharArray().Where(x => !char.IsDigit(x)).Count() == 0;
        }

        /// <summary>
        /// Determines whether the specified string to check is letter.
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified string to check is letter; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-30-2013</createdate>
        /// <author>
        /// James Gates Richardson
        /// </author>
        public static bool IsLettersOnly(string stringToCheck)
        {
            return IsMatch(stringToCheck, @"^[a-zA-Z\s,]*$");
        }

        /// <summary>
        /// Determines whether [is zip code] [the specified string to check].
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <returns>
        ///   <c>true</c> if [is zip code] [the specified string to check]; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-30-2013</createdate>
        /// <author>
        /// James Gates Richardson
        /// </author>
        public static bool IsZipCode(string stringToCheck)
        {
            return IsMatch(stringToCheck, @"^\d{5}-\d{4}$|^\d{5}$");
        }

        /// <summary>
        /// Determines whether [is I PV4 address] [the specified string to check].
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <returns>
        ///   <c>true</c> if [is I PV4 address] [the specified string to check]; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-30-2013</createdate>
        /// <author>
        /// James Gates Richardson
        /// </author>
        public static bool IsIPv4Address(string stringToCheck)
        {
            return IsMatch(stringToCheck, @"^(25[0-5]|2[0-4]\d|[01]?\d\d?)\.(25[0-5]|2[0-4]\d|[01]?\d\d?)\.(25[0-5]|2[0-4]\d|[01]?\d\d?)\.(25[0-5]|2[0-4]\d|[01]?\d\d?)$");
        }

        /// <summary>
        /// Determines whether [is I PV6 address] [the specified string to check].
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <returns>
        ///   <c>true</c> if [is I PV6 address] [the specified string to check]; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-30-2013</createdate>
        /// <author>
        /// James Gates Richardson
        /// </author>
        public static bool IsIPv6Address(string stringToCheck)
        {
            return IsMatch(stringToCheck, @"^((([0-9A-Fa-f]{1,4}:){7}[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){6}:[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){5}:([0-9A-Fa-f]{1,4}:)?[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){4}:([0-9A-Fa-f]{1,4}:){0,2}[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){3}:([0-9A-Fa-f]{1,4}:){0,3}[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){2}:([0-9A-Fa-f]{1,4}:){0,4}[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){6}((\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b)\.){3}(\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b))|(([0-9A-Fa-f]{1,4}:){0,5}:((\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b)\.){3}(\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b))|(::([0-9A-Fa-f]{1,4}:){0,5}((\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b)\.){3}(\b((25[0-5])|(1\d{2})|(2[0-4]\d)|(\d{1,2}))\b))|([0-9A-Fa-f]{1,4}::([0-9A-Fa-f]{1,4}:){0,5}[0-9A-Fa-f]{1,4})|(::([0-9A-Fa-f]{1,4}:){0,6}[0-9A-Fa-f]{1,4})|(([0-9A-Fa-f]{1,4}:){1,7}:))$");
        }

        /// <summary>
        /// Determines whether the specified string to check is match.
        /// </summary>
        /// <param name="stringToCheck">The string to check.</param>
        /// <param name="regExPattern">The reg ex pattern.</param>
        /// <returns>
        ///   <c>true</c> if the specified string to check is match; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-30-2013</createdate>
        /// <author>
        /// James Gates Richardson
        /// </author>
        public static bool IsMatch(string stringToCheck, string regExPattern)
        {
            Regex theRegex = new Regex(regExPattern);
            return theRegex.IsMatch(stringToCheck);
        }

        /// <summary>
        /// Determines whether the specified validate string is decimal.
        /// </summary>
        /// <param name="validateString">The validate string.</param>
        /// <returns>
        ///   <c>true</c> if the specified validate string is decimal; otherwise, <c>false</c>.
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static bool IsDecimal(string validateString)
        {
            decimal result = 0;
            return decimal.TryParse(validateString, out result);
        }
    }   /// End of Class
}
