using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Club;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.TestInfrastructure.Extensions;
using Foundation.ObjectHydrator;

namespace FLS.Server.TestInfrastructure
{
    public class ClubHelper : BaseHelper
    {
        public ClubHelper(DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
        }

        public Club CreateClub()
        {
            var hydrator = new Hydrator<Club>();
            var club = hydrator.GetSingle();
            club.RemoveMetadataInfo();
            return club;
        }

        public FlightTypeDetails CreateFlightType(Guid clubId)
        {
            var flightType = new FlightTypeDetails
            {
                ClubId = clubId,
                FlightTypeName = "Test Flight type @ " + DateTime.Now.Ticks,
                FlightCode = DateTime.Now.ToShortTimeString(),
                IsForGliderFlights = true,
                IsCheckFlight = true
            };
            return flightType;
        }

        public FlightType GetFirstGliderFlightType(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForGliderFlights && c.IsSummarizedSystemFlight == false);
            }
        }

        public FlightType GetFirstSoloGliderFlightType(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForGliderFlights && c.IsSummarizedSystemFlight == false
                    && c.IsSoloFlight);
            }
        }

        public FlightType GetFirstPassengerGliderFlightType(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForGliderFlights && c.IsSummarizedSystemFlight == false
                    && c.IsPassengerFlight);
            }
        }

        public FlightType GetFirstObserverPilotOrInstructorRequiredGliderFlightType(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForGliderFlights && c.IsSummarizedSystemFlight == false
                    && c.ObserverPilotOrInstructorRequired);
            }
        }

        public FlightType GetFirstInstructorRequiredGliderFlightType(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForGliderFlights && c.IsSummarizedSystemFlight == false
                    && c.InstructorRequired);
            }
        }

        public FlightType GetFirstTowingFlightType(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForTowFlights && c.IsSummarizedSystemFlight == false);
            }
        }

        public List<FlightType> GetFlightTypes(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.Where(c => c.ClubId == clubId).ToList();
            }
        }
    }
}
