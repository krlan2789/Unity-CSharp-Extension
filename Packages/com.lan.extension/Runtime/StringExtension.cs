using System.Collections.Generic;
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
            TextEditor te = new TextEditor { text = text };
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
    }
}
