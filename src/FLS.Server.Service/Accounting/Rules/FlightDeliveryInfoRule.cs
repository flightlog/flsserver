using FLS.Server.Data.DbEntities;
using FLS.Server.Service.RulesEngine;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Accounting.Rules
{
    internal class FlightDeliveryInfoRule : BaseRule<RuleBasedDeliveryDetails>
    {
        private readonly Flight _flight;

        internal FlightDeliveryInfoRule(Flight flight)
        {
            _flight = flight;
        }

        public override void Initialize(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            Conditions.Add(new Equals<bool>(ruleBasedDelivery.IsChargedToClubInternal, true));
        }

        public override RuleBasedDeliveryDetails Apply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (_flight.FlightType.IsPassengerFlight)
            {
                ruleBasedDelivery.DeliveryInformation = $"{_flight.FlightType.FlightTypeName} {_flight.PilotDisplayName} mit PAX: {_flight.PassengerDisplayName}";
            }
            else if (string.IsNullOrWhiteSpace(_flight.InstructorDisplayName) == false)
            {
                ruleBasedDelivery.DeliveryInformation = $"{_flight.FlightType.FlightTypeName} {_flight.PilotDisplayName} mit FL: {_flight.InstructorDisplayName}";
            }
            else if (string.IsNullOrWhiteSpace(_flight.CoPilotDisplayName) == false)
            {
                ruleBasedDelivery.DeliveryInformation = $"{_flight.FlightType.FlightTypeName} {_flight.PilotDisplayName} mit {_flight.CoPilotDisplayName}";
            }
            else
            {
                ruleBasedDelivery.DeliveryInformation = $"{_flight.FlightType.FlightTypeName} {_flight.PilotDisplayName}";
            }

            return base.Apply(ruleBasedDelivery);
        }

        public override RuleBasedDeliveryDetails ElseApply(RuleBasedDeliveryDetails ruleBasedDelivery)
        {
            if (_flight.FlightType.IsPassengerFlight)
            {
                ruleBasedDelivery.DeliveryInformation = $"{_flight.FlightType.FlightTypeName} mit PAX: {_flight.PassengerDisplayName}";
            }
            else if (string.IsNullOrWhiteSpace(_flight.InstructorDisplayName) == false)
            {
                ruleBasedDelivery.DeliveryInformation = $"{_flight.FlightType.FlightTypeName}";
            }
            else if (string.IsNullOrWhiteSpace(_flight.CoPilotDisplayName) == false)
            {
                ruleBasedDelivery.DeliveryInformation = $"{_flight.FlightType.FlightTypeName}";
            }
            else
            {
                ruleBasedDelivery.DeliveryInformation = $"{_flight.FlightType.FlightTypeName}";
            }

            return base.Apply(ruleBasedDelivery);
        }
    }
}
