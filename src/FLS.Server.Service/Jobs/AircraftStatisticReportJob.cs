using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Data.WebApi.Reporting;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service.Email;
using NLog;
using Quartz;

namespace FLS.Server.Service.Jobs
{
    /// <summary>
    /// The job for the quartz.net scheduler (http://quartznet.sourceforge.net/) which sends the monthly reports to the club.
    /// Every time, the scheduler execute this job a new instance of this class is created.
    /// </summary>
    public class AircraftStatisticReportJob : IJob
    {
        private readonly AircraftReportEmailBuildService _aircraftReportEmailService;
        private readonly FlightService _flightService;
        private readonly AircraftService _aircraftService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public AircraftStatisticReportJob(AircraftReportEmailBuildService aircraftReportEmailService, FlightService flightService, AircraftService aircraftService)
        {
            _aircraftReportEmailService = aircraftReportEmailService;
            _flightService = flightService;
            _aircraftService = aircraftService;
        }

        /// <summary>
        /// Every time when the scheduler executes a job this method is called.
        /// </summary>
        /// <param name="context">not used</param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Logger.Info("Executing monthly report job");

                var aircrafts =
                    _aircraftService.GetAircrafts(
                        q => q.AircraftOwnerClubId.HasValue || q.AircraftOwnerPersonId.HasValue);

                var aircraftOwnerDictionary = CreateAircraftOwnerDictionary(aircrafts);

                ProcessAircraftStatisticReports(aircraftOwnerDictionary);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while executing monthly workflow job. Error: {ex.Message}");
            }
        }

        private Dictionary<string, List<Aircraft>> CreateAircraftOwnerDictionary(List<Aircraft> aircrafts)
        {
            Dictionary<string, List<Aircraft>> aircraftOwnerDictionary = new Dictionary<string, List<Aircraft>>();

            foreach (var aircraft in aircrafts)
            {
                if (aircraft.AircraftOwnerClubId.HasValue)
                {
                    if (aircraft.AircraftOwnerClub == null)
                    {
                        Logger.Warn($"Aircraft owner club is null, but AircraftOwnerClubId has still a value for aircraft: {aircraft.Immatriculation}. So may the club is flagged as deleted!");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(aircraft.AircraftOwnerClub.SendAircraftStatisticReportTo)) continue;
                    
                    if (aircraftOwnerDictionary.Keys.Contains(
                            aircraft.AircraftOwnerClub.SendAircraftStatisticReportTo) == false)
                    {
                        aircraftOwnerDictionary.Add(aircraft.AircraftOwnerClub.SendAircraftStatisticReportTo, new List<Aircraft>());
                    }

                    aircraftOwnerDictionary[aircraft.AircraftOwnerClub.SendAircraftStatisticReportTo].Add(
                            aircraft);
                }
                else if (aircraft.AircraftOwnerPersonId.HasValue)
                {
                    if (aircraft.AircraftOwnerPerson == null)
                    {
                        Logger.Warn($"Aircraft owner person is null, but AircraftOwnerPersonId has still a value for aircraft: {aircraft.Immatriculation}. So may the person is flagged as deleted!");
                        continue;    
                    }

                    if (aircraft.AircraftOwnerPerson.ReceiveOwnedAircraftStatisticReports == false) continue;

                    if (aircraftOwnerDictionary.Keys.Contains(
                            aircraft.AircraftOwnerPerson.EmailAddressForCommunication) == false)
                    {
                        aircraftOwnerDictionary.Add(aircraft.AircraftOwnerPerson.EmailAddressForCommunication, new List<Aircraft>());
                    }

                    aircraftOwnerDictionary[aircraft.AircraftOwnerPerson.EmailAddressForCommunication].Add(
                            aircraft);
                }
            }

            return aircraftOwnerDictionary;
        }

        private void ProcessAircraftStatisticReports(Dictionary<string, List<Aircraft>> aircraftOwnerDictionary)
        {
            try
            {
                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
                var endDate = startDate.AddMonths(1).AddTicks(-1);

                var filterCriteria = new AircraftFlightReportFilterCriteria();
                filterCriteria.StatisticStartDateTime = startDate;
                filterCriteria.StatisticEndDateTime = endDate;

                foreach (var aircraftOwner in aircraftOwnerDictionary.Keys)
                {
                    filterCriteria.AircraftIds.Clear();
                    
                    try
                    {
                        foreach (var aircraft in aircraftOwnerDictionary[aircraftOwner])
                        {
                            filterCriteria.AircraftIds.Add(aircraft.AircraftId);
                        }

                        var aircraftFlightReport = _flightService.GetAircraftFlightReport(filterCriteria);
                    
                        var message =
                            _aircraftReportEmailService.CreateAircraftStatisticInformationEmail(aircraftFlightReport,
                                                                                                aircraftOwner);
                        _aircraftReportEmailService.SendEmail(message);

                        Logger.Info($"Sent aircraft statistic report to {aircraftOwner}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, $"Error while processing aircraft statistic report and sending per email for {aircraftOwner}. Error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while processing aircraft statistic report and sending per email. Error: {ex.Message}");
            }
        }
    }
}