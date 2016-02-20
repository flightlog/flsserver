using System;
using System.Linq;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.TestInfrastructure.Extensions;
using Foundation.ObjectHydrator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AircraftType = FLS.Data.WebApi.Aircraft.AircraftType;

namespace FLS.Server.TestInfrastructure
{
    public class AircraftHelper : BaseHelper
    {
        private readonly AircraftService _aircraftService;

        public AircraftHelper(AircraftService aircraftService, DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _aircraftService = aircraftService;
        }

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
                return context.Aircrafts.FirstOrDefault(a => a.AircraftTypeId == (int) AircraftType.Glider);
            }
        }

        public Aircraft GetFirstOneSeatGlider()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Aircrafts.FirstOrDefault(a => a.AircraftTypeId == (int)AircraftType.Glider && a.NrOfSeats == 1);
            }
        }

        public Aircraft GetFirstDoubleSeatGlider()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Aircrafts.FirstOrDefault(a => a.AircraftTypeId == (int)AircraftType.Glider && a.NrOfSeats == 2);
            }
        }

        public Aircraft GetFirstTowingAircraft()
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                return context.Aircrafts.FirstOrDefault(a => a.AircraftTypeId == (int)AircraftType.MotorAircraft && a.IsTowingAircraft);
            }
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
            Aircraft aircraft;
            Random random = new Random();
            string immatriculation;

            do
            {
                var number = random.Next(1, 9999);
                immatriculation = "HB-" + number;
                aircraft = _aircraftService.GetAircraft(immatriculation);
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
                aircraft = _aircraftService.GetAircraft(immatriculation);
            } while (aircraft != null);

            return immatriculation;
        }
    }
}
