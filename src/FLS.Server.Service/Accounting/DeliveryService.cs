using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.RuleFilters;
using FLS.Data.WebApi.Accounting.Testing;
using FLS.Data.WebApi.Exceptions;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Exceptions;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using FLS.Server.Interfaces;
using FLS.Server.Service.Accounting.RuleEngines;
using Newtonsoft.Json;
using FlightCrewType = FLS.Data.WebApi.Flight.FlightCrewType;
using FlightProcessState = FLS.Data.WebApi.Flight.FlightProcessState;


namespace FLS.Server.Service.Accounting
{
    public class DeliveryService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly AccountingRuleFilterFactory _accountingRuleFilterFactory;
        private readonly IPersonService _personService;
        private readonly FlightService _flightService;
        private readonly AccountingRuleService _accountingRuleService;

        public DeliveryService(DataAccessService dataAccessService, IdentityService identityService,
            AccountingRuleFilterFactory accountingRuleFilterFactory, IPersonService personService,
            FlightService flightService, AccountingRuleService accountingRuleService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            _accountingRuleFilterFactory = accountingRuleFilterFactory;
            _personService = personService;
            _flightService = flightService;
            _accountingRuleService = accountingRuleService;
        }

        public List<DeliveryDetails> CreateDeliveriesFromFlights()
        {
            return CreateDeliveriesFromFlights(CurrentAuthenticatedFLSUserClubId);
        }

        public List<DeliveryDetails> CreateDeliveriesFromFlights(Guid clubId)
        {
            if (clubId.IsValid() == false)
            {
                Logger.Error("No valid ClubId for getting the accountings!");
                throw new ArgumentException("clubId");
            }

            try
            {
                using (var context = _dataAccessService.CreateDbContext())
                {
                    var lockingDate = DateTime.Today.AddDays(-3);

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
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person + "." +
                                     Constants.PersonClubs)
                            .Include(Constants.TowFlight + "." + Constants.StartLocation)
                            .Include(Constants.TowFlight + "." + Constants.LdgLocation)
                            .OrderBy(c => c.StartDateTime)
                            .Where(flight => flight.FlightType.ClubId == clubId
                                             &&
                                             (flight.FlightAircraftType ==
                                              (int) FlightAircraftTypeValue.GliderFlight
                                              ||
                                              flight.FlightAircraftType ==
                                              (int) FlightAircraftTypeValue.MotorFlight)
                                             &&
                                             (flight.ProcessStateId ==
                                              (int) FLS.Data.WebApi.Flight.FlightProcessState.Locked)
                                             && DbFunctions.TruncateTime(flight.CreatedOn) <= lockingDate.Date)
                            .ToList();

                    Logger.Debug($"Queried Flights for accounting and got {flights.Count} flights back.");


                    var accountingRuleFilters =
                        _accountingRuleService.GetRuleBasedAccountingRuleFilterDetailsListByClubId(clubId);

                    if (accountingRuleFilters == null || accountingRuleFilters.Count == 0)
                    {
                        var accountingRuleFilterDetailsList =
                            _accountingRuleFilterFactory.CreateAccountingRuleFilterDetails();

                        foreach (var accountingRuleFilterDetails in accountingRuleFilterDetailsList)
                        {
                            _accountingRuleService.InsertAccountingRuleFilterDetails(accountingRuleFilterDetails, clubId);
                        }

                        accountingRuleFilters =
                            _accountingRuleService.GetRuleBasedAccountingRuleFilterDetailsListByClubId(clubId);
                    }

                    accountingRuleFilters.NotNull("accountingRuleFilters");

                    long batchId = 1;

                    var maxBatchId = context.Deliveries.MaxOrDefault(x => x.BatchId);
                    if (maxBatchId.HasValue)
                    {
                        batchId = maxBatchId.Value + 1;
                    }

                    var numberOfExceptions = 0;
                    Exception lastException = null;

                    foreach (var flight in flights)
                    {
                        try
                        {
                            var deliveryDetails = CreateDeliveryDetailsForFlight(flight, clubId, accountingRuleFilters);

                            if (deliveryDetails.DoNotInvoiceFlight)
                            {
                                Logger.Info($"FlightId/Flight: {flight.FlightId} / {flight} is excluded by DoNotInvoiceFlightRule! Delivery-Process stopped and flight process state is set to ExcludedFromDeliveryProcess!");

                                flight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.ExcludedFromDeliveryProcess;
                                flight.DeliveryCreatedOn = DateTime.UtcNow;
                                flight.DoNotUpdateMetaData = true;
                                context.SaveChanges();
                                continue;
                            }

                            if (deliveryDetails.DeliveryItems.Any() == false)
                            {
                                Logger.Warn($"Delivery without items created for FlightId/Flight: {flight.FlightId} / {flight}! Delivery-Process stopped and flight process state is set to DeliveryPreparationError!");

                                flight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.DeliveryPreparationError;
                                flight.DoNotUpdateMetaData = true;
                                context.SaveChanges();
                                continue;
                            }

                            if (deliveryDetails.RecipientDetails == null)
                            {
                                Logger.Warn($"Delivery without recipient details created for FlightId/Flight: {flight.FlightId} / {flight}! Delivery-Process stopped and flight process state is set to DeliveryPreparationError!");

                                flight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.DeliveryPreparationError;
                                flight.DoNotUpdateMetaData = true;
                                context.SaveChanges();
                                continue;
                            }

                            var delivery = deliveryDetails.ToDelivery(clubId);
                            delivery.BatchId = batchId;

                            context.Deliveries.Add(delivery);

                            flight.ProcessStateId = (int) FLS.Data.WebApi.Flight.FlightProcessState.DeliveryPrepared;
                            flight.DeliveryCreatedOn = DateTime.UtcNow;
                            flight.DoNotUpdateMetaData = true;

                            if (flight.TowFlight != null)
                            {
                                flight.TowFlight.ProcessStateId =
                                    (int) FLS.Data.WebApi.Flight.FlightProcessState.DeliveryPrepared;
                                flight.TowFlight.DeliveryCreatedOn = DateTime.UtcNow;
                                flight.TowFlight.DoNotUpdateMetaData = true;
                            }

                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error(ex,
                                $"Error while trying to create accounting details for flight: {flight}. Message: {ex.Message}");
                            numberOfExceptions++;
                            lastException = ex;
                        }
                    }

                    //TODO: Handle exceptions correctly
                    if (numberOfExceptions > 0 && lastException != null)
                    {
                        Logger.Error(lastException,
                            $"Total {numberOfExceptions} error(s) while trying to create accounting details for flights. Last exception message: {lastException.Message}");
                    }

                    var deliveries = context.Deliveries
                            .Include("Flight")
                            .Include("Flight." + Constants.Aircraft)
                            .Include("Flight." + Constants.FlightType)
                        .Where(x => x.BatchId == batchId).ToList();

                    var deliveryDetailsList = deliveries.Select(x => x.ToDeliveryDetails()).ToList();
                    Logger.Debug(
                        $"Have {deliveryDetailsList.Count} deliveries in result list for sending back to client.");

                    return deliveryDetailsList;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while trying to create accounting for flights. Message: {ex.Message}");
                throw new InternalServerException($"Error while trying to create accounting for flights. Message: {ex.Message}");
            }
        }

