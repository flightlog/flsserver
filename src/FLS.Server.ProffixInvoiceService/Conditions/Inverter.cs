using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Server.Interfaces.RulesEngine;

namespace FLS.Server.ProffixInvoiceService.Conditions
{
    internal class Inverter : ICondition
    {
        private readonly ICondition _condition;

        public Inverter(ICondition condition)
        {
            _condition = condition;
        }

        public bool IsSatisfied()
        {
            return _condition.IsSatisfied() == false;
        }
    }
}
