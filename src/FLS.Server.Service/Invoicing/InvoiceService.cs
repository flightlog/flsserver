using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi.Invoicing;
using FLS.Data.WebApi.Invoicing.RuleFilters;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Resources;
using FLS.Server.Interfaces;
using FLS.Server.Service.Invoicing.RuleEngines;
using Newtonsoft.Json;

namespace FLS.Server.Service.Invoicing
{
    public class InvoiceService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly InvoiceMappingFactory _invoiceMappingFactory;
        private readonly IPersonService _personService;
        private readonly IExtensionService _extensionService;
        private readonly IAircraftService _aircraftService;
        private readonly ILocationService _locationService;
        private Dictionary<string, InvoiceRecipientTarget> FlightTypeToInvoiceRecipientMapping { get; set; }
        private InvoiceLineRuleFilterContainer InvoiceLineRuleFilterContainer { get; set; }

        public InvoiceService(DataAccessService dataAccessService, IdentityService identityService, InvoiceMappingFactory invoiceMappingFactory, IPersonService personService, 
            IExtensionService extensionService, IAircraftService aircraftService, ILocationService locationService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            _invoiceMappingFactory = invoiceMappingFactory;
            _personService = personService;
            _extensionService = extensionService;
            _aircraftService = aircraftService;
            _locationService = locationService;
        }


        public List<FlightInvoiceDetails> GetFlightInvoiceDetails(DateTime fromDate, DateTime toDate, Guid clubId)
        {
            if (clubId.IsValid() == false)
            {
                Logger.Error("No valid ClubId for getting the invoices!");
                throw new InvalidDataException("No valid ClubId to fetch invoice data!");
            }

            //needed for flights without start time (null values in StartDateTime)
            DateTime fromDateTime = fromDate.Date;
            DateTime toDateTime = toDate.Date;
            if (toDate.Date < DateTime.MaxValue.Date)
            {
                toDateTime = toDate.Date.AddDays(1).AddTicks(-1);
            }
            else
            {
                toDateTime = DateTime.MaxValue;
            }

            bool isTodayIncluded = fromDate.Date <= DateTime.Now.Date && toDate.Date >= DateTime.Now.Date;

            try
            {
                using (var context = _dataAccessService.CreateDbContext())
                {
                    var flights =
                        context.Flights
                            .Include(Constants.Aircraft)
                            .Include(Constants.FlightType)
                            .Include(Constants.FlightCrews)
                            .Include(Constants.FlightCrews + "." + Constants.Person)
                            .Include(Constants.StartType)
                            .Include(Constants.StartLocation)
                            .Include(Constants.LdgLocation)
                            .Include(Constants.TowFlight)
                            .Include(Constants.TowFlight + "." + Constants.Aircraft)
                            .Include(Constants.TowFlight + "." + Constants.FlightType)
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews)
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person)
                            .Include(Constants.TowFlight + "." + Constants.StartLocation)
                            .Include(Constants.TowFlight + "." + Constants.LdgLocation)
                                     .OrderBy(c => c.StartDateTime)
                                     .Where(flight => (flight.StartDateTime.Value >= fromDateTime &&
                                                       flight.StartDateTime.Value <= toDateTime)
                                                      && flight.FlightType.ClubId == clubId
                                                      &&
                                                      (flight.FlightAircraftType ==
                                                       (int)FlightAircraftTypeValue.GliderFlight
                                                       ||
                                                       flight.FlightAircraftType ==
                                                       (int)FlightAircraftTypeValue.MotorFlight)
                                                      &&
                                                      (flight.ProcessStateId == (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked))
                                                      .ToList();

                    Logger.Debug(
                        string.Format("Queried Flights for Invoice between {1} and {2} and got {0} flights back.",
                                      flights.Count, fromDateTime, toDateTime));


                    var flightInvoiceDetails = GetFlightInvoiceDetails(flights, clubId);

                    return flightInvoiceDetails;

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while trying to create invoice for flights. Message: {ex.Message}");
                throw;
            }
        }

