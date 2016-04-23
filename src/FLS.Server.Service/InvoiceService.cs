using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi.Invoicing;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Resources;
using FLS.Server.Service.Invoicing;
using NLog;
using TrackerEnabledDbContext.Common.Extensions;

namespace FLS.Server.Service
{
    public class InvoiceService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly InvoiceMappingFactory _invoiceMappingFactory;
        private readonly string SystemFlightTypeCode = "999";
        private InvoiceMapping InvoiceMapping { get; set; }
        private Guid LocationIdLSZK { get; set; }

        public InvoiceService(DataAccessService dataAccessService, InvoiceMappingFactory invoiceMappingFactory, 
            IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            _invoiceMappingFactory = invoiceMappingFactory;
            Logger = LogManager.GetCurrentClassLogger();
            InvoiceMapping = _invoiceMappingFactory.CreateInvoiceMapping();
            LocationIdLSZK = _invoiceMappingFactory.GetLocationId("LSZK");
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
            var flightInvoiceDetailList = new List<FlightInvoiceDetails>();

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

                    var numberOfExceptions = 0;
                    Exception lastException = null;

                    foreach (var flight in flights)
                    {
                        try
                        {
                            //Do not invoice any system flights, so continue
                            if (flight.FlightType.FlightCode == SystemFlightTypeCode)
                            {
                                continue;
                            }

                            var flightInvoiceDetails = CreateFlightInvoiceDetails(flight);

                            Logger.Debug(
                                string.Format("Created flight invoice details for flight and start creating invoice line items: {0}.",
                                     flight));

                            CreateFlightInvoiceLineItems(flight, flightInvoiceDetails);

                            Logger.Info($"Created invoice for {flightInvoiceDetails.InvoiceRecipientPersonDisplayName}: Flight-Date: {flightInvoiceDetails.FlightDate.ToShortDateString()} Aircraft: {flightInvoiceDetails.AircraftImmatriculation} Nr of Lines: {flightInvoiceDetails.FlightInvoiceLineItems.Count}");

                            flightInvoiceDetailList.Add(flightInvoiceDetails);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex, $"Error while trying to create invoice details for flight: {flight}. Message: {ex.Message}");
                            numberOfExceptions++;
                            lastException = ex;
                        }
                    }

                    //TODO: Handle exceptions correctly
                    if (numberOfExceptions > 0 && lastException != null)
                    {
                        Logger.Error(lastException, $"Total {numberOfExceptions} error(s) while trying to create invoice details for flights. Last exception message: {lastException.Message}");
                    }

                    Logger.Debug($"Have {flightInvoiceDetailList.Count} flight invoice object(s) in result list for sending back to client.");

