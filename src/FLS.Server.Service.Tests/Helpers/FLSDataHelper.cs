using System;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Club;
using FLS.Data.WebApi.Person;
using FLS.Server.Data.DbEntities;
using Foundation.ObjectHydrator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using AircraftType = FLS.Data.WebApi.Aircraft.AircraftType;

namespace FLS.Server.Service.Tests.Helpers
{
    public class FLSDataHelper
    {
        private static FLSDataHelper _instance = null;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        public static FLSDataHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FLSDataHelper();
                }

                return _instance;
            }
        }

        private FLSDataHelper()
        {
        }
        
        public AircraftDetails CreateGliderAircraftDetails(int nrOfSeats, AircraftType aircraftType = AircraftType.Glider,
            bool isTowingOrWinchRequired = true, bool isTowingstartAllowed = true, bool isWinchstartAllowed = true)
        {
            Assert.IsTrue(nrOfSeats > 0);
            var aircraftDetails = new AircraftDetails
            {
                AircraftModel = "Test-Aircraft-Model",
                AircraftType = (int)aircraftType,
                Comment = "Test-Glider " + DateTime.Now.ToShortTimeString(),
                CompetitionSign = "TT",
                DaecIndex = 99,
                FLARMId = "ID" + DateTime.Now.Ticks,
                Immatriculation = GetAvailableGliderImmatriculation(),
                IsTowingOrWinchRequired = isTowingOrWinchRequired,
                IsTowingstartAllowed = isTowingstartAllowed,
                IsWinchstartAllowed = isWinchstartAllowed,
                ManufacturerName = "Test-Manufacturer",
                NrOfSeats = nrOfSeats
            };

            return aircraftDetails;
        }

        public AircraftDetails CreateTowingAircraftDetails()
        {
            var aircraftDetails = new AircraftDetails
            {
                AircraftModel = "Test-Towing-Aircraft-Model",
                AircraftType = (int)AircraftType.MotorAircraft,
                Comment = "Test-Towing " + DateTime.Now.ToShortTimeString(),
                FLARMId = "ID" + DateTime.Now.Ticks,
                Immatriculation = GetAvailableMotorImmatriculation(),
                IsTowingOrWinchRequired = false,
                IsTowingstartAllowed = false,
                IsWinchstartAllowed = false,
                IsTowingAircraft = true,
                ManufacturerName = "Test-Manufacturer",
                NrOfSeats = 4
            };

            return aircraftDetails;
        }

        public string GetAvailableGliderImmatriculation()
        {
            var aircraftService = new AircraftService();
            Aircraft aircraft;
            Random random = new Random();
            string immatriculation;

            do
            {
                var number = random.Next(1, 9999);
                immatriculation = "HB-" + number;
                aircraft = aircraftService.GetAircraft(immatriculation);
            } while (aircraft != null);

            return immatriculation;
        }

        public string GetAvailableMotorImmatriculation()
        {
            var aircraftService = new AircraftService();
            Aircraft aircraft;
            var hydrator = new Hydrator<Aircraft>().WithLastName(f => f.Immatriculation);
            string random;
            string immatriculation;

            do
            {
                var ac = hydrator.GetSingle();
                random = ac.Immatriculation;
                if (random.Length > 4)
                {
                    random = random.Substring(0, 4);
                }

                immatriculation = "HB-" + random.ToUpper();
                aircraft = aircraftService.GetAircraft(immatriculation);
            } while (aircraft != null);

            return immatriculation;
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

        //public GliderFlightDetailsData CreateGliderFlightDetailsData(string gliderImmatriculation, DateTime startTime, int flightDurationInMinutes = 90, string startLocationIcao = "LSZK", string ldgLocationIcao = "LSZK", bool stillFlying = false)
        //{
        //    var locations = GetAllLocationOverviews();
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



        public PersonDetails CreatePersonDetails(Guid clubId, Guid countryId)
        {
            var hydrator = new Hydrator<PersonDetails>();
            var personDetails = hydrator.GetSingle();

            if (personDetails.LicenseNumber.Length > 20) personDetails.LicenseNumber = personDetails.LicenseNumber.Substring(0, 20);
            personDetails.CountryId = countryId;
            personDetails.PersonId = Guid.Empty;
            if (personDetails.Lastname.Length > 80) personDetails.Lastname = personDetails.Lastname.Substring(0, 80);
            personDetails.Lastname = personDetails.Lastname + DateTime.Now.Ticks;
            if (personDetails.Lastname.Length > 100) personDetails.Lastname = personDetails.Lastname.Substring(0, 100);
            if (personDetails.FaxNumber.Length > 10) personDetails.FaxNumber = personDetails.FaxNumber.Substring(0, 10);

            var ownClubData = new ClubRelatedPersonDetails
                {
                    ClubId = clubId,
                    MemberNumber = DateTime.Now.Ticks.ToString()
                };

            personDetails.OwnClubRelatedPersonDetails = ownClubData;

            return personDetails;
        }
    }
}
