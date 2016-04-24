using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces.RulesEngine;
using FLS.Server.ProffixInvoiceService.Rules;

namespace FLS.Server.ProffixInvoiceService.RuleEngines
{
    internal class InvoiceDetailsRulesEngine
    {
        private readonly ProffixFlightInvoiceDetails _flightInvoiceDetails;
        private readonly Flight _flight;

        public InvoiceDetailsRulesEngine(ProffixFlightInvoiceDetails flightInvoiceDetails, Flight flight)
        {
            _flightInvoiceDetails = flightInvoiceDetails;
            _flight = flight;
        }

        public ProffixFlightInvoiceDetails Run()
        {
            _flightInvoiceDetails.ApplyRule(new FlightInvoiceInfoRule(_flight));
            _flightInvoiceDetails.ApplyRule(new AdditionalInfoRule(_flight));

            return _flightInvoiceDetails;
        }
    }
}
