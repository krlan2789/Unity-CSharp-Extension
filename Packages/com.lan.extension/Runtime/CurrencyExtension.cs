using System;
using System.Linq;

namespace LAN.Extension
{
    public static class CurrencyExtension
    {
        #region Format Money/currency
        /// <summary>
        /// Format string of numbers to standard money.
        /// </summary>
        /// <param name="insertWhitespace">Whitespace beetwen prefix and value</param>
        /// <param name="prefix">Prefix for the value, usually contains currency</param>
        /// <returns>10.500, 135.000, 505.050</returns>
        public static string ToMoney(this string text, string prefix = "", bool insertWhitespace = true)
        {
            char[] ca = text.ToCharArray();
            Array.Reverse(ca);
            string m = ConvertMoney(ca, insertWhitespace);
            if (Convert.ToInt64(text) < 0) m = m.Length % 4 == 0 ? m.Replace("-.", "-") : m;
            return prefix + m;
        }

        /// <summary>
        /// Format byte to standard money.
        /// </summary>
        /// <param name="insertWhitespace">Whitespace beetwen prefix and value</param>
        /// <param name="prefix">Prefix for the value, usually contains currency</param>
        /// <returns>10.500, 135.000, 505.050</returns>
        public static string ToMoney(this byte number, string prefix = "", bool insertWhitespace = true)
        {
            return ToMoney("" + number, prefix, insertWhitespace);
        }

        /// <summary>
        /// Format short to standard money.
        /// </summary>
        /// <param name="insertWhitespace">Whitespace beetwen prefix and value</param>
        /// <param name="prefix">Prefix for the value, usually contains currency</param>
        /// <returns>10.500, 135.000, 505.050</returns>
        public static string ToMoney(this short number, string prefix = "", bool insertWhitespace = true)
        {
            return ToMoney("" + number, prefix, insertWhitespace);
        }

        /// <summary>
        /// Format int to standard money.
        /// </summary>
        /// <param name="insertWhitespace">Whitespace beetwen prefix and value</param>
        /// <param name="prefix">Prefix for the value, usually contains currency</param>
        /// <returns>10.500, 135.000, 505.050</returns>
        public static string ToMoney(this int number, string prefix = "", bool insertWhitespace = true)
        {
            return ToMoney("" + number, prefix, insertWhitespace);
        }

        /// <summary>
        /// Format long to standard money.
        /// </summary>
        /// <param name="insertWhitespace">Whitespace beetwen prefix and value</param>
        /// <param name="prefix">Prefix for the value, usually contains currency</param>
        /// <returns>10.500, 135.000, 505.050</returns>
        public static string ToMoney(this long number, string prefix = "", bool insertWhitespace = true)
        {
            return ToMoney("" + number, prefix, insertWhitespace);
        }

        /// <summary>
        /// Remove non numeric characters
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MoneyToNumber(this string text)
        {
            string prefix = "";
            text = new string(text.Where(c => !char.IsLetter(c)).ToArray());

            if (text.StartsWith("-")) prefix = "-";
            text = new string(text.Where(c => char.IsDigit(c)).ToArray());

            return prefix + text;
        }

        private static string ConvertMoney(this char[] ca, bool insertWhitespace)
        {
            string t = "";
            for (int i = 0; i < ca.Length; i++)
            {
                if (i % 3 == 0 && i != 0) t = "." + t;
                t = ca[i] + t;
            }
            return (insertWhitespace ? " " : "") + t;
        }
        #endregion
    }
}
