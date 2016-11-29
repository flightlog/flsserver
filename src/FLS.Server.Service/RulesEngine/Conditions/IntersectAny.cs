using System.Collections.Generic;
using System.Linq;

namespace FLS.Server.Service.RulesEngine.Conditions
{
    internal class IntersectAny<T> : ICondition
    {
        private readonly IEnumerable<T> _source;
        private readonly IEnumerable<T> _keys;

        public IntersectAny(IEnumerable<T> source, IEnumerable<T> keys)
        {
            _source = source;
            _keys = keys;
        }

        public bool IsSatisfied()
        {
            return _source.Intersect(_keys).Any();
        }

        public override string ToString()
        {
            return $"('{string.Join(",", _source.Select(x => x))}' INTERSECTS ANY {string.Join(",", _keys.Select(x => x))} ==> {IsSatisfied()})";
        }
    }
}
