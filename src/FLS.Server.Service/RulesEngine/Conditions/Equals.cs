namespace FLS.Server.Service.RulesEngine.Conditions
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
