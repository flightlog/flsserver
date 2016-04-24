using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Server.Interfaces.RulesEngine;

namespace FLS.Server.ProffixInvoiceService.Conditions
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
    }
}
