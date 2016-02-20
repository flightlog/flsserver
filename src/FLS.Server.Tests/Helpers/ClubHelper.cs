using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Club;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.Tests.Extensions;
using Foundation.ObjectHydrator;

namespace FLS.Server.Tests.Helpers
{
    public class ClubHelper : BaseHelper
    {
        private readonly LocationHelper _locationHelper;

        public ClubHelper(DataAccessService dataAccessService, IdentityService identityService, 
            LocationHelper locationHelper)
            : base(dataAccessService, identityService)
        {
            _locationHelper = locationHelper;
        }

        public ClubDetails CreateTestClubDetails()
        {
            var clubDetails = new ClubDetails()
            {
                ClubName = "TestClub1",
                ClubKey = "TestClub1",
                Address = "Hauptstrasse 1",
                ZipCode = "9999",
                City = "New City",
                ContactName = "Nobody",
                EmailAddress = "test@glider-fls.ch",
                WebPage = "www.glider-fls.ch",
                SendAircraftStatisticReportTo = "test@glider-fls.ch",
                SendInvoiceReportsTo = "test@glider-fls.ch",
                SendPlanningDayInfoMailTo = "test@glider-fls.ch",
                CountryId = _locationHelper.GetCountry("CH").CountryId
            };

            return clubDetails;
        }

        public FlightTypeDetails CreateCharterFlightTypeDetails()
        {
            var flightType = new FlightTypeDetails
            {
                FlightTypeName = "Charter-Flug",
                FlightCode = "100",
                IsForGliderFlights = true,
                IsForMotorFlights = true,
                IsForTowFlights = false,
                IsCheckFlight = false,
                ObserverPilotOrInstructorRequired = false,
                InstructorRequired = false,
                IsFlightCostBalanceSelectable = true,
                IsPassengerFlight = false,
                IsCouponNumberRequired = false,
                IsSoloFlight = false,
            };
            return flightType;
        }

        public FlightTypeDetails CreatePrivateCharterFlightTypeDetails()
        {
            var flightType = new FlightTypeDetails
            {
                FlightTypeName = "Privater Charter-Flug",
                FlightCode = "110",
                IsForGliderFlights = true,
                IsForMotorFlights = true,
                IsForTowFlights = false,
                IsCheckFlight = false,
                ObserverPilotOrInstructorRequired = false,
                InstructorRequired = false,
                IsFlightCostBalanceSelectable = true,
                IsPassengerFlight = false,
                IsCouponNumberRequired = false,
                IsSoloFlight = false,
            };
            return flightType;
        }

        public FlightTypeDetails CreateGliderCheckFlightTypeDetails()
        {
            var flightType = new FlightTypeDetails
            {
                FlightTypeName = "Checkflug",
                FlightCode = "200",
                IsForGliderFlights = true,
                IsForMotorFlights = false,
                IsForTowFlights = false,
                IsCheckFlight = true,
                ObserverPilotOrInstructorRequired = false,
                InstructorRequired = true,
                IsFlightCostBalanceSelectable = true,
                IsPassengerFlight = false,
                IsCouponNumberRequired = false,
                IsSoloFlight = false,
            };
            return flightType;
        }

        public FlightTypeDetails CreateGliderTraineeFlightTypeDetails()
        {
            var flightType = new FlightTypeDetails
            {
                FlightTypeName = "Schulungsflug",
                FlightCode = "210",
                IsForGliderFlights = true,
                IsForMotorFlights = false,
                IsForTowFlights = false,
                IsCheckFlight = false,
                ObserverPilotOrInstructorRequired = false,
                InstructorRequired = true,
                IsFlightCostBalanceSelectable = true,
                IsPassengerFlight = false,
                IsCouponNumberRequired = false,
                IsSoloFlight = false,
            };
            return flightType;
        }

        public FlightTypeDetails CreateGliderUpgradeFlightTypeDetails()
        {
            var flightType = new FlightTypeDetails
            {
                FlightTypeName = "Weiterbildungsflug",
                FlightCode = "220",
                IsForGliderFlights = true,
                IsForMotorFlights = false,
                IsForTowFlights = false,
                IsCheckFlight = false,
                ObserverPilotOrInstructorRequired = true,
                InstructorRequired = false,
                IsFlightCostBalanceSelectable = true,
                IsPassengerFlight = false,
                IsCouponNumberRequired = false,
                IsSoloFlight = false,
            };
            return flightType;
        }

