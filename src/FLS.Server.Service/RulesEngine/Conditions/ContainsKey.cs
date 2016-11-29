using System.Collections.Generic;
using System.Linq;

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

        public override string ToString()
        {
            return $"(dictionary keys: '{string.Join(",", _dictionary.Keys.Select(x => x))}' CONTAINSKEY key: {_key} ==> {IsSatisfied()})";
        }
    }
}
