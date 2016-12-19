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
                            .Include(Constants.FlightCrews + "." + Constants.Person + "." + Constants.PersonClubs)
                            .Include(Constants.StartType)
                            .Include(Constants.StartLocation)
                            .Include(Constants.LdgLocation)
                            .Include(Constants.TowFlight)
                            .Include(Constants.TowFlight + "." + Constants.Aircraft)
                            .Include(Constants.TowFlight + "." + Constants.FlightType)
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews)
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person)
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person + "." + Constants.PersonClubs)
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
                }

                context.SaveChanges();

                return true;
            }
        }

        public bool SetFlightAsDelivered(FlightDeliveryBooking flightDeliveryBooking)
        {
            flightDeliveryBooking.ArgumentNotNull("flightDeliveryBooking");

            using (var context = _dataAccessService.CreateDbContext())
            {
                var flight = context.Flights.FirstOrDefault(f => f.FlightId == flightDeliveryBooking.FlightId);

                if (flight == null) return false;

                flight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Delivered;
                flight.DoNotUpdateMetaData = true;
                flight.DeliveredOn = flightDeliveryBooking.DeliveryDate;
                flight.DeliveryNumber = flightDeliveryBooking.DeliveryNumber;

                if (flightDeliveryBooking.IncludesTowFlightId.HasValue)
                {
                    var towFlight =
                        context.Flights.FirstOrDefault(
                            f => f.FlightId == flightDeliveryBooking.IncludesTowFlightId.Value);

                    if (towFlight == null) return false;

                    towFlight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Delivered;
                    towFlight.DoNotUpdateMetaData = true;
                    towFlight.DeliveredOn = flightDeliveryBooking.DeliveryDate;
                    towFlight.DeliveryNumber = flightDeliveryBooking.DeliveryNumber;
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
                    flight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Invoiced;
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
            List<InvoiceRuleFilterDetails> invoiceRuleFilters = _invoiceMappingFactory.CreateInvoiceRuleFilters();
            
            //validate rules and re-map keys to IDs
            foreach (var filter in invoiceRuleFilters)
            {
                foreach (var aircraftImmatriculation in filter.AircraftImmatriculations)
                {
                    var aircraft = _aircraftService.GetAircraftDetails(aircraftImmatriculation);

                    if (aircraft == null)
                    {
                        Logger.Warn($"Aircraft immatriculation {aircraftImmatriculation} for invoice rule {filter.RuleFilterName} not found!");
                    }
                    else
                    {
                        filter.Aircrafts.Add(aircraft.AircraftId);
                    }
                }

                foreach (var icaoCode in filter.MatchedLdgLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {filter.RuleFilterName} not found!");
                    }
                    else
                    {
                        filter.MatchedLdgLocationIds.Add(location.LocationId);
                    }
                }

                foreach (var icaoCode in filter.MatchedStartLocations)
                {
                    var location = _locationService.GetLocationDetailsByIcaoCode(icaoCode);

                    if (location == null)
                    {
                        Logger.Warn($"Location ICAO code {icaoCode} for invoice rule {filter.RuleFilterName} not found!");
                    }
                    else
                    {
                        filter.MatchedStartLocationIds.Add(location.LocationId);
                    }
                }
            }
            
            var flightInvoiceDetailList = new List<FlightInvoiceDetails>();

            var numberOfExceptions = 0;
            Exception lastException = null;

            foreach (var flight in flightsToInvoice)
            {
                Logger.Debug($"Start creating invoice for flight: {flight} using {invoiceRuleFilters.Count} recipient rule filters and {invoiceRuleFilters.Count} invoice line rule filters.");
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

                    var recipientRulesEngine = new RecipientRulesEngine(flightInvoiceDetails, flight, _personService, invoiceRuleFilters.Where(x => x.InvoiceRuleFilterTypeId == (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.RecipientInvoiceRuleFilter).ToList());
                    recipientRulesEngine.Run();

                    var invoiceDetailsRuleEngine = new InvoiceDetailsRulesEngine(flightInvoiceDetails, flight);
                    invoiceDetailsRuleEngine.Run();

                    var invoiceLineRulesEngine = new InvoiceLineRulesEngine(flightInvoiceDetails, flight,
                        _personService, invoiceRuleFilters.Where(x => x.InvoiceRuleFilterTypeId != (int)FLS.Data.WebApi.Invoicing.RuleFilters.InvoiceRuleFilterType.RecipientInvoiceRuleFilter).ToList());
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
