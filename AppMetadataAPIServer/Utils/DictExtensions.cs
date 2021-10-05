using System.Collections.Generic;

namespace AppMetadataAPIServer.Utils
{
    public static class DictExtensions
    {
        /// <summary>
        /// This one will return the value if key is in the dict, otherwise return the given default value.
        /// The key will still not be in the dictionary.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TVal GetOrDefault<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key, TVal defaultValue)
        {
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }
            return defaultValue;
        }


        /// <summary>
        /// This one will return the value if key is in the dict, otherwise it will insert the newValue into the dict and return it. 
        /// The new key and new value will be inserted into the dict.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static TVal GetOrCreate<TKey, TVal>(this IDictionary<TKey, TVal> dict, TKey key, TVal newValue)
        {
            if(!dict.ContainsKey(key))
            {
                dict[key] = newValue;
            }
            return dict[key];
        }
        
    }
}