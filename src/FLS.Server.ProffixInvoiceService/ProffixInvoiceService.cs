using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Interfaces.Invoicing;
using NLog;
using FLS.Server.Data.DbEntities;
using FLS.Server.Interfaces;
using FLS.Server.ProffixInvoiceService.RuleEngines;
using FLS.Server.ProffixInvoiceService.RuleFilters;
using Newtonsoft.Json;

namespace FLS.Server.ProffixInvoiceService
{
    public class ProffixInvoiceService : IInvoiceService
    {
        private readonly InvoiceMappingFactory _invoiceMappingFactory;
        private readonly IPersonService _personService;
        private readonly IExtensionService _extensionService;
        private readonly IAircraftService _aircraftService;
        private readonly ILocationService _locationService;
        protected Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();
        private Dictionary<string, InvoiceRecipientTarget> FlightTypeToInvoiceRecipientMapping { get; set; }
        private InvoiceLineRuleFilterContainer InvoiceLineRuleFilterContainer { get; set; }

        public ProffixInvoiceService(InvoiceMappingFactory invoiceMappingFactory, IPersonService personService, 
            IExtensionService extensionService, IAircraftService aircraftService, ILocationService locationService)
        {
            _invoiceMappingFactory = invoiceMappingFactory;
            _personService = personService;
            _extensionService = extensionService;
            _aircraftService = aircraftService;
            _locationService = locationService;
        }

        public List<FlightInvoiceDetails> CreateFlightInvoiceDetails(List<Flight> flightsToInvoice, Guid clubId)
        {
            var extensionValue = _extensionService.GetExtensionStringValue("ProffixInvoiceLineRuleFilterContainer", clubId);
            InvoiceLineRuleFilterContainer = JsonConvert.DeserializeObject<InvoiceLineRuleFilterContainer>(extensionValue);

            extensionValue = _extensionService.GetExtensionStringValue("FlightTypeToInvoiceRecipientMapping", clubId);
            FlightTypeToInvoiceRecipientMapping = JsonConvert.DeserializeObject<Dictionary<string, InvoiceRecipientTarget>>(extensionValue);


            if (InvoiceLineRuleFilterContainer == null)
            {
                InvoiceLineRuleFilterContainer = _invoiceMappingFactory.CreateInvoiceLineRuleFilterContainer();
                var stringValue = JsonConvert.SerializeObject(InvoiceLineRuleFilterContainer);
                _extensionService.SaveExtensionStringValue("ProffixInvoiceLineRuleFilterContainer", stringValue, clubId);
            }

            if (FlightTypeToInvoiceRecipientMapping == null)
            {
                FlightTypeToInvoiceRecipientMapping = _invoiceMappingFactory.CreateFlightTypeToInvoiceRecipientMapping();
                var stringValue = JsonConvert.SerializeObject(FlightTypeToInvoiceRecipientMapping);
                _extensionService.SaveExtensionStringValue("FlightTypeToInvoiceRecipientMapping", stringValue, clubId);
            }


            //validate rules and re-map keys to IDs
            foreach (var aircraftRuleFilter in InvoiceLineRuleFilterContainer.AircraftRuleFilters)
            {
                foreach (var aircraftImmatriculation in aircraftRuleFilter.AircraftImmatriculations)
                {
                    var aircraft = _aircraftService.GetAircraftDetails(aircraftImmatriculation);

                    if (aircraft == null)
                    {
                        Logger.Warn($"Aircraft immatriculation {aircraftImmatriculation} for invoice rule {aircraftRuleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        aircraftRuleFilter.Aircrafts.Add(aircraft.AircraftId);
                    }
                }

                foreach (var icaoCode in aircraftRuleFilter.MatchedLdgLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {aircraftRuleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        aircraftRuleFilter.MatchedLdgLocationIds.Add(location.LocationId);
                    }
                }

                foreach (var icaoCode in aircraftRuleFilter.MatchedStartLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {aircraftRuleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        aircraftRuleFilter.MatchedStartLocationIds.Add(location.LocationId);
                    }
                }
            }

            foreach (var ruleFilter in InvoiceLineRuleFilterContainer.LandingTaxRuleFilters)
            {
                foreach (var aircraftImmatriculation in ruleFilter.AircraftImmatriculations)
                {
                    var aircraft = _aircraftService.GetAircraftDetails(aircraftImmatriculation);

                    if (aircraft == null)
                    {
                        Logger.Warn($"Aircraft immatriculation {aircraftImmatriculation} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.Aircrafts.Add(aircraft.AircraftId);
                    }
                }

                foreach (var icaoCode in ruleFilter.MatchedLdgLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.MatchedLdgLocationIds.Add(location.LocationId);
                    }
                }

                foreach (var icaoCode in ruleFilter.MatchedStartLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.MatchedStartLocationIds.Add(location.LocationId);
                    }
                }
            }

            foreach (var ruleFilter in InvoiceLineRuleFilterContainer.AdditionalFuelFeeRuleFilters)
            {
                foreach (var aircraftImmatriculation in ruleFilter.AircraftImmatriculations)
                {
                    var aircraft = _aircraftService.GetAircraftDetails(aircraftImmatriculation);

                    if (aircraft == null)
                    {
                        Logger.Warn($"Aircraft immatriculation {aircraftImmatriculation} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.Aircrafts.Add(aircraft.AircraftId);
                    }
                }

                foreach (var icaoCode in ruleFilter.MatchedLdgLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.MatchedLdgLocationIds.Add(location.LocationId);
                    }
                }

                foreach (var icaoCode in ruleFilter.MatchedStartLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.MatchedStartLocationIds.Add(location.LocationId);
                    }
                }
            }

