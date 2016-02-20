using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Flight;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLS.Server.TestInfrastructure
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
            gliderData.PilotPersonId = _personHelper.GetFirstPerson(clubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;

            return flightDetails;
        }

        public FlightDetails CreateGliderFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = _aircraftHelper.GetFirstGlider().AircraftId;
            gliderData.PilotPersonId = _personHelper.GetFirstPerson(clubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;
            flightDetails.StartType = (int) FLS.Server.Data.Enums.AircraftStartType.TowingByAircraft;
            return flightDetails;
        }

        public FlightDetails CreateOneSeatGliderFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = _aircraftHelper.GetFirstOneSeatGlider().AircraftId;
            gliderData.PilotPersonId = _personHelper.GetFirstPerson(clubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;
            flightDetails.StartType = (int)FLS.Server.Data.Enums.AircraftStartType.TowingByAircraft;
            return flightDetails;
        }

        public FlightDetails CreateDoubleSeatGliderFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = _aircraftHelper.GetFirstDoubleSeatGlider().AircraftId;
            gliderData.PilotPersonId = _personHelper.GetFirstPerson(clubId).PersonId;
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
            gliderData.PilotPersonId = _personHelper.GetFirstPerson(clubId).PersonId;
            gliderData.FlightCostBalanceType = 1;

            gliderData.PassengerPersonId = _personHelper.GetDifferentPerson(gliderData.PilotPersonId).PersonId;
            gliderData.FlightComment = "PAX flight";

            flightDetails.GliderFlightDetailsData = gliderData;

            return flightDetails;
        }

        //public GliderFlightDetailsData CreateGliderFlightDetailsData(string gliderImmatriculation, DateTime startTime, int flightDurationInMinutes = 90, string startLocationIcao = "LSZK", string ldgLocationIcao = "LSZK", bool stillFlying = false)
        //{
        //    var locations = LocationHelper.GetFirstLocation();
        //    var gliderAircrafts = GetGliderAircraftOverviews();
        //    var myClubFlightTableMasterData = GetMyClubsFlightTableMasterData();
        //    Assert.IsNotNull(myClubFlightTableMasterData);
        //    var gliderFlightTypes = myClubFlightTableMasterData.GliderFlightTypes;

        //    var glider = gliderAircrafts.FirstOrDefault(a => a.Immatriculation.ToUpper() == gliderImmatriculation.ToUpper());
        //    Assert.IsNotNull(glider);
        //    var startlocation = locations.FirstOrDefault(l => l.IcaoCode == startLocationIcao);
        //    Assert.IsNotNull(startlocation);

        //    var gliderPilot = GetGliderPilotPersonOverviews(true).LastOrDefault();
        //    var instructor = GetGliderInstructorPersonOverviews(true).FirstOrDefault();
        //    Assert.IsNotNull(gliderPilot);
        //    Assert.IsNotNull(instructor);
        //    Assert.IsNotNull(gliderFlightTypes);
        //    var gliderFlightType = gliderFlightTypes.FirstOrDefault();
        //    Assert.IsNotNull(gliderFlightType);

        //    var gliderFlightDetailsData = new GliderFlightDetailsData
        //        {
        //            AircraftId = glider.AircraftId,
        //            FlightComment = "Schoolflight",
        //            StartDateTime = startTime,
        //            PilotPersonId = gliderPilot.PersonId,
        //            StartLocationId = startlocation.LocationId,
        //            FlightTypeId = gliderFlightType.FlightTypeId
        //        };

        //    if (stillFlying == false)
        //    {
        //        var ldgLocation = locations.FirstOrDefault(l => l.IcaoCode == ldgLocationIcao);
        //        Assert.IsNotNull(ldgLocation);
        //        gliderFlightDetailsData.LdgLocationId = ldgLocation.LocationId;
        //        gliderFlightDetailsData.LdgDateTime = startTime.AddMinutes(flightDurationInMinutes);
        //    }

        //    return gliderFlightDetailsData;
        //}

        //public TowFlightDetailsData CreateTowFlightDetailsData(DateTime startTime, int towFlightTime = 8, string startLocationIcao = "LSZK", string ldgLocationIcao = "LSZK", string towAircraftImmatriculation = "HB-KCB")
        //{
        //    var locations = GetAllLocationOverviews();
        //    var towingAircrafts = GetTowingAircraftOverviews();
        //    var myClubFlightTableMasterData = GetMyClubsFlightTableMasterData();
        //    Assert.IsNotNull(myClubFlightTableMasterData);
        //    var towFlightTypes = myClubFlightTableMasterData.TowingFlightTypes;

        //    var startLocation = locations.FirstOrDefault(l => l.IcaoCode == startLocationIcao);
        //    Assert.IsNotNull(startLocation);
        //    var ldgLocation = locations.FirstOrDefault(l => l.IcaoCode == ldgLocationIcao);
        //    Assert.IsNotNull(ldgLocation);
        //    Assert.IsNotNull(towFlightTypes);

        //    var towPilot = GetTowingPilotPersonOverviews(true).FirstOrDefault();
        //    Assert.IsNotNull(towPilot);
        //    towingAircrafts = towingAircrafts.Where(a => a.IsTowingAircraft);
        //    var towingAircraft = towingAircrafts.FirstOrDefault();

        //    if (string.IsNullOrEmpty(towAircraftImmatriculation) == false)
        //    {
        //        towingAircraft = towingAircrafts.FirstOrDefault(a => a.Immatriculation == towAircraftImmatriculation);
        //    }

        //    Assert.IsNotNull(towingAircraft);
        //    var towFlightType = towFlightTypes.FirstOrDefault();
        //    Assert.IsNotNull(towFlightType);

        //    var towFlight = new TowFlightDetailsData
        //        {
        //            FlightComment = "test",
        //            AircraftId = towingAircraft.AircraftId,
        //            PilotPersonId = towPilot.PersonId,
        //            StartDateTime = startTime,
        //            LdgDateTime = startTime.AddMinutes(towFlightTime),
        //            StartLocationId = startLocation.LocationId,
        //            LdgLocationId = ldgLocation.LocationId,
        //            FlightTypeId = towFlightType.FlightTypeId
        //        };

        //    return towFlight;
        //}

        //public void InsertTowedGliderFlightDetails(DateTime startTime)
        //{
        //    var glider = Instance.GetFirstAircraftOverview(a => a.IsTowingOrWinchRequired && a.NrOfSeats == 1);
        //    var gliderFlight = Instance.CreateGliderFlightDetailsData(glider.Immatriculation, startTime);
        //    Assert.IsNotNull(gliderFlight);
        //    var towFlight = Instance.CreateTowFlightDetailsData(startTime);
        //    Assert.IsNotNull(towFlight);

        //    var flightDetails = new FlightDetails
        //    {
        //        GliderFlightDetailsData = gliderFlight,
        //        TowFlightDetailsData = towFlight,
        //        StartType = (int)AircraftStartType.TowingByAircraft
        //    };

        //    Instance.Insert(flightDetails);
        //}

        public void CreateFlightsForInvoicingTests(Guid clubId)
        {
            var startTime = new DateTime(2015, 1, 15, 10, 0, 0);
            var flightDetails = new FlightDetails();
            flightDetails.StartType = 1; //Towing GLider flight
            flightDetails.GliderFlightDetailsData = CreateSchoolGliderFlightDetailsData(clubId, "", startTime, 45);
            flightDetails.TowFlightDetailsData = CreateTowFlightDetailsData(clubId, "", startTime, 12);

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

        public GliderFlightDetailsData CreateSchoolGliderFlightDetailsData(Guid clubId, string gliderImmatriculation, DateTime startTime, int flightDurationInMinutes = 90)
        {
            var glider = _aircraftHelper.GetFirstGlider();
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
            var towingAircraft = _aircraftHelper.GetFirstTowingAircraft();
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
    }
}
