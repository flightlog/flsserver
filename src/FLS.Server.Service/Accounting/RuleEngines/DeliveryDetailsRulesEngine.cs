using FLS.Server.Data.DbEntities;
using FLS.Server.Service.Accounting.Rules;
using FLS.Server.Service.RulesEngine;

namespace FLS.Server.Service.Accounting.RuleEngines
{
    internal class DeliveryDetailsRulesEngine
    {
        private readonly RuleBasedDeliveryDetails _ruleBasedDelivery;
        private readonly Flight _flight;

        public DeliveryDetailsRulesEngine(RuleBasedDeliveryDetails ruleBasedDelivery, Flight flight)
        {
            _ruleBasedDelivery = ruleBasedDelivery;
            _flight = flight;
        }

        public RuleBasedDeliveryDetails Run()
        {
            _ruleBasedDelivery.ApplyRule(new FlightDeliveryInfoRule(_flight));
            _ruleBasedDelivery.ApplyRule(new AdditionalInfoRule(_flight));

            return _ruleBasedDelivery;
        }
    }
}
