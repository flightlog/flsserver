using FLS.Common.Validators;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Data.DbEntities;

namespace FLS.Server.Data.Mapping
{
    public static class AircraftOperatingCounterMappingExtensions
    {

        #region AircraftOperatingCounter
        public static AircraftOperatingCounterOverview ToAircraftOperatingCounterOverview(this AircraftOperatingCounter entity, AircraftOperatingCounterOverview overview = null)
        {
            entity.ArgumentNotNull("entity");

            if (overview == null)
            {
                overview = new AircraftOperatingCounterOverview();
            }

            overview.AircraftOperatingCounterId = entity.AircraftOperatingCounterId;

            overview.AtDateTime = entity.AtDateTime;
            overview.TotalStarts = entity.TotalTowedGliderStarts.GetValueOrDefault() 
                + entity.TotalWinchLaunchStarts.GetValueOrDefault() 
                + entity.TotalSelfStarts.GetValueOrDefault();
            overview.FlightOperatingCounterInSeconds = entity.FlightOperatingCounterInSeconds;
            overview.EngineOperatingCounterInSeconds = entity.EngineOperatingCounterInSeconds;

            if (entity.Aircraft != null)
            {
                overview.Immatriculation = entity.Aircraft.Immatriculation;
            }
            
            return overview;
        }

        public static AircraftOperatingCounterDetails ToAircraftOperatingCounterDetails(this AircraftOperatingCounter entity, AircraftOperatingCounterDetails details = null)
        {
            entity.ArgumentNotNull("entity");

            if (details == null)
            {
                details = new AircraftOperatingCounterDetails();
            }

            details.AircraftOperatingCounterId = entity.AircraftOperatingCounterId;

            details.AircraftId = entity.AircraftId;
            details.AtDateTime = entity.AtDateTime;
            details.TotalTowedGliderStarts = entity.TotalTowedGliderStarts;
            details.TotalWinchLaunchStarts = entity.TotalWinchLaunchStarts;
            details.TotalSelfStarts = entity.TotalSelfStarts;
            details.FlightOperatingCounterInSeconds = entity.FlightOperatingCounterInSeconds;
            details.EngineOperatingCounterInSeconds = entity.EngineOperatingCounterInSeconds;
            details.NextMaintenanceAtFlightOperatingCounterInSeconds = entity.NextMaintenanceAtFlightOperatingCounterInSeconds;
            details.NextMaintenanceAtEngineOperatingCounterInSeconds = entity.NextMaintenanceAtEngineOperatingCounterInSeconds;

            return details;
        }

        public static AircraftOperatingCounter ToAircraftOperatingCounter(this AircraftOperatingCounterDetails details, AircraftOperatingCounter entity = null, bool overwriteAircraftOperatingCounterId = false)
        {
            details.ArgumentNotNull("details");

            if (entity == null)
            {
                entity = new AircraftOperatingCounter();
            }

            if (overwriteAircraftOperatingCounterId) entity.AircraftOperatingCounterId = details.AircraftOperatingCounterId;

            entity.AircraftId = details.AircraftId;
            entity.AtDateTime = details.AtDateTime;
            entity.TotalTowedGliderStarts = details.TotalTowedGliderStarts;
            entity.TotalWinchLaunchStarts = details.TotalWinchLaunchStarts;
            entity.TotalSelfStarts = details.TotalSelfStarts;
            entity.FlightOperatingCounterInSeconds = details.FlightOperatingCounterInSeconds;
            entity.EngineOperatingCounterInSeconds = details.EngineOperatingCounterInSeconds;
            entity.NextMaintenanceAtFlightOperatingCounterInSeconds = details.NextMaintenanceAtFlightOperatingCounterInSeconds;
            entity.NextMaintenanceAtEngineOperatingCounterInSeconds = details.NextMaintenanceAtEngineOperatingCounterInSeconds;

            return entity;
        }
        #endregion AircraftOperatingCounter

    }
}
