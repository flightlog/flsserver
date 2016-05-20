using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Server.Interfaces.RulesEngine;

namespace FLS.Server.ProffixInvoiceService.Conditions
{
    internal class And : ICondition
    {
        private readonly ICondition _conditionA;
        private readonly ICondition _conditionB;

        public And(ICondition conditionA, ICondition conditionB)
        {
            _conditionA = conditionA;
            _conditionB = conditionB;
        }

        public bool IsSatisfied()
        {
            if (_conditionA == null || _conditionB == null) return false;
            return _conditionA.IsSatisfied() && _conditionB.IsSatisfied();
        }
    }
}
