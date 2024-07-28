using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LAN.Extension
{
    public static class JsonExtension
    {
        public static string DictionaryToJson(this Dictionary<string, string> dict)
        {
            var entries = dict.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            return "{" + string.Join(",", entries) + "}";
        }

        public static T FromJson<T>(this string value)
        {
            if (value.Contains("\"try\":"))
            {
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

        public static string ToJson(this object value)
        {
            return JsonUtility.ToJson(value);
        }

        public static string ToJson(this object[] values)
        {
            string json = "[";
            foreach (object value in values)
            {
                json += JsonUtility.ToJson(value) + ",";
            }
            json = json.Substring(0, json.Length - 1);
            return json + "]";
        }
    }
}
