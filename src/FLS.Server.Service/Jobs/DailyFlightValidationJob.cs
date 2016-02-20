using System;
using FLS.Server.Data;
using FLS.Server.Data.Mapping;
using NLog;
using Quartz;

namespace FLS.Server.Service.Jobs
{
    /// <summary>
    /// The job for the quartz.net scheduler (http://quartznet.sourceforge.net/) which validates the flights daily.
    /// Every time, the scheduler execute this job a new instance of this class is created.
    /// </summary>
    public class DailyFlightValidationJob : IJob
    {
        private readonly FlightService _flightService;
        private readonly ClubService _clubService;
        private readonly DataAccessService _dataAccessService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public DailyFlightValidationJob(FlightService flightService, ClubService clubService, 
            DataAccessService dataAccessService)
        {
            _flightService = flightService;
            _clubService = clubService;
            _dataAccessService = dataAccessService;
        }

        /// <summary>
        /// Every time when the scheduler executes a job this method is called.
        /// </summary>
        /// <param name="context">not used</param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Logger.Info("Executing daily flight validation job");

                //get all clubs to iterate through each club then
                var clubs = _clubService.GetClubs();

                foreach (var club in clubs)
                {
                    ProcessFlightValidation(club.ClubId);
                    ProcessFlightsForLocking(club.ClubId);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while executing daily workflow job. Error: {ex.Message}");
            }
        }
        
        private void ProcessFlightValidation(Guid clubId)
        {
            try
            {
                var todaysFlights = _flightService.GetFlightsForValidating(clubId);
                
                using (var context = _dataAccessService.CreateDbContext())
                {
                    foreach (var flight in todaysFlights)
                    {
                        context.Flights.Attach(flight);

                        flight.ValidateFlight();
                        flight.DoNotUpdateMetaData = true;

                        Logger.Info(
                            string.Format(
                                "The currently validated flight {0} has now the following Flight-State: {1} ({2})",
                                flight,
                                flight.FlightState, ((FLS.Data.WebApi.Flight.FlightState)flight.FlightStateId).ToFlightStateName()));
                    }

                    context.SaveChanges();
                }

                Logger.Info(string.Format("Saved the validated flights of today to database."));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while processing today flights for validating. Error: {ex.Message}");
            }
        }

        private void ProcessFlightsForLocking(Guid clubId)
        {
            try
            {
                var flights = _flightService.GetValidFlightsForLocking(clubId);

                using (var context = _dataAccessService.CreateDbContext())
                {
                    foreach (var flight in flights)
                    {
                        context.Flights.Attach(flight);

                        flight.FlightStateId = (int)FLS.Data.WebApi.Flight.FlightState.Locked;

                        Logger.Info(string.Format("The valid flight {0} has now been locked.", flight));
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while processing flights for locking. Error: {ex.Message}");
            }
        }
    }
}