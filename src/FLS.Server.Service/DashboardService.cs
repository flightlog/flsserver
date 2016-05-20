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

                var now = DateTime.UtcNow;
                var from = now.AddMonths(-24);

                var gliderFlights = _flightService.GetFlights(flight => (flight.StartDateTime.Value >= from
                                                                         && flight.LdgDateTime.Value <= now
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
                statistic.StatisticStartDateTime = from;
                statistic.StatisticEndDateTime = now;

                var gliderLicenceStateDetails = new GliderLicenceStateDetails();
                gliderLicenceStateDetails.FlightTimeInHoursRequired = 5;
                gliderLicenceStateDetails.LandingsRequired = 15;
                gliderLicenceStateDetails.NumberOfCheckFlightsRequired = 2;
                gliderLicenceStateDetails.LastMonthsCount = 24;

                var gliderLicenceStateForecast = new GliderLicenceStateDetails();
                gliderLicenceStateForecast.FlightTimeInHoursRequired = 5;
                gliderLicenceStateForecast.LandingsRequired = 15;
                gliderLicenceStateForecast.NumberOfCheckFlightsRequired = 2;
                gliderLicenceStateForecast.LastMonthsCount = 22;

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
                    statistic.TotalFlightHours += gliderFlight.Duration.TotalHours;
                    statistic.TotalLandings += gliderFlight.NrOfLdgs.GetValueOrDefault(0);

                    if (gliderFlight.StartDateTime.Value >= now.AddMonths(-safetyDetails.StatisticBasedOnLastMonths))
                    {
                        safetyDetails.Starts += gliderFlight.NrOfLdgs.GetValueOrDefault(0);
                        safetyDetails.FlightTimeInHours += gliderFlight.Duration.TotalHours;
                    }

                    if (gliderFlight.FlightType != null && gliderFlight.FlightType.IsCheckFlight)
                    {
                        gliderLicenceStateDetails.NumberOfCheckFlights++;
                    }

                    if (gliderFlight.StartDateTime.Value >= now.AddMonths(-gliderLicenceStateForecast.LastMonthsCount))
                    {
                        gliderLicenceStateForecast.Landings += gliderFlight.NrOfLdgs.GetValueOrDefault(0);
                        gliderLicenceStateForecast.FlightTimeInHours += gliderFlight.Duration.TotalHours;
                    }
                }

                dashboardDetails.SafetyDashboardDetails = safetyDetails;
                dashboardDetails.GliderPilotFlightStatisticDashboardDetails = statistic;

                gliderLicenceStateDetails.FlightTimeInHours = statistic.TotalFlightHours;
                gliderLicenceStateDetails.Landings = statistic.TotalLandings;

                //check licence state
                if (gliderLicenceStateDetails.FlightTimeInHours < gliderLicenceStateDetails.FlightTimeInHoursRequired
                    || gliderLicenceStateDetails.Landings < gliderLicenceStateDetails.LandingsRequired
                    ||
                    gliderLicenceStateDetails.NumberOfCheckFlights <
                    gliderLicenceStateDetails.NumberOfCheckFlightsRequired)
                {
                    gliderLicenceStateDetails.LicenceStateInformation = "EASA Segelflug-Lizenz nicht gültig (Fluglehrer kontaktieren)";
                    gliderLicenceStateDetails.LicenceStateKey = "NotOK";
                }
                else if (gliderLicenceStateForecast.FlightTimeInHours < gliderLicenceStateForecast.FlightTimeInHoursRequired
                    || gliderLicenceStateForecast.Landings < gliderLicenceStateForecast.LandingsRequired
                    ||
                    gliderLicenceStateForecast.NumberOfCheckFlights <
                    gliderLicenceStateForecast.NumberOfCheckFlightsRequired)
                {
                    gliderLicenceStateDetails.LicenceStateInformation = "EASA Segelflug-Lizenz-Status ist gefährdet";
                    gliderLicenceStateDetails.LicenceStateKey = "Warning";
                }

                dashboardDetails.GliderLicenceStateDetails = gliderLicenceStateDetails;

                var motorFlights = _flightService.GetFlights(flight => (flight.StartDateTime.Value >= from
                                               && flight.LdgDateTime.Value <= now
                                              && flight.FlightCrews.Any(x => x.PersonId == CurrentAuthenticatedFLSUser.PersonId.Value && x.FlightCrewTypeId == (int)FlightCrewType.PilotOrStudent)
                                              && (flight.FlightAircraftType == (int)FlightAircraftTypeValue.MotorFlight
                                              || flight.FlightAircraftType == (int)FlightAircraftTypeValue.TowFlight)), includeTowFlight:false);

                statistic = new FlightStatisticDashboardDetails();
                statistic.FlightStatisticName = "Motorflights";
                statistic.StatisticStartDateTime = from;
                statistic.StatisticEndDateTime = now;

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
                    statistic.TotalFlightHours += motorFlight.Duration.TotalHours;
                    statistic.TotalLandings += motorFlight.NrOfLdgs.GetValueOrDefault(0);
                }

                dashboardDetails.MotorPilotFlightStatisticDashboardDetails = statistic;
            }

            return dashboardDetails;
        }
    }
}