                    return flightInvoiceDetailList.ToList();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while trying to create invoice for flights. Message: {ex.Message}");
                throw;
            }

            //return flightInvoiceDetailList.ToList();
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

        #region Private Methods
        private void CreateFlightInvoiceLineItems(Flight flight, FlightInvoiceDetails flightInvoiceDetails)
        {
            CreateFlightInvoiceLineItemsForFlight(flight, flightInvoiceDetails);

            if (flight.StartTypeId.HasValue && flight.StartTypeId == (int)AircraftStartType.TowingByAircraft)
            {
                CreateFlightInvoiceLineItemsForTowFlight(flight, flightInvoiceDetails);
                CreateFlightInvoiceLineItemsForFuelFee(flight.FlightId, flight.TowFlight, flightInvoiceDetails);
            }
            else if (flight.StartTypeId.HasValue && flight.StartTypeId == (int)AircraftStartType.MotorFlightStart)
            {
                CreateFlightInvoiceLineItemsForFuelFee(flight.FlightId, flight, flightInvoiceDetails);
            }

            //TODO:
            //e.g. Flight from Lukas Robers 16.7.2014 (Montricher, Yverdon)
            //e.g. Flight from Viktor Leuenberger 26.6.2014 (Speck, Schänis and back)
            //CreateFlightInvoiceLineItemsForStart(flight, flightInvoiceDetails);
            CreateFlightInvoiceLineItemsForLandings(flight, flightInvoiceDetails);

            //TODO: Implement it more generic in CreateFlightInvoiceLineItemsForVFSFee method
            if (flight.StartLocationId.HasValue && flight.StartLocationId.Value == LocationIdLSZK)
            {
                CreateFlightInvoiceLineItemsForVFSFee(flight, flightInvoiceDetails);
            }
        }

        /// <summary>
        /// Creates the invoice line items for fuel fee based on Additional fuel fee rules.
        /// </summary>
        /// <param name="invoicedFlightId">The invoiced flight identifier.</param>
        /// <param name="flight">The flight (mainly the TowFlight).</param>
        /// <param name="flightInvoiceDetails">The flight invoice details.</param>
        private void CreateFlightInvoiceLineItemsForFuelFee(Guid invoicedFlightId, Flight flight, FlightInvoiceDetails flightInvoiceDetails)
        {
            try
            {
                if (flight == null)
                {
                    Logger.Error(string.Format("TowFlight of flight Id: {0} is NULL. Can not add additional fuel fee.", invoicedFlightId));
                    return;
                }

                var additionalFuelFeeRule = FindMatchingAdditionalFuelFeeRule(flight);

                if (additionalFuelFeeRule != null)
                {
                    //Treibstoffzuschlag 
                    var invoiceLineItem = new FlightInvoiceLineItem
                        {
                            FlightId = invoicedFlightId,
                            InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1,
                            ERPArticleNumber = additionalFuelFeeRule.ERPArticleNumber,
                            InvoiceLineText =
                                string.Format("{0} {1}", additionalFuelFeeRule.InvoiceLineText,
                                              flight.AircraftImmatriculation),
                            Quantity = Convert.ToDecimal(flight.Duration.TotalMinutes),
                            UnitType = GetCostCenterUnitTypeString(CostCenterUnitType.PerFlightMinute)
                        };

                    flightInvoiceDetails.FlightInvoiceLineItems.Add(invoiceLineItem);
                }
                else
                {
                    if (InvoiceMapping.IsErrorWhenNoAdditionalFuelFeeRuleMatches)
                    {
                        var message =
                            string.Format(
                                "Towing aircraft {0} of Flight ID: {1} has no additional fuel fee, but must have.",
                                flight.AircraftImmatriculation, flight.FlightId);
                        Logger.Error(message);
                        throw new Exception(message);
                    }
                    else
                    {
                        Logger.Debug(string.Format("Towing aircraft {0} of Flight ID: {1} has no additional fuel fee.", flight.AircraftImmatriculation, flight.FlightId));                    
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Format("Error '{1}' while trying to get fuel fee mapping rule for flight: {0}.",
                                           flight, ex.Message));
                throw;
            }
        }

        private AdditionalFuelFeeRule FindMatchingAdditionalFuelFeeRule(Flight flight)
        {
            flight.ArgumentNotNull("flight");

            if (flight.StartLocationId.HasValue == false)
            {
                Logger.Error(string.Format("Flight {0} has no start location. Can't find matching fuel fee rule.", flight));
                return null;
            }

            var rules =
                InvoiceMapping.AdditionalFuelFeeRules.Where(
                    a => ((a.UseRuleForAllAircraftsExceptListed == false && a.AircraftIds.Contains(flight.AircraftId)) 
                        || (a.UseRuleForAllAircraftsExceptListed && a.AircraftIds.Contains(flight.AircraftId) == false))
                        && ((a.UseRuleForAllStartLocationsExceptListed == false && a.MatchedStartLocations.Contains(flight.StartLocationId.Value))
                        || (a.UseRuleForAllStartLocationsExceptListed && a.MatchedStartLocations.Contains(flight.StartLocationId.Value) == false)))
                              .OrderBy(i => i.SortIndicator);

            if (rules.Any())
            {
                return rules.First();
            }

            return null;
        }

        /// <summary>
        /// Creates the invoice line items for landings based on landing tax rules.
        /// </summary>
        /// <param name="flight">The flight.</param>
        /// <param name="flightInvoiceDetails">The flight invoice details.</param>
        private void CreateFlightInvoiceLineItemsForLandings(Flight flight, FlightInvoiceDetails flightInvoiceDetails)
        {
            try
            {
                var landingTaxRule = FindMatchingLandingTaxRule(flight);

                if (landingTaxRule != null)
                {
                    var invoiceLineItem = new FlightInvoiceLineItem
                    {
                        FlightId = flight.FlightId,
                        InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1,
                        ERPArticleNumber = landingTaxRule.ERPArticleNumber,
                        InvoiceLineText =
                            string.Format("{0}", landingTaxRule.InvoiceLineText),
                        Quantity = 1,
                        UnitType = GetCostCenterUnitTypeString(CostCenterUnitType.PerLanding)
                    };

                    flightInvoiceDetails.FlightInvoiceLineItems.Add(invoiceLineItem);


                    if (landingTaxRule.IncludesTowingLandingTaxes == false)
                    {
                        landingTaxRule = FindMatchingLandingTaxRule(flight.TowFlight);

                        if (landingTaxRule != null)
                        {
                            invoiceLineItem = new FlightInvoiceLineItem
                                {
                                    FlightId = flight.FlightId,
                                    InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1,
                                    ERPArticleNumber = landingTaxRule.ERPArticleNumber,
                                    InvoiceLineText =
                                        string.Format("{0}", landingTaxRule.InvoiceLineText),
                                    Quantity = 1,
                                    UnitType = GetCostCenterUnitTypeString(CostCenterUnitType.PerLanding)
                                };

                            flightInvoiceDetails.FlightInvoiceLineItems.Add(invoiceLineItem);
                        }
                        else
                        {
                            if (InvoiceMapping.IsErrorWhenNoLandingTaxRuleMatches)
                            {
                                var message =
                                    string.Format(
                                        "No landing tax rule found for tow flight {0}, but rule match is required",
                                        flight.TowFlight);
                                Logger.Error(message);
                                throw new Exception(message);
                            }
                            else
                            {
                                Logger.Info(string.Format("No landing tax rule found for tow flight {0}", flight.TowFlight));
                            }
                        }
                    }
                }
                else
                {
                    if (InvoiceMapping.IsErrorWhenNoLandingTaxRuleMatches)
                    {
                        var message = $"No landing tax rule found for flight {flight}, but rule match is required";
                        Logger.Error(message);
                        throw new Exception(message);
                    }
                    else
                    {
                        Logger.Info($"No landing tax rule found for flight {flight}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error '{ex.Message}' while trying to get landing mapping rule for flight: {flight}.");
                throw;
            }
        }

        /// <summary>
        /// Creates the invoice line items for VSF-Fee.
        /// </summary>
        /// <param name="flight">The flight.</param>
        /// <param name="flightInvoiceDetails">The flight invoice details.</param>
        private void CreateFlightInvoiceLineItemsForVFSFee(Flight flight, FlightInvoiceDetails flightInvoiceDetails)
        {
            try
            {
                if (InvoiceMapping.VFSMappingRule != null && InvoiceMapping.VFSMappingRule.AddVFSFeePerLanding)
                {
                    //if ((InvoiceMapping.VFSMappingRule.UseRuleForAllLdgLocationsExceptListed == false 
                    //    && InvoiceMapping.VFSMappingRule.MatchedLdgLocations.Contains(flight.LdgLocationId.Value))
                    //    || (InvoiceMapping.VFSMappingRule.UseRuleForAllLdgLocationsExceptListed 
                    //    && InvoiceMapping.VFSMappingRule.MatchedLdgLocations.Contains(flight.LdgLocationId.Value) == false))
                    //{}
             
                    //VSF-Gebühr (pro Landung)
                    var vsfQuantity = 1;

                    if (flight.IsTowed.HasValue && flight.IsTowed.Value)
                    {
                        vsfQuantity = 2;
                    }

                    var invoiceLineItem = new FlightInvoiceLineItem
                    {
                        FlightId = flight.FlightId,
                        InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1,
                        ERPArticleNumber = InvoiceMapping.VFSMappingRule.ERPArticleNumber,
                        InvoiceLineText =
                            string.Format("{0}", InvoiceMapping.VFSMappingRule.InvoiceLineText),
                        Quantity = vsfQuantity,
                        UnitType = GetCostCenterUnitTypeString(CostCenterUnitType.PerLanding)
                    };

                    flightInvoiceDetails.FlightInvoiceLineItems.Add(invoiceLineItem);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error '{ex.Message}' while trying to get VFS fee mapping rule for flight: {flight}.");
                throw;
            }
        }
        
        /// <summary>
        /// Finds the matching landing tax rule based on Ldg location, start type, flight type and aircraft.
        /// </summary>
        /// <param name="flight">The flight.</param>
        /// <returns></returns>
        private LandingTaxRule FindMatchingLandingTaxRule(Flight flight)
        {
            flight.ArgumentNotNull("flight");

            if (flight.LdgLocationId.HasValue == false)
            {
                Logger.Error(string.Format("Flight {0} has no landing location. Can't find matching landing tax rule.", flight));
                return null;
            }

            if (flight.StartTypeId.HasValue == false)
            {
                Logger.Error(string.Format("Flight {0} has no start type. Can't find matching landing tax rule.", flight));
                return null;
            }

            var rules =
                InvoiceMapping.LandingTaxRules.Where(
                    a => ((a.UseRuleForAllAircraftsExceptListed == false && a.AircraftIds.Contains(flight.AircraftId)) 
                        || (a.UseRuleForAllAircraftsExceptListed && a.AircraftIds.Contains(flight.AircraftId) == false))
                        && ((a.UseRuleForAllFlightTypesExceptListed == false && a.MatchedFlightTypeCodes.Contains(flight.FlightType.FlightCode))
                        || (a.UseRuleForAllFlightTypesExceptListed && a.MatchedFlightTypeCodes.Contains(flight.FlightType.FlightCode) == false))
                        && ((a.UseRuleForAllLdgLocationsExceptListed == false && a.MatchedLdgLocations.Contains(flight.LdgLocationId.Value))
                        || (a.UseRuleForAllLdgLocationsExceptListed && a.MatchedLdgLocations.Contains(flight.LdgLocationId.Value) == false))
                        && ((a.IsRuleForSelfstartedGliderFlights && flight.StartTypeId.Value == (int)AircraftStartType.SelfStart)
                        || (a.IsRuleForSelfstartedGliderFlights == false && flight.StartTypeId.Value != (int)AircraftStartType.SelfStart)))
                              .OrderBy(i => i.SortIndicator);

            if (rules.Any())
            {
                return rules.First();
            }

            return null;
        }

        /// <summary>
        /// Creates the invoice line items for the flight (mainly glider flight) based on the Aircraft mapping rules. Adds flight instructor fee if instructor flight (lookup in invoice mapping <code>FlightCodesForInstructorFee</code>).
        /// </summary>
        /// <param name="flight">The flight.</param>
        /// <param name="flightInvoiceDetails">The flight invoice details.</param>
        private void CreateFlightInvoiceLineItemsForFlight(Flight flight, FlightInvoiceDetails flightInvoiceDetails)
        {
            FlightInvoiceLineItem invoiceLineItem;

            try
            {
                var activeAircraftMappingRule = FindMatchingAircraftMappingRule(flight, flight.Duration.TotalMinutes, flight.FlightType.FlightCode);

                if (activeAircraftMappingRule != null)
                {
                    invoiceLineItem = new FlightInvoiceLineItem
                        {
                            FlightId = flight.FlightId,
                            InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1,
                            ERPArticleNumber = activeAircraftMappingRule.ERPArticleNumber,
                            InvoiceLineText = $"{flight.AircraftImmatriculation} {activeAircraftMappingRule.InvoiceLineText} {flight.FlightType.FlightTypeName}",
                            Quantity =
                                Convert.ToDecimal(flight.Duration.TotalMinutes),
                            UnitType = GetCostCenterUnitTypeString(CostCenterUnitType.PerFlightMinute)
                        };

                    flightInvoiceDetails.FlightInvoiceLineItems.Add(invoiceLineItem);
                }
                else
                {
                    Logger.Warn($"No aircraft mapping rule found for flight {flight}.");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error '{ex.Message}' while trying to get aircraft mapping rule for flight: {flight}.");
                throw;
            }

            try
            {
                //Fluglehrer-Honorar
                if (InvoiceMapping.FlightCodesForInstructorFee.Contains(flight.FlightType.FlightCode))
                {
                    if (flight.Instructor != null && flight.Instructor.Person != null)
                    {
                        string erpArticleNumber = GetMatchingERPArticleNumberOfInstructor(flight.Instructor.Person, flight.FlightType.ClubId);
                    
                        if (string.IsNullOrEmpty(erpArticleNumber) == false)
                        {
                            invoiceLineItem = new FlightInvoiceLineItem
                            {
                                FlightId = flight.FlightId,
                                InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1,
                                ERPArticleNumber = erpArticleNumber,
                                InvoiceLineText =
                                    string.Format("Fluglehrer-Honorar {0}", flight.Instructor.Person.DisplayName),
                                Quantity =
                                    Convert.ToDecimal(flight.Duration.TotalMinutes),
                                UnitType = GetCostCenterUnitTypeString(CostCenterUnitType.PerFlightMinute)
                            };

                            if (flight.FlightCostBalanceTypeId.HasValue &&
                                flight.FlightCostBalanceTypeId.Value ==
                                (int)FLS.Data.WebApi.Flight.FlightCostBalanceType.NoInstructorFee)
                            {
                                //no instructor fee for this flight, so set quantity to 0
                                invoiceLineItem.Quantity = 0;
                            }

                            flightInvoiceDetails.FlightInvoiceLineItems.Add(invoiceLineItem);
                        }
                        else
                        {
                            Logger.Error(string.Format("ERPArticleNumber not found for instructor {0}.",
                                                      flight.Instructor.Person.DisplayName));
                        }
                    }
                    else
                    {
                        if (flight.Instructor == null)
                        {
                            Logger.Error(string.Format("No instructor found for flight {0}. Could not add instructor fee to invoice.",
                                                      flight));
                        }
                        else if (flight.Instructor.Person == null)
                        {
                            Logger.Error(string.Format("No Person reference loaded for flight {0}. Could not add instructor fee to invoice.",
                                                      flight));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error '{ex.Message}' while trying to get flight instructor fee rule for flight: {flight}.");
                throw;
            }
        }

        private string GetMatchingERPArticleNumberOfInstructor(Person person, Guid clubId)
        {
            try
            {
                person.ArgumentNotNull("person");

                using (var context = _dataAccessService.CreateDbContext())
                {
                    var personClub = context.PersonClubs.FirstOrDefault(c => c.ClubId == clubId && c.PersonId == person.PersonId);

                    if (personClub != null)
                    {
                        if (string.IsNullOrEmpty(personClub.MemberKey))
                        {
                            Logger.Error(string.Format("Instructor {0} has no MemberKey. Update personal data.",
                                                       person.DisplayName));
                            return string.Empty;
                        }

                        if (InvoiceMapping.InstructorToERPArticleMapping.ContainsKey(personClub.MemberKey))
                        {
                            return InvoiceMapping.InstructorToERPArticleMapping[personClub.MemberKey];
                        }

                        Logger.Error(
                            string.Format("Instructor {0} in InvoiceMapping.InstructorToERPArticleMapping not found.",
                                          person.DisplayName));

                        return personClub.MemberKey;
                    }

                    Logger.Error(string.Format("PersonClub for instructor {0} and ClubId {1} not found.",
                                               person.DisplayName, clubId));

                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error '{ex.Message}' while trying to get instructor ERP number for person: {person.DisplayName}.");
                throw;
            }
        }

        private AircraftMappingRule FindMatchingAircraftMappingRule(Flight flight, double flightTimeInMinutes, string relevantFlightTypeCode)
        {
            flight.ArgumentNotNull("flight");

            if (flight.StartLocationId.HasValue == false)
            {
                Logger.Error(string.Format("Flight {0} has no start location. Can't find matching aircraft rule.", flight));
                return null;
            }

            var rules =
                InvoiceMapping.AircraftERPArticleMapping.Where(
                    a => a.AircraftId == flight.AircraftId
                        && ((a.UseRuleForAllFlightTypesExceptListed == false && a.MatchedFlightTypeCodes.Contains(relevantFlightTypeCode))
                        || (a.UseRuleForAllFlightTypesExceptListed && a.MatchedFlightTypeCodes.Contains(relevantFlightTypeCode) == false))
                    && ((a.UseRuleForAllStartLocationsExceptListed == false && a.MatchedStartLocations.Contains(flight.StartLocationId.Value))
                        || (a.UseRuleForAllStartLocationsExceptListed && a.MatchedStartLocations.Contains(flight.StartLocationId.Value) == false))
                        && flightTimeInMinutes > a.MinFlightTimeMatchingValue
                    && flightTimeInMinutes <= a.MaxFlightTimeMatchingValue)
                              .OrderBy(i => i.SortIndicator);

            if (rules.Any())
            {
                return rules.First();
            }

            return null;
        }

        /// <summary>
        /// Creates the invoice line items for tow flight using the Aircraft mapping rules.
        /// e.g. "Schlepp 1. bis 10. Min." 8 Minuten
        /// </summary>
        /// <param name="gliderFlight">The glider flight.</param>
        /// <param name="flightInvoiceDetails">The flight invoice details.</param>
        /// <returns>if gliderFlight.TowFlight == null, otherwise adds invoice lines</returns>
        private void CreateFlightInvoiceLineItemsForTowFlight(Flight gliderFlight, FlightInvoiceDetails flightInvoiceDetails)
        {
            try
            {
                if (gliderFlight.TowFlight == null) return;
                double activeFlightTime = gliderFlight.TowFlight.Duration.TotalMinutes;
                bool hasUpperThresholdRules = false;
                var lineItems = new SortedList<int, FlightInvoiceLineItem>();

                var activeAircraftMappingRule = FindMatchingAircraftMappingRule(gliderFlight.TowFlight, activeFlightTime, gliderFlight.FlightType.FlightCode);

                while (activeAircraftMappingRule != null)
                {
                    flightInvoiceDetails.IncludesTowFlightId = gliderFlight.TowFlightId;
                    if (activeAircraftMappingRule.MinFlightTimeMatchingValue > 0 && activeAircraftMappingRule.UseRuleBelowFlightTimeMatchingValue)
                    {
                        hasUpperThresholdRules = true;
                    }

                    var currentFlightTime = activeFlightTime - activeAircraftMappingRule.MinFlightTimeMatchingValue;
                    string thresholdInvoiceText = string.Format("ab {0}. Min.",
                                                                activeAircraftMappingRule.MinFlightTimeMatchingValue);

                    if (hasUpperThresholdRules && activeAircraftMappingRule.MinFlightTimeMatchingValue == 0 && activeAircraftMappingRule.UseRuleBelowFlightTimeMatchingValue == false)
                    {
                        thresholdInvoiceText = string.Format("1. bis {0}. Min.", activeAircraftMappingRule.MaxFlightTimeMatchingValue);
                    }

                    var invoiceLineItem = new FlightInvoiceLineItem
                    {
                        FlightId = gliderFlight.FlightId,
                        InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1,
                        ERPArticleNumber = activeAircraftMappingRule.ERPArticleNumber,
                        InvoiceLineText =
                            string.Format("Schlepp {0} {1} {2}", activeAircraftMappingRule.InvoiceLineText, gliderFlight.TowFlight.AircraftImmatriculation,
                                thresholdInvoiceText),
                        Quantity =
                            Convert.ToDecimal(currentFlightTime),
                        UnitType = GetCostCenterUnitTypeString(CostCenterUnitType.PerFlightMinute)
                    };

                    lineItems.Add(activeAircraftMappingRule.SortIndicator, invoiceLineItem);

                    if (activeAircraftMappingRule.UseRuleBelowFlightTimeMatchingValue)
                    {
                        activeFlightTime = activeAircraftMappingRule.MinFlightTimeMatchingValue;
                        activeAircraftMappingRule = FindMatchingAircraftMappingRule(gliderFlight.TowFlight, activeFlightTime, gliderFlight.FlightType.FlightCode);
                    }
                    else
                    {
                        activeAircraftMappingRule = null;
                    }
                }

                foreach (var item in lineItems.OrderBy(x => x.Key))
                {
                    item.Value.InvoiceLinePosition = flightInvoiceDetails.FlightInvoiceLineItems.Count + 1;
                    flightInvoiceDetails.FlightInvoiceLineItems.Add(item.Value);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error '{ex.Message}' while trying to tow flight mapping rule for flight: {gliderFlight}.");
                throw;
            }
        }

        private string GetCostCenterUnitTypeString(CostCenterUnitType costCenterUnitType)
        {
            string unitTypeString;

            switch (costCenterUnitType)
            {
                case CostCenterUnitType.PerFlightMinute:
                    unitTypeString = "Minuten";
                    break;
                case CostCenterUnitType.PerStartOrFlight:
                    unitTypeString = "Start";
                    break;
                case CostCenterUnitType.PerLanding:
                    unitTypeString = "Landung";
                    break;
                default:
                    unitTypeString = string.Empty;
                    break;
            }

            return unitTypeString;
        }

        private FlightInvoiceDetails CreateFlightInvoiceDetails(Flight flight)
        {
            flight.ArgumentNotNull("flight");

            if (flight.FlightDate == null
                || string.IsNullOrEmpty(flight.AircraftImmatriculation)
                || string.IsNullOrEmpty(flight.PilotDisplayName))
            {
                throw new InvalidDataException("Flight is not valid for invoice");
            }

            var flightInvoiceDetails = new FlightInvoiceDetails
            {
                FlightId = flight.FlightId,
                FlightDate = flight.FlightDate.Value,
                AircraftImmatriculation = flight.AircraftImmatriculation,
                FlightInvoiceLineItems = new List<FlightInvoiceLineItem>(),
                FlightInvoiceInfo = flight.FlightType.FlightTypeName
            };

            try
            {
                //Check if we have an invoice recipient remapping (Passenger-, Schnupper-, Marketing-Flights)
                //which will be invoiced to club internal instead to the pilots
                if (InvoiceMapping.FlightTypeToInvoiceRecipientMapping.ContainsKey(flight.FlightType.FlightCode))
                {
                    var invoiceRecipientTarget =
                        InvoiceMapping.FlightTypeToInvoiceRecipientMapping[flight.FlightType.FlightCode];

                    if (invoiceRecipientTarget != null)
                    {
                        flightInvoiceDetails.InvoiceRecipientPersonDisplayName = invoiceRecipientTarget.DisplayName;
                        flightInvoiceDetails.InvoiceRecipientPersonClubMemberNumber = invoiceRecipientTarget.MemberNumber;
                        flightInvoiceDetails.InvoiceRecipientPersonClubMemberKey = invoiceRecipientTarget.MemberKey;

                        if (flight.FlightType.IsPassengerFlight)
                        {
                            flightInvoiceDetails.FlightInvoiceInfo += " " + flight.PilotDisplayName + " mit PAX: " + flight.PassengerDisplayName;
                        }
                        else
                        {
                            flightInvoiceDetails.FlightInvoiceInfo += " " + flight.PilotDisplayName + " mit FL: " + flight.InstructorDisplayName;                        
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("Invoice recipient target is null. Can not create invoice for flight with Id: {0}",
                                                      flight.FlightId));
                    }
                }
                else
                {
                    //Invoice the flight to the pilot or to a person which is the invoice recipient

                    //Check if we have to send the invoice to a 3rd party person
                    if (flight.FlightCostBalanceTypeId == (int) FLS.Data.WebApi.Flight.FlightCostBalanceType.CostsPaidByPerson)
                    {
                        var invoiceRecipient =
                            flight.FlightCrews.FirstOrDefault(
                                fc => fc.FlightCrewTypeId == (int) FLS.Data.WebApi.Flight.FlightCrewType.FlightCostInvoiceRecipient);

                        if (invoiceRecipient != null && invoiceRecipient.Person != null)
                        {
                            SetInvoiceRecipient(flightInvoiceDetails, flight, invoiceRecipient.Person.DisplayName, invoiceRecipient.PersonId);
                        }
                    }
                    else
                    {
                        //invoice the pilot
                        SetInvoiceRecipient(flightInvoiceDetails, flight, flight.PilotDisplayName, flight.Pilot.PersonId);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while trying to find invoice recipient target for flight {flight}");
                throw;
            }

            try
            {
                //Set additional flight invoice info text

                if (flight.FlightType.IsPassengerFlight && flightInvoiceDetails.FlightInvoiceInfo.Contains(flight.PassengerDisplayName) == false)
                {
                    flightInvoiceDetails.FlightInvoiceInfo += " mit PAX: " + flight.PassengerDisplayName;
                }
                else if (flight.FlightType.IsCheckFlight && flightInvoiceDetails.FlightInvoiceInfo.Contains(flight.InstructorDisplayName) == false)
                {
                    flightInvoiceDetails.FlightInvoiceInfo += " mit FL: " + flight.InstructorDisplayName;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while trying to set additional invoice info text for flight {flight}");
                throw;
            }

            try
            {
                //Set flag "Additional Info" to 1 if we have a teaching flight or 0
                //is not used somewhere
                if (InvoiceMapping.FlightCodesForInstructorFee.Contains(flight.FlightType.FlightCode))
                {
                    //Schulungsflug
                    flightInvoiceDetails.AdditionalInfo = "1";
                }
                else
                {
                    //kein Schulungsflug
                    flightInvoiceDetails.AdditionalInfo = "0";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while trying to set additional info for flight {flight}");
                throw;
            }

            return flightInvoiceDetails;
        }

        private void SetInvoiceRecipient(FlightInvoiceDetails flightInvoiceDetails, Flight flight, string recipientDisplayName,
                                         Guid personId)
        {
            try
            {
                flightInvoiceDetails.InvoiceRecipientPersonDisplayName = recipientDisplayName;

                using (var context = _dataAccessService.CreateDbContext())
                {
                    var personClub =
                        context.PersonClubs.FirstOrDefault(
                            pc => pc.ClubId == flight.FlightType.ClubId && pc.PersonId == personId);

                    if (personClub != null)
                    {
                        flightInvoiceDetails.InvoiceRecipientPersonClubMemberNumber = personClub.MemberNumber;
                        flightInvoiceDetails.InvoiceRecipientPersonClubMemberKey = personClub.MemberKey;
                    }
                    else
                    {
                        throw new Exception($"PersonClub was not found. Can not create invoice for flight with Id: {flight.FlightId}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while trying to set invoice recipient {recipientDisplayName} for flight {flight}");
                throw;
            }
        }

        #endregion Private Methods
    }
}



