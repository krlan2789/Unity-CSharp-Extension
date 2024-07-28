using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine.UI;


namespace LAN.Helper {
    /// <summary>
    /// DD_MM_YYYY = 31-12-2020, YYYY_MM_DD = 2020-12-31, DD_MMM_YYYY = 31 Jan 2020, D_M_Y = 31 Januari 2020
    /// </summary>
    public enum DateFormat {
        DD_MM_YYYY = 0, YYYY_MM_DD, DD_MMM_YYYY, D_M_Y
    }

    public static class Tools {
        private static string IV = "";

        #region Format Money/currency
        /// <summary>Format string of plain money to standard money in Indonesia Rupiah.
        /// <para>Return be like : Rp 10.500, Rp 135.000, Rp 505.050</para>
        /// </summary>
        public static string ToMoney(string text, bool insertWhitespace = true, bool showUnit = true) {
            char[] ca = text.ToCharArray();
            Array.Reverse(ca);
            string m = ConvertMoney(ca, insertWhitespace, showUnit);
            if (Convert.ToInt64(text) < 0) m = m.Length % 4 == 0 ? m.Replace("-.", "-") : m;
            return m;
        }

        /// <summary>Format long/int/short/byte to standard money in Indonesia Rupiah.
        /// <para>Return be like : Rp 10.500, Rp 135.000, Rp 505.050</para>
        /// </summary>
        public static string ToMoney(long number, bool insertWhitespace = true, bool showUnit = true) {
            char[] ca = Convert.ToString(number).ToCharArray();
            Array.Reverse(ca);
            string m = ConvertMoney(ca, insertWhitespace, showUnit);
            if (number < 0) m = m.Length % 4 == 0 ? m.Replace("-.", "-") : m;
            return m;
        }

        public static string MoneyToNumber(string text) {
            text = text.ToLower().Replace("-", "").Replace("rp", "").Replace(" ", "").Replace(",", "").Replace(".", "");
            //string[] ta = text.Split('.');
            //text = "";
            //foreach (string f in ta) {
            //    text += f;
            //}
            return text;
        }

        private static string ConvertMoney(char[] ca, bool insertWhitespace, bool showUnit) {
            string t = "";
            for (int i = 0; i < ca.Length; i++) {
                if (i % 3 == 0 && i != 0) t = "." + t;
                t = ca[i] + t;
            }
            return (showUnit ? "Rp" : "") + (insertWhitespace ? " " : "") + t;
        }
        #endregion

        #region Date and Time
        /// <summary>Convert month (int) (0-11) from month (string.)
        /// <para>Return month (string) (default = Indonesian), return '-' if not identification.</para>
        /// </summary>
        /// <param name="index">Month in number.</param>
        /// <param name="isShortName">If true, return a short name of month.</param>
        /// <param name="language">Month in selected language by code (default = ID).</param>
        public static string ToStringMonth(int index, bool isShortName = false, string language = "ID") {
            if (language == "EN") {
                switch (index) {
                    case 0:
                        return isShortName ? "Jan" : "January";
                    case 1:
                        return isShortName ? "Feb" : "February";
                    case 2:
                        return isShortName ? "Mar" : "March";
                    case 3:
                        return isShortName ? "Apr" : "April";
                    case 4:
                        return "Mei";
                    case 5:
                        return isShortName ? "Jun" : "June";
                    case 6:
                        return isShortName ? "Jul" : "July";
                    case 7:
                        return isShortName ? "Aug" : "August";
                    case 8:
                        return isShortName ? "Sep" : "September";
                    case 9:
                        return isShortName ? "Oct" : "October";
                    case 10:
                        return isShortName ? "Nov" : "November";
                    case 11:
                        return isShortName ? "Dec" : "December";
                    default:
                        return "-";
                }
            } else {
                switch (index) {
                    case 0:
                        return isShortName ? "Jan" : "Januari";
                    case 1:
                        return isShortName ? "Feb" : "Februari";
                    case 2:
                        return isShortName ? "Mar" : "Maret";
                    case 3:
                        return isShortName ? "Apr" : "April";
                    case 4:
                        return "Mei";
                    case 5:
                        return isShortName ? "Jun" : "Juni";
                    case 6:
                        return isShortName ? "Jul" : "Juli";
                    case 7:
                        return isShortName ? "Ags" : "Agustus";
                    case 8:
                        return isShortName ? "Sep" : "September";
                    case 9:
                        return isShortName ? "Okt" : "Oktober";
                    case 10:
                        return isShortName ? "Nov" : "November";
                    case 11:
                        return isShortName ? "Des" : "Desember";
                    default:
                        return "-";
                }
            }
        }

