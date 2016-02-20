using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.Club;
using FLS.Data.WebApi.Location;
using FLS.Data.WebApi.Person;
using FLS.Data.WebApi.User;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Helpers;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AircraftType = FLS.Data.WebApi.Aircraft.AircraftType;

namespace FLS.Server.Tests.Infrastructure.WebApi
{
    [TestClass]
    public class AdditionalTestDataController : BaseAuthenticatedTests
    {
        private ClubHelper _clubHelper;
        private FlightHelper _flightHelper;
        private UserHelper _userHelper;
        private LocationHelper _locationHelper;
        private PersonHelper _personHelper;
        private AircraftHelper _aircraftHelper;

        public void Initialize()
        {
            _clubHelper = UnityContainer.Resolve<ClubHelper>();
            _flightHelper = UnityContainer.Resolve<FlightHelper>();
            _userHelper = UnityContainer.Resolve<UserHelper>();
            _locationHelper = UnityContainer.Resolve<LocationHelper>();
            _personHelper = UnityContainer.Resolve<PersonHelper>();
            _aircraftHelper = UnityContainer.Resolve<AircraftHelper>();
        }
        
        protected override string Uri
        {
            get { return RoutePrefix; }
        }

        protected override string RoutePrefix
        {
            get { return "/api/v1/clubs"; }
        }

        public void SetupFullTestClub()
        {
            Setup();
            Initialize();
            //Setup new Club with Club Admin User
            LoginAsSystemAdmin();
            var clubDetails = InsertTestClubDetailsWebApi();
            var clubAdminUserDetails = InsertTestClubAdminUserWebApi(clubDetails.ClubId);
            var clubUserAdminPassword = "TestClubAdminUserPassword";
            _userHelper.SetUser(TestConfigurationSettings.Instance.TestSystemAdminUsername);
            _userHelper.SetUsersPassword(clubAdminUserDetails.UserId, clubUserAdminPassword);

            //Setup new basic club settings and FlightTypes for TestClub
            Login(clubAdminUserDetails.UserName, clubUserAdminPassword);
            InsertFlightTypesWebApi();

            var homebaseDetails = InsertTestClubHomebase();
            clubDetails = SetClubsDefaults(clubDetails, homebaseDetails);

            //Setup new persons
            InsertPersons(clubDetails);

            //Setup new Aircrafts
            InsertAircrafts(clubDetails);

            Teardown();
        }

        private void InsertAircrafts(ClubDetails clubDetails)
        {
            var glider = _aircraftHelper.CreateGliderAircraftDetails(1);
            glider.AircraftOwnerClubId = clubDetails.ClubId;
            InsertAircraftWebApi(glider);

            glider = _aircraftHelper.CreateGliderAircraftDetails(2);
            glider.AircraftOwnerClubId = clubDetails.ClubId;
            InsertAircraftWebApi(glider);

            glider = _aircraftHelper.CreateGliderAircraftDetails(1, AircraftType.GliderWithMotor);
            glider.AircraftOwnerClubId = clubDetails.ClubId;
            InsertAircraftWebApi(glider);

            glider = _aircraftHelper.CreateGliderAircraftDetails(1, AircraftType.GliderWithMotor,
                isTowingOrWinchRequired: false);
            var persons = _personHelper.GetPersons(clubDetails.ClubId);
            var person = persons.First(p => p.HasGliderPilotLicence);
            Assert.IsNotNull(person);
            glider.AircraftOwnerPersonId = person.PersonId;
            InsertAircraftWebApi(glider);

            var towingAircraft = _aircraftHelper.CreateTowingAircraftDetails();
            towingAircraft.AircraftOwnerClubId = clubDetails.ClubId;
            InsertAircraftWebApi(towingAircraft);

            var motorAircraft = _aircraftHelper.CreateMotorAircraftDetails();
            motorAircraft.AircraftOwnerClubId = clubDetails.ClubId;
            InsertAircraftWebApi(motorAircraft);
        }

        public AircraftDetails InsertAircraftWebApi(AircraftDetails aircraftDetails)
        {
            var response = PostAsync(aircraftDetails, "/api/v1/aircrafts").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<AircraftDetails>(response);
            Assert.IsNotNull(aircraftDetails);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            return responseDetails;
        }

        private void InsertPersons(ClubDetails clubDetails)
        {
            var personDetails = _personHelper.CreateGliderPilotPersonDetails(clubDetails.CountryId);
            InsertPilotPersonDetailsWebApi(personDetails);

            personDetails = _personHelper.CreateGliderInstructorPersonDetails(clubDetails.CountryId);
            InsertPilotPersonDetailsWebApi(personDetails);

            personDetails = _personHelper.CreateGliderTraineePersonDetails(clubDetails.CountryId);
            InsertPilotPersonDetailsWebApi(personDetails);

            personDetails = _personHelper.CreateTowPilotPersonDetails(clubDetails.CountryId);
            InsertPilotPersonDetailsWebApi(personDetails);

            personDetails = _personHelper.CreateWinchOperatorPilotPersonDetails(clubDetails.CountryId);
            InsertPilotPersonDetailsWebApi(personDetails);
        }

