using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Data.DbEntities;
using FLS.Server.ProffixInvoiceService.Conditions;

namespace FLS.Server.ProffixInvoiceService.Rules
{
    internal class FlightInvoiceInfoRule : BaseRule<ProffixFlightInvoiceDetails>
    {
        private readonly Flight _flight;

        internal FlightInvoiceInfoRule(Flight flight)
        {
            _flight = flight;
        }

        public override void Initialize(ProffixFlightInvoiceDetails flightInvoiceDetails)
        {
            Conditions.Add(new Equals<bool>(flightInvoiceDetails.IsInvoicedToClubInternal, true));
        }

        public override ProffixFlightInvoiceDetails Apply(ProffixFlightInvoiceDetails flightInvoiceDetails)
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

        public override ProffixFlightInvoiceDetails ElseApply(ProffixFlightInvoiceDetails flightInvoiceDetails)
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
