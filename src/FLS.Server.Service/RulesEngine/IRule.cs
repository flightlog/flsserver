namespace FLS.Server.Service.RulesEngine
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
