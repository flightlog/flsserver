using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FLS.Common.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Returns the default value of type U if the key does not exist in the dictionary
        /// http://www.nilzorblog.com/2013/01/improving-generic-c-dictionary-with.html
        /// </summary>
        public static U GetOrDefault<T, U>(this Dictionary<T, U> dic, T key)
        {
            if (dic.ContainsKey(key)) return dic[key];
            return default(U);
        }

        public static string GetOrReturnKey(this Dictionary<string, string> dic, string key)
        {
            if (dic.ContainsKey(key)) return dic[key];
            return key;
        }

        /// <summary>
        /// Returns an existing value U for key T, or creates a new instance of type U using the default constructor, 
        /// adds it to the dictionary and returns it.
        /// http://www.nilzorblog.com/2013/01/improving-generic-c-dictionary-with.html
        /// </summary>
        public static U GetOrInsertNew<T, U>(this Dictionary<T, U> dic, T key)
            where U : new()
        {
            if (dic.ContainsKey(key)) return dic[key];
            U newObj = new U();
            dic[key] = newObj;
            return newObj;
        }
    }
}
