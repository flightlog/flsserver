using FLS.Server.Data.DbEntities;
using FLS.Server.Service.Invoicing.Rules;
using FLS.Server.Service.RulesEngine;

namespace FLS.Server.Service.Invoicing.RuleEngines
{
    internal class InvoiceDetailsRulesEngine
    {
        private readonly RuleBasedFlightInvoiceDetails _flightInvoiceDetails;
        private readonly Flight _flight;

        public InvoiceDetailsRulesEngine(RuleBasedFlightInvoiceDetails flightInvoiceDetails, Flight flight)
        {
            _flightInvoiceDetails = flightInvoiceDetails;
            _flight = flight;
        }

        public RuleBasedFlightInvoiceDetails Run()
        {
            _flightInvoiceDetails.ApplyRule(new FlightInvoiceInfoRule(_flight));
            _flightInvoiceDetails.ApplyRule(new AdditionalInfoRule(_flight));

            return _flightInvoiceDetails;
        }
    }
}