        public bool SetFlightAsInvoiced(FlightInvoiceBooking flightInvoiceBooking)
        {
            flightInvoiceBooking.ArgumentNotNull("flightInvoiceBooking");

            using (var context = _dataAccessService.CreateDbContext())
            {
                var flight = context.Flights.FirstOrDefault(f => f.FlightId == flightInvoiceBooking.FlightId);

                if (flight == null) return false;

                flight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Invoiced;
                flight.DoNotUpdateMetaData = true;
                flight.InvoicedOn = flightInvoiceBooking.InvoiceDate;
                flight.InvoiceNumber = flightInvoiceBooking.InvoiceNumber;
                flight.DeliveryNumber = flightInvoiceBooking.DeliveryNumber;

                if (flightInvoiceBooking.IncludesTowFlightId.HasValue)
                {
                    var towFlight =
                        context.Flights.FirstOrDefault(
                            f => f.FlightId == flightInvoiceBooking.IncludesTowFlightId.Value);

                    if (towFlight == null) return false;

                    towFlight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Invoiced;
                    towFlight.DoNotUpdateMetaData = true;
                    towFlight.InvoicedOn = flightInvoiceBooking.InvoiceDate;
                    towFlight.InvoiceNumber = flightInvoiceBooking.InvoiceNumber;
                    towFlight.DeliveryNumber = flightInvoiceBooking.DeliveryNumber;
                }

                context.SaveChanges();

                return true;
            }
        }

        public bool SetInvoiceNumberForDeliverables(string deliveryNumber, string invoiceNumber, Nullable<DateTime> invoicePaymentDate)
        {
            if (string.IsNullOrEmpty(deliveryNumber))
            {
                return false;
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var flights = context.Flights.Where(f => f.DeliveryNumber == deliveryNumber);

                foreach (var flight in flights)
                {
                    flight.InvoiceNumber = invoiceNumber;
                    flight.DoNotUpdateMetaData = true;

                    if (invoicePaymentDate.HasValue)
                    {
                        flight.InvoicePaidOn = invoicePaymentDate.Value;
                    }
                }

                context.SaveChanges();
                return true;
            }
        }

        public bool SetInvoiceAsPaid(string invoiceNumber, DateTime invoicePaymentDate)
        {
            if (string.IsNullOrEmpty(invoiceNumber))
            {
                return false;
            }

            if (invoicePaymentDate > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("invoicePaymentDate", "invoicePaymentDate can not be in future");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var flights = context.Flights.Where(f => f.InvoiceNumber == invoiceNumber);

                foreach (var flight in flights)
                {
                    flight.InvoicePaidOn = invoicePaymentDate;
                    flight.DoNotUpdateMetaData = true;
                }

                context.SaveChanges();
                return true;
            }
        }

        private List<FlightInvoiceDetails> GetFlightInvoiceDetails(List<Flight> flightsToInvoice, Guid clubId)
        {
            var extensionValue = _extensionService.GetExtensionStringValue("InvoiceLineRuleFilterContainer", clubId);
            InvoiceLineRuleFilterContainer = JsonConvert.DeserializeObject<InvoiceLineRuleFilterContainer>(extensionValue);

            extensionValue = _extensionService.GetExtensionStringValue("FlightTypeToInvoiceRecipientMapping", clubId);
            FlightTypeToInvoiceRecipientMapping = JsonConvert.DeserializeObject<Dictionary<string, InvoiceRecipientTarget>>(extensionValue);


            if (InvoiceLineRuleFilterContainer == null)
            {
                InvoiceLineRuleFilterContainer = _invoiceMappingFactory.CreateInvoiceLineRuleFilterContainer();
                var stringValue = JsonConvert.SerializeObject(InvoiceLineRuleFilterContainer);
                _extensionService.SaveExtensionStringValue("InvoiceLineRuleFilterContainer", stringValue, clubId);
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
                    var flightInvoiceDetails = new RuleBasedFlightInvoiceDetails
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
