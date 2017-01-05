using System;
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


        public List<DeliveryDetails> CreateDeliveriesFromFlights(DateTime fromDate, DateTime toDate, Guid clubId)
        {
            if (clubId.IsValid() == false)
            {
                Logger.Error("No valid ClubId for getting the accountings!");
                throw new InvalidDataException("No valid ClubId to fetch accounting data!");
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
                            .Include(Constants.TowFlight + "." + Constants.FlightCrews + "." + Constants.Person + "." +
                                     Constants.PersonClubs)
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
                                             (flight.ProcessStateId ==
                                              (int) FLS.Data.WebApi.Flight.FlightProcessState.Locked))
                            .ToList();

                    Logger.Debug(
                        string.Format("Queried Flights for accounting between {1} and {2} and got {0} flights back.",
                            flights.Count, fromDateTime, toDateTime));


                    var accountingRuleFilters = _accountingRuleService.GetRuleBasedAccountingRuleFilterDetailsListByClubId(clubId);

                    if (accountingRuleFilters == null || accountingRuleFilters.Count == 0)
                    {
                        var accountingRuleFilterDetailsList =
                            _accountingRuleFilterFactory.CreateAccountingRuleFilterDetails();

                        foreach (var accountingRuleFilterDetails in accountingRuleFilterDetailsList)
                        {
                            _accountingRuleService.InsertAccountingRuleFilterDetails(accountingRuleFilterDetails, clubId);
                        }

                        accountingRuleFilters = _accountingRuleService.GetRuleBasedAccountingRuleFilterDetailsListByClubId(clubId);
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
                                flight.TowFlight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Delivered;
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

        private Delivery CreateDeliveryForFlight(Flight flight, Guid clubId, long batchId, List<RuleBasedAccountingRuleFilterDetails> accountingRuleFilters)
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
                        (int)AccountingRuleFilterType.RecipientAccountingRuleFilter).ToList());
            recipientRulesEngine.Run();

            var accountingDetailsRuleEngine = new DeliveryDetailsRulesEngine(ruleBasedDelivery, flight);
            accountingDetailsRuleEngine.Run();

            var accountingLineRulesEngine = new DeliveryItemRulesEngine(ruleBasedDelivery, flight,
                _personService,
                accountingRuleFilters.Where(
                    x =>
                        x.AccountingRuleFilterTypeId !=
                        (int)AccountingRuleFilterType.RecipientAccountingRuleFilter).ToList());
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
        public DeliveryDetails CreateDeliveryDetailsForTest(Guid flightId)
        {
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

                    var accountingRuleFilters = _accountingRuleService.GetRuleBasedAccountingRuleFilterDetailsListByClubId(CurrentAuthenticatedFLSUserClubId);

                    accountingRuleFilters.NotNull("accountingRuleFilters");


                    var delivery = CreateDeliveryForFlight(flight, CurrentAuthenticatedFLSUserClubId, 1, accountingRuleFilters);
                    var deliveryDetails = delivery.ToDeliveryDetails();
                    return deliveryDetails;
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
        public AccountingRuleFiltersTestResult RunDeliveryCreationTest(Guid deliveryCreationTestId)
        {
            var result = new AccountingRuleFiltersTestResult();

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

                    var accountingRuleFilters = _accountingRuleService.GetRuleBasedAccountingRuleFilterDetailsListByClubId(CurrentAuthenticatedFLSUserClubId);
                    
                    accountingRuleFilters.NotNull("accountingRuleFilters");


                    var delivery = CreateDeliveryForFlight(flight, CurrentAuthenticatedFLSUserClubId, 1, accountingRuleFilters);
                    var accountingFilterTypes = context.AccountingRuleFilterTypes.ToList();

                    var matchedFilters = accountingRuleFilters.Where(x => x.HasMatched).ToList();
                    result.DeliveryDetails = delivery.ToDeliveryDetails();
                    result.MatchedAccountingRuleFilters =
                        matchedFilters.Select(x => x.ToAccountingRuleFilterOverview(accountingFilterTypes)).ToList();

                    if (result.MatchedAccountingRuleFilters.Any())
                    {
                        result.IsTestSuccessful = true;
                    }
                    else
                    {
                        result.Errors = $"No accounting rule filters has been matched for the flight: {flight}{Environment.NewLine}{result.DeliveryDetails}";
                    }
                }

            }
            catch (Exception ex)
            {
                var error = $"Error while trying to create accounting for flights. Message: {ex.Message}";
                Logger.Error(ex, error);

                result.IsTestSuccessful = false;
                result.Errors += $"Exception: {error}";
            }

            return result;
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
        #endregion Security
    }

}