        /// <summary>Convert month (string) (January, feb, oktober, may) to month (byte).
        /// <para>Return month (byte) (0-11), 12 if not idetification.</para>
        /// </summary>
        /// <param name="name">Month in text (string).</param>
        public static byte ToIntMonth(string name) {
            switch (name.ToLower()) {
                case "januari":
                case "january":
                case "jan":
                case "1":
                    return 0;
                case "februari":
                case "february":
                case "feb":
                case "2":
                    return 1;
                case "maret":
                case "march":
                case "mar":
                case "3":
                    return 2;
                case "april":
                case "apr":
                case "4":
                    return 3;
                case "mei":
                case "may":
                case "5":
                    return 4;
                case "juni":
                case "june":
                case "jun":
                case "6":
                    return 5;
                case "juli":
                case "july":
                case "jul":
                case "7":
                    return 6;
                case "agustus":
                case "august":
                case "aug":
                case "ags":
                case "8":
                    return 7;
                case "september":
                case "sep":
                case "9":
                    return 8;
                case "oktober":
                case "october":
                case "oct":
                case "okt":
                case "10":
                    return 9;
                case "november":
                case "nov":
                case "11":
                    return 10;
                case "desember":
                case "december":
                case "dec":
                case "des":
                case "12":
                    return 11;
                default:
                    return 12;
            }
        }

        /// <summary>
        /// Changing Date to desired format
        /// </summary>
        /// <param name="dateRaw">Date raw (2020-12-31)</param>
        /// <param name="dateFormat">Date format type</param>
        /// <returns></returns>
        public static string FormatDate(string dateRaw, DateFormat dateFormat) {
            if (dateRaw.Length > 10) dateRaw = dateRaw.Substring(0, 10);
            string[] time = dateRaw.Split('-');
            short day = Convert.ToInt16(time[2]);
            short month = Convert.ToInt16(time[1]);
            short year = Convert.ToInt16(time[0]);
            switch (dateFormat) {
                case DateFormat.DD_MMM_YYYY:
                    dateRaw = string.Format("{0} {1} {2}", day, ToStringMonth(month - 1, true), year);
                    break;
                case DateFormat.DD_MM_YYYY:
                    dateRaw = string.Format("{0:00}-{1:00}-{2:0000}", day, month, year);
                    break;
                case DateFormat.D_M_Y:
                    dateRaw = string.Format("{0} {1} {2}", day, ToStringMonth(month - 1), year);
                    break;
            }
            return dateRaw;
        }

