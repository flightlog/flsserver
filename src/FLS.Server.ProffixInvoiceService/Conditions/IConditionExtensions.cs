using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Server.Interfaces.RulesEngine;

namespace FLS.Server.ProffixInvoiceService.Conditions
{
    public static class IConditionExtensions
    {
        public static bool Or(this ICondition conditionA, ICondition conditionB)
        {
            if (conditionA == null) return conditionB.IsSatisfied();
            if (conditionB == null) return conditionA.IsSatisfied();

            return conditionA.IsSatisfied() || conditionB.IsSatisfied();
        }

        public static bool And(this ICondition conditionA, ICondition conditionB)
        {
            if (conditionA == null || conditionB == null) return false;

            return conditionA.IsSatisfied() && conditionB.IsSatisfied();
        }
    }
}
