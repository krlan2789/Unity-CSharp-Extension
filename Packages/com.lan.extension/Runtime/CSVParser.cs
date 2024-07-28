using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LAN.Helper {
    public static class CSVParser {
        public static Dictionary<string, Dictionary<string, string>> ParseAreaPDAM(string dataset, char sparator, char codeSparator) {
            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            string[] dataLines = dataset.ToLower().Split('\n');

            for (int i = 0; i < dataLines.Length; i++) {
                if (!dataLines[i].Contains(codeSparator.ToString())) continue;
                string[] data = dataLines[i].Split(sparator);

                for (int d = 0; d < data.Length; d++) {
                    if (d == 0) {
                        data[0] = data[0].Trim();
                        string key = Capitalize(data[0]);

                        if (!values.ContainsKey(key)) values.Add(key, new Dictionary<string, string>());
                    } else {
                        if (!data[d].Contains(codeSparator.ToString())) continue;
                        data[d] = data[d].Trim();
                        string[] k = data[d].Split(codeSparator);
                        string key = Capitalize(data[0]);
                        k[0] = Capitalize(k[0]);
                        if (values.ContainsKey(key)) {
                            if (values[key].ContainsKey(k[0])) values[key][k[0]] = k[1].ToUpper();
                            else values[key].Add(k[0], k[1].ToUpper());
                        } else {
                            Debug.Log("Key -" + key + "- not present!");
                        }
                    }
                }
            }
            return values;
        }

        public static string Capitalize(string text) {
            if (text.Contains(".")) {
                string temp = text.Substring(1, text.IndexOf(' ') - 1);
                text = temp.ToUpper() + text.Substring(text.IndexOf(' '));
            }
            if (!string.IsNullOrEmpty(text)) {
                if (text.Contains(" ")) {
                    string[] p = text.Split(' ');
                    text = "";
                    foreach (string s in p) {
                        int a = 0;
                        string t = "";
                        if (!Regex.IsMatch(s[0].ToString(), "^[a-zA-Z0-9]*$")) {
                            a = 1;
                            t = s[0].ToString();
                        }
                        text += t + s[a].ToString().ToUpper() + s.Substring(a + 1) + " ";
                    }
                } else {
                    text = text[0].ToString().ToUpper() + text.Substring(1);
                }
            }
            return text;
        }
    }
}
