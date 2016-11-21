namespace FLS.Server.Service.RulesEngine.Conditions
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
