using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FLS.Common.Validators;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using FLS.Server.Interfaces;
using NLog;

namespace FLS.Server.Service
{
    public class AircraftService : BaseService, IAircraftService
    {
        private readonly DataAccessService _dataAccessService;

        public AircraftService(DataAccessService dataAccessService, IdentityService identityService) 
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region AircraftState
        public List<AircraftStateListItem> GetAircraftStateListItems()
        {
            var entities = GetAircraftStates();

            var items = entities.Select(aircraftState => aircraftState.ToAircraftStateListItem()).ToList();

            return items;
        }

        internal List<AircraftState> GetAircraftStates()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var aircraftStates = context.AircraftStates.OrderBy(a => a.AircraftStateId).ToList();

                return aircraftStates;
            }
        }
        #endregion AircraftState

        #region AircraftType
        public List<AircraftTypeListItem> GetAircraftTypeListItems()
        {
            var entities = GetAircraftTypes();

            var items = entities.Select(aircraftType => aircraftType.ToAircraftTypeListItem()).ToList();

            return items;
        }

        internal List<FLS.Server.Data.DbEntities.AircraftType> GetAircraftTypes()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var aircraftTypes = context.AircraftTypes.OrderBy(a => a.AircraftTypeName).ToList();

                return aircraftTypes;
            }
        }
        #endregion AircraftType

        #region Aircraft
        public List<AircraftListItem> GetGliderAircraftListItems()
        {
            var aircrafts = GetGliderAircrafts();

            return PrepareAircraftListItems(aircrafts);
        }

        public List<AircraftListItem> GetTowingAircraftListItems()
        {
            var aircrafts = GetTowingAircrafts();

            return PrepareAircraftListItems(aircrafts);
        }

        public List<AircraftListItem> GetMotorAircraftListItems()
        {
            var aircrafts = GetAircrafts(a => a.AircraftTypeId >= (int)FLS.Data.WebApi.Aircraft.AircraftType.MotorGlider);

            return PrepareAircraftListItems(aircrafts);
        }

        private List<AircraftListItem> PrepareAircraftListItems(List<Aircraft> aircrafts)
        {
            return aircrafts.Select(e => e.ToAircraftListItem()).ToList();
        }

        public List<AircraftOverview> GetGliderAircraftOverviews()
        {
            var aircrafts = GetGliderAircrafts();

            return PrepareAircraftOverviews(aircrafts);
        }

        public List<AircraftOverview> GetTowingAircraftOverviews()
        {
            var aircrafts = GetTowingAircrafts();

            return PrepareAircraftOverviews(aircrafts);
        }

        public List<AircraftOverview> GetAircraftOverviews()
        {
            var aircrafts = GetAircrafts();

            return PrepareAircraftOverviews(aircrafts);
        }

        public List<AircraftOverview> GetAircraftOverviews(int aircraftType)
        {
            var aircrafts = GetAircrafts(aircraftType);

            return PrepareAircraftOverviews(aircrafts);
        }

        private List<AircraftOverview> PrepareAircraftOverviews(List<Aircraft> aircrafts)
        {
            var aircraftOverviewList = aircrafts.Select(e => e.ToAircraftOverview()).ToList();
            SetAircraftOverviewSecurity(aircraftOverviewList);

            return aircraftOverviewList;
        }

        public AircraftDetails GetAircraftDetails(Guid aircraftId)
        {
            var aircraft = GetAircraft(aircraftId);

            var aircraftDetails = aircraft.ToAircraftDetails();
            SetAircraftDetailsSecurity(aircraftDetails, aircraft);

            return aircraftDetails;
        }

        public AircraftDetails GetAircraftDetails(string immatriculation)
        {
            var aircraft = GetAircraft(immatriculation);

            var aircraftDetails = aircraft.ToAircraftDetails();
            SetAircraftDetailsSecurity(aircraftDetails, aircraft);

            return aircraftDetails;
        }

        internal List<Aircraft> GetGliderAircrafts()
        {
            return GetAircrafts(a => a.AircraftTypeId == (int) FLS.Data.WebApi.Aircraft.AircraftType.Glider
                                      || a.AircraftTypeId == (int) FLS.Data.WebApi.Aircraft.AircraftType.GliderWithMotor);
        }

        internal List<Aircraft> GetTowingAircrafts()
        {
            return GetAircrafts(a => a.IsTowingAircraft);
        }

        internal List<Aircraft> GetAircrafts(int aircraftTypeId)
        {
            return GetAircrafts(a => a.AircraftTypeId == aircraftTypeId);
        }

        internal List<Aircraft> GetAircrafts(Expression<Func<Aircraft, bool>> aircraftFilter)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var aircrafts = context.Aircrafts
                    .Include(Constants.AircraftAircraftStates)
                    .Include(Constants.AircraftAircraftStatesAircraftStateRelation)
                    .Include("AircraftOwnerPerson")
                    .Include("AircraftOwnerClub")
                    .Include("AircraftType")
                    .Where(aircraftFilter)
                    .OrderBy(a => a.Immatriculation)
                    .ToList();

                return aircrafts;
            }
        }

        internal List<Aircraft> GetAircrafts()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var aircrafts = context.Aircrafts
                    .Include(Constants.AircraftAircraftStates)
                    .Include(Constants.AircraftAircraftStatesAircraftStateRelation)
                    .Include("AircraftOwnerPerson")
                    .Include("AircraftOwnerClub")
                    .Include("AircraftType")
                    .OrderBy(a => a.Immatriculation)
                    .ToList();

                return aircrafts;
            }
        }

        internal Aircraft GetAircraft(Guid aircraftId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var aircraft = context.Aircrafts
                    .Include(Constants.AircraftAircraftStates)
                    .Include(Constants.AircraftAircraftStatesAircraftStateRelation)
                    .Include("AircraftOwnerPerson")
                    .Include("AircraftOwnerClub")
                    .Include("AircraftType")
                    .FirstOrDefault(a => a.AircraftId == aircraftId);

                return aircraft;
            }
        }

        internal Aircraft GetAircraft(string immatriculation)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var aircraft = context.Aircrafts
                    .Include(Constants.AircraftAircraftStates)
                    .Include(Constants.AircraftAircraftStatesAircraftStateRelation)
                    .Include("AircraftOwnerPerson")
                    .Include("AircraftOwnerClub")
                    .Include("AircraftType")
                    .FirstOrDefault(a => a.Immatriculation.Replace("-", "").ToUpper() == immatriculation.Replace("-", "").ToUpper());

                return aircraft;
            }
        }

        public void InsertAircraftDetails(AircraftDetails aircraftDetails)
        {
            var aircraft = aircraftDetails.ToAircraft();
            aircraft.NotNull("Aircraft");

            InsertAircraft(aircraft);

            //Map it back to details
            aircraft.ToAircraftDetails(aircraftDetails);
        }

        internal void InsertAircraft(Aircraft aircraft)
        {
            aircraft.ArgumentNotNull("aircraft");

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Aircrafts.Add(aircraft);
                context.SaveChanges();
            }
        }

        public void UpdateAircraftDetails(AircraftDetails currentAircraftDetails)
        {
            currentAircraftDetails.ArgumentNotNull("currentAircraftDetails");
            var original = GetAircraft(currentAircraftDetails.AircraftId);
            original.EntityNotNull("Aircraft", currentAircraftDetails.AircraftId);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Aircrafts.Attach(original);
                currentAircraftDetails.ToAircraft(original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();

                    //Map it back to details
                    original.ToAircraftDetails(currentAircraftDetails);
                }
            }
        }

        public void DeleteAircraft(Guid aircraftId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Aircrafts.FirstOrDefault(l => l.AircraftId == aircraftId);
                original.EntityNotNull("Aircraft", aircraftId);

                context.Aircrafts.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion Aircraft

        #region AircraftOperatingCounter
        public List<AircraftOperatingCounterOverview> GetAircraftOperatingCounterOverviewByAircraftId(Guid aircraftId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var list = context.AircraftOperatingCounters.Where(a => a.Aircraft.AircraftId == aircraftId).ToList()
                    .Select(e => e.ToAircraftOperatingCounterOverview()).ToList();
                SetAircraftOperatingCounterOverviewSecurity(list);

                return list;
            }
        }

        public List<AircraftOperatingCounterOverview> GetAircraftOperatingCounterOverviewByImmatriculation(string immatriculation)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var list = context.AircraftOperatingCounters.Where(a => a.Aircraft.Immatriculation.Replace("-", "").ToUpper() == immatriculation.Replace("-", "").ToUpper()).ToList()
                    .Select(e => e.ToAircraftOperatingCounterOverview()).ToList();
                SetAircraftOperatingCounterOverviewSecurity(list);

                return list;
            }
        }

        public AircraftOperatingCounterDetails GetAircraftOperatingCounterDetails(Guid aircraftOperatingCounterId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var entity = context.AircraftOperatingCounters
                    .Include("Aircraft")
                    .FirstOrDefault(a => a.AircraftOperatingCounterId == aircraftOperatingCounterId);

                entity.EntityNotNull("AircraftOperatingCounter");
                var aircraftOperatingCounterDetails = entity.ToAircraftOperatingCounterDetails();
                SetAircraftOperatingCounterDetailsSecurity(aircraftOperatingCounterDetails, entity.Aircraft);

                return aircraftOperatingCounterDetails;
            }
        }

        public void InsertAircraftOperatingCounterDetails(AircraftOperatingCounterDetails aircraftOperatingCounterDetails)
        {
            var entity = aircraftOperatingCounterDetails.ToAircraftOperatingCounter();
            entity.NotNull("AircraftOperatingCounter");

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.AircraftOperatingCounters.Add(entity);
                context.SaveChanges();
            }

            //Map it back to details
            entity.ToAircraftOperatingCounterDetails(aircraftOperatingCounterDetails);
        }

        public void UpdateAircraftOperatingCounterDetails(AircraftOperatingCounterDetails currentAircraftOperatingCounterDetails)
        {
            currentAircraftOperatingCounterDetails.ArgumentNotNull("currentAircraftOperatingCounterDetails");

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.AircraftOperatingCounters.FirstOrDefault(l => l.AircraftOperatingCounterId == currentAircraftOperatingCounterDetails.AircraftOperatingCounterId);
                original.EntityNotNull("AircraftOperatingCounter", currentAircraftOperatingCounterDetails.AircraftOperatingCounterId);
                currentAircraftOperatingCounterDetails.ToAircraftOperatingCounter(original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();

                    //Map it back to details
                    original.ToAircraftOperatingCounterDetails(currentAircraftOperatingCounterDetails);
                }
            }
        }

        public void DeleteAircraftOperatingCounterDetails(Guid aircraftOperatingCounterId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.AircraftOperatingCounters.FirstOrDefault(l => l.AircraftOperatingCounterId == aircraftOperatingCounterId);
                original.EntityNotNull("AircraftOperatingCounter", aircraftOperatingCounterId);

                context.AircraftOperatingCounters.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion AircraftOperatingCounter

        #region CounterUnitType
        /// <summary>
        /// Gets the length unit type list items.
        /// </summary>
        /// <returns></returns>
        public List<CounterUnitTypeListItem> GetCounterUnitTypeListItems()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var entities = context.CounterUnitTypes.OrderBy(l => l.CounterUnitTypeName).ToList();

                var items = entities.Select(t => new CounterUnitTypeListItem
                {
                    CounterUnitTypeId = t.CounterUnitTypeId,
                    CounterUnitTypeKeyName = t.CounterUnitTypeKeyName,
                    CounterUnitTypeName = t.CounterUnitTypeName
                }).ToList();

                return items;
            }
        }
        #endregion CounterUnitType

        #region Security
        private void SetAircraftOverviewSecurity(IEnumerable<AircraftOverview> list)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in list)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                foreach (var aircraftOverview in list)
                {
                    if (IsCurrentUserInRoleClubAdministrator ||
                        IsOwner(context.Aircrafts.First(a => a.AircraftId == aircraftOverview.AircraftId)))
                    {
                        aircraftOverview.CanUpdateRecord = true;
                        aircraftOverview.CanDeleteRecord = true;
                    }
                    else
                    {
                        aircraftOverview.CanUpdateRecord = false;
                        aircraftOverview.CanDeleteRecord = false;
                    }
                }
            }
        }

        private void SetAircraftDetailsSecurity(AircraftDetails details, Aircraft aircraft)
        {
            if (details == null)
            {
                Logger.Error(string.Format("AircraftDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleClubAdministrator || IsOwner(aircraft))
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

        private void SetAircraftOperatingCounterOverviewSecurity(IEnumerable<AircraftOperatingCounterOverview> list)
        {
            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                foreach (var overview in list)
                {
                    overview.CanUpdateRecord = false;
                    overview.CanDeleteRecord = false;
                }

                return;
            }

            using (var context = _dataAccessService.CreateDbContext())
            {
                foreach (var aircraftOverview in list)
                {
                    if (IsCurrentUserInRoleClubAdministrator ||
                        IsOwner(context.Aircrafts.First(a => a.Immatriculation == aircraftOverview.Immatriculation)))
                    {
                        aircraftOverview.CanUpdateRecord = true;
                        aircraftOverview.CanDeleteRecord = true;
                    }
                    else
                    {
                        aircraftOverview.CanUpdateRecord = false;
                        aircraftOverview.CanDeleteRecord = false;
                    }
                }
            }
        }

        private void SetAircraftOperatingCounterDetailsSecurity(AircraftOperatingCounterDetails details, Aircraft aircraft)
        {
            if (details == null)
            {
                Logger.Error(string.Format("AircraftOperatingCounterDetails is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleClubAdministrator || IsOwner(aircraft))
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
