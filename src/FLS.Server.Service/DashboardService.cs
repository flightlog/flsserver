using System;
using System.Linq;
using FLS.Data.WebApi.Dashboard;
using FLS.Data.WebApi.Flight;
using FLS.Data.WebApi.Licensing;
using FLS.Server.Data.Enums;
using FLS.Server.Data.Mapping;
using NLog;
using AircraftType = FLS.Data.WebApi.Aircraft.AircraftType;

namespace FLS.Server.Service
{
    public class DashboardService : BaseService
    {
        private readonly FlightService _flightService;
        private readonly PersonService _personService;

        public DashboardService(DataAccessService dataAccessService, IdentityService identityService, 
            FlightService flightService, PersonService personService)
            : base(dataAccessService, identityService)
        {
            _flightService = flightService;
            _personService = personService;
            Logger = LogManager.GetCurrentClassLogger();
        }
        
        public DashboardDetails GetDashboardDetails()
        {
            var dashboardDetails = new DashboardDetails();
            
            if (CurrentAuthenticatedFLSUser != null && CurrentAuthenticatedFLSUser.PersonId.HasValue)
            {
                var person = _personService.GetPerson(CurrentAuthenticatedFLSUser.PersonId.Value, false);

                if (person != null)
                {
                    var personDashboardDetails = person.ToPersonDashboardDetails();
                    dashboardDetails.PersonDashboardDetails = personDashboardDetails;
                }

                var to = DateTime.UtcNow;
                var from = to.AddMonths(-24);

                var gliderFlights = _flightService.GetFlights(flight => (flight.StartDateTime.Value >= from
                                                                         && flight.LdgDateTime.Value <= to
                                                                         &&
                                                                         flight.FlightCrews.Any(
                                                                             x =>
                                                                                 x.PersonId ==
                                                                                 CurrentAuthenticatedFLSUser.PersonId
                                                                                     .Value && (x.FlightCrewTypeId == (int)FlightCrewType.PilotOrStudent
                                                                                     || x.FlightCrewTypeId == (int)FlightCrewType.FlightInstructor))
                                                                         &&
                                                                         flight.FlightAircraftType ==
                                                                         (int) FlightAircraftTypeValue.GliderFlight),
                    includeTowFlight: false);

                var safetyDetails = new SafetyDashboardDetails();
                safetyDetails.StatisticBasedOnLastMonths = 6;

                var statistic = new FlightStatisticDashboardDetails();
                statistic.FlightStatisticName = "Gliderflights";

                foreach (var gliderFlight in gliderFlights)
                {
                    var keyDate = new DateTime(gliderFlight.StartDateTime.Value.Year, gliderFlight.StartDateTime.Value.Month, 1);
                    if (statistic.MonthlyFlightHours.ContainsKey(keyDate) == false)
                    {
                        statistic.MonthlyFlightHours.Add(keyDate, 0);
                    }

                    if (statistic.MonthlyLandings.ContainsKey(keyDate) == false)
                    {
                        statistic.MonthlyLandings.Add(keyDate, 0);
                    }

                    //TODO: splitted flight time calculation
                    statistic.MonthlyFlightHours[keyDate] += gliderFlight.Duration.TotalHours;
                    statistic.MonthlyLandings[keyDate] += gliderFlight.NrOfLdgs.GetValueOrDefault(0);

                    if (gliderFlight.StartDateTime.Value >= to.AddMonths(-safetyDetails.StatisticBasedOnLastMonths))
                    {
                        safetyDetails.Starts += gliderFlight.NrOfLdgs.GetValueOrDefault(0);
                        safetyDetails.FlightTimeInHours += gliderFlight.Duration.TotalHours;
                    }
                }

                dashboardDetails.SafetyDashboardDetails = safetyDetails;
                dashboardDetails.GliderPilotFlightStatisticDashboardDetails = statistic;


                var motorFlights = _flightService.GetFlights(flight => (flight.StartDateTime.Value >= from
                                               && flight.LdgDateTime.Value <= to
                                              && flight.FlightCrews.Any(x => x.PersonId == CurrentAuthenticatedFLSUser.PersonId.Value && x.FlightCrewTypeId == (int)FlightCrewType.PilotOrStudent)
                                              && (flight.FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight
                                              || flight.FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight)), includeTowFlight:false);

                statistic = new FlightStatisticDashboardDetails();
                statistic.FlightStatisticName = "Motorflights";

                foreach (var motorFlight in motorFlights)
                {
                    var keyDate = new DateTime(motorFlight.StartDateTime.Value.Year, motorFlight.StartDateTime.Value.Month, 1);
                    if (statistic.MonthlyFlightHours.ContainsKey(keyDate) == false)
                    {
                        statistic.MonthlyFlightHours.Add(keyDate, 0);
                    }

                    if (statistic.MonthlyLandings.ContainsKey(keyDate) == false)
                    {
                        statistic.MonthlyLandings.Add(keyDate, 0);
                    }

                    //TODO: splitted flight time calculation
                    statistic.MonthlyFlightHours[keyDate] += motorFlight.Duration.TotalHours;
                    statistic.MonthlyLandings[keyDate] += motorFlight.NrOfLdgs.GetValueOrDefault(0);
                }

                dashboardDetails.MotorPilotFlightStatisticDashboardDetails = statistic;
            }

            return dashboardDetails;
        }
    }
}