        /// <summary>
        /// Get last date by month, year
        /// if month/year not filled, means this month/year
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string GetLastDate(sbyte month = -1, short year = 0) {
            string date = "" + (year != 0 ? year : DateTime.Now.Year);

            if (month < 0) {
                month = (sbyte)(DateTime.Now.Month - 1);
            }

            switch (month) {
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
        public static byte GetMonthTotalDay(sbyte month = -1, short year = 0) {
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
        public static string DateTimeNow(string iso = "", float secondFromNow = 0) {
            DateTime date = secondFromNow == 0 ? DateTime.Now : DateTime.Now.AddSeconds(secondFromNow);
            if (iso == "ISO8601") return date.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);
            else return date.ToString("yyyy-MM-dd HH:mm:ss").Replace(".", ":").Replace("/", "-");
        }

        /// <summary>
        /// Get today's date. Example: 2023-02-25
        /// </summary>
        /// <param name="secondFromNow"></param>
        /// <returns></returns>
        public static string DateShortNow(float secondFromNow = 0) {
            DateTime date = secondFromNow == 0 ? DateTime.Now : DateTime.Now.AddSeconds(secondFromNow);
            return date.ToString("yyyy-MM-dd").Replace("/", "-");
        }

        /// <summary>
        /// Get today's time. Example: 21:53:28
        /// </summary>
        /// <param name="secondFromNow"></param>
        /// <returns></returns>
        public static string TimeShortNow(float secondFromNow = 0) {
            DateTime date = secondFromNow == 0 ? DateTime.Now : DateTime.Now.AddSeconds(secondFromNow);
            return date.ToString("HH:mm:ss").Replace(".", ":");
        }

        public static long TimeToSeconds(int seconds, int minutes, int hours = 0, int days = 0) {
            long totalInSeconds = seconds;
            totalInSeconds += minutes * 60;
            totalInSeconds += hours * 60 * 60;
            totalInSeconds += days * 60 * 60 * 24;
            return totalInSeconds;
        }

        public static long TimeToSeconds(DateTime dateTime) {
            byte hour = (byte)dateTime.Hour;
            byte minute = (byte)dateTime.Minute;
            byte second = (byte)dateTime.Second;
            return TimeToSeconds(second, minute, hour);
        }
        #endregion

        /// <summary>
        /// Convert a phone number to a valid international phone number.
        /// </summary>
        /// <param name="phoneNumber">A valid/invalid phone number</param>
        public static string GetValidInterPhoneNumber(string phoneNumber) {
            if (phoneNumber.IsEmpty()) return "";

            string validPhoneNumber;

            //  Validasi format nomor telp
            if (phoneNumber[0] == '0') {
                validPhoneNumber = "62" + phoneNumber.Substring(1);
            } else if (phoneNumber[0] == '+') {
                validPhoneNumber = phoneNumber.Substring(1);
            } else {
                validPhoneNumber = phoneNumber;
            }
            return validPhoneNumber;
        }

        /// <summary>
        /// Encrypt a text(string) with own created method.
        /// </summary>
        /// <param name="text">Text to be encrypt.</param>
        /// <returns>Encrypted text</returns>
        public static string EncryptText(string text, byte key = 0) {
            string d = ER1.Encrypt(text, key);
            // Debug.Log(d);
            return d;
        }

        /// <summary>
        /// Decrypt encrypted text with own created method.
        /// </summary>
        /// <param name="code">Code to be decrypt.</param>
        /// <returns>Decrypted text</returns>
        public static string DecryptText(string code, byte key = 0) {
            return ER1.Decrypt(code, key);
        }

        /// <summary>
        /// Split text by group of character count.
        /// </summary>
        /// <param name="text">Text to be split.</param>
        /// <param name="charCount">Length of text.</param>
        /// <returns>Array of string</returns>
        public static string[] SplitString(string text, byte charCount) {
            List<string> listString = new List<string>();
            for (int i = 0; i < text.Length; i += charCount) {
                listString.Add(text.Substring(i, charCount));
            }

            return listString.ToArray();
        }

        public static Sprite[] GetSpritesAtPath(string path) {
            List<Sprite> result = new List<Sprite>();
            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

            foreach (string fileName in fileEntries) {
                if (fileName.Contains(".meta")) {
                    continue;
                }

                int index = fileName.LastIndexOf("/");
                string localPath = "Assets/" + path;

                if (index > 0) localPath += fileName.Substring(index);

                Debug.Log(localPath);

                Sprite t = Resources.Load<Sprite>(localPath);

                if (t != null) result.Add(t);
            }

            return result.ToArray();
        }

        public static string GetPlatform() {
            string device = "PC";
            if (Application.platform == RuntimePlatform.Android) {
                device = "ANDROID";
            } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                device = "iPHONE";
            }

            return device;
        }

        public static bool IsValidEmail(string email) {
            if (string.IsNullOrWhiteSpace(email)) return false;

            try {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match) {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            } catch (RegexMatchTimeoutException e) {
                return false;
            } catch (ArgumentException e) {
                return false;
            }

            try {
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            } catch (RegexMatchTimeoutException) {
                return false;
            }
        }

        public static string DictionaryToJson(Dictionary<string, string> dict) {
            var entries = dict.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            return "{" + string.Join(",", entries) + "}";
        }

        public static Color32 PaletteMaroon { get { return new Color32(95, 14, 34, 255); } }
        public static Color32 PaletteRed { get { return new Color32(215, 17, 6, 255); } }
        public static Color32 PaletteDark { get { return new Color32(21, 21, 21, 255); } }
    }

