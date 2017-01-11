using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Exceptions;
using FLS.Common.Extensions;
using FLS.Common.Paging;
using FLS.Common.Validators;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Location;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using FLS.Server.Interfaces;
using NLog;

namespace FLS.Server.Service
{
    public class LocationService : BaseService, ILocationService
    {
        private readonly DataAccessService _dataAccessService;

        public LocationService(DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region Country
        public List<CountryListItem> GetCountryListItems()
        {
            var entities = GetCountries();

            var items = entities.Select(country => country.ToCountryListItem()).ToList();

            return items;
        }

        public List<CountryOverview> GetCountryOverviews()
        {
            var entities = GetCountries();

            var items = entities.Select(country => country.ToCountryOverview()).ToList();

            SetCountryOverviewSecurity(items);
            return items;
        }

        internal List<Country> GetCountries()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var list = context.Countries.OrderBy(c => c.CountryName).ToList();

                return list;
            }
        }
        #endregion Country

        #region ElevationUnitType
        /// <summary>
        /// Gets the length unit type list items.
        /// </summary>
        /// <returns></returns>
        public List<ElevationUnitTypeListItem> GetElevationUnitTypeListItems()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var entities = context.ElevationUnitTypes.OrderBy(l => l.ElevationUnitTypeId).ToList();

                var items = entities.Select(t => t.ToElevationUnitTypeListItem()).ToList();

