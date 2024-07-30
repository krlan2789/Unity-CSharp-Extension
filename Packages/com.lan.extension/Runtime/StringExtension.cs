using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LAN.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// Split text by group of character count.
        /// </summary>
        /// <param name="text">Text to be split.</param>
        /// <param name="charCount">Length of text.</param>
        /// <returns>Array of string</returns>
        public static string[] SplitString(this string text, byte charCount)
        {
            List<string> listString = new List<string>();
            for (int i = 0; i < text.Length; i += charCount)
            {
                listString.Add(text.Substring(i, charCount));
            }

            return listString.ToArray();
        }

        public static void CopyToClipboard(this string text)
        {
            TextEditor te = new() { text = text };
            te.SelectAll();
            te.Copy();
            Debug.Log("Text copied!");
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static string Capitalize(string text)
        {
            if (text.Contains("."))
            {
                string temp = text.Substring(1, text.IndexOf(' ') - 1);
                text = temp.ToUpper() + text.Substring(text.IndexOf(' '));
            }
            if (!string.IsNullOrEmpty(text))
            {
                if (text.Contains(" "))
                {
                    string[] p = text.Split(' ');
                    text = "";
                    foreach (string s in p)
                    {
                        int a = 0;
                        string t = "";
                        if (!Regex.IsMatch(s[0].ToString(), "^[a-zA-Z0-9]*$"))
                        {
                            a = 1;
                            t = s[0].ToString();
                        }
                        text += t + s[a].ToString().ToUpper() + s.Substring(a + 1) + " ";
                    }
                } else
                {
                    text = text[0].ToString().ToUpper() + text.Substring(1);
                }
            }
            return text;
        }
    }
}
