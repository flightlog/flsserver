using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Server.Interfaces.RulesEngine;

namespace FLS.Server.ProffixInvoiceService.Conditions
{
    internal class Equals<T> : ICondition
    {
        private readonly T _actual;
        private readonly T _threshold;

        public Equals(T threshold, T actual)
        {
            _threshold = threshold;
            _actual = actual;
        }

        public bool IsSatisfied()
        {
            return Equals(_actual, _threshold);
        }
    }
}
