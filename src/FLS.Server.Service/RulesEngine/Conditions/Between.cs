using System;
using System.Collections.Generic;

namespace FLS.Server.Service.RulesEngine.Conditions
{
    internal class Between<T> : ICondition where T : IComparable, IComparable<T>
    {
        private readonly T _value;
        private readonly T _minValue;
        private readonly T _maxValue;
        private readonly bool _includeMinValue;
        private readonly bool _includeMaxValue;

        public Between(T value, T minValue, T maxValue, bool includeMinValue = true, bool includeMaxValue = true)
        {
            _value = value;
            _minValue = minValue;
            _maxValue = maxValue;
            _includeMinValue = includeMinValue;
            _includeMaxValue = includeMaxValue;
        }

        public bool IsSatisfied()
        {
            bool lowerConditionTrue;
            bool upperConditionTrue;

            if (_includeMinValue)
            {
                lowerConditionTrue = Comparer<T>.Default.Compare(_value, _minValue) >= 0;
            }
            else
            {
                lowerConditionTrue = Comparer<T>.Default.Compare(_value, _minValue) > 0;
            }

            if (_includeMaxValue)
            {
                upperConditionTrue = Comparer<T>.Default.Compare(_value, _maxValue) <= 0;
            }
            else
            {
                upperConditionTrue = Comparer<T>.Default.Compare(_value, _maxValue) < 0;
            }

            return lowerConditionTrue && upperConditionTrue;
        }
    }
}
