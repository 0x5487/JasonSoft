using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JasonSoft
{
    public static partial class CoreExtension
    {
        #region Date Math

        /// <summary>
        /// Dayses the ago.
        /// </summary>
        /// <param name="days">The days.</param>
        /// <returns></returns>
        public static DateTime DaysAgo(this int days)
        {
            TimeSpan t = new TimeSpan(days, 0, 0, 0);
            return DateTime.Now.Subtract(t);
        }

        /// <summary>
        /// Dayses from now.
        /// </summary>
        /// <param name="days">The days.</param>
        /// <returns></returns>
        public static DateTime DaysFromNow(this int days)
        {
            TimeSpan t = new TimeSpan(days, 0, 0, 0);
            return DateTime.Now.Add(t);
        }

        /// <summary>
        /// Hourses the ago.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <returns></returns>
        public static DateTime HoursAgo(this int hours)
        {
            TimeSpan t = new TimeSpan(hours, 0, 0);
            return DateTime.Now.Subtract(t);
        }

        /// <summary>
        /// Hourses from now.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <returns></returns>
        public static DateTime HoursFromNow(this int hours)
        {
            TimeSpan t = new TimeSpan(hours, 0, 0);
            return DateTime.Now.Add(t);
        }

        /// <summary>
        /// Minuteses the ago.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public static DateTime MinutesAgo(this int minutes)
        {
            TimeSpan t = new TimeSpan(0, minutes, 0);
            return DateTime.Now.Subtract(t);
        }

        /// <summary>
        /// Minuteses from now.
        /// </summary>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public static DateTime MinutesFromNow(this int minutes)
        {
            TimeSpan t = new TimeSpan(0, minutes, 0);
            return DateTime.Now.Add(t);
        }

        /// <summary>
        /// Secondses the ago.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static DateTime SecondsAgo(this int seconds)
        {
            TimeSpan t = new TimeSpan(0, 0, seconds);
            return DateTime.Now.Subtract(t);
        }

        /// <summary>
        /// Seconds from now.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static DateTime SecondsFromNow(this int seconds)
        {
            TimeSpan t = new TimeSpan(0, 0, seconds);
            return DateTime.Now.Add(t);
        }

        /// <summary>
        /// Gets a DateTime representing the first day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current)
        {
            DateTime first = current.AddDays(1 - current.Day);
            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the first specified day in the current month
        /// </summary>
        /// <param name="current">The current day</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime First(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime first = current.First();

            if (first.DayOfWeek != dayOfWeek)
            {
                first = first.Next(dayOfWeek);
            }

            return first;
        }

        /// <summary>
        /// Gets a DateTime representing the last day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current)
        {
            int daysInMonth = DateTime.DaysInMonth(current.Year, current.Month);

            DateTime last = current.First().AddDays(daysInMonth - 1);
            return last;
        }

        /// <summary>
        /// Gets a DateTime representing the last specified day in the current month
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The current day of week</param>
        /// <returns></returns>
        public static DateTime Last(this DateTime current, DayOfWeek dayOfWeek)
        {
            DateTime last = current.Last();

            last = last.AddDays(System.Math.Abs(dayOfWeek - last.DayOfWeek) * -1);
            return last;
        }

        /// <summary>
        /// Gets a DateTime representing the first date following the current date which falls on the given day of the week
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="dayOfWeek">The day of week for the next date to get</param>
        public static DateTime Next(this DateTime current, DayOfWeek dayOfWeek)
        {
            int offsetDays = dayOfWeek - current.DayOfWeek;

            if (offsetDays <= 0)
            {
                offsetDays += 7;
            }

            DateTime result = current.AddDays(offsetDays);
            return result;
        }

        /// <summary>
        /// Gets a DateTime representing midnight on the current date
        /// </summary>
        /// <param name="current">The current date</param>
        public static DateTime Midnight(this DateTime current)
        {
            DateTime midnight = new DateTime(current.Year, current.Month, current.Day);
            return midnight;
        }

        /// <summary>
        /// Gets a DateTime representing noon on the current date
        /// </summary>
        /// <param name="current">The current date</param>
        public static DateTime Noon(this DateTime current)
        {
            DateTime noon = new DateTime(current.Year, current.Month, current.Day, 12, 0, 0);
            return noon;
        }
        /// <summary>
        /// Sets the time of the current date with minute precision
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="hour">The hour</param>
        /// <param name="minute">The minute</param>
        public static DateTime SetTime(this DateTime current, int hour, int minute)
        {
            return SetTime(current, hour, minute, 0, 0);
        }

        /// <summary>
        /// Sets the time of the current date with second precision
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="hour">The hour</param>
        /// <param name="minute">The minute</param>
        /// <param name="second">The second</param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime current, int hour, int minute, int second)
        {
            return SetTime(current, hour, minute, second, 0);
        }

        /// <summary>
        /// Sets the time of the current date with millisecond precision
        /// </summary>
        /// <param name="current">The current date</param>
        /// <param name="hour">The hour</param>
        /// <param name="minute">The minute</param>
        /// <param name="second">The second</param>
        /// <param name="millisecond">The millisecond</param>
        /// <returns></returns>
        public static DateTime SetTime(this DateTime current, int hour, int minute, int second, int millisecond)
        {
            DateTime atTime = new DateTime(current.Year, current.Month, current.Day, hour, minute, second, millisecond);
            return atTime;
        }


        #endregion

        #region Diffs


        /// <summary>
        /// Diffs the specified date one.
        /// </summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static TimeSpan Diff(this DateTime dateOne, DateTime dateTwo)
        {
            TimeSpan t = dateOne.Subtract(dateTwo);
            return t;
        }

        /// <summary>
        /// Diffs the days.
        /// </summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffDays(this string dateOne, string dateTwo)
        {
            DateTime dtOne;
            DateTime dtTwo;
            if (DateTime.TryParse(dateOne, out dtOne) && DateTime.TryParse(dateTwo, out dtTwo))
                return Diff(dtOne, dtTwo).TotalDays;
            return 0;
        }

        /// <summary>
        /// Diffs the days.
        /// </summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffDays(this DateTime dateOne, DateTime dateTwo)
        {
            return Diff(dateOne, dateTwo).TotalDays;
        }

        /// <summary>
        /// Diffs the hours.
        /// </summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffHours(this string dateOne, string dateTwo)
        {
            DateTime dtOne;
            DateTime dtTwo;
            if (DateTime.TryParse(dateOne, out dtOne) && DateTime.TryParse(dateTwo, out dtTwo))
                return Diff(dtOne, dtTwo).TotalHours;
            return 0;
        }

        /// <summary>
        /// Diffs the hours.
        /// </summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffHours(this DateTime dateOne, DateTime dateTwo)
        {
            return Diff(dateOne, dateTwo).TotalHours;
        }

        /// <summary>
        /// Diffs the minutes.
        /// </summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffMinutes(this string dateOne, string dateTwo)
        {
            DateTime dtOne;
            DateTime dtTwo;
            if (DateTime.TryParse(dateOne, out dtOne) && DateTime.TryParse(dateTwo, out dtTwo))
                return Diff(dtOne, dtTwo).TotalMinutes;
            return 0;
        }

        /// <summary>
        /// Diffs the minutes.
        /// </summary>
        /// <param name="dateOne">The date one.</param>
        /// <param name="dateTwo">The date two.</param>
        /// <returns></returns>
        public static double DiffMinutes(this DateTime dateOne, DateTime dateTwo)
        {
            return Diff(dateOne, dateTwo).TotalMinutes;
        }


        /// <summary>
        /// Checks to see if the date is a week day (Mon - Fri)
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>
        ///     <c>true</c> if [is week day] [the specified dt]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekDay(this DateTime dt)
        {
            return (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday);
        }

        /// <summary>
        /// Checks to see if the date is Saturday or Sunday
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>
        ///     <c>true</c> if [is week end] [the specified dt]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekEnd(this DateTime dt)
        {
            return (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday);
        }

        #endregion

    }
}