        private DeliveryDetails CreateDeliveryDetailsForFlight(Flight flight, Guid clubId, List<RuleBasedAccountingRuleFilterDetails> accountingRuleFilters)
        {
            Logger.Debug(
                $"Start creating accounting for flight: {flight} using {accountingRuleFilters.Count} recipient rule filters and {accountingRuleFilters.Count} accounting line rule filters.");


            var ruleBasedDelivery = new RuleBasedDeliveryDetails
            {
                DeliveryId = Guid.NewGuid(),
                FlightInformation = new FlightInformation()
                {
                    FlightId = flight.FlightId,
                    AircraftImmatriculation = flight.AircraftImmatriculation,
                    FlightDate = flight.FlightDate.GetValueOrDefault(),
                    PilotName = flight.PilotDisplayName
                },
                DeliveryInformation = flight.FlightType.FlightTypeName,
                ClubId = clubId
            };

            if (flight.Passenger != null) ruleBasedDelivery.FlightInformation.SecondCrewName = flight.PassengerDisplayName;
            if (flight.CoPilot != null) ruleBasedDelivery.FlightInformation.SecondCrewName = flight.CoPilotDisplayName;
            if (flight.Instructor != null) ruleBasedDelivery.FlightInformation.SecondCrewName = flight.InstructorDisplayName;

            var ignoreFlightRulesEngine = new IgnoreFlightRulesEngine(ruleBasedDelivery, flight,
                accountingRuleFilters.Where(
                    x =>
                        x.AccountingRuleFilterTypeId ==
                        (int)FLS.Data.WebApi.Accounting.RuleFilters.AccountingRuleFilterType.DoNotInvoiceFlightRuleFilter).ToList());
            ignoreFlightRulesEngine.Run();

            if (ruleBasedDelivery.DoNotInvoiceFlight)
            {
                Logger.Info(
                    $"Flight must not be invoiced: Flight-Date: {ruleBasedDelivery.FlightInformation.FlightDate.ToShortDateString()} Aircraft: {ruleBasedDelivery.FlightInformation.AircraftImmatriculation}");

                return ruleBasedDelivery;
            }

            var recipientRulesEngine = new RecipientRulesEngine(ruleBasedDelivery, flight,
                _personService,
                accountingRuleFilters.Where(
                    x =>
                        x.AccountingRuleFilterTypeId ==
                        (int)FLS.Data.WebApi.Accounting.RuleFilters.AccountingRuleFilterType.RecipientAccountingRuleFilter).ToList());
            recipientRulesEngine.Run();

            var accountingDetailsRuleEngine = new DeliveryDetailsRulesEngine(ruleBasedDelivery, flight);
            accountingDetailsRuleEngine.Run();

            var accountingLineRulesEngine = new DeliveryItemRulesEngine(ruleBasedDelivery, flight,
                _personService,
                accountingRuleFilters.Where(
                    x =>
                        x.AccountingRuleFilterTypeId !=
                        (int)FLS.Data.WebApi.Accounting.RuleFilters.AccountingRuleFilterType.RecipientAccountingRuleFilter).ToList());
            accountingLineRulesEngine.Run();

            Logger.Info(
                $"Created accounting for {ruleBasedDelivery.RecipientDetails.RecipientName}: Flight-Date: {ruleBasedDelivery.FlightInformation.FlightDate.ToShortDateString()} Aircraft: {ruleBasedDelivery.FlightInformation.AircraftImmatriculation} Nr of Lines: {ruleBasedDelivery.DeliveryItems.Count}");

            return ruleBasedDelivery;
        }

