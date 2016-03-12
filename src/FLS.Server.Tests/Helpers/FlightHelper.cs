using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Flight;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.Tests.Helpers
{
    public class FlightHelper : BaseHelper
    {
        private readonly ClubHelper _clubHelper;
        private readonly LocationHelper _locationHelper;
        private readonly AircraftHelper _aircraftHelper;
        private readonly PersonHelper _personHelper;
        private readonly FlightService _flightService;

        public FlightHelper(DataAccessService dataAccessService, ClubHelper clubHelper, LocationHelper locationHelper,
            AircraftHelper aircraftHelper, PersonHelper personHelper, FlightService flightService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _clubHelper = clubHelper;
            _locationHelper = locationHelper;
            _aircraftHelper = aircraftHelper;
            _personHelper = personHelper;
            _flightService = flightService;
        }

        public List<StartType> GetStartTypes()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.StartTypes.ToList();
            }
        }

        public StartType GetFirstLengthUnitType()
        {
            return GetStartTypes().FirstOrDefault();
        }

        public FlightDetails CreateMinimalGliderFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = _aircraftHelper.GetFirstGlider().AircraftId;
            gliderData.PilotPersonId = _personHelper.GetFirstGliderPilotPerson(clubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;

            return flightDetails;
        }

        public FlightDetails CreateGliderFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = _aircraftHelper.GetFirstGlider().AircraftId;
            gliderData.PilotPersonId = _personHelper.GetFirstGliderPilotPerson(clubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;
            flightDetails.StartType = (int) FLS.Server.Data.Enums.AircraftStartType.TowingByAircraft;
            return flightDetails;
        }

        public FlightDetails CreateOneSeatGliderFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = _aircraftHelper.GetFirstOneSeatGlider().AircraftId;
            gliderData.PilotPersonId = _personHelper.GetFirstGliderPilotPerson(clubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;
            flightDetails.StartType = (int)FLS.Server.Data.Enums.AircraftStartType.TowingByAircraft;
            return flightDetails;
        }

        public FlightDetails CreateDoubleSeatGliderFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = _aircraftHelper.GetFirstDoubleSeatGlider().AircraftId;
            gliderData.PilotPersonId = _personHelper.GetFirstGliderPilotPerson(clubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;
            flightDetails.StartType = (int)FLS.Server.Data.Enums.AircraftStartType.TowingByAircraft;
            return flightDetails;
        }

        public FlightDetails CreateFailedFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            flightDetails.StartType = 1;

            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = _aircraftHelper.GetFirstGlider().AircraftId;
            gliderData.PilotPersonId = _personHelper.GetFirstGliderPilotPerson(clubId).PersonId;
            gliderData.FlightCostBalanceType = 1;

            gliderData.PassengerPersonId = _personHelper.GetDifferentPerson(gliderData.PilotPersonId).PersonId;
            gliderData.FlightComment = "PAX flight";

            flightDetails.GliderFlightDetailsData = gliderData;

            return flightDetails;
        }

        public void CreateFlightsForInvoicingTests(Guid clubId)
        {
            var startTime = DateTime.Today.AddMonths(-1).AddHours(10);
            var flightDetails = new FlightDetails();
            flightDetails.StartType = 1; //Towing GLider flight
            flightDetails.GliderFlightDetailsData = CreateSchoolGliderFlightDetailsData(clubId, "HB-1824", startTime, 45);
            flightDetails.TowFlightDetailsData = CreateTowFlightDetailsData(clubId, "HB-KCB", startTime, 12);

            _flightService.InsertFlightDetails(flightDetails);

            Assert.IsTrue(flightDetails.FlightId.IsValid());

            var flight = _flightService.GetFlight(flightDetails.FlightId);

            Assert.IsTrue(flight.FlightId.IsValid());

            using (var context = DataAccessService.CreateDbContext())
            {
                context.Flights.Attach(flight);
                flight.FlightStateId = (int)FLS.Data.WebApi.Flight.FlightState.Locked;

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                }
            }

        }

        public Flight CreateGliderFlight(Guid clubId, DateTime startTime)
        {
            var flightDetails = new FlightDetails();
            flightDetails.StartType = 1; //Towing GLider flight
            flightDetails.GliderFlightDetailsData = CreateSchoolGliderFlightDetailsData(clubId, "HB-1824", startTime, 45);
            flightDetails.TowFlightDetailsData = CreateTowFlightDetailsData(clubId, "HB-KCB", startTime, 12);

            _flightService.InsertFlightDetails(flightDetails);

            Assert.IsTrue(flightDetails.FlightId.IsValid());

            var flight = _flightService.GetFlight(flightDetails.FlightId);

            Assert.IsTrue(flight.FlightId.IsValid());

            return flight;
        }

        public GliderFlightDetailsData CreateSchoolGliderFlightDetailsData(Guid clubId, string immatriculation, DateTime startTime, int flightDurationInMinutes = 90)
        {
            Aircraft glider = null;

            if (string.IsNullOrEmpty(immatriculation) == false)
            {
                glider = _aircraftHelper.GetAircraft(immatriculation);
            }

            if (glider == null)
            {
                glider = _aircraftHelper.GetFirstTowingAircraft();
            }

            Assert.IsNotNull(glider);
            var startlocation = _locationHelper.GetFirstLocation();
            Assert.IsNotNull(startlocation);

            var gliderPilot = _personHelper.GetFirstGliderPilotPerson(clubId);
            var instructor = _personHelper.GetFirstGliderInstructorPerson(clubId);
            Assert.IsNotNull(gliderPilot);
            Assert.IsNotNull(instructor);
            var gliderFlightType = _clubHelper.GetFirstInstructorRequiredGliderFlightType(clubId);
            Assert.IsNotNull(gliderFlightType);

            var gliderFlightDetailsData = new GliderFlightDetailsData
                {
                    AircraftId = glider.AircraftId,
                    FlightComment = "Schoolflight",
                    StartDateTime = startTime,
                    LdgDateTime = startTime.AddMinutes(flightDurationInMinutes),
                    PilotPersonId = gliderPilot.PersonId,
                    StartLocationId = startlocation.LocationId,
                    LdgLocationId =  startlocation.LocationId,
                    FlightTypeId = gliderFlightType.FlightTypeId,
                    InstructorPersonId = instructor.PersonId
                };

            return gliderFlightDetailsData;
        }

        public TowFlightDetailsData CreateTowFlightDetailsData(Guid clubId, string immatriculation, DateTime startTime, int flightDurationInMinutes = 12)
        {
            Aircraft towingAircraft = null;

            if (string.IsNullOrEmpty(immatriculation) == false)
            {
                towingAircraft = _aircraftHelper.GetAircraft(immatriculation);
            }

            if (towingAircraft == null)
            {
                towingAircraft = _aircraftHelper.GetFirstTowingAircraft();
            }

            Assert.IsNotNull(towingAircraft);
            var startlocation = _locationHelper.GetFirstLocation();
            Assert.IsNotNull(startlocation);

            var towingPilot = _personHelper.GetFirstTowingPilotPerson(clubId);
            Assert.IsNotNull(towingPilot);
            var towingFlightType = _clubHelper.GetFirstTowingFlightType(clubId);
            Assert.IsNotNull(towingFlightType);

            var towFlightDetailsData = new TowFlightDetailsData
            {
                AircraftId = towingAircraft.AircraftId,
                FlightComment = "TowFlight",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(flightDurationInMinutes),
                PilotPersonId = towingPilot.PersonId,
                StartLocationId = startlocation.LocationId,
                LdgLocationId = startlocation.LocationId,
                FlightTypeId = towingFlightType.FlightTypeId
            };

            return towFlightDetailsData;
        }

        public Flight GetFlight(Guid flightId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Flights.FirstOrDefault(f => f.FlightId == flightId);
            }
        }

        public List<FlightCrew> GetFlightCrew(Guid flightId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightCrews.Where(f => f.FlightId == flightId).ToList();
            }
        }

        public FlightDetails CreateTowedGliderFlightDetails(Guid clubId)
        {
            var startTime = DateTime.Now;
            var flightDetails = new FlightDetails();
            flightDetails.StartType = 1; //Towing GLider flight
            flightDetails.GliderFlightDetailsData = CreateSchoolGliderFlightDetailsData(clubId, "HB-1824", startTime, 45);
            flightDetails.TowFlightDetailsData = CreateTowFlightDetailsData(clubId, "HB-KCB", startTime, 12);

            return flightDetails;
        }

        public FlightDetails CreateMotorFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            var motorFlightData = new MotorFlightDetailsData();
            motorFlightData.AircraftId = _aircraftHelper.GetFirstTowingAircraft().AircraftId;
            motorFlightData.PilotPersonId = _personHelper.GetFirstTowingPilotPerson(clubId).PersonId;
            motorFlightData.StartLocationId = _locationHelper.GetFirstLocation().LocationId;
            motorFlightData.LdgLocationId = motorFlightData.StartLocationId;
            flightDetails.MotorFlightDetailsData = motorFlightData;
            flightDetails.StartType = (int)FLS.Server.Data.Enums.AircraftStartType.MotorFlightStart;
            return flightDetails;
        }
    }
}
