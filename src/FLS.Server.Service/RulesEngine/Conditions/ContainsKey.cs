using System.Collections.Generic;

namespace FLS.Server.Service.RulesEngine.Conditions
{
    internal class ContainsKey<TKey, TValue> : ICondition
    {
        private readonly IDictionary<TKey, TValue> _dictionary;
        private readonly TKey _key;

        public ContainsKey(IDictionary<TKey, TValue> dictionary, TKey key)
        {
            _dictionary = dictionary;
            _key = key;
        }

        public bool IsSatisfied()
        {
            return _dictionary.ContainsKey(_key);
        }
    }
}
