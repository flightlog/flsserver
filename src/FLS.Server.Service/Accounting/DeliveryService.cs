﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi.Accounting;
using FLS.Data.WebApi.Accounting.Testing;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using FLS.Server.Interfaces;
using FLS.Server.Service.Accounting.RuleEngines;
using Newtonsoft.Json;
using AccountingRuleFilterType = FLS.Data.WebApi.Accounting.RuleFilters.AccountingRuleFilterType;

namespace FLS.Server.Service.Accounting
{
    public class DeliveryService : BaseService
    {
        private readonly DataAccessService _dataAccessService;
        private readonly AccountingRuleFilterFactory _accountingRuleFilterFactory;
        private readonly IPersonService _personService;
        private readonly IAircraftService _aircraftService;
        private readonly ILocationService _locationService;
        private readonly AccountingRuleService _accountingRuleService;

        public DeliveryService(DataAccessService dataAccessService, IdentityService identityService,
            AccountingRuleFilterFactory accountingRuleFilterFactory, IPersonService personService,
            IAircraftService aircraftService, ILocationService locationService,
            AccountingRuleService accountingRuleService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            _accountingRuleFilterFactory = accountingRuleFilterFactory;
            _personService = personService;
            _aircraftService = aircraftService;
            _locationService = locationService;
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
                throw new InvalidDataException("No valid ClubId to fetch accounting data!");
            }

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
                                              (int) FLS.Data.WebApi.Flight.FlightProcessState.Locked))
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
                            var delivery = CreateDeliveryForFlight(flight, clubId, batchId, accountingRuleFilters);

                            context.Deliveries.Add(delivery);

                            flight.ProcessStateId = (int) FLS.Data.WebApi.Flight.FlightProcessState.Delivered;
                            flight.DeliveryCreatedOn = DateTime.UtcNow;
                            flight.DoNotUpdateMetaData = true;

                            if (flight.TowFlight != null)
                            {
                                flight.TowFlight.ProcessStateId =
                                    (int) FLS.Data.WebApi.Flight.FlightProcessState.Delivered;
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

                    var deliveries = context.Deliveries.Where(x => x.BatchId == batchId).ToList();

                    var deliveryDetailsList = deliveries.Select(x => x.ToDeliveryDetails()).ToList();
                    Logger.Debug(
                        $"Have {deliveryDetailsList.Count} deliveries in result list for sending back to client.");

                    return deliveryDetailsList;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while trying to create accounting for flights. Message: {ex.Message}");
                throw;
            }
        }

        private Delivery CreateDeliveryForFlight(Flight flight, Guid clubId, long batchId,
            List<RuleBasedAccountingRuleFilterDetails> accountingRuleFilters)
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
                    FlightDate = flight.FlightDate.Value
                },
                DeliveryInformation = flight.FlightType.FlightTypeName,
                ClubId = clubId
            };

            var recipientRulesEngine = new RecipientRulesEngine(ruleBasedDelivery, flight,
                _personService,
                accountingRuleFilters.Where(
                    x =>
                        x.AccountingRuleFilterTypeId ==
                        (int) AccountingRuleFilterType.RecipientAccountingRuleFilter).ToList());
            recipientRulesEngine.Run();

            var accountingDetailsRuleEngine = new DeliveryDetailsRulesEngine(ruleBasedDelivery, flight);
            accountingDetailsRuleEngine.Run();

            var accountingLineRulesEngine = new DeliveryItemRulesEngine(ruleBasedDelivery, flight,
                _personService,
                accountingRuleFilters.Where(
                    x =>
                        x.AccountingRuleFilterTypeId !=
                        (int) AccountingRuleFilterType.RecipientAccountingRuleFilter).ToList());
            accountingLineRulesEngine.Run();

            Logger.Info(
                $"Created accounting for {ruleBasedDelivery.RecipientDetails.RecipientName}: Flight-Date: {ruleBasedDelivery.FlightInformation.FlightDate.ToShortDateString()} Aircraft: {ruleBasedDelivery.FlightInformation.AircraftImmatriculation} Nr of Lines: {ruleBasedDelivery.DeliveryItems.Count}");

            var delivery = ruleBasedDelivery.ToDelivery(clubId);
            delivery.BatchId = batchId;

            return delivery;
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


                    var delivery = CreateDeliveryForFlight(flight, CurrentAuthenticatedFLSUserClubId, 1,
                        accountingRuleFilters);
                    var deliveryDetails = delivery.ToDeliveryDetails();
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
        /// <returns>AccountingRuleFilterTestResult</returns>
        public void RunDeliveryCreationTest(Guid deliveryCreationTestId)
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


                    var createdDelivery = CreateDeliveryForFlight(flight, CurrentAuthenticatedFLSUserClubId, 1,
                        accountingRuleFilters);
                    var accountingFilterTypes = context.AccountingRuleFilterTypes.ToList();

                    var matchedAccountingRuleFilters = accountingRuleFilters.Where(x => x.HasMatched).ToList();

                    // ReSharper disable once PossibleNullReferenceException
                    deliveryCreationTest.LastTestCreatedDeliveryDetails = JsonConvert.SerializeObject(createdDelivery.ToDeliveryDetails());
                    deliveryCreationTest.LastTestMatchedAccountingRuleFilterIds =
                        JsonConvert.SerializeObject(
                            matchedAccountingRuleFilters.Select(x => x.AccountingRuleFilterId).ToList());
                    deliveryCreationTest.LastTestRunOn = DateTime.UtcNow;
                    deliveryCreationTest.LastTestSuccessful = true;

                    try
                    {
                        // compare delivery with expected delivery and consider flags

                        if (deliveryCreationTest.MustNotCreateDeliveryForFlight)
                        {
                            if (createdDelivery == null || createdDelivery.DeliveryItems == null ||
                                createdDelivery.DeliveryItems.Any() == false)
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
                                if (createdDelivery.DeliveryInformation != expectedDeliveryDetails.DeliveryInformation)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage =
                                        $"DeliveryInformation doesn't match";
                                }
                            }

                            if (deliveryCreationTest.IgnoreAdditionalInformation == false)
                            {
                                if (createdDelivery.AdditionalInformation !=
                                    expectedDeliveryDetails.AdditionalInformation)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}AdditionalInformation doesn't match";
                                }
                            }

                            var createdRecipientDetails =
                                JsonConvert.DeserializeObject<RecipientDetails>(createdDelivery.RecipientDetails);

                            if (deliveryCreationTest.IgnoreRecipientPersonId == false)
                            {
                                if (createdRecipientDetails.PersonId !=
                                    expectedDeliveryDetails.RecipientDetails.PersonId)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}PersonId doesn't match";
                                }
                            }

                            if (deliveryCreationTest.IgnoreRecipientClubMemberNumber == false)
                            {
                                if (createdRecipientDetails.PersonClubMemberNumber !=
                                    expectedDeliveryDetails.RecipientDetails.PersonClubMemberNumber)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}PersonClubMemberNumber doesn't match";
                                }
                            }

                            if (deliveryCreationTest.IgnoreRecipientName == false)
                            {
                                if (createdRecipientDetails.Lastname !=
                                    expectedDeliveryDetails.RecipientDetails.Lastname)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}Lastname doesn't match";
                                }

                                if (createdRecipientDetails.Firstname !=
                                    expectedDeliveryDetails.RecipientDetails.Firstname)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}Firstname doesn't match";
                                }
                            }

                            if (deliveryCreationTest.IgnoreRecipientAddress == false)
                            {
                                if (createdRecipientDetails.AddressLine1 !=
                                    expectedDeliveryDetails.RecipientDetails.AddressLine1)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}AddressLine1 doesn't match";
                                }

                                if (createdRecipientDetails.AddressLine2 !=
                                    expectedDeliveryDetails.RecipientDetails.AddressLine2)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}AddressLine2 doesn't match";
                                }

                                if (createdRecipientDetails.ZipCode != expectedDeliveryDetails.RecipientDetails.ZipCode)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}ZipCode doesn't match";
                                }

                                if (createdRecipientDetails.City != expectedDeliveryDetails.RecipientDetails.City)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}City doesn't match";
                                }

                                if (createdRecipientDetails.CountryName !=
                                    expectedDeliveryDetails.RecipientDetails.CountryName)
                                {
                                    deliveryCreationTest.LastTestSuccessful = false;
                                    deliveryCreationTest.LastTestResultMessage +=
                                        $"{Environment.NewLine}CountryName doesn't match";
                                }
                            }

                            if (createdDelivery.DeliveryItems.Count != expectedDeliveryDetails.DeliveryItems.Count)
                            {
                                deliveryCreationTest.LastTestSuccessful = false;
                                deliveryCreationTest.LastTestResultMessage +=
                                    $"{Environment.NewLine}Numbers of delivery items doesn't match";
                            }

                            foreach (var expectedDeliveryItem in expectedDeliveryDetails.DeliveryItems)
                            {
                                DeliveryItem createdItem = null;

                                if (deliveryCreationTest.IgnoreItemPositioning == false)
                                {
                                    createdItem =
                                        createdDelivery.DeliveryItems.FirstOrDefault(
                                            x => x.Position == expectedDeliveryItem.Position);
                                }
                                else
                                {
                                    createdItem =
                                        createdDelivery.DeliveryItems.FirstOrDefault(
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

                            if (createdDelivery.DeliveryItems.Count > expectedDeliveryDetails.DeliveryItems.Count)
                            {
                                foreach (var createdItem in createdDelivery.DeliveryItems)
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
                }
            }
            catch (Exception ex)
            {
                var error = $"Error while trying to create accounting for flights. Message: {ex.Message}";
                Logger.Error(ex, error);
            }
        }

        public List<DeliveryCreationTestOverview> GetDeliveryCreationTestOverviews()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<DeliveryCreationTest> deliveryCreationTests = context.DeliveryCreationTests.Where(c => c.ClubId == CurrentAuthenticatedFLSUserClubId).OrderBy(t => t.DeliveryCreationTestName).ToList();

                var overviewList = deliveryCreationTests.Select(x => new DeliveryCreationTestOverview()
                {
                    DeliveryCreationTestId = x.DeliveryCreationTestId,
                    FlightId = x.FlightId,
                    DeliveryCreationTestName = x.DeliveryCreationTestName,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    LastTestSuccessful = x.LastTestSuccessful,
                    LastTestResultMessage = x.LastTestResultMessage,
                    LastTestRunOn = x.LastTestRunOn
                }).ToList();

                SetDeliveryCreationTestOverviewSecurity(overviewList);
                return overviewList;
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
                throw new UnauthorizedAccessException("You must be a club administrator to insert a new DeliveryCreationTest!");
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
                throw new UnauthorizedAccessException("You must be a club administrator to delete a DeliveryCreationTest!");
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

        public List<DeliveryOverview> GetDeliveryOverviews()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                List<Delivery> deliveries =
                    context.Deliveries.Include("DeliveryItems").Include("Flight").Include("Flight.FlightType")
                        .Where(c => c.ClubId == CurrentAuthenticatedFLSUserClubId)
                        .OrderBy(t => t.DeliveryNumber)
                        .ToList();

                var overviewList = deliveries.Select(x => x.ToDeliveryOverview()).ToList();

                SetDeliveryOverviewSecurity(overviewList);
                return overviewList;
            }
        }

        public DeliveryDetails GetDeliveryDetails(Guid deliveryId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var delivery =
                    context.Deliveries.FirstOrDefault(
                        c => c.DeliveryId == deliveryId && c.ClubId == CurrentAuthenticatedFLSUserClubId);

                var deliveryDetails = delivery.ToDeliveryDetails();
                SetDeliveryDetailsSecurity(deliveryDetails);
                return deliveryDetails;
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
                var query = context.Deliveries.Include("DeliveryItems")
                    .Where(c => c.ClubId == CurrentAuthenticatedFLSUserClubId);

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
            var article = deliveryDetails.ToDelivery(CurrentAuthenticatedFLSUserClubId);
            article.EntityNotNull("Delivery", Guid.Empty);

            if (IsCurrentUserInRoleClubAdministrator == false)
            {
                throw new UnauthorizedAccessException("You must be a club administrator to insert a new delivery!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Deliveries.Add(article);
                context.SaveChanges();
            }

            //Map it back to details
            article.ToDeliveryDetails(deliveryDetails);
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
                throw new UnauthorizedAccessException("You must be a club administrator to delete a delivery!");
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Deliveries.FirstOrDefault(x => x.DeliveryId == deliveryId);
                original.EntityNotNull("Delivery", deliveryId);

                context.Deliveries.Remove(original);
                context.SaveChanges();
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
                throw new UnauthorizedAccessException("You must be a club administrator to set the last delivery synchronisation datetime value!");
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
        private void SetDeliveryOverviewSecurity(List<DeliveryOverview> overviewList)
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
                details.CanUpdateRecord = true;
                details.CanDeleteRecord = true;

                foreach (var item in details.DeliveryItems)
                {
                    item.CanDeleteRecord = true;
                    item.CanUpdateRecord = true;
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
