using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace LAN.Helper
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Get last date by month, year
        /// if month/year not filled, means this month/year
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string GetLastDate(sbyte month = -1, short year = 0)
        {
            string date = "" + (year != 0 ? year : DateTime.Now.Year);

            if (month < 0)
            {
                month = (sbyte)(DateTime.Now.Month - 1);
            }

            switch (month)
            {
                case 1: //  Februari
                    if (year % 4 == 0) date = string.Format("{0}-{1}-{2}", date, month + 1, "29");
                    else date = string.Format("{0}-{1}-{2}", date, month + 1, "28");
                    break;
                case 0: //  Januari
                case 2: //  Maret
                case 4: //  Mei
                case 6: //  Juli
                case 7: //  Agustus
                case 9: //  Oktober
                case 11://  Desember
                    date = string.Format("{0}-{1}-{2}", date, month + 1, "31");
                    break;
                case 3: //  April
                case 5: //  Juni
                case 8: //  September
                case 10://  November
                    date = string.Format("{0}-{1}-{2}", date, month + 1, "30");
                    break;
                default:
                    break;
            }

            return date;
        }

        /// <summary>
        /// Total date in month by month and year
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static byte GetMonthTotalDay(sbyte month = -1, short year = 0)
        {
            string lastDate = GetLastDate(month, year);
            Debug.Log(lastDate);
            return byte.Parse(lastDate.Split('-')[2]);
        }

        /// <summary>
        /// Get today's date time. Example: 2023-02-25 21:53:28
        /// </summary>
        /// <param name="iso"></param>
        /// <param name="secondFromNow"></param>
        /// <returns></returns>
        public static string DateTimeNow(string iso = "", float secondFromNow = 0)
        {
            DateTime date = secondFromNow == 0 ? DateTime.Now : DateTime.Now.AddSeconds(secondFromNow);
            if (iso == "ISO8601") return date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
            else return date.ToString("yyyy-MM-dd HH:mm:ss").Replace(".", ":").Replace("/", "-");
        }

        /// <summary>
        /// Get today's date. Example: 2023-02-25
        /// </summary>
        /// <param name="secondFromNow"></param>
        /// <returns></returns>
        public static string DateShortNow(float secondFromNow = 0)
        {
            DateTime date = secondFromNow == 0 ? DateTime.Now : DateTime.Now.AddSeconds(secondFromNow);
            return date.ToString("yyyy-MM-dd").Replace("/", "-");
        }

        /// <summary>
        /// Get today's time. Example: 21:53:28
        /// </summary>
        /// <param name="secondFromNow"></param>
        /// <returns></returns>
        public static string TimeShortNow(float secondFromNow = 0)
        {
            DateTime date = secondFromNow == 0 ? DateTime.Now : DateTime.Now.AddSeconds(secondFromNow);
            return date.ToString("HH:mm:ss").Replace(".", ":");
        }

        public static long TotalSeconds(int seconds, int minutes, int hours = 0, int days = 0)
        {
            long totalInSeconds = seconds;
            totalInSeconds += minutes * 60;
            totalInSeconds += hours * 60 * 60;
            totalInSeconds += days * 60 * 60 * 24;
            return totalInSeconds;
        }
    }
}
