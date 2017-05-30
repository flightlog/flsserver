using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Aircraft;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.Club;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Location;
using FLS.Data.WebApi.Person;
using FLS.Data.WebApi.PlanningDay;
using FLS.Data.WebApi.Resources;
using FLS.Data.WebApi.User;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Mapping;
using FLS.Server.Interfaces;
using FLS.Server.Service;
using FLS.Server.Service.Email;
using FLS.Server.Service.Identity;
using FLS.Server.TestInfrastructure;
using FLS.Server.Tests.Extensions;
using FLS.Server.Tests.Helpers;
using FLS.Server.Tests.Infrastructure;
using FLS.Server.Tests.Mocks.Services;
using FLS.Server.WebApi;
using FLS.Server.WebApi.ActionFilters;
using FLS.Server.WebApi.Identity;
using Foundation.ObjectHydrator;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using NLog.Targets.Wrappers;
using AircraftType = FLS.Data.WebApi.Aircraft.AircraftType;
using Constants = FLS.Server.Data.Resources.Constants;
using ElevationUnitType = FLS.Server.Data.DbEntities.ElevationUnitType;
using LengthUnitType = FLS.Server.Data.DbEntities.LengthUnitType;
using LocationType = FLS.Server.Data.DbEntities.LocationType;
using System.Threading;
using FLS.Server.Service.Accounting;
using FLS.Server.Service.Exporting;