        public bool SetDeliveryAsDelivered(DeliveryBooking flightDeliveryBooking)
        {
            flightDeliveryBooking.ArgumentNotNull("flightDeliveryBooking");

            using (var context = _dataAccessService.CreateDbContext())
            {
                var delivery = context.Deliveries.FirstOrDefault(q => q.DeliveryId == flightDeliveryBooking.DeliveryId);

                if (delivery == null) return false;

                delivery.DeliveryNumber = flightDeliveryBooking.DeliveryNumber;
                delivery.DeliveredOn = flightDeliveryBooking.DeliveryDateTime;
                
                delivery.IsFurtherProcessed = true;

                if (delivery.FlightId.HasValue)
                {
                    var flight = context.Flights.FirstOrDefault(x => x.FlightId == delivery.FlightId.Value);

                    if (flight != null)
                    {
                        flight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.DeliveryBooked;
                        flight.DoNotUpdateMetaData = true;

                        if (flight.TowFlightId.HasValue)
                        {
                            var towFlight = context.Flights.FirstOrDefault(x => x.FlightId == flight.TowFlightId.Value);

                            if (towFlight != null)
                            {
                                towFlight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.DeliveryBooked;
                                towFlight.DoNotUpdateMetaData = true;
                            }
                        }
                    }
                }
                
                context.SaveChanges();

                return true;
            }
        }

        #region DeliveryCreationTesting

        public DeliveryCreationResult CreateDeliveryDetailsForTest(Guid flightId)
        {
            DeliveryCreationResult deliveryCreationResult = new DeliveryCreationResult()
            {
                FlightId = flightId
            };

            try
            {
                using (var context = _dataAccessService.CreateDbContext())
                {
                    var flight =
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
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person + "." +
                                     Constants.PersonClubs)
                            .Include(Constants.TowFlight + "." + Constants.StartLocation)
                            .Include(Constants.TowFlight + "." + Constants.LdgLocation)
                            .OrderBy(c => c.StartDateTime)
                            .FirstOrDefault(f => f.FlightId == flightId);

                    var accountingRuleFilters =
                        _accountingRuleService.GetRuleBasedAccountingRuleFilterDetailsListByClubId(
                            CurrentAuthenticatedFLSUserClubId);

                    accountingRuleFilters.NotNull("accountingRuleFilters");


                    var deliveryDetails = CreateDeliveryDetailsForFlight(flight, CurrentAuthenticatedFLSUserClubId,
                        accountingRuleFilters);

                    deliveryCreationResult.CreatedDeliveryDetails = deliveryDetails;

                    var matchedAccountingRuleFilters = accountingRuleFilters.Where(x => x.HasMatched).ToList();
                    var accountingFilterTypes = context.AccountingRuleFilterTypes.ToList();

                    deliveryCreationResult.MatchedAccountingRuleFilterIds =
                        matchedAccountingRuleFilters.Select(r => r.AccountingRuleFilterId).ToList();

                    deliveryCreationResult.MatchedAccountingRuleFilters =
                        matchedAccountingRuleFilters.Select(x => x.ToAccountingRuleFilterOverview(accountingFilterTypes))
                            .ToList();

                    return deliveryCreationResult;
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while trying to create delivery for flight. Message: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryCreationTestId"></param>
        /// <returns>List<DeliveryCreationTestDetails></returns>
        public List<DeliveryCreationTestDetails> RunDeliveryCreationTests()
        {
            try
            {
                using (var context = _dataAccessService.CreateDbContext())
                {
                    var deliveryCreationTests =
                        context.DeliveryCreationTests.Where(x => x.ClubId == CurrentAuthenticatedFLSUserClubId);

                    var resultList = new List<DeliveryCreationTestDetails>();

                    foreach (var deliveryCreationTest in deliveryCreationTests)
                    {
                        var result = RunDeliveryCreationTest(deliveryCreationTest.DeliveryCreationTestId);
                        resultList.Add(result);
                    }

                    return resultList;
                }
            }
            catch (Exception ex)
            {
                var error = $"Error while trying to create accounting for flights. Message: {ex.Message}";
                Logger.Error(ex, error);
            }

            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryCreationTestId"></param>
        /// <returns>DeliveryCreationTestDetails</returns>
        public DeliveryCreationTestDetails RunDeliveryCreationTest(Guid deliveryCreationTestId)
        {
            try
            {
                using (var context = _dataAccessService.CreateDbContext())
                {
                    var deliveryCreationTest =
                        context.DeliveryCreationTests.FirstOrDefault(
                            x => x.DeliveryCreationTestId == deliveryCreationTestId);

                    deliveryCreationTest.EntityNotNull("DeliveryCreationTest");

                    var flight =
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
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person + "." +
                                     Constants.PersonClubs)
                            .Include(Constants.TowFlight + "." + Constants.StartLocation)
                            .Include(Constants.TowFlight + "." + Constants.LdgLocation)
                            .OrderBy(c => c.StartDateTime)
                            .FirstOrDefault(f => f.FlightId == deliveryCreationTest.FlightId);

                    var accountingRuleFilters =
                        _accountingRuleService.GetRuleBasedAccountingRuleFilterDetailsListByClubId(
                            CurrentAuthenticatedFLSUserClubId);

                    accountingRuleFilters.NotNull("accountingRuleFilters");


                    var createdDeliveryDetails = CreateDeliveryDetailsForFlight(flight, CurrentAuthenticatedFLSUserClubId,
                        accountingRuleFilters);

                    var matchedAccountingRuleFilters = accountingRuleFilters.Where(x => x.HasMatched).ToList();

                    // ReSharper disable once PossibleNullReferenceException
                    deliveryCreationTest.LastTestCreatedDeliveryDetails = JsonConvert.SerializeObject(createdDeliveryDetails);
                    deliveryCreationTest.LastTestMatchedAccountingRuleFilterIds =
                        JsonConvert.SerializeObject(
                            matchedAccountingRuleFilters.Select(x => x.AccountingRuleFilterId).ToList());
                    deliveryCreationTest.LastTestRunOn = DateTime.UtcNow;
                    deliveryCreationTest.LastTestSuccessful = true;
                    deliveryCreationTest.LastTestResultMessage = string.Empty;

                    try
                    {
                        // compare delivery with expected delivery and consider flags

                        if (deliveryCreationTest.MustNotCreateDeliveryForFlight)
                        {
                            if (createdDeliveryDetails == null || createdDeliveryDetails.DeliveryItems == null ||
                                createdDeliveryDetails.DeliveryItems.Any() == false)
                            {
                                deliveryCreationTest.LastTestSuccessful = true;
                            }
                            else
                            {
                                deliveryCreationTest.LastTestSuccessful = false;
                            }

                            deliveryCreationTest.LastTestResultMessage = "Flug erzeugt kein Lieferschein";
                        }
                        else
                        {
                            var expectedDeliveryDetails =
                                JsonConvert.DeserializeObject<DeliveryDetails>(
                                    deliveryCreationTest.ExpectedDeliveryDetails);

                            if (deliveryCreationTest.IgnoreDeliveryInformation == false)
                            {
                                if (createdDeliveryDetails.DeliveryInformation != expectedDeliveryDetails.DeliveryInformation)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage =
                                        $"DeliveryInformation doesn't match";
                                }
                            }

                            if (deliveryCreationTest.IgnoreAdditionalInformation == false)
                            {
                                if (createdDeliveryDetails.AdditionalInformation !=
                                    expectedDeliveryDetails.AdditionalInformation)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}AdditionalInformation doesn't match";
                                }
                            }

                            if (deliveryCreationTest.IgnoreRecipientPersonId == false)
                            {
                                if (createdDeliveryDetails.RecipientDetails.PersonId !=
                                    expectedDeliveryDetails.RecipientDetails.PersonId)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}PersonId doesn't match";
                                }
                            }

                            if (deliveryCreationTest.IgnoreRecipientClubMemberNumber == false)
                            {
                                if (createdDeliveryDetails.RecipientDetails.PersonClubMemberNumber !=
                                    expectedDeliveryDetails.RecipientDetails.PersonClubMemberNumber)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}PersonClubMemberNumber doesn't match";
                                }
                            }

