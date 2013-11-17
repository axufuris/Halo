using System;
using System.Globalization;

namespace Halo.Utilities
{
    public class Parser
    {
        /// <summary>
        /// Gets the parsed boolean.
        /// </summary>
        /// <param name="value">The value to get the boolean from.</param>
        /// <returns>
        /// Returns a Boolean Value.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static bool GetBoolean(object value)
        {
            return GetBoolean(value, default(bool));
        }

        /// <summary>
        /// Gets the boolean.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns>
        /// The parsed boolean value of the object, or the passed default value if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static bool GetBoolean(object value, bool defaultValue)
        {
            try
            {
                int i = GetInt(value, int.MinValue);
                if (i == int.MinValue)
                {
                    return bool.Parse(value.ToString());
                }

                return i != 0;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed DateTime value of the object, or default(DateTime) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static DateTime GetDateTime(object value)
        {
            return GetDateTime(value, default(DateTime));
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed DateTime value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static DateTime GetDateTime(object value, DateTime defaultValue)
        {
            try
            {
                return DateTime.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed DateTime value of the object, or default(DateTime) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static DateTime? GetDateTimeNullable(object value)
        {
            return GetDateTimeNullable(value, default(DateTime?));
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed DateTime value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static DateTime? GetDateTimeNullable(object value, DateTime? defaultValue)
        {
            try
            {
                return DateTime.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed DateTime value of the object, or default(DateTime) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static DateTimeOffset GetDateTimeOffset(object value)
        {
            return GetDateTimeOffset(value, default(DateTimeOffset));
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed DateTime value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static DateTimeOffset GetDateTimeOffset(object value, DateTimeOffset defaultValue)
        {
            try
            {
                return DateTimeOffset.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the time span.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed time span value of the object, or default(time span) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static TimeSpan GetTimeSpan(object value)
        {
            return GetTimeSpan(value, default(TimeSpan));
        }

        /// <summary>
        /// Gets the time span.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed time span value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static TimeSpan GetTimeSpan(object value, TimeSpan defaultValue)
        {
            try
            {
                string[] formats = { "hh", "hhtt", "hh tt", "%h", "%htt", "%h tt", @"h\:mm", @"h\:mmtt", @"h\:mm tt", @"hh\:mm", 
                                @"hh\:mmtt", @"hh\:mm tt", @"d\.hh\:mm\:ss", @"d\.hh\:mm\:sstt", @"d\.hh\:mm\:ss tt", "fffff", 
                                "hhmm", "hhmmtt", "hhmm tt" };

                DateTime newDate = new DateTime();

                if (DateTime.TryParseExact(value.ToString(), formats, null, DateTimeStyles.None, out newDate))
                {
                    return newDate.TimeOfDay;
                }

                if (DateTime.TryParse(value.ToString(), out newDate))
                {
                    return newDate.TimeOfDay;
                }

                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed decimal value of the object, or default(decimal) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static decimal GetDecimal(object value)
        {
            return GetDecimal(value, default(decimal));
        }

        /// <summary>
        /// Gets the decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed decimal value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static decimal GetDecimal(object value, decimal defaultValue)
        {
            try
            {
                return decimal.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed double value of the object, or default(double) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static double GetDouble(object value)
        {
            return GetDouble(value, default(double));
        }

        /// <summary>
        /// Gets the double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed double value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static double GetDouble(object value, double defaultValue)
        {
            try
            {
                return double.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the float.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed float value of the object, or default(float) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static float GetFloat(object value)
        {
            return GetFloat(value, default(float));
        }

        /// <summary>
        /// Gets the float.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed float value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static float GetFloat(object value, float defaultValue)
        {
            try
            {
                return float.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the short.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed short value of the object, or default(short) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static short GetShort(object value)
        {
            return GetShort(value, default(short));
        }

        /// <summary>
        /// Gets the short.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed short value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static short GetShort(object value, short defaultValue)
        {
            try
            {
                return short.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed int value of the object, or default(int) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static int GetInt(object value)
        {
            return GetInt(value, default(int));
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed int value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static int GetInt(object value, int defaultValue)
        {
            try
            {
                return int.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed int value of the object, or default(int) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static int? GetIntNullable(object value)
        {
            return GetIntNullable(value, default(int?));
        }

        /// <summary>
        /// Gets the int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed int value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static int? GetIntNullable(object value, int? defaultValue)
        {
            try
            {
                return int.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed long value of the object, or default(long) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static long GetLong(object value)
        {
            return GetLong(value, default(long));
        }

        /// <summary>
        /// Gets the long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed long value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static long GetLong(object value, long defaultValue)
        {
            try
            {
                return long.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the int16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed Int16 value of the object, or default(Int16) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static Int16 GetInt16(object value)
        {
            return GetInt16(value, default(Int16));
        }

        /// <summary>
        /// Gets the int16.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed Int16 value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static Int16 GetInt16(object value, Int16 defaultValue)
        {
            try
            {
                return short.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed Int32 value of the object, or default(Int32) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static Int32 GetInt32(object value)
        {
            return GetInt32(value, default(Int32));
        }

        /// <summary>
        /// Gets the int32.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed Int32 value of the object, or the passed default(Int32) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static Int32 GetInt32(object value, Int32 defaultValue)
        {
            try
            {
                return Int32.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed Int64 value of the object, or default(Int64) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static Int64 GetInt64(object value)
        {
            return GetInt64(value, default(Int64));
        }

        /// <summary>
        /// Gets the int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed Int64 value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static Int64 GetInt64(object value, Int64 defaultValue)
        {
            try
            {
                return Int64.Parse(value.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The parsed string value of the object, or default(string) if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static string GetString(object value)
        {
            return GetString(value, default(string));
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The parsed string value of the object, or the passed default if unable to parse.
        /// </returns>
        /// <createdate>11/5/2010</createdate>
        /// <author>
        /// xufurisa
        /// </author>
        public static string GetString(object value, string defaultValue)
        {
            try
            {
                return value.ToString();
            }
            catch
            {
                return defaultValue;
            }
        }
    }  /// End of Class
}
