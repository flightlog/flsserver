using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Interfaces.Invoicing;
using NLog;
using FLS.Common.Validators;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.Interfaces.RulesEngine;
using FLS.Server.ProffixInvoiceService.RuleEngines;
using FLS.Server.ProffixInvoiceService.RuleFilters;

namespace FLS.Server.ProffixInvoiceService
{
    public class ProffixInvoiceService : IInvoiceService
    {
        private readonly IPersonService _personService;
        protected Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();
        private InvoiceMapping InvoiceMapping { get; set; }

        public ProffixInvoiceService(InvoiceMappingFactory invoiceMappingFactory, IPersonService personService)
        {
            _personService = personService;
            InvoiceMapping = invoiceMappingFactory.CreateInvoiceMapping();
        }

        public List<FlightInvoiceDetails> CreateFlightInvoiceDetails(List<Flight> flightsToInvoice, Guid clubId)
        {
            var flightInvoiceDetailList = new List<FlightInvoiceDetails>();

            var numberOfExceptions = 0;
            Exception lastException = null;

            foreach (var flight in flightsToInvoice)
            {
                Logger.Debug($"Start creating invoice for flight: {flight}.");
                try
                {
                    var flightInvoiceDetails = new ProffixFlightInvoiceDetails
                    {
                        FlightId = flight.FlightId,
                        FlightDate = flight.FlightDate.Value,
                        AircraftImmatriculation = flight.AircraftImmatriculation,
                        FlightInvoiceInfo = flight.FlightType.FlightTypeName,
                        ClubId = clubId
                    };

                    var recipientRulesEngine = new RecipientRulesEngine(flightInvoiceDetails, InvoiceMapping, flight, _personService);
                    recipientRulesEngine.Run();

                    var invoideDetailsRuleEngine = new InvoiceDetailsRulesEngine(flightInvoiceDetails, flight);
                    invoideDetailsRuleEngine.Run();

                    var invoiceLineRulesEngine = new InvoiceLineRulesEngine(flightInvoiceDetails, flight,
                        InvoiceMapping, _personService);
                    invoiceLineRulesEngine.Run();

                    Logger.Info($"Created invoice for {flightInvoiceDetails.RecipientDetails.RecipientName}: Flight-Date: {flightInvoiceDetails.FlightDate.ToShortDateString()} Aircraft: {flightInvoiceDetails.AircraftImmatriculation} Nr of Lines: {flightInvoiceDetails.FlightInvoiceLineItems.Count}");

                    flightInvoiceDetailList.Add(flightInvoiceDetails);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex,
                        $"Error while trying to create invoice details for flight: {flight}. Message: {ex.Message}");
                    numberOfExceptions++;
                    lastException = ex;
                }
            }

            //TODO: Handle exceptions correctly
            if (numberOfExceptions > 0 && lastException != null)
            {
                Logger.Error(lastException,
                    $"Total {numberOfExceptions} error(s) while trying to create invoice details for flights. Last exception message: {lastException.Message}");
            }

            Logger.Debug(
                $"Have {flightInvoiceDetailList.Count} flight invoice object(s) in result list for sending back to client.");

            return flightInvoiceDetailList.ToList();
        }
    }

}
