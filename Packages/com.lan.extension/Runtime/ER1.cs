using LAN.Helper;
using System;
using System.Collections.Generic;

namespace LAN.Helper {

    public static class ER1 {
        //private static string allAsciiChar = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ!\"#$%&\'()*+,-./:;<=>?@[\\]^_`{|}~ \t\n\r\x0b\x0c";
        //private static string asciiLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //private static string digits = "0123456789";
        //private static string punctuation = "!\"#$%&\'()*+,-./:;<=>?@[\\]^_`{|}~";

        /// <summary>
        /// Ecrypt a plain text
        /// </summary>
        /// <param name="plainText">Plain text to be encrypted</param>
        /// <returns>String of cipher text</returns>
        public static string Encrypt(string plainText, byte key = 0) {
            byte[] code = new byte[plainText.Length];

            for (int i = 0; i < plainText.Length; i++) {
                code[i] = (byte)(Convert.ToByte(plainText[i]) + key);
            }

            return BytesToString(code);
        }

        private static string BytesToString(byte[] code) {
            string text = "";

            foreach (byte b in code) {
                if (b < 10) {
                    text += "00" + b;
                } else if (b < 100) {
                    text += "0" + b;
                } else {
                    text += "" + b;
                }
            }

            return text;
        }

        /// <summary>
        /// Decrypt a cipher text
        /// </summary>
        /// <param name="cipherText">Cipher text to be decrypted</param>
        /// <returns>String of plain text</returns>
        public static string Decrypt(string cipherText, byte key = 0) {
            string plainText = "";

            foreach (byte b in StringToBytes(cipherText)) {
                plainText += Convert.ToChar(b - key);
            }

            return plainText;
        }

        private static byte[] StringToBytes(string text) {
            List<byte> code = new List<byte>();

            foreach (string st in Tools.SplitString(text, 3)) {
                code.Add(Convert.ToByte(st));
            }

            return code.ToArray();
        }

        //private static string[] SplitString(string text, byte charCount) {
        //    List<string> listString = new List<string>();
        //    for (short i = 0; i < text.Length; i += charCount) {
        //        listString.Add(text.Substring(i, charCount));
        //    }

        //    return listString.ToArray();
        //}
    }
}