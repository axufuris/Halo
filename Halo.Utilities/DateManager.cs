using System;
using System.Globalization;

namespace Halo.Utilities
{
    public class DateManager
    {
        // <summary>
        /// Returns the first day of the week that the specified
        /// date is in using the current culture.
        /// </summary>
        /// <param name="dayInWeek">The day in week.</param>
        /// <returns></returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
        {
            CultureInfo defaultCultureInfo = CultureInfo.CurrentCulture;

            return DateManager.GetFirstDayOfWeek(dayInWeek, defaultCultureInfo);
        }

        /// <summary>
        /// Returns the first day of the week that the specified date
        /// is in.
        /// </summary>
        /// <param name="dayInWeek">The day in week.</param>
        /// <param name="cultureInfo">The culture info.</param>
        /// <returns></returns>
        /// <createdate>7-24-2013</createdate>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;

            while (firstDayInWeek.DayOfWeek != firstDay)
            {
                firstDayInWeek = firstDayInWeek.AddDays(-1);
            }

            return firstDayInWeek;
        }

        /// <summary>
        /// Formats the time into a standard format.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        /// <createdate>4-12-2013</createdate>
        /// <exception cref="System.Exception">
        /// Not a valid time.
        /// </exception>
        /// <author>
        /// James Gates Richardson
        /// </author>
        public static string FormatTime(string time)
        {
            DateTime formatted = CombineDateAndTime(time);

            return formatted.ToString("t");
        }

        /// <summary>
        /// Combines the date and time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>
        /// Returns the Combined Date and Time.
        /// </returns>
        /// <createdate>4-12-2013</createdate>
        /// <exception cref="System.Exception">Not a valid date.
        /// or
        /// Not a valid time.
        /// </exception>
        /// <author>
        /// James Gates Richardson
        /// </author>
        public static DateTime CombineDateAndTime(string time)
        {
            var timeSpan = Parser.GetTimeSpan(time);
            var newDate = Parser.GetDateTime(DateTime.Now);

            return CombineDateAndTime(newDate, timeSpan);
        }

        /// <summary>
        /// Combines the date and time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="time">The time.</param>
        /// <returns>
        /// Returns the Combined Date and Time.
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <exception cref="System.Exception">
        /// Not a valid date.
        /// or
        /// Not a valid time.
        /// </exception>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static DateTime CombineDateAndTime(string date, string time)
        {
            var timeSpan = Parser.GetTimeSpan(time);
            var newDate = Parser.GetDateTime(date);

            return CombineDateAndTime(newDate, timeSpan);
        }

        /// <summary>
        /// Combines the date and time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="time">The time.</param>
        /// <returns>
        /// Returns the Combined Date and Time.
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <exception cref="System.Exception">
        /// Not a valid date.
        /// or
        /// Not a valid time.
        /// </exception>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static DateTime CombineDateAndTime(DateTime date, string time)
        {
            var timeSpan = Parser.GetTimeSpan(time);

            return CombineDateAndTime(date, timeSpan);
        }

        /// <summary>
        /// Combines the date and time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="time">The time.</param>
        /// <returns>
        /// Returns the Combined Date and Time.
        /// </returns>
        /// <createdate>7-24-2013</createdate>
        /// <exception cref="System.Exception">
        /// Not a valid date.
        /// or
        /// Not a valid time.
        /// </exception>
        /// <author>
        /// Andy Xufuris
        /// </author>
        public static DateTime CombineDateAndTime(DateTime date, TimeSpan time)
        {
            if (time != null)
            {
                if (date != null)
                {
                    return date.Date.Add(time);
                }
                else
                {
                    throw new Exception("Not a valid date.");
                }
            }
            else
            {
                throw new Exception("Not a valid time.");
            }
        }
    }  /// End of Class
}