                return items;
            }
        }
        #endregion ElevationUnitType

        #region LengthUnitType
        /// <summary>
        /// Gets the length unit type list items.
        /// </summary>
        /// <returns></returns>
        public List<LengthUnitTypeListItem> GetLengthUnitTypeListItems()
        {
            var entities = GetLengthUnitTypes();

            var items = entities.Select(l => l.ToLengthUnitTypeListItem()).ToList();

            return items;
        }

        internal List<FLS.Server.Data.DbEntities.LengthUnitType> GetLengthUnitTypes()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var entities = context.LengthUnitTypes.OrderBy(l => l.LengthUnitTypeId);

                return entities.ToList();
            }
        }
        #endregion LengthUnitType

        #region LocationType
        /// <summary>
        /// Get locationTypes ordered by locationTypeCupId.
        /// </summary>
        /// <returns></returns>
        public List<LocationTypeListItem> GetLocationTypeListItems()
        {
            var entities = GetLocationTypes();

            var items = entities.Select(l => l.ToLocationTypeListItem()).ToList();

            return items;
        }

        /// <summary>
        /// Get locationTypes ordered by locationTypeCupId.
        /// </summary>
        /// <returns></returns>
        internal List<FLS.Server.Data.DbEntities.LocationType> GetLocationTypes()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var locationTypes = context.LocationTypes.OrderBy(l => l.LocationTypeCupId);

                return locationTypes.ToList();
            }
        }
        #endregion LocationType

        #region Location
        public List<LocationListItem> GetLocationListItems(bool airfieldsOnly)
        {
            var locations = GetLocations(airfieldsOnly);

            var items = locations.Select(location => location.ToLocationListItem()).ToList();

            return items;
        }

        public List<LocationOverview> GetLocationOverviews(bool airfieldsOnly = false)
        {
            var locations = GetLocations(airfieldsOnly);

            var locationOverviews = locations.Select(location => location.ToLocationOverview())
                .Where(loc => loc != null)
                .ToList();

            SetLocationOverviewSecurity(locationOverviews);
            return locationOverviews;
        }

        public PagedList<LocationOverview> GetPagedLocationOverviews(int? pageStart, int? pageSize, string orderBy, bool airfieldsOnly = false)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                IQueryable<Location> locations = context.Locations
                    .Include(Constants.Country)
                    .Include(Constants.LocationType)
                    .Include("LengthUnitType")
                    .Include("ElevationUnitType")
                    .OrderByPropertyNames(orderBy);

                if (airfieldsOnly)
                {
                    locations = locations.Where(l => l.LocationType.IsAirfield);
                }

                var pagedQuery = new PagedQuery<Location>(locations, pageStart, pageSize);

                var locationOverviews = pagedQuery.Items.ToList().Select(location => location.ToLocationOverview())
                .Where(loc => loc != null)
                .ToList();

                SetLocationOverviewSecurity(locationOverviews);

                var pagedList = new PagedList<LocationOverview>(locationOverviews, pagedQuery.PageStart,
                    pagedQuery.PageSize, pagedQuery.TotalRows);

                return pagedList;
            }
        }

        /// <summary>
        /// Gets the locations including the referenced objects <see cref="Country"/> and <see cref="FLS.Server.Data.DbEntities.LocationType"/>.
        /// </summary>
        /// <param name="airfieldsOnly">if set to <c>true</c> [airfields only].</param>
        /// <returns></returns>
        internal List<Location> GetLocations(bool airfieldsOnly = false)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                IQueryable<Location> locations = context.Locations
                        .Include(Constants.Country)
                        .Include(Constants.LocationType)
                        .Include("LengthUnitType")
                        .Include("ElevationUnitType")
                        .OrderBy(l => l.LocationName);

                if (airfieldsOnly)
                {
                    locations = locations.Where(l => l.LocationType.IsAirfield);
                }

                return locations.ToList();
            }
        }

        public LocationDetails GetLocationDetails(Guid locationId)
        {
            var location = GetLocation(locationId);
            var locationDetails = location.ToLocationDetails();
            SetLocationDetailsSecurity(locationDetails, location);
            return locationDetails;
        }

        internal Location GetLocation(Guid locationId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var location =
                    context.Locations.Include(Constants.Country)
                                 .Include(Constants.LocationType)
                                 .FirstOrDefault(l => l.LocationId == locationId);

                return location;
            }
        }

        public LocationDetails GetLocationDetailsByIcaoCode(string icaoCode)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var location =
                    context.Locations.Include(Constants.Country)
                                 .Include(Constants.LocationType)
                                 .FirstOrDefault(l => l.IcaoCode.ToLower() == icaoCode.ToLower());

                if (location == null)
                {
                    Logger.Warn($"Location with ICAO code {icaoCode} not found in database.");
                    throw new EntityNotFoundException("location");
                }

                return location.ToLocationDetails();
            }
        }

        /// <summary>
        /// Inserts the location.
        /// </summary>
        /// <param name="locationDetails">The location.</param>
        public void InsertLocationDetails(LocationDetails locationDetails)
        {
            var location = locationDetails.ToLocation();
            location.NotNull("Location");
            InsertLocation(location);
         
            location.ToLocationDetails(locationDetails);
        }

        /// <summary>
        /// Inserts the location.
        /// </summary>
        /// <param name="location">The location.</param>
        internal void InsertLocation(Location location)
        {
            location.EntityNotNull("Location", Guid.Empty);

            using (var context = _dataAccessService.CreateDbContext())
            {
                context.Locations.Add(location);
                
                context.SaveChanges();
            }
        }

        public void UpdateLocationDetails(LocationDetails currentLocationDetails)
        {
            currentLocationDetails.ArgumentNotNull("currentLocationDetails");

            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Locations.FirstOrDefault(l => l.LocationId == currentLocationDetails.LocationId);
                original.EntityNotNull("Location", currentLocationDetails.LocationId);

                currentLocationDetails.ToLocation(original);

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                }

                original.ToLocationDetails(currentLocationDetails);
            }
        }
        
        public void DeleteLocation(Guid locationId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var original = context.Locations.FirstOrDefault(l => l.LocationId == locationId);
                original.EntityNotNull("Location", locationId);

                context.Locations.Remove(original);
                context.SaveChanges();
            }
        }
        #endregion Location

        #region Security
        private void SetCountryOverviewSecurity(IEnumerable<CountryOverview> list)
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
                foreach (var overview in list)
                {
                    if (IsCurrentUserInRoleClubAdministrator ||
                        IsOwner(context.Countries.First(a => a.CountryId == overview.CountryId)))
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
        }

        private void SetLocationOverviewSecurity(IEnumerable<LocationOverview> list)
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
                foreach (var overview in list)
                {
                    if (IsCurrentUserInRoleClubAdministrator ||
                        IsOwner(context.Locations.First(a => a.LocationId == overview.LocationId)))
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
        }

        private void SetLocationDetailsSecurity(LocationDetails details, Location location)
        {
            if (details == null || location == null)
            {
                Logger.Error(string.Format("LocationDetails or location is null while trying to set security properties"));
                return;
            }

            if (CurrentAuthenticatedFLSUser == null)
            {
                Logger.Warn(string.Format("CurrentAuthenticatedFLSUser is NULL. Can't set correct security flags to the object."));
                details.CanUpdateRecord = false;
                details.CanDeleteRecord = false;
                return;
            }

            if (IsCurrentUserInRoleClubAdministrator || IsOwner(location))
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
