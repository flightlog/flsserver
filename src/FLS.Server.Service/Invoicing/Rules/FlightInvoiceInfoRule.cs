using FLS.Server.Data.DbEntities;
using FLS.Server.Service.RulesEngine;
using FLS.Server.Service.RulesEngine.Conditions;

namespace FLS.Server.Service.Invoicing.Rules
{
    internal class FlightInvoiceInfoRule : BaseRule<RuleBasedFlightInvoiceDetails>
    {
        private readonly Flight _flight;

        internal FlightInvoiceInfoRule(Flight flight)
        {
            _flight = flight;
        }

        public override void Initialize(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            Conditions.Add(new Equals<bool>(flightInvoiceDetails.IsInvoicedToClubInternal, true));
        }

        public override RuleBasedFlightInvoiceDetails Apply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            if (_flight.FlightType.IsPassengerFlight)
            {
                flightInvoiceDetails.FlightInvoiceInfo = $"{_flight.FlightType.FlightTypeName} {_flight.PilotDisplayName} mit PAX: {_flight.PassengerDisplayName}";
            }
            else if (string.IsNullOrWhiteSpace(_flight.InstructorDisplayName) == false)
            {
                flightInvoiceDetails.FlightInvoiceInfo = $"{_flight.FlightType.FlightTypeName} {_flight.PilotDisplayName} mit FL: {_flight.InstructorDisplayName}";
            }
            else if (string.IsNullOrWhiteSpace(_flight.CoPilotDisplayName) == false)
            {
                flightInvoiceDetails.FlightInvoiceInfo = $"{_flight.FlightType.FlightTypeName} {_flight.PilotDisplayName} mit {_flight.CoPilotDisplayName}";
            }
            else
            {
                flightInvoiceDetails.FlightInvoiceInfo = $"{_flight.FlightType.FlightTypeName} {_flight.PilotDisplayName}";
            }

            return base.Apply(flightInvoiceDetails);
        }

        public override RuleBasedFlightInvoiceDetails ElseApply(RuleBasedFlightInvoiceDetails flightInvoiceDetails)
        {
            if (_flight.FlightType.IsPassengerFlight)
            {
                flightInvoiceDetails.FlightInvoiceInfo = $"{_flight.FlightType.FlightTypeName} mit PAX: {_flight.PassengerDisplayName}";
            }
            else if (string.IsNullOrWhiteSpace(_flight.InstructorDisplayName) == false)
            {
                flightInvoiceDetails.FlightInvoiceInfo = $"{_flight.FlightType.FlightTypeName}";
            }
            else if (string.IsNullOrWhiteSpace(_flight.CoPilotDisplayName) == false)
            {
                flightInvoiceDetails.FlightInvoiceInfo = $"{_flight.FlightType.FlightTypeName}";
            }
            else
            {
                flightInvoiceDetails.FlightInvoiceInfo = $"{_flight.FlightType.FlightTypeName}";
            }

            return base.Apply(flightInvoiceDetails);
        }
    }
}
