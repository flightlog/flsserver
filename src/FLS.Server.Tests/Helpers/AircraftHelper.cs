using System;
using System.Linq;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Resources;
using FLS.Server.Service;
using FLS.Server.TestInfrastructure;
using Foundation.ObjectHydrator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AircraftType = FLS.Data.WebApi.Aircraft.AircraftType;

namespace FLS.Server.Tests.Helpers
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

        public Aircraft GetAircraft(string immatriculation)
        {
            using (var context = DataAccessService.CreateDbContext())
            {
                var aircraft = context.Aircrafts
                    .Include(Constants.AircraftAircraftStates)
                    .Include(Constants.AircraftAircraftStatesAircraftStateRelation)
                    .Include("AircraftOwnerPerson")
                    .Include("AircraftOwnerClub")
                    .Include("AircraftType")
                    .FirstOrDefault(a => a.Immatriculation.Replace("-", "").ToUpper() == immatriculation.Replace("-", "").ToUpper());

                return aircraft;
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
                AircraftType = (int) AircraftType.MotorAircraft,
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
