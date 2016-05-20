using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FLS.Server.Interfaces.RulesEngine;
using NLog;

namespace FLS.Server.ProffixInvoiceService.Rules
{
    public abstract class BaseRule<T> : IRule<T>
    {
        protected Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        protected BaseRule()
        {
            Conditions = new List<ICondition>();
        }

        public IList<ICondition> Conditions { get; set; }

        public void ClearConditions()
        {
            Conditions.Clear();
            RuleApplied = false;
        }

        public virtual bool IsValid()
        {
            return Conditions.All(x => x.IsSatisfied());
        }

        public abstract void Initialize(T obj);

        public virtual T Apply(T obj)
        {
            Logger.Debug($"Applied rule {this.GetType().Name}");
            RuleApplied = true;
            return obj;
        }

        public virtual T ElseApply(T obj)
        {
            return obj;
        }

        public bool RuleApplied { get; protected set; }

        public bool StopRuleEngineWhenRuleApplied { get; set; }
    }
}
