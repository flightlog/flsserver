using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLS.Server.Interfaces.RulesEngine
{
    public interface IRule<T>
    {
        void ClearConditions();
        void Initialize(T obj);
        bool IsValid();
        T Apply(T obj);

        T ElseApply(T obj);

        bool RuleApplied { get; }

        bool StopRuleEngineWhenRuleApplied { get; set; }
    }
}
