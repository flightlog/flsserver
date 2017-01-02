using FLS.Server.Data.DbEntities;
using FLS.Server.Service.RulesEngine;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Accounting.Rules
{
    internal class AdditionalInfoRule : BaseRule<RuleBasedDeliveryDetails>
    {
        private readonly Flight _flight;

        internal AdditionalInfoRule(Flight flight)
        {
            _flight = flight;
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            Conditions.Add(new Equals<bool>(_flight.FlightType.InstructorRequired, true));
        }

        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            ruleBasedDelivery.AdditionalInformation = "1";

            return base.Apply(ruleBasedDelivery);
        }

        public override RuleBasedDeliveryDetails ElseApply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            ruleBasedDelivery.AdditionalInformation = "0";

            return base.Apply(ruleBasedDelivery);
        }
    }
}
