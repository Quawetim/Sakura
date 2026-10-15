using System;
using System.Collections.Generic;
using UnityEngine;

namespace BadPractice.Cursoriam.Extension
{
    public static partial class DictionaryExtensions
    {
        public static T GetValueOrDefault<T>(this IDictionary<string, object> source, string key, T defaultValue = default)
        {
            if (source == null || !source.TryGetValue(key, out var value))
                return defaultValue;

            if (value is T genericValue)
                return genericValue;

            var type = typeof(T);
            if (type.IsEnum && Enum.TryParse(type, source[key].ToString(), true, out var result))
                return (T)result;

            try
            {
                return (T)Convert.ChangeType(source[key], type);
            }
            catch (Exception e)
            {
                Debug.LogError($"Can't convert type: {e}");
                return defaultValue;
            }
        }

        public static void AddOrUpdate<T, S>(this IDictionary<T, S> dict, T key, S item)
        {
            if (dict.TryAdd(key, item))
                return;

            dict[key] = item;
        }
    }
}