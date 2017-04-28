using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Validators;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service.Email;
using NLog;
using Quartz;

namespace FLS.Server.Service.Jobs
{
    /// <summary>
    /// The job for the quartz.net scheduler (http://quartznet.sourceforge.net/) which sends the daily reports to the pilots.
    /// Every time, the scheduler execute this job a new instance of this class is created.
    /// </summary>
    public class DailyReportJob : IJob
    {
        private readonly FlightService _flightService;
        private readonly ClubService _clubService;
        private readonly PersonService _personService;
        private readonly FlightInformationEmailBuildService _flightInformationEmailService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }
        
        public DailyReportJob(FlightService flightService, ClubService clubService,
            PersonService personService,
            FlightInformationEmailBuildService flightInformationEmailService)
        {
            _flightService = flightService;
            _clubService = clubService;
            _personService = personService;
            _flightInformationEmailService = flightInformationEmailService;
        }

        /// <summary>
        /// Every time when the scheduler executes a job this method is called.
        /// </summary>
        /// <param name="context">not used</param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Logger.Info("Executing daily report job");

                //get all clubs to iterate through each club then
                var clubs = _clubService.GetClubs();

                foreach (var club in clubs)
                {
                    Logger.Info($"Executing daily report job for club: {club.Clubname}");

                    //get all flights related to this club
                    var flights = _flightService.GetFlightsCreatedOrValidatedToday(club.ClubId);

                    Logger.Info($"{flights.Count} flights found for club: {club.Clubname}");

                    //prepare a dictionary with persons and the related flights
                    var personFlightList = CreatePersonFlightList(flights);

                    if (personFlightList != null)
                    {
                        foreach (var person in personFlightList.Keys)
                        {
                            //get person details to have the information if we need to send the flight report to this person
                            var personDetails = _personService.GetPilotPersonDetailsInternal(person.PersonId,
                                                                                             club.ClubId, false);

                            if (personDetails.ClubRelatedPersonDetails != null
                                && personDetails.ClubRelatedPersonDetails.ReceiveFlightReports)
                            {
                                PrepareAndSendFlightReports(personFlightList[person], person);
                            }
                            else
                            {
                                Logger.Info($"Club related details not available or ReceiveFlightReports for person {person.DisplayName} not set. Did not send email with flight report to person!");
                            }
                        }
                    }
                    else
                    {
                        Logger.Error($"Could not built person list for flights for club: {club.Clubname}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while executing daily flight report workflow job. Error: {ex.Message}");
            }
        }

        private Dictionary<Person, List<Flight>> CreatePersonFlightList(List<Flight> flights)
        {
            flights.ArgumentNotNull("flights");

            if (flights.Any() == false)
            {
                Logger.Info("No flight reports to send by email");
                return null;
            }

            var personFlightList = new Dictionary<Person, List<Flight>>();

            foreach (var flight in flights)
            {
                var person = flight.Pilot.Person;
                if (person != null)
                {
                    if (personFlightList.ContainsKey(person) == false)
                    {
                        var flightList = new List<Flight>();
                        flightList.Add(flight);
                        personFlightList.Add(person, flightList);
                    }
                    else
                    {
                        personFlightList[person].Add(flight);
                    }
                }

                if (flight.CoPilot != null)
                {
                    person = flight.CoPilot.Person;

                    if (person != null)
                    {
                        if (personFlightList.ContainsKey(person) == false)
                        {
                            var flightList = new List<Flight>();
                            flightList.Add(flight);
                            personFlightList.Add(person, flightList);
                        }
                        else
                        {
                            personFlightList[person].Add(flight);
                        }
                    }
                }

                if (flight.Instructor != null)
                {
                    person = flight.Instructor.Person;

                    if (person != null)
                    {
                        if (personFlightList.ContainsKey(person) == false)
                        {
                            var flightList = new List<Flight>();
                            flightList.Add(flight);
                            personFlightList.Add(person, flightList);
                        }
                        else
                        {
                            personFlightList[person].Add(flight);
                        }
                    }
                }
            }

            return personFlightList;
        }

        /// <summary>
        /// Fetches all todays flights of pilots, copilots and instructors and send the flights to them.
        /// </summary>
        /// <param name="flights">The flights.</param>
        /// <param name="person">The person.</param>
        private void PrepareAndSendFlightReports(List<Flight> flights, Person person)
        {
            try
            {
                var message = _flightInformationEmailService.CreateFlightReportEmail(flights, person);
                _flightInformationEmailService.SendEmail(message);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while trying to create or send flight information email. Message: {ex.Message}");
            }
        }
    }
}