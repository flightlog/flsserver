﻿using System;
using System.Linq;
using FLS.Data.WebApi.Location;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.Tests.Extensions;
using Foundation.ObjectHydrator;
using ElevationUnitType = FLS.Server.Data.DbEntities.ElevationUnitType;
using LengthUnitType = FLS.Server.Data.DbEntities.LengthUnitType;
using LocationType = FLS.Server.Data.DbEntities.LocationType;

namespace FLS.Server.Tests.Helpers
{
    public class LocationHelper : BaseHelper
    {
        public LocationHelper(DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
        }

        public Country CreateCountry()
        {
            var hydrator = new Hydrator<Country>();
            var entity = hydrator.GetSingle();
            entity.RemoveMetadataInfo();
            return entity;
        }

        public Country GetFirstCountry()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Countries.FirstOrDefault();
            }
        }

        public Location GetFirstLocation()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Locations.FirstOrDefault();
            }
        }

        public Country GetCountry(string countryCode)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Countries.FirstOrDefault(c => c.CountryCodeIso2.ToUpper() == countryCode.ToUpper());
            }
        }

        public LocationType GetFirstLocationType()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.LocationTypes.FirstOrDefault();
            }
        }

        public LocationType GetLocationType(int locationTypeCupId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.LocationTypes.FirstOrDefault(c => c.LocationTypeCupId.Value == locationTypeCupId);
            }
        }

        public ElevationUnitType GetFirstElevationUnitType()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.ElevationUnitTypes.FirstOrDefault();
            }
        }

        public LengthUnitType GetFirstLengthUnitType()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.LengthUnitTypes.FirstOrDefault();
            }
        }

        public Location CreateLocation(Country country, LocationType locationType, ElevationUnitType elevationUnitType = null, LengthUnitType lengthUnitType = null)
        {
            if (country == null)
            {
                country = GetFirstCountry();
            }

            if (locationType == null)
            {
                locationType = GetFirstLocationType();
            }
            
            var hydrator = new Hydrator<Location>();
            var entity = hydrator.GetSingle();
            entity.RemoveMetadataInfo();
            entity.Country = null;
            entity.LocationType = null;
            entity.ElevationUnitType = null;
            entity.LengthUnitType = null;
            entity.CountryId = country.CountryId;
            entity.LocationTypeId = locationType.LocationTypeId;

            if (elevationUnitType != null)
            {
                entity.ElevationUnitTypeId = elevationUnitType.ElevationUnitTypeId;
                entity.Elevation = new Random().Next(-100, 2500);

            }
            else
            {
                entity.ElevationUnitType = null;
                entity.ElevationUnitTypeId = null;
            }

            if (lengthUnitType != null)
            {
                entity.RunwayLengthUnitType = lengthUnitType.LengthUnitTypeId;
                entity.RunwayLength = new Random().Next(200, 4500);
            }
            else
            {
                entity.LengthUnitType = null;
                entity.RunwayLengthUnitType = null;
            }

            return entity;
        }

        public LocationDetails CreateLocationDetails(Country country, LocationType locationType)
        {
            if (country == null)
            {
                country = GetFirstCountry();
            }

            if (locationType == null)
            {
                locationType = GetFirstLocationType();
            }

            var hydrator = new Hydrator<LocationDetails>();
            var entity = hydrator.GetSingle();
            entity.CountryId = country.CountryId;
            entity.LocationTypeId = locationType.LocationTypeId;
            return entity;
        }

        public LocationDetails CreateTestClubHomebaseLocationDetails()
        {
            var country = GetCountry("CH");
            var locationDetails = new LocationDetails()
            {
                CountryId = country.CountryId,
                LocationName = "Swiss Narromine Airfield",
                AirportFrequency = "123.45",
                Description = "Nice airfield",
                LocationShortName = "SNA",
                RunwayLength = 3450,
                LocationTypeId = GetLocationType((int)FLS.Data.WebApi.Location.LocationType.AirfieldSolid).LocationTypeId
            };

            return locationDetails;
        }

        public Location GetLocation(string locationIcaoCode)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Locations.FirstOrDefault(l => l.IcaoCode.ToLower() == locationIcaoCode.ToLower());
            }
        }
    }
}