namespace FLS.Server.Tests
{
    [DeploymentItem("NLog.config")]
    [DeploymentItem(@"bin\Debug\NLog.Extended.dll")]
    [DeploymentItem(@"bin\Debug\NLog.Web.dll")]
    [DeploymentItem(@"bin\Debug\EntityFramework.SqlServer.dll")]
    [TestClass]
    public abstract class BaseTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        public IUnityContainer UnityContainer { get; protected set; }
        protected Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();
        protected IIdentityService IdentityService { get; set; }
        private IUserStore<User, Guid> _userStoreService;
        protected DataAccessService DataAccessService { get; set; }
        protected AircraftService AircraftService { get; set; }
        protected FlightService FlightService { get; set; }
        protected DeliveryService DeliveryService { get; set; }
        protected AccountingRuleService AccountingRuleService { get; set; }
        protected AccountingRuleFilterFactory AccountingRuleFilterFactory { get; set; }
        protected LocationService LocationService { get; set; }
        protected ClubService ClubService { get; set; }
        protected SystemService SystemService { get; set; }
        protected WorkflowService WorkflowService { get; set; }
        protected UserAccountEmailBuildService PasswordEmailService { get; set; }
        protected PlanningDayEmailBuildService PlanningDayEmailService { get; set; }
        protected PlanningDayService PlanningDayService { get; set; }
        protected AircraftReservationService AircraftReservationService { get; set; }
        protected AircraftReportEmailBuildService AircraftReportEmailService { get; set; }
        protected FlightInformationEmailBuildService FlightInformationEmailService { get; set; }
        protected UserService UserService { get; set; }


        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            DatabasePreparer.Instance.PrepareDatabaseForTests();
        }

        public void UnityInitialize()
        {
            //UnityContainer = UnityConfig.GetEmptyContainer();
            UnityContainer = new UnityContainer();
            RegisterTypes(UnityContainer);
            
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();
            //container.RegisterType<ValidateModelStateAttribute>();
            //container.RegisterType<CheckModelForNullAttribute>();
            //container.RegisterType<UnhandledExceptionFilterAttribute>();

            container.RegisterType<IDataProtectionProvider, MachineKeyDataProtectionProvider>(new HierarchicalLifetimeManager());

            container.RegisterType<IIdentityMessageService, IdentityEmailService>();
            container.RegisterType<IIdentityService, IdentityService>(new HierarchicalLifetimeManager());
            container.RegisterType<IdentityUserManager>();
            container.RegisterType<UserManager<User, Guid>, IdentityUserManager>();
            container.RegisterType<IExtensionService, ExtensionService>();
            //container.RegisterType<IdentitySignInManager<User, Guid>, IdentitySignInManager>();
            container.RegisterType<IUserStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserPasswordStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserRoleStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserLockoutStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserEmailStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IUserSecurityStampStore<User, Guid>, IdentityUserStoreService>();
            container.RegisterType<IRoleStore<Role, Guid>, IdentityRoleStoreService>();
            container.RegisterType<IEmailSendService, MockEmailSendService>();
            container.RegisterType<ILocationService, LocationService>();
            container.RegisterType<IAircraftService, AircraftService>();
            container.RegisterType<IPersonService, PersonService>();
            //container.RegisterType<IDeliveryExcelExporter, AddFlightIdExcelExporter>();

            //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
            //    new InjectionConstructor(typeof(ApplicationDbContext)));
        }

        [TestInitialize]
        public void Setup()
        {
            //LogManager.Configuration.AllTargets
            //    .OfType<BufferingTargetWrapper>()
            //    .ToList()
            //    .ForEach(b => b.Flush(e =>
            //    {
            //        //do nothing here
            //    }));
            Console.WriteLine("TestInitialize: BaseTest.Setup()");
            UnityInitialize();
            _userStoreService = UnityContainer.Resolve<IUserStore<User, Guid>>();
            IdentityService = UnityContainer.Resolve<IIdentityService>();
            DataAccessService = UnityContainer.Resolve<DataAccessService>();
            UserService = UnityContainer.Resolve<UserService>();
            SetCurrentUser(TestConfigurationSettings.Instance.TestClubAdminUsername);

            AircraftService = UnityContainer.Resolve<AircraftService>();
            DeliveryService = UnityContainer.Resolve<DeliveryService>();
            SystemService = UnityContainer.Resolve<SystemService>();
            PasswordEmailService = UnityContainer.Resolve<UserAccountEmailBuildService>();
            PlanningDayEmailService = UnityContainer.Resolve<PlanningDayEmailBuildService>();
            PlanningDayService = UnityContainer.Resolve<PlanningDayService>();
            AircraftReservationService = UnityContainer.Resolve<AircraftReservationService>();
            FlightInformationEmailService = UnityContainer.Resolve<FlightInformationEmailBuildService>();
            FlightService = UnityContainer.Resolve<FlightService>();
            LocationService = UnityContainer.Resolve<LocationService>();
            WorkflowService = UnityContainer.Resolve<WorkflowService>();
            AircraftReportEmailService = UnityContainer.Resolve<AircraftReportEmailBuildService>();
            AccountingRuleService = UnityContainer.Resolve<AccountingRuleService>();
            AccountingRuleFilterFactory = UnityContainer.Resolve<AccountingRuleFilterFactory>();
            ClubService = UnityContainer.Resolve<ClubService>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            LogManager.Flush();
            //LogManager.Shutdown();
            Console.WriteLine("BaseTest.TestCleanup");
            //Thread.Sleep(1000);
        }

        protected void SetCurrentUser(string userName)
        {
            var user = _userStoreService.FindByNameAsync(userName).Result;
            IdentityService.SetUser(user);
        }
        
        protected User CurrentIdentityUser
        {
            get { return IdentityService.CurrentAuthenticatedFLSUser; }
        }

        #region Aircraft
        public Aircraft GetFirstAircraft()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Aircrafts.FirstOrDefault();
            }
        }

        public Aircraft GetFirstGlider()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Aircrafts.FirstOrDefault(a => a.AircraftTypeId == (int)FLS.Data.WebApi.Aircraft.AircraftType.Glider);
            }
        }

        public Aircraft GetAircraft(string immatriculation)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var aircraft = context.Aircrafts
                    .Include(Data.Resources.Constants.AircraftAircraftStates)
                    .Include(Constants.AircraftAircraftStatesAircraftStateRelation)
                    .Include("AircraftOwnerPerson")
                    .Include("AircraftOwnerClub")
                    .Include("AircraftType")
                    .FirstOrDefault(a => a.Immatriculation.Replace("-", "").ToUpper() == immatriculation.Replace("-", "").ToUpper());

                if (aircraft == null)
                {
                    var aircraftDetails = CreateGliderAircraftDetails(2);
                    aircraftDetails.Immatriculation = immatriculation;
                    AircraftService.InsertAircraftDetails(aircraftDetails);

                    aircraft = context.Aircrafts
                    .Include(Data.Resources.Constants.AircraftAircraftStates)
                    .Include(Constants.AircraftAircraftStatesAircraftStateRelation)
                    .Include("AircraftOwnerPerson")
                    .Include("AircraftOwnerClub")
                    .Include("AircraftType")
                    .FirstOrDefault(a => a.Immatriculation.Replace("-", "").ToUpper() == immatriculation.Replace("-", "").ToUpper());
                }

                return aircraft;
            }
        }

        public Aircraft GetFirstOneSeatGlider()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Aircrafts.FirstOrDefault(a => a.AircraftTypeId == (int)FLS.Data.WebApi.Aircraft.AircraftType.Glider && a.NrOfSeats == 1);
            }
        }

        public Aircraft GetFirstDoubleSeatGlider()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Aircrafts.FirstOrDefault(a => a.AircraftTypeId == (int)FLS.Data.WebApi.Aircraft.AircraftType.Glider && a.NrOfSeats == 2);
            }
        }

        public Aircraft GetFirstTowingAircraft()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Aircrafts.FirstOrDefault(a => a.AircraftTypeId == (int)FLS.Data.WebApi.Aircraft.AircraftType.MotorAircraft && a.IsTowingAircraft);
            }
        }

        public AircraftDetails CreateGliderAircraftDetails(int nrOfSeats, FLS.Data.WebApi.Aircraft.AircraftType aircraftType = FLS.Data.WebApi.Aircraft.AircraftType.Glider,
            bool isTowingOrWinchRequired = true, bool isTowingstartAllowed = true, bool isWinchstartAllowed = true)
        {
            Assert.IsTrue(nrOfSeats > 0);

            var aircraftDetails = new AircraftDetails
            {
                AircraftModel = "Test-Aircraft-Model",
                AircraftType = (int)aircraftType,
                Comment = "Test-Glider " + DateTime.Now.ToShortTimeString(),
                CompetitionSign = GetCompetitionSign(),
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

        private string GetCompetitionSign()
        {
            Hydrator<CompetitionSignWrapper> hydrator = new Hydrator<CompetitionSignWrapper>().WithCompanyName(p => p.CompetitionSign);
            var competitionSign = hydrator.Generate();

            competitionSign.CompetitionSign = competitionSign.CompetitionSign.Replace(" ", "");

            while (competitionSign.CompetitionSign.Length < 2)
            {
                competitionSign = hydrator.GetSingle();
                competitionSign.CompetitionSign = competitionSign.CompetitionSign.Replace(" ", "");
            }

            competitionSign.CompetitionSign = competitionSign.CompetitionSign.Substring(0, 2);

            return competitionSign.CompetitionSign.ToUpper();
        }

        public AircraftDetails CreateMotorAircraftDetails()
        {
            var aircraftDetails = new AircraftDetails
            {
                AircraftModel = "Test-Motor-Aircraft-Model",
                AircraftType = (int)FLS.Data.WebApi.Aircraft.AircraftType.MotorAircraft,
                Comment = "Test-Motor-Aircraft " + DateTime.Now.ToShortTimeString(),
                FLARMId = "ID" + DateTime.Now.Ticks,
                Immatriculation = GetAvailableMotorImmatriculation(),
                IsTowingOrWinchRequired = false,
                IsTowingstartAllowed = false,
                IsWinchstartAllowed = false,
                IsTowingAircraft = false,
                ManufacturerName = "Test-Manufacturer",
                NrOfSeats = 4
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
            Aircraft aircraft;
            Random random = new Random();
            string immatriculation;

            do
            {
                var number = random.Next(1, 9999);
                immatriculation = "HB-" + number;
                aircraft = AircraftService.GetAircraft(immatriculation);
            } while (aircraft != null);

            return immatriculation;
        }

        public string GetAvailableMotorImmatriculation()
        {
            Aircraft aircraft;
            var hydrator = new Hydrator<Aircraft>().WithLastName(f => f.Immatriculation).Ignoring(f => f.NoiseClass);
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
                aircraft = AircraftService.GetAircraft(immatriculation);
            } while (aircraft != null);

            return immatriculation;
        }
        #endregion Aircraft

        #region AircraftReservation
        public AircraftReservationType GetFirstAircraftReservationType()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.AircraftReservationTypes.FirstOrDefault();
            }
        }

        public AircraftReservationDetails CreateAircraftReservationDetails()
        {
            var reservation = new AircraftReservationDetails();
            reservation.AircraftId = GetFirstAircraft().AircraftId;
            reservation.Start = TestUtilities.GetRandomDateTimeInFuture();
            reservation.End = reservation.Start.AddHours(4);
            reservation.Remarks = "Test";
            reservation.IsAllDayReservation = false;
            reservation.PilotPersonId = GetFirstPerson().PersonId;
            reservation.LocationId = GetFirstLocation().LocationId;
            reservation.InstructorPersonId = GetFirstPerson().PersonId;
            reservation.ReservationTypeId = GetFirstAircraftReservationType().AircraftReservationTypeId;

            return reservation;
        }
        
        #endregion AircraftReservation

        #region Club
        protected FlightType GetFlightType(string flightCode)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var clubId = CurrentIdentityUser.ClubId;
                return
                    context.FlightTypes.FirstOrDefault(
                        c => c.ClubId == clubId && c.FlightCode.ToLower() == flightCode.ToLower());
            }
        }
        

        public ClubDetails CreateClubDetails()
        {
            var hydrator = new Hydrator<ClubDetails>();
            var club = hydrator.GetSingle();

            club.CountryId = GetFirstCountry().CountryId;

            club.HomebaseId = GetFirstLocation().LocationId;
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
        
        public FlightType GetFirstSoloGliderFlightType(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.FlightTypes.FirstOrDefault(c => c.ClubId == clubId && c.IsForGliderFlights
                    && c.IsSoloFlight);
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
        
        #endregion Club

        #region Flight
        public List<StartType> GetStartTypes()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.StartTypes.ToList();
            }
        }

        public FlightDetails CreateMinimalGliderFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = GetFirstGlider().AircraftId;
            gliderData.PilotPersonId = GetFirstGliderPilotPerson(clubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;

            return flightDetails;
        }

        public FlightDetails CreateGliderFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = GetFirstGlider().AircraftId;
            gliderData.PilotPersonId = GetFirstGliderPilotPerson(clubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;
            flightDetails.StartType = (int)FLS.Server.Data.Enums.AircraftStartType.TowingByAircraft;
            return flightDetails;
        }

        public FlightDetails CreateOneSeatGliderFlightDetails(Guid clubId)
        {
            var flightDetails = new FlightDetails();
            var gliderData = new GliderFlightDetailsData();
            gliderData.AircraftId = GetFirstOneSeatGlider().AircraftId;
            gliderData.PilotPersonId = GetFirstGliderPilotPerson(clubId).PersonId;
            flightDetails.GliderFlightDetailsData = gliderData;
            flightDetails.StartType = (int)FLS.Server.Data.Enums.AircraftStartType.TowingByAircraft;
            return flightDetails;
        }
        
        public Flight CreateGliderFlight(Guid clubId, DateTime startTime)
        {
            var flightDetails = new FlightDetails();
            flightDetails.StartType = 1; //Towing GLider flight
            flightDetails.GliderFlightDetailsData = CreateSchoolGliderFlightDetailsData(clubId, "HB-1824", startTime, 45);
            flightDetails.TowFlightDetailsData = CreateTowFlightDetailsData(clubId, "HB-KCB", startTime, 12);

            FlightService.InsertFlightDetails(flightDetails);

            Assert.IsTrue(flightDetails.FlightId.IsValid());

            var flight = FlightService.GetFlight(flightDetails.FlightId);

            Assert.IsTrue(flight.FlightId.IsValid());

            return flight;
        }
        
        public GliderFlightDetailsData CreateSchoolGliderFlightDetailsData(Guid clubId, string immatriculation, DateTime startTime, int flightDurationInMinutes = 90, string locationIcaoCode = "LSZK")
        {
            Aircraft glider = null;

            if (string.IsNullOrEmpty(immatriculation) == false)
            {
                glider = GetAircraft(immatriculation);
            }

            if (glider == null)
            {
                glider = GetFirstDoubleSeatGlider();
            }

            Assert.IsNotNull(glider);

            var startlocation = GetLocation(locationIcaoCode);

            if (startlocation == null)
            {
                startlocation = GetFirstLocation();
            }

            Assert.IsNotNull(startlocation);

            var gliderPilot = GetFirstGliderPilotPerson(clubId);
            var instructor = GetFirstGliderInstructorPerson(clubId);
            Assert.IsNotNull(gliderPilot);
            Assert.IsNotNull(instructor);
            var gliderFlightType = GetFirstInstructorRequiredGliderFlightType(clubId);
            Assert.IsNotNull(gliderFlightType);

            var gliderFlightDetailsData = new GliderFlightDetailsData
            {
                AircraftId = glider.AircraftId,
                FlightComment = "Schoolflight",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(flightDurationInMinutes),
                PilotPersonId = gliderPilot.PersonId,
                StartLocationId = startlocation.LocationId,
                LdgLocationId = startlocation.LocationId,
                FlightTypeId = gliderFlightType.FlightTypeId,
                InstructorPersonId = instructor.PersonId
            };

            return gliderFlightDetailsData;
        }

        public TowFlightDetailsData CreateTowFlightDetailsData(Guid clubId, string immatriculation, DateTime startTime, int flightDurationInMinutes = 12, string locationIcaoCode = "LSZK")
        {
            Aircraft towingAircraft = null;

            if (string.IsNullOrEmpty(immatriculation) == false)
            {
                towingAircraft = GetAircraft(immatriculation);
            }

            if (towingAircraft == null)
            {
                towingAircraft = GetFirstTowingAircraft();
            }

            Assert.IsNotNull(towingAircraft);
            var startlocation = GetLocation(locationIcaoCode);

            if (startlocation == null)
            {
                startlocation = GetFirstLocation();
            }

            Assert.IsNotNull(startlocation);

            var towingPilot = GetFirstTowingPilotPerson(clubId);
            Assert.IsNotNull(towingPilot);
            var towingFlightType = GetFirstTowingFlightType(clubId);
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
            motorFlightData.AircraftId = GetFirstTowingAircraft().AircraftId;
            motorFlightData.PilotPersonId = GetFirstTowingPilotPerson(clubId).PersonId;
            motorFlightData.StartLocationId = GetFirstLocation().LocationId;
            motorFlightData.LdgLocationId = motorFlightData.StartLocationId;
            flightDetails.MotorFlightDetailsData = motorFlightData;
            flightDetails.StartType = (int)FLS.Server.Data.Enums.AircraftStartType.MotorFlightStart;
            return flightDetails;
        }
        
        protected void SetFlightAsLocked(FlightDetails flightDetails)
        {
            Assert.IsTrue(flightDetails.FlightId.IsValid());

            var flight = FlightService.GetFlight(flightDetails.FlightId);

            Assert.IsTrue(flight.FlightId.IsValid());

            using (var context = DataAccessService.CreateDbContext())
            {
                context.Flights.Attach(flight);
                flight.ProcessStateId = (int)FLS.Data.WebApi.Flight.FlightProcessState.Locked;

                if (context.ChangeTracker.HasChanges())
                {
                    context.SaveChanges();
                }
            }
        }
        #endregion Flight

        #region Location

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
        
        public Location GetLocation(string locationIcaoCode)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var location = context.Locations.FirstOrDefault(l => l.IcaoCode.ToLower() == locationIcaoCode.ToLower());

                if (location == null)
                {
                    var country = GetCountry("CH");
                    location = new Location()
                    {
                        CountryId = country.CountryId,
                        LocationName = locationIcaoCode,
                        IcaoCode = locationIcaoCode
                    };

                    if (locationIcaoCode.Length == 4)
                    {
                        location.LocationTypeId =
                            GetLocationType((int) FLS.Data.WebApi.Location.LocationType.AirfieldSolid).LocationTypeId;
                    }
                    else
                    {
                        location.LocationTypeId =
                            GetLocationType((int) FLS.Data.WebApi.Location.LocationType.Outlanding).LocationTypeId;
                    }

                    context.Locations.Add(location);
                    context.SaveChanges();
                }

                return location;
            }
        }
        #endregion Location

        #region Person
        protected Person GetPerson(string displayname, string countryCode = "CH", bool createIfNotExists = true)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var clubId = CurrentIdentityUser.ClubId;
                var country = GetCountry(countryCode);
                var firstname = displayname.Split(' ')[1];
                var lastname = displayname.Split(' ')[0];

                var person = context.Persons.FirstOrDefault(p => p.Lastname.ToLower().Contains(lastname.ToLower())
                        && p.Firstname.ToLower().Contains(firstname.ToLower())
                        && p.CountryId == country.CountryId);

                if (person == null)
                {
                    Assert.IsTrue(createIfNotExists);

                    var personDetails = new PersonDetails();
                    personDetails.Firstname = firstname;
                    personDetails.Lastname = lastname;
                    personDetails.CountryId = country.CountryId;
                    person = personDetails.ToPerson(clubId);
                    context.Persons.Add(person);
                    context.SaveChanges();
                }

                return person;
            }
        }

        public PersonDetails CreateGliderPilotPersonDetails(Guid countryId)
        {
            var personDetails = new PersonDetails()
            {
                CountryId = countryId,
                Lastname = "Huter-Pilot",
                Firstname = "Peter",
                AddressLine1 = "Segelflugstrasse 199",
                ZipCode = "1234",
                City = "Segelflug-City",
                Birthday = new DateTime(1965, 11, 13),
                BusinessEmail = "test@glider-fls.ch",
                HasGliderPilotLicence = true,
                HasGliderPassengerLicence = true,
                Region = "ZH",
                LicenceNumber = "CH.FCL.19999",
                MedicalLaplExpireDate = DateTime.Now.Date.AddMonths(4)
            };

            var ownClubData = new ClubRelatedPersonDetails
            {
                MemberNumber = DateTime.Now.Ticks.ToString(),
                IsGliderPilot = true,
            };

            personDetails.ClubRelatedPersonDetails = ownClubData;

            return personDetails;
        }

        public PersonDetails CreateGliderInstructorPersonDetails(Guid countryId)
        {
            var personDetails = new PersonDetails()
            {
                CountryId = countryId,
                Lastname = "Ingold-Instructor",
                Firstname = "Hansi",
                AddressLine1 = "Instruktorstrasse 25",
                ZipCode = "4321",
                City = "Instruktor-City",
                Birthday = new DateTime(1955, 4, 23),
                BusinessEmail = "test@glider-fls.ch",
                HasGliderPilotLicence = true,
                HasGliderPassengerLicence = true,
                HasGliderInstructorLicence = true,
                Region = "ZH",
                LicenceNumber = "CH.FCL.4567",
                GliderInstructorLicenceExpireDate = DateTime.Now.Date.AddMonths(24)
            };

            var ownClubData = new ClubRelatedPersonDetails
            {
                MemberNumber = DateTime.Now.Ticks.ToString(),
                IsGliderPilot = true,
                IsGliderInstructor = true,
            };

            personDetails.ClubRelatedPersonDetails = ownClubData;

            return personDetails;
        }
        
        public PersonDetails CreatePersonDetails(Guid countryId)
        {
            var hydrator = new Hydrator<PersonDetails>();
            var personDetails = hydrator.GetSingle();

            personDetails.CountryId = countryId;
            personDetails.PersonId = Guid.Empty;
            if (personDetails.Lastname.Length > 80) personDetails.Lastname = personDetails.Lastname.Substring(0, 80);
            personDetails.Lastname = personDetails.Lastname + DateTime.Now.Ticks;

            var ownClubData = new ClubRelatedPersonDetails
            {
                MemberNumber = DateTime.Now.Ticks.ToString()
            };

            personDetails.ClubRelatedPersonDetails = ownClubData;

            return personDetails;
        }

        public PersonFullDetails CreatePersonFullDetails(Guid clubId, Guid countryId)
        {
            var hydrator = new Hydrator<PersonFullDetails>();
            var personDetails = hydrator.GetSingle();

            personDetails.CountryId = countryId;
            personDetails.PersonId = Guid.Empty;
            if (personDetails.Lastname.Length > 80) personDetails.Lastname = personDetails.Lastname.Substring(0, 80);
            personDetails.Lastname = personDetails.Lastname + DateTime.Now.Ticks;

            var ownClubData = new ClubRelatedPersonDetails
            {
                MemberNumber = DateTime.Now.Ticks.ToString()
            };

            personDetails.ClubRelatedPersonDetails = ownClubData;
            personDetails.RemoveMetadataInfo();

            return personDetails;
        }

        public Person GetFirstPerson()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault();
            }
        }

        public Person GetFirstPerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId));
            }
        }
        

        public Person GetFirstGliderPilotPerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId) && q.HasGliderPilotLicence);
            }
        }
        
        public Person GetFirstGliderTraineePerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId) && q.HasGliderTraineeLicence);
            }
        }

        public Person GetFirstTowingPilotPerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId) && q.HasTowPilotLicence);
            }
        }

        public Person GetFirstGliderInstructorPerson(Guid clubId)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Persons.FirstOrDefault(q => q.PersonClubs.Any(pc => pc.ClubId == clubId) && q.HasGliderInstructorLicence);
            }
        }
        
        public Person GetDifferentPerson(Guid? personId)
        {
            if (personId == null) return GetFirstPerson();

            using (var context = DataAccessService.CreateDbContext())
            {
                foreach (var person in context.Persons)
                {
                    if (person.PersonId != personId)
                    {
                        return person;
                    }
                }
            }

            return null;
        }

        public void InsertPerson(Person person)
        {
            Assert.IsNotNull(person);

            using (var context = DataAccessService.CreateDbContext())
            {
                context.Persons.Add(person);
                context.SaveChanges();
            }
        }
        #endregion Person

        #region PlanningDay
        public PlanningDayOverview GetFirstPlanningDayOverview()
        {
            var planningDays = PlanningDayService.GetPlanningDayOverview(DateTime.MinValue);
            Assert.IsTrue(planningDays.Any());
            return planningDays.FirstOrDefault();
        }
        #endregion PlanningDay

        #region User
        public User GetUser(string username)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserName.ToLower() == username.ToLower());
                return user;
            }
        }

        public Role GetRole(string roleName)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var role = context.Roles.FirstOrDefault(u => u.RoleApplicationKeyString.ToLower() == roleName.ToLower());
                return role;
            }
        }
        
        public UserDetails CreateUserDetails(Guid clubId)
        {
            var hydrator = new Hydrator<UserDetails>();
            var userDetails = hydrator.GetSingle();
            userDetails.UserName = "User" + DateTime.Now.Ticks;
            userDetails.UserId = Guid.Empty;
            userDetails.ClubId = clubId;
            userDetails.PersonId = null;
            userDetails.UserRoleIds = new List<Guid>();
            userDetails.AccountState = (int)FLS.Data.WebApi.User.UserAccountState.Active;
            var role = GetRole(RoleApplicationKeyStrings.FlightOperator);
            Assert.IsNotNull(role);
            userDetails.UserRoleIds.Add(role.Id);
            return userDetails;
        }

        public UserRegistrationDetails CreateUserRegistrationDetails(Guid clubId)
        {
            var hydrator = new Hydrator<UserRegistrationDetails>();
            var userDetails = hydrator.GetSingle();
            userDetails.UserName = "User" + DateTime.Now.Ticks;
            userDetails.PersonId = null;
            userDetails.UserRoleIds = new List<Guid>();
            var role = GetRole(RoleApplicationKeyStrings.FlightOperator);
            Assert.IsNotNull(role);
            userDetails.UserRoleIds.Add(role.Id);
            userDetails.EmailConfirmationLink = "http://localhost/api/account/ConfirmEmail?userId={userid}&code={code}";
            return userDetails;
        }
        
        public User CreateNewUserInDb(Guid clubId, string plainPassword, bool emailConfirmed = false)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                if (DataAccessService.IdentityService != null &&
                    DataAccessService.IdentityService.CurrentAuthenticatedFLSUser == null)
                {
                    DataAccessService.IdentityService.SetUser(IdentityService.CurrentAuthenticatedFLSUser);
                }

                var hydrator = new Hydrator<User>()
                    .Ignoring(x => x.Person)
                    .Ignoring(x => x.UserAccountState)
                    .Ignoring(x => x.Club)
                    .Ignoring(x => x.UserRoles);
                var user = hydrator.GetSingle();
                user.RemoveMetadataInfo();

                user.ClubId = clubId;
                user.AccountState = (int)FLS.Data.WebApi.User.UserAccountState.Active;
                user.EmailConfirmed = emailConfirmed;
                var passwordHasher = new PasswordHasher();
                var hashedPassword = passwordHasher.HashPassword(plainPassword);
                user.UserName = "User" + DateTime.Now.Ticks;
                user.PasswordHash = hashedPassword;
                user.LastPasswordChangeOn = DateTime.UtcNow;
                context.Users.Add(user);
                context.SaveChanges();

                return user;
            }
        }
        
        #endregion User
    }
}