        public FlightTypeDetails CreateGliderPassengerWithCouponFlightTypeDetails()
        {
            var flightType = new FlightTypeDetails
            {
                FlightTypeName = "Passagierflug mit Gutschein",
                FlightCode = "300",
                IsForGliderFlights = true,
                IsForMotorFlights = false,
                IsForTowFlights = false,
                IsCheckFlight = false,
                ObserverPilotOrInstructorRequired = false,
                InstructorRequired = false,
                IsFlightCostBalanceSelectable = true,
                IsPassengerFlight = true,
                IsCouponNumberRequired = true,
                IsSoloFlight = false,
            };
            return flightType;
        }

        public FlightTypeDetails CreateGliderPassengerWithoutCouponFlightTypeDetails()
        {
            var flightType = new FlightTypeDetails
            {
                FlightTypeName = "Passagierflug ohne Gutschein",
                FlightCode = "310",
                IsForGliderFlights = true,
                IsForMotorFlights = false,
                IsForTowFlights = false,
                IsCheckFlight = false,
                ObserverPilotOrInstructorRequired = false,
                InstructorRequired = false,
                IsFlightCostBalanceSelectable = true,
                IsPassengerFlight = true,
                IsCouponNumberRequired = false,
                IsSoloFlight = false,
            };
            return flightType;
        }

        public FlightTypeDetails CreateTowFlightTypeDetails()
        {
            var flightType = new FlightTypeDetails
            {
                FlightTypeName = "Schleppflug",
                FlightCode = "800",
                IsForGliderFlights = false,
                IsForMotorFlights = false,
                IsForTowFlights = true,
                IsCheckFlight = false,
                ObserverPilotOrInstructorRequired = false,
                InstructorRequired = false,
                IsFlightCostBalanceSelectable = false,
                IsPassengerFlight = false,
                IsCouponNumberRequired = false,
                IsSoloFlight = false,
            };
            return flightType;
        }

        public Club CreateClub()
        {
            var hydrator = new Hydrator<Club>();
            var club = hydrator.GetSingle();
            club.RemoveMetadataInfo();
            return club;
        }

        public ClubDetails CreateClubDetails()
        {
            var hydrator = new Hydrator<ClubDetails>();
            var club = hydrator.GetSingle();

            club.CountryId = _locationHelper.GetFirstCountry().CountryId;

            club.HomebaseId = _locationHelper.GetFirstLocation().LocationId;
            club.DefaultStartType = null;
            club.DefaultGliderFlightTypeId = null; //can only be null during creation
            club.DefaultMotorFlightTypeId = null; //can only be null during creation
            club.DefaultTowFlightTypeId = null;    //can only be null during creation

            using (var context = DataAccessService.CreateDbContext())
            {
                var clubExists = context.Clubs.Any(c => c.ClubKey == club.ClubKey);
                var baseClubKey = club.ClubKey;
                if (baseClubKey.Length > 8) baseClubKey = baseClubKey.Substring(1, 8);

                while (clubExists)
                {
                    club.ClubKey = baseClubKey + DateTime.UtcNow.Ticks.GetHashCode();
                    clubExists = context.Clubs.Any(c => c.ClubKey == club.ClubKey);
                }
            }

            return club;
        }

        public FlightTypeDetails CreateFlightType()
        {
            var flightType = new FlightTypeDetails
            {
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
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForGliderFlights);
            }
        }

        public FlightType GetFirstSoloGliderFlightType(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForGliderFlights
                    && c.IsSoloFlight);
            }
        }

        public FlightType GetFirstPassengerGliderFlightType(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForGliderFlights
                    && c.IsPassengerFlight);
            }
        }

        public FlightType GetFirstObserverPilotOrInstructorRequiredGliderFlightType(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForGliderFlights
                    && c.ObserverPilotOrInstructorRequired);
            }
        }

        public FlightType GetFirstInstructorRequiredGliderFlightType(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForGliderFlights
                    && c.InstructorRequired);
            }
        }

        public FlightType GetFirstTowingFlightType(Guid clubId, bool instructorRequired = false)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId 
                    && c.IsForTowFlights
                    && c.InstructorRequired == instructorRequired);
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