                            if (deliveryCreationTest.IgnoreRecipientName == false)
                            {
                                if (createdDeliveryDetails.RecipientDetails.Lastname !=
                                    expectedDeliveryDetails.RecipientDetails.Lastname)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}Lastname doesn't match";
                                }

                                if (createdDeliveryDetails.RecipientDetails.Firstname !=
                                    expectedDeliveryDetails.RecipientDetails.Firstname)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}Firstname doesn't match";
                                }
                            }

                            if (deliveryCreationTest.IgnoreRecipientAddress == false)
                            {
                                if (createdDeliveryDetails.RecipientDetails.AddressLine1 !=
                                    expectedDeliveryDetails.RecipientDetails.AddressLine1)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}AddressLine1 doesn't match";
                                }

                                if (createdDeliveryDetails.RecipientDetails.AddressLine2 !=
                                    expectedDeliveryDetails.RecipientDetails.AddressLine2)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}AddressLine2 doesn't match";
                                }

                                if (createdDeliveryDetails.RecipientDetails.ZipCode != expectedDeliveryDetails.RecipientDetails.ZipCode)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}ZipCode doesn't match";
                                }

                                if (createdDeliveryDetails.RecipientDetails.City != expectedDeliveryDetails.RecipientDetails.City)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}City doesn't match";
                                }

                                if (createdDeliveryDetails.RecipientDetails.CountryName !=
                                    expectedDeliveryDetails.RecipientDetails.CountryName)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}CountryName doesn't match";
                                }
                            }

                            if (createdDeliveryDetails.DeliveryItems.Count != expectedDeliveryDetails.DeliveryItems.Count)
                            {
                                deliveryCreationTest.LastTestSuccessful = false;
                                deliveryCreationTest.LastTestResultMessage +=
                                    $"{Environment.NewLine}Numbers of delivery items doesn't match";
                            }

                            foreach (var expectedDeliveryItem in expectedDeliveryDetails.DeliveryItems)
                            {
                                DeliveryItemDetails createdItem = null;

                                if (deliveryCreationTest.IgnoreItemPositioning == false)
                                {
                                    createdItem =
                                        createdDeliveryDetails.DeliveryItems.FirstOrDefault(
                                            x => x.Position == expectedDeliveryItem.Position);
                                }
                                else
                                {
                                    createdItem =
                                        createdDeliveryDetails.DeliveryItems.FirstOrDefault(
                                            x => x.ArticleNumber == expectedDeliveryItem.ArticleNumber);
                                }

                                if (createdItem == null)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}Item at position {expectedDeliveryItem.Position} or with ArticleNumber {expectedDeliveryItem.ArticleNumber} not found!";
                                    continue;
                                }

                                if (createdItem.ArticleNumber != expectedDeliveryItem.ArticleNumber)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}Expected ArticleNumber {expectedDeliveryItem.ArticleNumber} doesn't match with {createdItem.ArticleNumber} for expected item at position {expectedDeliveryItem.Position}";
                                }

                                if (createdItem.Quantity != expectedDeliveryItem.Quantity)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}Expected Quantity {expectedDeliveryItem.Quantity} doesn't match with {createdItem.Quantity} for expected item at position {expectedDeliveryItem.Position}";
                                }

                                if (createdItem.DiscountInPercent != expectedDeliveryItem.DiscountInPercent)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}Expected DiscountInPercent {expectedDeliveryItem.DiscountInPercent} doesn't match with {createdItem.DiscountInPercent} for expected item at position {expectedDeliveryItem.Position}";
                                }

                                if (createdItem.UnitType != expectedDeliveryItem.UnitType)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}Expected UnitType {expectedDeliveryItem.UnitType} doesn't match with {createdItem.UnitType} for expected item at position {expectedDeliveryItem.Position}";
                                }

                                if (deliveryCreationTest.IgnoreItemText == false)
                                {
                                    if (createdItem.ItemText != expectedDeliveryItem.ItemText)
                                    {
                                        deliveryCreationTest.LastTestSuccessful = false;
                                        deliveryCreationTest.LastTestResultMessage +=
                                            $"{Environment.NewLine}Expected ItemText {expectedDeliveryItem.ItemText} doesn't match with {createdItem.ItemText} for expected item at position {expectedDeliveryItem.Position}";
                                    }
                                }

                                if (deliveryCreationTest.IgnoreItemAdditionalInformation == false)
                                {
                                    if (createdItem.AdditionalInformation != expectedDeliveryItem.AdditionalInformation)
                                    {
                                        deliveryCreationTest.LastTestSuccessful = false;
                                        deliveryCreationTest.LastTestResultMessage +=
                                            $"{Environment.NewLine}Expected AdditionalInformation {expectedDeliveryItem.AdditionalInformation} doesn't match with {createdItem.AdditionalInformation} for expected item at position {expectedDeliveryItem.Position}";
                                    }
                                }
                            }

                            if (createdDeliveryDetails.DeliveryItems.Count > expectedDeliveryDetails.DeliveryItems.Count)
                            {
                                foreach (var createdItem in createdDeliveryDetails.DeliveryItems)
                                {
                                    if (expectedDeliveryDetails.DeliveryItems.Any(
                                            x => x.ArticleNumber == createdItem.ArticleNumber) == false)
                                    {
                                        deliveryCreationTest.LastTestSuccessful = false;
                                        deliveryCreationTest.LastTestResultMessage +=
                                            $"{Environment.NewLine}Item with ArticleNumber {createdItem.ArticleNumber} not found in expected delivery items! Created item to much.";
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.Error(exception, $"Error while testing the delivery creation. Error-Message: {exception.Message}");
                        deliveryCreationTest.LastTestSuccessful = false;
                        deliveryCreationTest.LastTestResultMessage += $"{Environment.NewLine}Error while testing the delivery creation. Error-Message: {exception.Message}";
                    }

                    if (context.ChangeTracker.HasChanges())
                    {
                        context.SaveChanges();
                    }

                    var deliveryCreationTestDetails = deliveryCreationTest.ToDeliveryCreationTestDetails();
                    SetDeliveryCreationTestDetailsSecurity(deliveryCreationTestDetails);

                    return deliveryCreationTestDetails;
                }
            }
            catch (Exception ex)
            {
                var error = $"Error while trying to create accounting for flights. Message: {ex.Message}";
                Logger.Error(ex, error);
            }

            return null;
        }
        
        public PagedList<DeliveryCreationTestOverview> GetPagedDeliveryCreationTestOverview(int? pageStart, int? pageSize, PageableSearchFilter<DeliveryCreationTestOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<DeliveryCreationTestOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new DeliveryCreationTestOverviewSearchFilter();
            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("DeliveryCreationTestName", "asc");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var deliveryCreationTests = context.DeliveryCreationTests
                    .Where(q => q.ClubId == CurrentAuthenticatedFLSUserClubId)
                    .OrderByPropertyNames(pageableSearchFilter.Sorting);

                var filter = pageableSearchFilter.SearchFilter;
                deliveryCreationTests = deliveryCreationTests.WhereIf(filter.DeliveryCreationTestName,
                        test => test.DeliveryCreationTestName.Contains(filter.DeliveryCreationTestName));
                deliveryCreationTests = deliveryCreationTests.WhereIf(filter.Description,
                        test => test.Description.Contains(filter.Description));
                deliveryCreationTests = deliveryCreationTests.WhereIf(filter.LastTestResultMessage,
                        test => test.LastTestResultMessage.Contains(filter.LastTestResultMessage));

                if (filter.LastTestRunOn != null)
                {
                    var dateTimeFilter = filter.LastTestRunOn;

                    if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                    {
                        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                        deliveryCreationTests = deliveryCreationTests.Where(test => DbFunctions.TruncateTime(test.LastTestRunOn) >= from.Date
                            && DbFunctions.TruncateTime(test.LastTestRunOn) <= to.Date);
                    }
                }

                if (filter.IsActive.HasValue)
                    deliveryCreationTests = deliveryCreationTests.Where(test => test.IsActive == filter.IsActive.Value);

                if (filter.LastTestSuccessful.HasValue)
                    deliveryCreationTests = deliveryCreationTests.Where(test => test.LastTestSuccessful == filter.LastTestSuccessful.Value);
                
                var pagedQuery = new PagedQuery<DeliveryCreationTest>(deliveryCreationTests, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList().Select(x => new DeliveryCreationTestOverview()
                {
                    DeliveryCreationTestId = x.DeliveryCreationTestId,
                    DeliveryCreationTestName = x.DeliveryCreationTestName,
                    FlightInformationOverview = new FlightInformationOverview()
                    {
                        FlightId  = x.FlightId
                    },
                    Description = x.Description,
                    IsActive = x.IsActive,
                    LastTestSuccessful = x.LastTestSuccessful,
                    LastTestResultMessage = x.LastTestResultMessage,
                    LastTestRunOn = x.LastTestRunOn
                }).ToList();

                foreach (var deliveryCreationTestOverview in overviewList)
                {
                    var flight =
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
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person + "." +
                                     Constants.PersonClubs)
                            .Include(Constants.TowFlight + "." + Constants.StartLocation)
                            .Include(Constants.TowFlight + "." + Constants.LdgLocation)
                            .FirstOrDefault(
                                f =>
                                    f.FlightId ==
                                        deliveryCreationTestOverview.FlightInformationOverview.FlightId);

                    if (flight != null)
                    {
                        deliveryCreationTestOverview.FlightInformationOverview.FlightDate =
                            flight.FlightDate.GetValueOrDefault();
                        deliveryCreationTestOverview.FlightInformationOverview.AircraftImmatriculation =
                            flight.AircraftImmatriculation;
                        deliveryCreationTestOverview.FlightInformationOverview.FlightCrewNames = flight.PilotDisplayName;

                        if (string.IsNullOrWhiteSpace(flight.PassengerDisplayName) == false)
                            deliveryCreationTestOverview.FlightInformationOverview.FlightCrewNames +=
                                $"/{flight.PassengerDisplayName}";
                        if (string.IsNullOrWhiteSpace(flight.CoPilotDisplayName) == false)
                            deliveryCreationTestOverview.FlightInformationOverview.FlightCrewNames +=
                                $"/{flight.CoPilotDisplayName}";
                        if (string.IsNullOrWhiteSpace(flight.InstructorDisplayName) == false)
                            deliveryCreationTestOverview.FlightInformationOverview.FlightCrewNames +=
                                $"/{flight.InstructorDisplayName}";

                        deliveryCreationTestOverview.FlightInformationOverview.FlightDurationInSeconds =
                            Convert.ToInt32(flight.FlightDuration.GetValueOrDefault().TotalSeconds);
                        deliveryCreationTestOverview.FlightInformationOverview.FlightTypeName = $"{flight.FlightType.FlightCode} - {flight.FlightType.FlightTypeName}";
                        deliveryCreationTestOverview.FlightInformationOverview.StartAndLdgLocationNames =
                            $"{flight.StartLocation.IcaoCode} -> {flight.LdgLocation.IcaoCode}";

                        if (flight.TowFlight != null)
                        {
                            deliveryCreationTestOverview.FlightInformationOverview.TowFlightInformation =
                                $"{flight.TowFlight.AircraftImmatriculation} / {flight.TowFlight.PilotDisplayName} / {Convert.ToInt32(flight.TowFlight.FlightDuration.GetValueOrDefault().TotalSeconds)}s";
                        }

                    }
                }


                var pagedList = new PagedList<DeliveryCreationTestOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        public DeliveryCreationTestDetails GetDeliveryCreationTestDetails(Guid deliveryCreationTestId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var deliveryCreationTest = context.DeliveryCreationTests.FirstOrDefault(c => c.DeliveryCreationTestId == deliveryCreationTestId && c.ClubId == CurrentAuthenticatedFLSUserClubId);

                var deliveryCreationTestDetails = deliveryCreationTest.ToDeliveryCreationTestDetails();
                SetDeliveryCreationTestDetailsSecurity(deliveryCreationTestDetails);
                return deliveryCreationTestDetails;
            }
        }

        public void InsertDeliveryCreationTestDetails(DeliveryCreationTestDetails deliveryCreationTestDetails)
        {
            var deliveryCreationTest = deliveryCreationTestDetails.ToDeliveryCreationTest(CurrentAuthenticatedFLSUserClubId);
            deliveryCreationTest.EntityNotNull("DeliveryCreationTest", Guid.Empty);

            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.DeliveryCreationTests.Add(deliveryCreationTest);
                context.SaveChanges();
            }

            //Map it back to details
            deliveryCreationTest.ToDeliveryCreationTestDetails(deliveryCreationTestDetails);
        }
        public void UpdateDeliveryCreationTestDetails(DeliveryCreationTestDetails currentDeliveryCreationTestDetails)
        {
            currentDeliveryCreationTestDetails.ArgumentNotNull("currentDeliveryCreationTestDetails");

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.DeliveryCreationTests.FirstOrDefault(x => x.DeliveryCreationTestId == currentDeliveryCreationTestDetails.DeliveryCreationTestId);
                original.EntityNotNull("DeliveryCreationTest", currentDeliveryCreationTestDetails.DeliveryCreationTestId);

                currentDeliveryCreationTestDetails.ToDeliveryCreationTest(CurrentAuthenticatedFLSUserClubId, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    original.ToDeliveryCreationTestDetails(currentDeliveryCreationTestDetails);
                }
            }
        }

        public void DeleteDeliveryCreationTest(Guid deliveryCreationTestId)
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.DeliveryCreationTests.FirstOrDefault(x => x.DeliveryCreationTestId == deliveryCreationTestId);
                original.EntityNotNull("DeliveryCreationTest", deliveryCreationTestId);

                context.DeliveryCreationTests.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion DeliveryCreationTesting

        #region Delivery

        public DeliveryDetails GetDeliveryDetails(Guid deliveryId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var delivery =
                    context.Deliveries
                        .Include("Flight")
                        .Include("Flight." + Constants.Aircraft)
                        .Include("Flight." + Constants.FlightType)
                        .Include("Flight.FlightCrews")
                        .Include("Flight.FlightCrews.Person")
                        .Include("Flight.FlightCrews.Person.PersonClubs")
                        .Include("DeliveryItems")
                        .FirstOrDefault(
                        c => c.DeliveryId == deliveryId && c.ClubId == CurrentAuthenticatedFLSUserClubId);

                var deliveryDetails = delivery.ToDeliveryDetails();
                SetDeliveryDetailsSecurity(deliveryDetails);
                return deliveryDetails;
            }
        }

        public PagedList<DeliveryOverview> GetPagedDeliveryOverview(int? pageStart, int? pageSize, PageableSearchFilter<DeliveryOverviewSearchFilter> pageableSearchFilter)
        {
            if (pageableSearchFilter == null) pageableSearchFilter = new PageableSearchFilter<DeliveryOverviewSearchFilter>();
            if (pageableSearchFilter.SearchFilter == null) pageableSearchFilter.SearchFilter = new DeliveryOverviewSearchFilter();

            ////needs to remap related table columns for correct sorting
            ////http://stackoverflow.com/questions/3515105/using-first-with-orderby-and-dynamicquery-in-one-to-many-related-tables
            foreach (var sort in pageableSearchFilter.Sorting.Keys.ToList())
            {
                if (sort == "FlightAircraftImmatriculation")
                {
                    pageableSearchFilter.Sorting.Add("FlightInformation.AircraftImmatriculation", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }

                if (sort == "FlightStartDateTime")
                {
                    pageableSearchFilter.Sorting.Add("FlightInformation.StartDateTime", pageableSearchFilter.Sorting[sort]);
                    pageableSearchFilter.Sorting.Remove(sort);
                }
            }

            if (pageableSearchFilter.Sorting == null || pageableSearchFilter.Sorting.Any() == false)
            {
                pageableSearchFilter.Sorting = new Dictionary<string, string>();
                pageableSearchFilter.Sorting.Add("BatchId", "desc");
            }
            
            using (var context = _dataAccessService.CreateDbContext())
            {
                var includedFlightCrewTypes = new int[]
                {
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.Observer,
                    (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger
                };

                var flightCrews = context
                    .FlightCrews
                    .Include("Person")
                    .Where(fc => includedFlightCrewTypes.Contains(fc.FlightCrewTypeId))
                    .GroupBy(fc => fc.FlightId)
                    .Select(fc => new
                    {
                        FlightId = fc.Key,
                        Pilot = fc.FirstOrDefault(ffc => ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.PilotOrStudent),
                        SecondCrew = fc.OrderBy(ffc => ffc.FlightCrewTypeId).FirstOrDefault(ffc =>
                            ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.CoPilot
                            || ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.FlightInstructor
                            || ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Observer
                            || ffc.FlightCrewTypeId == (int)FLS.Data.WebApi.Flight.FlightCrewType.Passenger)
                    });

                var deliveries = context.Deliveries
                    .Where(q => q.ClubId == CurrentAuthenticatedFLSUserClubId);

                var filter = pageableSearchFilter.SearchFilter;
                deliveries = deliveries.WhereIf(filter.DeliveryInformation,
                        delivery => delivery.DeliveryInformation.Contains(filter.DeliveryInformation));
                deliveries = deliveries.WhereIf(filter.DeliveryNumber,
                        delivery => delivery.DeliveryNumber.Contains(filter.DeliveryNumber));

                if (filter.DeliveredOn != null)
                {
                    var dateTimeFilter = filter.DeliveredOn;

                    if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                    {
                        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                        deliveries = deliveries.Where(delivery => DbFunctions.TruncateTime(delivery.DeliveredOn) >= DbFunctions.TruncateTime(from)
                                                                         && DbFunctions.TruncateTime(delivery.DeliveredOn) <= DbFunctions.TruncateTime(to));
                    }
                }

                if (filter.FlightStartDateTime != null)
                {
                    var dateTimeFilter = filter.FlightStartDateTime;

                    if (dateTimeFilter.From.HasValue || dateTimeFilter.To.HasValue)
                    {
                        var from = dateTimeFilter.From.GetValueOrDefault(DateTime.MinValue);
                        var to = dateTimeFilter.To.GetValueOrDefault(DateTime.MaxValue);

                        deliveries = deliveries.Where(delivery => DbFunctions.TruncateTime(delivery.Flight.StartDateTime) >= DbFunctions.TruncateTime(from)
                                                                  && DbFunctions.TruncateTime(delivery.Flight.StartDateTime) <= DbFunctions.TruncateTime(to));
                    }
                }

                deliveries = deliveries.WhereIf(filter.RecipientName,
                    delivery => delivery.RecipientName.Contains(filter.RecipientName));

                deliveries = deliveries.WhereIf(filter.FlightAircraftImmatriculation,
                    delivery => delivery.Flight.Aircraft.Immatriculation.Contains(filter.FlightAircraftImmatriculation));

                deliveries = deliveries.WhereIf(filter.BatchId.HasValue,
                        delivery => delivery.BatchId.ToString().Contains(filter.BatchId.ToString()));
                deliveries = deliveries.WhereIf(filter.IsFurtherProcessed.HasValue,
                        delivery => delivery.IsFurtherProcessed == filter.IsFurtherProcessed.Value);
                
                var deliveriesAndFlights = deliveries.GroupJoin(flightCrews, f => f.FlightId, fcp => fcp.FlightId,
                        (f, fcp) => new {f, fcp})
                    .SelectMany(x => x.fcp.DefaultIfEmpty(), (f, fcp) => new DeliveryOverview()
                    {
                        DeliveryId = f.f.DeliveryId,
                        BatchId = f.f.BatchId,
                        DeliveredOn = f.f.DeliveredOn,
                        IsFurtherProcessed = f.f.IsFurtherProcessed,
                        DeliveryNumber = f.f.DeliveryNumber,
                        DeliveryInformation = f.f.DeliveryInformation,
                        RecipientName = f.f.RecipientName,
                        FlightInformation = new DeliveryOverviewFlightInformation()
                        {
                            AircraftImmatriculation = f.f.Flight.Aircraft.Immatriculation,
                            StartDateTime = f.f.Flight.StartDateTime
                        },
                        CanUpdateRecord = true,
                        CanDeleteRecord = true
                    }).OrderByPropertyNames(pageableSearchFilter.Sorting);

                var pagedQuery = new PagedQuery<DeliveryOverview>(deliveriesAndFlights, pageStart, pageSize);

                var overviewList = pagedQuery.Items.ToList();
                
                var pagedList = new PagedList<DeliveryOverview>(overviewList, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);
                
                return pagedList;
            }
        }

        public List<DeliveryDetails> GetDeliveryDetailsList(bool? furtherProcessed)
        {
            return GetDeliveryDetailsList(furtherProcessed, CurrentAuthenticatedFLSUserClubId);
        }

        public List<DeliveryDetails> GetDeliveryDetailsList(bool? furtherProcessed, Guid clubId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var query = context.Deliveries
                            .Include("Flight")
                            .Include("Flight." + Constants.Aircraft)
                            .Include("Flight." + Constants.FlightType)
                            .Include("Flight.FlightCrews")
                            .Include("Flight.FlightCrews.Person")
                            .Include("Flight.FlightCrews.Person.PersonClubs")
                            .Include("DeliveryItems")
                            .Where(c => c.ClubId == clubId);

                if (furtherProcessed.HasValue)
                {
                    query = query.Where(x => x.IsFurtherProcessed == furtherProcessed.Value);
                }

                var deliveries = query.OrderBy(t => t.DeliveryNumber).ToList();

                var deliveryDetailList = deliveries.Select(x => x.ToDeliveryDetails()).ToList();

                foreach (var delivery in deliveryDetailList)
                {
                    SetDeliveryDetailsSecurity(delivery);
                }

                return deliveryDetailList;
            }
        }

        public void InsertDeliveryDetails(DeliveryDetails deliveryDetails)
        {
            var delivery = deliveryDetails.ToDelivery(CurrentAuthenticatedFLSUserClubId);
            delivery.EntityNotNull("Delivery", Guid.Empty);

            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Deliveries.Add(delivery);
                context.SaveChanges();
            }

            //Map it back to details
            delivery.ToDeliveryDetails(deliveryDetails);
        }

        public void UpdateDeliveryDetails(DeliveryDetails currentDeliveryDetails)
        {
            currentDeliveryDetails.ArgumentNotNull("currentDeliveryDetails");

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Deliveries.FirstOrDefault(x => x.DeliveryId == currentDeliveryDetails.DeliveryId);
                original.EntityNotNull("Delivery", currentDeliveryDetails.DeliveryId);

                currentDeliveryDetails.ToDelivery(CurrentAuthenticatedFLSUserClubId, original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                    original.ToDeliveryDetails(currentDeliveryDetails);
                }
            }
        }

        public void DeleteDelivery(Guid deliveryId)
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var delivery = context.Deliveries.FirstOrDefault(x => x.DeliveryId == deliveryId);
                delivery.EntityNotNull("Delivery", deliveryId);

                if (delivery.FlightId.HasValue)
                {
                    var flightId = delivery.FlightId.Value;

                    if (context.Deliveries.Count(
                            x => x.FlightId.HasValue && x.FlightId.Value == flightId) > 1)
                    {
                        Logger.Info(
                            "Delivery can not be deleted with this method, as there are other deliveries assigned to the same flight!");
                        throw new FLSServerException(
                            "Delivery can not be deleted with this method, as there are other deliveries assigned to the same flight!");
                    }

                    context.Deliveries.Remove(delivery);
                    var flight = context.Flights.FirstOrDefault(x => x.FlightId == flightId);
                    flight.EntityNotNull("Flight", flightId);
                    flight.DeletedDeliveryForFlight(); //reset process state to locked

                    context.SaveChanges();
                }
                else
                {
                    //the delivery has no relation to a flight
                    context.Deliveries.Remove(delivery);
                    context.SaveChanges();
                }
            }
        }

        public Nullable<DateTime> GetLastDeliverySynchronisationOn()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var club = context.Clubs.FirstOrDefault(c => c.ClubId == CurrentAuthenticatedFLSUserClubId);
                club.EntityNotNull("Club");

                return club.LastDeliverySynchronisationOn;
            }
        }

        public void SetLastDeliverySynchronisationOn(Nullable<DateTime> lastDeliverySynchronisationOn)
        {
            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException(ErrorMessage.NotInRoleClubAdmin);
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var club = context.Clubs.FirstOrDefault(c => c.ClubId == CurrentAuthenticatedFLSUserClubId);
                club.EntityNotNull("Club");

                club.LastDeliverySynchronisationOn = lastDeliverySynchronisationOn;
                club.DoNotUpdateMetaData = true;

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                }
            }
        }
        #endregion Delivery

        #region Security
        private void SetDeliveryDetailsSecurity(DeliveryDetails details)
        {
            if (details == null)
            {
                Logger.Error(string.Format("DeliveryDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleClubAdministrator)
            {
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = true;

                foreach (var item in details.DeliveryItems)
                {
                    item.CanUpdateRecord = false;
                    item.CanDeleteRecord = true;
                }
            }
            else
            {
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
            }
        }

        private void SetDeliveryCreationTestOverviewSecurity(List<DeliveryCreationTestOverview> overviewList)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in overviewList)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            foreach (var overview in overviewList)
            {
                if (IsCurrentUserInRoleClubAdministrator)
                {
                    overview.CanUpdateRecord = true;
                    overview.CanDeleteRecord = true;
                }
                else
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }
            }
        }

        private void SetDeliveryCreationTestDetailsSecurity(DeliveryCreationTestDetails details)
        {
            if (details == null)
            {
                Logger.Error(string.Format("DeliveryDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleClubAdministrator)
            {
                details.CanUpdateRecord = true;
                details.CanDeleteRecord = true;
            }
            else
            {
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
            }
        }
        #endregion Security
    }

}

