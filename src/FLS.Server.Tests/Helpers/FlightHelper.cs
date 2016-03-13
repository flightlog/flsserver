using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Extensions;
using FLS.Data.WebApi.Flight;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
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

        public GliderFlightDetailsData CreateOneSeatGliderFlightDetailsData(Guid clubId, string immatriculation, DateTime startTime, int flightDurationInMinutes = 90, string locationIcaoCode = "LSZK")
        {
            Aircraft glider = null;

            if (string.IsNullOrEmpty(immatriculation) == false)
            {
                glider = _aircraftHelper.GetAircraft(immatriculation);
            }

            if (glider == null)
            {
                glider = _aircraftHelper.GetFirstOneSeatGlider();
            }

            Assert.IsNotNull(glider);

            var startlocation = _locationHelper.GetLocation(locationIcaoCode);

            if (startlocation == null)
            {
                startlocation = _locationHelper.GetFirstLocation();
            }

            Assert.IsNotNull(startlocation);

            var gliderPilot = _personHelper.GetFirstGliderPilotPerson(clubId);
            Assert.IsNotNull(gliderPilot);
            var gliderFlightType = _clubHelper.GetFirstGliderFlightType(clubId);
            Assert.IsNotNull(gliderFlightType);

            var gliderFlightDetailsData = new GliderFlightDetailsData
            {
                AircraftId = glider.AircraftId,
                FlightComment = "Gliderflight",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(flightDurationInMinutes),
                PilotPersonId = gliderPilot.PersonId,
                StartLocationId = startlocation.LocationId,
                LdgLocationId = startlocation.LocationId,
                FlightTypeId = gliderFlightType.FlightTypeId
            };

            return gliderFlightDetailsData;
        }

        public GliderFlightDetailsData CreateSchoolGliderFlightDetailsData(Guid clubId, string immatriculation, DateTime startTime, int flightDurationInMinutes = 90, string locationIcaoCode = "LSZK")
        {
            Aircraft glider = null;

            if (string.IsNullOrEmpty(immatriculation) == false)
            {
                glider = _aircraftHelper.GetAircraft(immatriculation);
            }

            if (glider == null)
            {
                glider = _aircraftHelper.GetFirstDoubleSeatGlider();
            }

            Assert.IsNotNull(glider);

            var startlocation = _locationHelper.GetLocation(locationIcaoCode);

            if (startlocation == null)
            {
                startlocation = _locationHelper.GetFirstLocation();
            }

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

        public TowFlightDetailsData CreateTowFlightDetailsData(Guid clubId, string immatriculation, DateTime startTime, int flightDurationInMinutes = 12, string locationIcaoCode = "LSZK")
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
            var startlocation = _locationHelper.GetLocation(locationIcaoCode);

            if (startlocation == null)
            {
                startlocation = _locationHelper.GetFirstLocation();
            }

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

        public Dictionary<string, Guid> CreateFlightsForProffixInvoicingTests(Guid clubId, DateTime flightDate)
        {
            var flightDictionary = new Dictionary<string, Guid>();
            
            var gliderHB1824 = _aircraftHelper.GetAircraft("HB-1824");
            var gliderHB1841 = _aircraftHelper.GetAircraft("HB-1841");
            var gliderHB2464 = _aircraftHelper.GetAircraft("HB-2464");
            var gliderHB3256 = _aircraftHelper.GetAircraft("HB-3256");
            var gliderHB3407 = _aircraftHelper.GetAircraft("HB-3407");
            var kcb = _aircraftHelper.GetAircraft("HB-KCB");
            var kio = _aircraftHelper.GetAircraft("HB-KIO"); //Montricher Schlepp
            var gliderPilot = _personHelper.GetFirstGliderPilotPerson(clubId);
            var instructor = _personHelper.GetFirstGliderInstructorPerson(clubId);
            var towPilot = _personHelper.GetFirstTowingPilotPerson(clubId);
            var gliderTrainee = _personHelper.GetFirstGliderTraineePerson(clubId);
            var lszk = _locationHelper.GetLocation("LSZK"); //Speck
            var lszx = _locationHelper.GetLocation("LSZX"); //Schänis
            var lst = _locationHelper.GetLocation("LSTR"); //Montricher
            var lsgk = _locationHelper.GetLocation("LSGK"); //Saanen
            var flightTypes = _clubHelper.GetFlightTypes(clubId);
            var towFlightTypeId = _clubHelper.GetFirstTowingFlightType(clubId).FlightTypeId;

            //UC1: create local charter flight with 1 seat glider and less then 10 min. towing
            //HB-1824 Privat Charter Clubflugzeug
            var startTime = flightDate.AddHours(10);
            var flightDetails = new FlightDetails();
            flightDetails.StartType = (int)AircraftStartType.TowingByAircraft;

            flightDetails.GliderFlightDetailsData = new GliderFlightDetailsData
            {
                AircraftId = gliderHB1824.AircraftId,
                FlightComment = "Charterflug",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(42),
                PilotPersonId = gliderPilot.PersonId,
                StartLocationId = lszk.LocationId,
                LdgLocationId = lszk.LocationId,
                FlightTypeId = flightTypes.First(x => x.FlightCode == "60").FlightTypeId
            };

            flightDetails.TowFlightDetailsData = new TowFlightDetailsData
            {
                AircraftId = kcb.AircraftId,
                FlightComment = "TowFlight",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(8),
                PilotPersonId = towPilot.PersonId,
                StartLocationId = lszk.LocationId,
                LdgLocationId = lszk.LocationId,
                FlightTypeId = towFlightTypeId
            };

            _flightService.InsertFlightDetails(flightDetails);
            flightDictionary.Add("UC1", flightDetails.FlightId);
            SetFlightAsLocked(flightDetails);

            //UC2: create local charter flight with 1 seat glider and more then 10 min. towing
            startTime = flightDate.AddHours(10).AddMinutes(30);
            flightDetails = new FlightDetails();
            flightDetails.StartType = (int)AircraftStartType.TowingByAircraft;

            flightDetails.GliderFlightDetailsData = new GliderFlightDetailsData
            {
                AircraftId = gliderHB2464.AircraftId,
                FlightComment = "Charterflug",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(355),
                PilotPersonId = gliderPilot.PersonId,
                StartLocationId = lszk.LocationId,
                LdgLocationId = lszk.LocationId,
                FlightTypeId = flightTypes.First(x => x.FlightCode == "60").FlightTypeId
            };

            flightDetails.TowFlightDetailsData = new TowFlightDetailsData
            {
                AircraftId = kcb.AircraftId,
                FlightComment = "TowFlight",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(22),
                PilotPersonId = towPilot.PersonId,
                StartLocationId = lszk.LocationId,
                LdgLocationId = lszk.LocationId,
                FlightTypeId = towFlightTypeId
            };

            _flightService.InsertFlightDetails(flightDetails);
            flightDictionary.Add("UC2", flightDetails.FlightId);
            SetFlightAsLocked(flightDetails);

            //UC3: create charter flight from local and landing extern with 1 seat glider and more then 10 min. towing
            startTime = flightDate.AddHours(10).AddMinutes(30);
            flightDetails = new FlightDetails();
            flightDetails.StartType = (int)AircraftStartType.TowingByAircraft;

            flightDetails.GliderFlightDetailsData = new GliderFlightDetailsData
            {
                AircraftId = gliderHB2464.AircraftId,
                FlightComment = "Charterflug",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(355),
                PilotPersonId = gliderPilot.PersonId,
                StartLocationId = lszk.LocationId,
                LdgLocationId = lszk.LocationId,
                FlightTypeId = flightTypes.First(x => x.FlightCode == "60").FlightTypeId
            };

            flightDetails.TowFlightDetailsData = new TowFlightDetailsData
            {
                AircraftId = kcb.AircraftId,
                FlightComment = "TowFlight",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(22),
                PilotPersonId = towPilot.PersonId,
                StartLocationId = lszk.LocationId,
                LdgLocationId = lszk.LocationId,
                FlightTypeId = towFlightTypeId
            };

            _flightService.InsertFlightDetails(flightDetails);
            flightDictionary.Add("UC2", flightDetails.FlightId);
            SetFlightAsLocked(flightDetails);

            //UC4: create local trainee flight with 2 seat glider and less then 10 min. towing
            //HB - 3256 Schulung Grundschulung Doppelsteuer
            startTime = flightDate.AddHours(11);
            flightDetails = new FlightDetails();
            flightDetails.StartType = (int)AircraftStartType.TowingByAircraft;

            flightDetails.GliderFlightDetailsData = new GliderFlightDetailsData
            {
                AircraftId = gliderHB3256.AircraftId,
                FlightComment = "Kurvenflug i.O., Anfluggeschwindigkeit zu tief",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(22),
                PilotPersonId = gliderTrainee.PersonId,
                StartLocationId = lszk.LocationId,
                LdgLocationId = lszk.LocationId,
                FlightTypeId = flightTypes.First(x => x.FlightCode == "70").FlightTypeId,
                InstructorPersonId = instructor.PersonId
            };

            flightDetails.TowFlightDetailsData = new TowFlightDetailsData
            {
                AircraftId = kcb.AircraftId,
                FlightComment = "TowFlight",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(8),
                PilotPersonId = towPilot.PersonId,
                StartLocationId = lszk.LocationId,
                LdgLocationId = lszk.LocationId,
                FlightTypeId = towFlightTypeId
            };

            _flightService.InsertFlightDetails(flightDetails);
            flightDictionary.Add("UC4", flightDetails.FlightId);
            SetFlightAsLocked(flightDetails);

            //UC5: create local trainee flight with 2 seat glider and more then 10 min. towing
            //HB - 3256 Schulung Grundschulung Doppelsteuer
            startTime = flightDate.AddHours(12);
            flightDetails = new FlightDetails();
            flightDetails.StartType = (int)AircraftStartType.TowingByAircraft;

            flightDetails.GliderFlightDetailsData = new GliderFlightDetailsData
            {
                AircraftId = gliderHB3256.AircraftId,
                FlightComment = "Streckenflug i.O.",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(185),
                PilotPersonId = gliderTrainee.PersonId,
                StartLocationId = lszk.LocationId,
                LdgLocationId = lszk.LocationId,
                FlightTypeId = flightTypes.First(x => x.FlightCode == "70").FlightTypeId,
                InstructorPersonId = instructor.PersonId
            };

            flightDetails.TowFlightDetailsData = new TowFlightDetailsData
            {
                AircraftId = kcb.AircraftId,
                FlightComment = "TowFlight",
                StartDateTime = startTime,
                LdgDateTime = startTime.AddMinutes(14),
                PilotPersonId = towPilot.PersonId,
                StartLocationId = lszk.LocationId,
                LdgLocationId = lszk.LocationId,
                FlightTypeId = towFlightTypeId
            };

            _flightService.InsertFlightDetails(flightDetails);
            flightDictionary.Add("UC5", flightDetails.FlightId);
            SetFlightAsLocked(flightDetails);

            //UC6: create local yearly check flight with 2 seat glider and less then 10 min. towing
            //UC7: create local yearly check flight with 2 seat glider and more then 10 min. towing

            //UC8: create local passenger bar flight with 2 seat glider and more then 10 min. towing

            //UC9: create local passenger coupon flight with 2 seat glider and more then 10 min. towing

            //UC10: create local possible trainee flight with 2 seat glider and more then 10 min. towing

            //UC11: create local marketing flight with 2 seat glider and more then 10 min. towing

            //UC12: create charter glider flight from external airport to local airport with 2 seat club owned glider
            //UC13: create external glider flight from external airport to local airport with 1 seat foreign glider

            return flightDictionary;
        }

        private void SetFlightAsLocked(FlightDetails flightDetails)
        {
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
    }
}
