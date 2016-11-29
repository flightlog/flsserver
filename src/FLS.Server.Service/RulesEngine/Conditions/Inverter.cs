namespace FLS.Server.Service.RulesEngine.Conditions
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

        public override string ToString()
        {
            return $"(INVERTS condition: {_condition} ==> {IsSatisfied()})";
        }
    }
}
