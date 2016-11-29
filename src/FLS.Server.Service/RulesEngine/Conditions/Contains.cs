using System.Collections.Generic;
using System.Linq;

namespace FLS.Server.Service.RulesEngine.Conditions
{
    internal class Contains<T> : ICondition
    {
        private readonly ICollection<T> _collection;
        private readonly T _key;

        public Contains(ICollection<T> collection, T key)
        {
            _collection = collection;
            _key = key;
        }

        public bool IsSatisfied()
        {
            return _collection.Contains(_key);
        }

        public override string ToString()
        {
            return $"(collection: '{string.Join(",", _collection.Select(x => x))}' CONTAINS key: {_key} ==> {IsSatisfied()})";
        }
    }
}