    #region Extension
    public static class CustomExtension {
        public static void SetActive(this Transform go, bool value) {
            go.gameObject.SetActive(value);
        }

        /// <summary>
        /// Find child from received path inside current gameobject if parent equals null
        /// </summary>
        /// <typeparam name="T">Type of Companent</typeparam>
        /// <param name="path">Path to target gameobject</param>
        /// <param name="parent">Parent of target gameobject</param>
        /// <returns></returns>
        public static T Find<T>(this Transform transform, string path) where T : UnityEngine.Object {
            Transform obj = transform.Find(path);
            if (obj != null) return obj.GetComponent<T>();
            else return null;
        }

        public static T FromJson<T>(this string value) {
            if (value.Contains("\"try\":")) {
                value.Replace("\"try\":", "\"try_\":");
            }
            return JsonUtility.FromJson<T>(value);
        }

        //public static T[] FromJsonArray<T>(this string value) {
        //    value = $"{{\"data\":{value}}}";
        //    if (value.Contains("\"try\":")) {
        //        value.Replace("\"try\":", "\"try_\":");
        //    }
        //    ResponseDataArray<T> res = JsonUtility.FromJson<ResponseDataArray<T>>(value);
        //    return res.data;
        //}

        public static string ToJson(this object value) {
            return JsonUtility.ToJson(value);
        }

        public static string ToJson(this object[] values) {
            string json = "[";
            foreach (object value in values) {
                json += JsonUtility.ToJson(value) + ",";
            }
            json = json.Substring(0, json.Length - 1);
            return json + "]";
        }

        public static void CopyToClipboard(this string text) {
            TextEditor te = new TextEditor { text = text };
            te.SelectAll();
            te.Copy();
            Debug.Log("Text copied!");
        }

        public static bool IsEmpty(this string value) {
            return string.IsNullOrEmpty(value);
        }

        public static void AddField<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value) where TValue : struct {
            if (dictionary.ContainsKey(key)) dictionary[key] = value;
            else dictionary.Add(key, value);
        }

        public static TValue GetField<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) where TValue : struct {
            if (dictionary.ContainsKey(key)) defaultValue = dictionary[key];
            return defaultValue;
        }

        public static void SetLeft(this RectTransform rt, float left) {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right) {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top) {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom) {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }

        public static Vector2Int GetOriginalSize(this Image img) {
            if (img.sprite == null) return new(0, 0);
            int imageWidth = img.sprite.texture.width;
            int imageHeight = img.sprite.texture.height;
            return new(imageWidth, imageHeight);
        }

        public static int GetRelativeKeyboardHeight(this RectTransform rectTransform, bool includeInput) {
            int keyboardHeight = GetKeyboardHeight(includeInput);
            float screenToRectRatio = Screen.height / rectTransform.rect.height;
            float keyboardHeightRelativeToRect = keyboardHeight / screenToRectRatio;

            return (int)keyboardHeightRelativeToRect;
        }

        private static int GetKeyboardHeight(bool includeInput) {
            if (Application.platform == RuntimePlatform.Android) {
                using (AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
                    AndroidJavaObject unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
                    AndroidJavaObject view = unityPlayer.Call<AndroidJavaObject>("getView");
                    AndroidJavaObject dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");
                    if (view == null || dialog == null)
                        return 0;
                    var decorHeight = 0;
                    if (includeInput) {
                        AndroidJavaObject decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");
                        if (decorView != null)
                            decorHeight = decorView.Call<int>("getHeight");
                    }
                    using (AndroidJavaObject rect = new AndroidJavaObject("android.graphics.Rect")) {
                        view.Call("getWindowVisibleDisplayFrame", rect);
                        return Screen.height - rect.Call<int>("height") + decorHeight;
                    }
                }
            } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                return (int)TouchScreenKeyboard.area.height;
            } else return 0;
        }
    }
    #endregion

    #region IComparer inherited class
    //public class ComparerDateCreateAtEn : IComparer {
    //    //  Sorting Object array
    //    public int Compare(object x, object y) {
    //        return new CaseInsensitiveComparer().Compare(((DateCreateAtEn)x).create_at, ((DateCreateAtEn)y).create_at);
    //    }
    //}

    public static class GenericsTool {
        public static object GetPropValue(object src, string propName) {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
    #endregion
}