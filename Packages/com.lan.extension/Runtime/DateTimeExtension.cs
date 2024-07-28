using LAN.Helper;
using System;

namespace LAN.Extension
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// DD_MM_YYYY = 31-12-2020, YYYY_MM_DD = 2020-12-31, DD_MMM_YYYY = 31 Jan 2020, D_M_Y = 31 Januari 2020
        /// </summary>
        public enum DateFormatEnum
        {
            DD_MM_YYYY = 0, YYYY_MM_DD, DD_MMM_YYYY, D_M_Y
        }

        #region Date and Time
        /// <summary>
        /// Convert month (int) (0-11) from month (string.)
        /// <para>Return month (string) (default = Indonesian), return '-' if not identification.</para>
        /// </summary>
        /// <param name="index">Month in number.</param>
        /// <param name="isShortName">If true, return a short name of month.</param>
        /// <param name="language">Month in selected language by code (default = ID).</param>
        /// <returns></returns>
        public static string ToStringMonth(this int index, bool isShortName = false, string language = "ID")
        {
            if (language == "EN")
            {
                return index switch
                {
                    0 => isShortName ? "Jan" : "January",
                    1 => isShortName ? "Feb" : "February",
                    2 => isShortName ? "Mar" : "March",
                    3 => isShortName ? "Apr" : "April",
                    4 => "Mei",
                    5 => isShortName ? "Jun" : "June",
                    6 => isShortName ? "Jul" : "July",
                    7 => isShortName ? "Aug" : "August",
                    8 => isShortName ? "Sep" : "September",
                    9 => isShortName ? "Oct" : "October",
                    10 => isShortName ? "Nov" : "November",
                    11 => isShortName ? "Dec" : "December",
                    _ => "-",
                };
            }
            else
            {
                return index switch
                {
                    0 => isShortName ? "Jan" : "Januari",
                    1 => isShortName ? "Feb" : "Februari",
                    2 => isShortName ? "Mar" : "Maret",
                    3 => isShortName ? "Apr" : "April",
                    4 => "Mei",
                    5 => isShortName ? "Jun" : "Juni",
                    6 => isShortName ? "Jul" : "Juli",
                    7 => isShortName ? "Ags" : "Agustus",
                    8 => isShortName ? "Sep" : "September",
                    9 => isShortName ? "Okt" : "Oktober",
                    10 => isShortName ? "Nov" : "November",
                    11 => isShortName ? "Des" : "Desember",
                    _ => "-",
                };
            }
        }

        /// <summary>
        /// Convert month (string) (January, feb, oktober, may) to month (byte).
        /// <para>Return month (byte) (0-11), 12 if not idetification.</para>
        /// </summary>
        /// <param name="name">Month in text (string).</param>
        /// <returns></returns>
        public static byte ToIntMonth(this string name)
        {
            return name.ToLower() switch
            {
                "januari" or "january" or "jan" or "1" => 0,
                "februari" or "february" or "feb" or "2" => 1,
                "maret" or "march" or "mar" or "3" => 2,
                "april" or "apr" or "4" => 3,
                "mei" or "may" or "5" => 4,
                "juni" or "june" or "jun" or "6" => 5,
                "juli" or "july" or "jul" or "7" => 6,
                "agustus" or "august" or "aug" or "ags" or "8" => 7,
                "september" or "sep" or "9" => 8,
                "oktober" or "october" or "oct" or "okt" or "10" => 9,
                "november" or "nov" or "11" => 10,
                "desember" or "december" or "dec" or "des" or "12" => 11,
                _ => 12,
            };
        }

        /// <summary>
        /// Changing Date to desired format
        /// </summary>
        /// <param name="dateRaw">Date raw (2020-12-31)</param>
        /// <param name="dateFormat">Date format type</param>
        /// <returns></returns>
        public static string FormatDate(this string dateRaw, DateFormatEnum dateFormat)
        {
            if (dateRaw.Length > 10) dateRaw = dateRaw.Substring(0, 10);
            string[] time = dateRaw.Split('-');
            short day = Convert.ToInt16(time[2]);
            short month = Convert.ToInt16(time[1]);
            short year = Convert.ToInt16(time[0]);
            switch (dateFormat)
            {
                case DateFormatEnum.DD_MMM_YYYY:
                    dateRaw = string.Format("{0} {1} {2}", day, ToStringMonth(month - 1, true), year);
                    break;
                case DateFormatEnum.DD_MM_YYYY:
                    dateRaw = string.Format("{0:00}-{1:00}-{2:0000}", day, month, year);
                    break;
                case DateFormatEnum.D_M_Y:
                    dateRaw = string.Format("{0} {1} {2}", day, ToStringMonth(month - 1), year);
                    break;
            }
            return dateRaw;
        }

        /// <summary>
        /// Calculate DateTime value into total of seconds
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long TotalSeconds(this DateTime dateTime)
        {
            byte hour = (byte)dateTime.Hour;
            byte minute = (byte)dateTime.Minute;
            byte second = (byte)dateTime.Second;
            return DateTimeHelper.TotalSeconds(second, minute, hour);
        }
        #endregion
    }
}
