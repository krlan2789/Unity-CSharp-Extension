using System;

namespace LAN.Data {

    public class DataSecure {

        /// <summary>
        /// Encode plain text to Base64
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Encode(string plainText) {
            if (!string.IsNullOrEmpty(plainText)) {
                byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            }
            return "";
        }

        /// <summary>
        /// Decode Base64 to plain text
        /// </summary>
        /// <param name="encodedData"></param>
        /// <returns></returns>
        public static string Decode(string encodedData) {
            if (!string.IsNullOrEmpty(encodedData)) {
                byte[] base64EncodedBytes = System.Convert.FromBase64String(encodedData);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            return "";
        }

        public static string ToHexString(string plainText) {
            string results = "";
            foreach (char c in plainText) {
                results += Convert.ToByte(c).ToString("X") + " ";
            }
            return results.Trim();
        }

        public static string FromHexString(string hexString) {
            string results = "";
            string[] strs = hexString.Trim().Split(' ');
            foreach (string str in strs) {
                byte b = byte.Parse(str, System.Globalization.NumberStyles.HexNumber);
                results += Convert.ToChar(b);
            }
            return results;
        }
    }
}