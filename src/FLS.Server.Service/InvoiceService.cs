using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Resources;
using FLS.Server.Interfaces.Invoicing;
using NLog;

namespace FLS.Server.Service
{
    public class InvoiceService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly IInvoiceService _invoiceService;

        public InvoiceService(DataAccessService dataAccessService,  
            IdentityService identityService, IInvoiceService invoiceService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            _invoiceService = invoiceService;
            Logger = LogManager.GetCurrentClassLogger();
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
                                                       (int) FlightAircraftTypeValue.GliderFlight
                                                       ||
                                                       flight.FlightAircraftType ==
                                                       (int) FlightAircraftTypeValue.MotorFlight)
                                                      &&
                                                      (flight.FlightStateId == (int) FLS.Data.WebApi.Flight.FlightState.Locked))
                                                      .ToList();

                    Logger.Debug(
                        string.Format("Queried Flights for Invoice between {1} and {2} and got {0} flights back.",
                                      flights.Count, fromDateTime, toDateTime));


                    var flightInvoiceDetails = _invoiceService.CreateFlightInvoiceDetails(flights);

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

                flight.FlightStateId = (int) FLS.Data.WebApi.Flight.FlightState.Invoiced;
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

                    towFlight.FlightStateId = (int) FLS.Data.WebApi.Flight.FlightState.Invoiced;
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
    }
}



