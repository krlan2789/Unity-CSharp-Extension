using System.Collections.Generic;

namespace LAN.Extension
{
    public static class DictionaryExtension
    {
        public static void AddFieldSafe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value) where TValue : struct
        {
            if (dictionary.ContainsKey(key)) dictionary[key] = value;
            else dictionary.Add(key, value);
        }

        public static TValue GetFieldSafe<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue) where TValue : struct
        {
            if (dictionary.ContainsKey(key)) defaultValue = dictionary[key];
            return defaultValue;
        }
    }
}