            foreach (var ruleFilter in InvoiceLineRuleFilterContainer.NoLandingTaxRuleFilters)
            {
                foreach (var aircraftImmatriculation in ruleFilter.AircraftImmatriculations)
                {
                    var aircraft = _aircraftService.GetAircraftDetails(aircraftImmatriculation);

                    if (aircraft == null)
                    {
                        Logger.Warn($"Aircraft immatriculation {aircraftImmatriculation} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.Aircrafts.Add(aircraft.AircraftId);
                    }
                }

                foreach (var icaoCode in ruleFilter.MatchedLdgLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.MatchedLdgLocationIds.Add(location.LocationId);
                    }
                }

                foreach (var icaoCode in ruleFilter.MatchedStartLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.MatchedStartLocationIds.Add(location.LocationId);
                    }
                }
            }

            foreach (var ruleFilter in InvoiceLineRuleFilterContainer.VsfFeeRuleFilters)
            {
                foreach (var aircraftImmatriculation in ruleFilter.AircraftImmatriculations)
                {
                    var aircraft = _aircraftService.GetAircraftDetails(aircraftImmatriculation);

                    if (aircraft == null)
                    {
                        Logger.Warn($"Aircraft immatriculation {aircraftImmatriculation} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.Aircrafts.Add(aircraft.AircraftId);
                    }
                }

                foreach (var icaoCode in ruleFilter.MatchedLdgLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.MatchedLdgLocationIds.Add(location.LocationId);
                    }
                }

                foreach (var icaoCode in ruleFilter.MatchedStartLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {ruleFilter.RuleFilterName} not found!");
                    }
                    else
                    {
                        ruleFilter.MatchedStartLocationIds.Add(location.LocationId);
                    }
                }
            }
            
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

                    var recipientRulesEngine = new RecipientRulesEngine(flightInvoiceDetails, flight, _personService, FlightTypeToInvoiceRecipientMapping);
                    recipientRulesEngine.Run();

                    var invoiceDetailsRuleEngine = new InvoiceDetailsRulesEngine(flightInvoiceDetails, flight);
                    invoiceDetailsRuleEngine.Run();

                    var invoiceLineRulesEngine = new InvoiceLineRulesEngine(flightInvoiceDetails, flight,
                        _personService, InvoiceLineRuleFilterContainer);
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
