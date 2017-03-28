using System.Collections.Generic;

namespace Essence.Util.Collections
{
    public static class DictionaryUtils
    {
        public static void AddOrSet<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static void AddOrSetAll<TK, TV>(this IDictionary<TK, TV> dictionary, IEnumerable<KeyValuePair<TK, TV>> enumer)
        {
            foreach (KeyValuePair<TK, TV> kvp in enumer)
            {
                dictionary.AddOrSet(kvp.Key, kvp.Value);
            }
        }

        public static void AddOnlyFirst<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV value)
        {
            if (dictionary.ContainsKey(key))
            {
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static void AddOnlyFirstAll<TK, TV>(this IDictionary<TK, TV> dictionary, IEnumerable<KeyValuePair<TK, TV>> enumer)
        {
            foreach (KeyValuePair<TK, TV> kvp in enumer)
            {
                dictionary.AddOnlyFirst(kvp.Key, kvp.Value);
            }
        }

        public static TV GetSafe<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV defValue = default(TV))
        {
            TV value;
            if (!dictionary.TryGetValue(key, out value))
            {
                return defValue;
            }
            return value;
        }

        public static TV? GetSafeSt<TK, TV>(this IDictionary<TK, TV> dictionary, TK key)
            where TV : struct
        {
            TV value;
            if (!dictionary.TryGetValue(key, out value))
            {
                return null;
            }
            return value;
        }
    }
}