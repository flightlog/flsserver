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

                    if (gliderFlight.IsSoloFlight &&
                        gliderFlight.FlightCrews.Any(x => x.PersonId == CurrentAuthenticatedFLSUser.PersonId.Value
                                                          &&
                                                          (x.FlightCrewTypeId == (int) FlightCrewType.FlightInstructor)))
                    {
                        //do not count solo flights of trainee or pilots in role of instructor
                    }
                    else
                    { 
                        statistic.MonthlyFlightHours[keyDate] += gliderFlight.FlightDurationZeroBased.TotalHours;
                        statistic.MonthlyLandings[keyDate] += gliderFlight.NrOfLdgs.GetValueOrDefault(0);
                        statistic.TotalFlightHours += gliderFlight.FlightDurationZeroBased.TotalHours;
                        statistic.TotalLandings += gliderFlight.NrOfLdgs.GetValueOrDefault(0);

                        if (gliderFlight.StartDateTime.Value >= now.AddMonths(-safetyDetails.StatisticBasedOnLastMonths))
                        {
                            safetyDetails.Starts += gliderFlight.NrOfLdgs.GetValueOrDefault(0);
                            safetyDetails.FlightTimeInHours += gliderFlight.FlightDurationZeroBased.TotalHours;
                        }
                    }

                    if (gliderFlight.FlightType != null && gliderFlight.FlightType.IsCheckFlight 
                        && gliderFlight.FlightCrews.Any(x => x.PersonId == CurrentAuthenticatedFLSUser.PersonId.Value 
                            && (x.FlightCrewTypeId == (int)FlightCrewType.PilotOrStudent)))
                    {
                        gliderLicenceStateDetails.NumberOfCheckFlights++;
                    }

                    if (gliderFlight.StartDateTime.Value >= now.AddMonths(-gliderLicenceStateForecast.LastMonthsCount))
                    {
                        if (gliderFlight.IsSoloFlight &&
                            gliderFlight.FlightCrews.Any(x => x.PersonId == CurrentAuthenticatedFLSUser.PersonId.Value
                                                              &&
                                                              (x.FlightCrewTypeId ==
                                                               (int) FlightCrewType.FlightInstructor)))
                        {
                            //do not count solo flights of trainee or pilots in role of instructor
                        }
                        else
                        {
                            gliderLicenceStateForecast.Landings += gliderFlight.NrOfLdgs.GetValueOrDefault(0);
                            gliderLicenceStateForecast.FlightTimeInHours += gliderFlight.FlightDurationZeroBased.TotalHours;
                        }

                        if (gliderFlight.FlightType != null && gliderFlight.FlightType.IsCheckFlight
                        && gliderFlight.FlightCrews.Any(x => x.PersonId == CurrentAuthenticatedFLSUser.PersonId.Value
                            && (x.FlightCrewTypeId == (int)FlightCrewType.PilotOrStudent)))
                        {
                            gliderLicenceStateForecast.NumberOfCheckFlights++;
                        }
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
                else if (gliderLicenceStateForecast.FlightTimeInHours <
                         gliderLicenceStateForecast.FlightTimeInHoursRequired
                         || gliderLicenceStateForecast.Landings < gliderLicenceStateForecast.LandingsRequired
                         ||
                         gliderLicenceStateForecast.NumberOfCheckFlights <
                         gliderLicenceStateForecast.NumberOfCheckFlightsRequired)
                {
                    gliderLicenceStateDetails.LicenceStateInformation = "EASA Segelflug-Lizenz-Status ist gefährdet";
                    gliderLicenceStateDetails.LicenceStateKey = "Warning";
                }
                else
                {
                    gliderLicenceStateDetails.LicenceStateInformation = "EASA Segelflug-Lizenz-Status ist OK";
                    gliderLicenceStateDetails.LicenceStateKey = "OK";
                }

                if (dashboardDetails.PersonDashboardDetails.MedicalLaplExpireDate.HasValue == false
                    && dashboardDetails.PersonDashboardDetails.MedicalClass2ExpireDate.HasValue == false)
                {
                    //no medical data
                    gliderLicenceStateDetails.LicenceStateInformation = $"{gliderLicenceStateDetails.LicenceStateInformation}, aber es sind keine Medical-Daten vorhanden!";
                    gliderLicenceStateDetails.LicenceStateKey = "Warning";
                }
                else if ((dashboardDetails.PersonDashboardDetails.MedicalLaplExpireDate.HasValue &&
                         dashboardDetails.PersonDashboardDetails.MedicalLaplExpireDate.Value <= now)
                         || (dashboardDetails.PersonDashboardDetails.MedicalClass2ExpireDate.HasValue &&
                         dashboardDetails.PersonDashboardDetails.MedicalClass2ExpireDate.Value <= now))
                {
                    //minimum one medical has been expired
                    gliderLicenceStateDetails.LicenceStateInformation = $"{gliderLicenceStateDetails.LicenceStateInformation}. Medical abgelaufen!";
                    gliderLicenceStateDetails.LicenceStateKey = "NotOK";
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
                    statistic.MonthlyFlightHours[keyDate] += motorFlight.FlightDurationZeroBased.TotalHours;
                    statistic.MonthlyLandings[keyDate] += motorFlight.NrOfLdgs.GetValueOrDefault(0);
                    statistic.TotalFlightHours += motorFlight.FlightDurationZeroBased.TotalHours;
                    statistic.TotalLandings += motorFlight.NrOfLdgs.GetValueOrDefault(0);
                }

                dashboardDetails.MotorPilotFlightStatisticDashboardDetails = statistic;
            }

            return dashboardDetails;
        }
    }
}
