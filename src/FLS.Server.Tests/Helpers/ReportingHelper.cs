using System;
using System.Linq;
using FLS.Data.WebApi.Aircraft;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service;
using FLS.Server.TestInfrastructure;
using Foundation.ObjectHydrator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AircraftType = FLS.Data.WebApi.Aircraft.AircraftType;

namespace FLS.Server.Tests.Helpers
{
    public class ReportingHelper : BaseHelper
    {
        private readonly AircraftService _aircraftService;
        private readonly FlightService _flightService;

        public ReportingHelper(AircraftService aircraftService, FlightService flightService, DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _aircraftService = aircraftService;
            _flightService = flightService;
        }

    }
}