        public PilotPersonDetails InsertPilotPersonDetailsWebApi(PilotPersonDetails pilotPersonDetails)
        {
            var response = PostAsync(pilotPersonDetails, "/api/v1/persons").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<PilotPersonDetails>(response);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
            return responseDetails;
        }

        private ClubDetails SetClubsDefaults(ClubDetails clubDetails, LocationDetails homebaseDetails)
        {
            clubDetails.HomebaseId = homebaseDetails.LocationId;
            var flightTypes = _clubHelper.GetFlightTypes(clubDetails.ClubId);
            clubDetails.DefaultGliderFlightTypeId = flightTypes.First(f => f.FlightCode == "100").FlightTypeId;
            clubDetails.DefaultTowFlightTypeId = flightTypes.First(f => f.IsForTowFlights).FlightTypeId;
            clubDetails.DefaultMotorFlightTypeId = flightTypes.First(f => f.IsForMotorFlights).FlightTypeId;
            clubDetails.DefaultStartType = (int)AircraftStartType.TowingByAircraft;
            
            var response = PutAsync(clubDetails, "/api/v1/clubs/" + clubDetails.ClubId).Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<ClubDetails>(response);
            return responseDetails;
        }

        private LocationDetails InsertTestClubHomebase()
        {
            var locationDetails = _locationHelper.CreateTestClubHomebaseLocationDetails();
            var response = PostAsync(locationDetails, "/api/v1/locations").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<LocationDetails>(response);
            Assert.IsNotNull(responseDetails);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            return responseDetails;
        }

        #region Private methods
        public ClubDetails InsertTestClubDetailsWebApi()
        {
            var clubDetails = _clubHelper.CreateTestClubDetails();

            var response = PostAsync(clubDetails, "/api/v1/clubs").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<ClubDetails>(response);
            Assert.IsNotNull(responseDetails);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
            Assert.AreEqual(clubDetails.HomebaseId, responseDetails.HomebaseId);
            Assert.AreEqual(clubDetails.DefaultStartType, responseDetails.DefaultStartType);
            Assert.AreEqual(clubDetails.DefaultGliderFlightTypeId, responseDetails.DefaultGliderFlightTypeId);
            Assert.AreEqual(clubDetails.DefaultMotorFlightTypeId, responseDetails.DefaultMotorFlightTypeId);
            Assert.AreEqual(clubDetails.DefaultTowFlightTypeId, responseDetails.DefaultTowFlightTypeId);

            return responseDetails;
        }

        public UserDetails InsertTestClubAdminUserWebApi(Guid clubId)
        {
            var userDetails = _userHelper.CreateTestClubAdminUserDetails(clubId);
            var response = PostAsync(userDetails, "/api/v1/users").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<UserDetails>(response);
            Assert.IsNotNull(responseDetails);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));
            
            return responseDetails;
        }

        public void InsertFlightTypesWebApi()
        {
            var flightTypeDetails = _clubHelper.CreateCharterFlightTypeDetails();
            InsertFlightTypesWebApi(flightTypeDetails);

            flightTypeDetails = _clubHelper.CreatePrivateCharterFlightTypeDetails();
            InsertFlightTypesWebApi(flightTypeDetails);

            flightTypeDetails = _clubHelper.CreateGliderCheckFlightTypeDetails();
            InsertFlightTypesWebApi(flightTypeDetails);

            flightTypeDetails = _clubHelper.CreateGliderTraineeFlightTypeDetails();
            InsertFlightTypesWebApi(flightTypeDetails);

            flightTypeDetails = _clubHelper.CreateGliderUpgradeFlightTypeDetails();
            InsertFlightTypesWebApi(flightTypeDetails);

            flightTypeDetails = _clubHelper.CreateGliderPassengerWithCouponFlightTypeDetails();
            InsertFlightTypesWebApi(flightTypeDetails);

            flightTypeDetails = _clubHelper.CreateGliderPassengerWithoutCouponFlightTypeDetails();
            InsertFlightTypesWebApi(flightTypeDetails);

            flightTypeDetails = _clubHelper.CreateTowFlightTypeDetails();
            InsertFlightTypesWebApi(flightTypeDetails);

        }

        public FlightTypeDetails InsertFlightTypesWebApi(FlightTypeDetails flightTypeDetails)
        {
            Assert.IsNotNull(flightTypeDetails, "flightTypeDetails != null");
            var response = PostAsync(flightTypeDetails, "/api/v1/flighttypes").Result;

            Assert.IsTrue(response.IsSuccessStatusCode, string.Format("Error with Status Code: {0}", response.StatusCode));
            var responseDetails = ConvertToModel<FlightTypeDetails>(response);
            Assert.IsNotNull(responseDetails);
            Assert.IsTrue(responseDetails.Id.IsValid(), string.Format("Primary key not set/mapped after insert or update. Entity-Info: {0}", responseDetails));

            return responseDetails;
        }
        #endregion Private methods
    }
}
