using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Validators;
using FLS.Data.WebApi.Licensing;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using FLS.Server.Data.Resources;
using NLog;
using AircraftType = FLS.Data.WebApi.Aircraft.AircraftType;

namespace FLS.Server.Service
{
    public class LicenseService : BaseService
    {
        private readonly AircraftService _aircraftService;
        private readonly FlightService _flightService;
        private readonly PersonService _personService;

        public LicenseService(DataAccessService dataAccessService, IdentityService identityService, 
            AircraftService aircraftService, FlightService flightService, PersonService personService)
            : base(dataAccessService, identityService)
        {
            _aircraftService = aircraftService;
            _flightService = flightService;
            _personService = personService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        public LicenseTrainingStateResult GetCurrentPilotLicenseTrainingState(LicenseTrainingStateRequest request)
        {
            //check medical state
            //bool isMedicalValid = IsMedicalValid()
            var person = _personService.GetPerson(request.PilotPersonId);
            if (person.MedicalClass2ExpireDate.HasValue && person.MedicalClass2ExpireDate.Value < DateTime.Now)
            {
                //medical class 2 is expired --> check LAPL
                if (person.MedicalLaplExpireDate.HasValue && person.MedicalLaplExpireDate.Value < DateTime.Now)
                {
                    //LAPL medical expired too

                }
            }

            var aircraft = _aircraftService.GetAircraftDetails(request.AircraftId.Value);
            var toDate = DateTime.UtcNow;
            var fromDate = toDate.AddMonths(-24);
            
            //var flights = _flightService.GetFlightStatistics(fromDate, toDate, request.StartTypeId, request.PilotPersonId);

            if (aircraft.AircraftType == (int) AircraftType.Glider
                || aircraft.AircraftType == (int) AircraftType.GliderWithMotor)
            {
                
            }

            return null;
        }
    }
}
