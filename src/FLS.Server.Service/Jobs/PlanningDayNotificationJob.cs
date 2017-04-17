using System;
using System.Linq;
using System.Net.Mail;
using FLS.Server.Service.Email;
using NLog;
using Quartz;

namespace FLS.Server.Service.Jobs
{
    /// <summary>
    /// The job for the quartz.net scheduler (http://quartznet.sourceforge.net/) which sends the monthly reports to the club.
    /// Every time, the scheduler execute this job a new instance of this class is created.
    /// </summary>
    public class PlanningDayNotificationJob : IJob
    {
        private readonly ClubService _clubService;
        private readonly PlanningDayService _planningDayService;
        private readonly AircraftReservationService _aircraftReservationService;
        private readonly PlanningDayEmailBuildService _planningDayEmailService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public PlanningDayNotificationJob(ClubService clubService, PlanningDayService planningDayService,
            AircraftReservationService aircraftReservationService, PlanningDayEmailBuildService planningDayEmailService)
        {
            _clubService = clubService;
            _planningDayService = planningDayService;
            _aircraftReservationService = aircraftReservationService;
            _planningDayEmailService = planningDayEmailService;
        }

        /// <summary>
        /// Every time when the scheduler executes a job this method is called.
        /// </summary>
        /// <param name="context">not used</param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Logger.Info("Executing planning day notification job");

                var clubs = _clubService.GetClubs();

                foreach (var club in clubs.Where(c => string.IsNullOrEmpty(c.SendPlanningDayInfoMailTo) == false))
                {
                    try
                    {
                        var planningDays = _planningDayService.GetPlanningDayOverview(DateTime.Now.Date.AddDays(1), club.ClubId);

                        bool foundPlanningDays = false;

                        foreach (var planningDay in planningDays.Where(planningDay => planningDay.Day.Date == DateTime.Now.Date.AddDays(1)))
                        {
                            try
                            {
                                foundPlanningDays = true;
                                var reservations = _aircraftReservationService.GetAircraftReservationsByPlanningDayId(planningDay.PlanningDayId);


                                MailMessage message = null;
                                if (reservations.Any())
                                {
                                    //create mail with reservations
                                    message = _planningDayEmailService.CreatePlanningDayTakesPlaceEmail(planningDay, reservations, club.SendPlanningDayInfoMailTo, club.ClubId);
                                }
                                else
                                {
                                    //create cancel planning day email as no reservations have been done
                                    message = _planningDayEmailService.CreatePlanningDayNoReservationsEmail(planningDay, club.SendPlanningDayInfoMailTo, club.ClubId);
                                }

                                if (message != null)
                                {
                                    //send email
                                    _planningDayEmailService.SendEmail(message);
                                }
                                else
                                {
                                    Logger.Error(
                                        "Error while creating email message in planningDayEmailService. Email message is null");
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Error(e, $"Error while trying to create a planningday email for day: {planningDay}. Error: {e.Message}");
                            }
                        }

                        if (foundPlanningDays == false)
                        {
                            Logger.Info($"No planning days for tomorrow for club {club.Clubname} found, no email will be sent.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, $"Error while executing planning day notification job for club: {club.Clubname}. Error: {ex.Message}");
                    }
                }

                foreach (var club in clubs)
                {
                    try
                    {
                        var planningDays = _planningDayService.GetPlanningDays(club.ClubId, DateTime.Now.Date.AddDays(7));

                        foreach (var planningDay in planningDays.Where(planningDay => planningDay.Day.Date == DateTime.Now.Date.AddDays(7)))
                        {
                            foreach (var planningDayPlanningDayAssignment in planningDay.PlanningDayAssignments)
                            {
                                try
                                {
                                    MailMessage message =
                                        _planningDayEmailService.CreatePlanningDayAssignmentNotificationEmail(
                                            planningDayPlanningDayAssignment, club.ClubId);

                                    if (message != null)
                                    {
                                        //send email
                                        _planningDayEmailService.SendEmail(message);
                                    }
                                    else
                                    {
                                        Logger.Error(
                                            "Error while creating email message in planningDayEmailService. Email message is null");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Logger.Error(e,
                                        $"Error while trying to create a planningday assignment notification email for day: {planningDay}. Error: {e.Message}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, $"Error while executing planning day notification job for club: {club.Clubname}. Error: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error while executing planning day notification job. Error: {ex.Message}");
            }
        }
    }
}