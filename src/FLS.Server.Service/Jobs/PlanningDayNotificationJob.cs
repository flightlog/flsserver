using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Server.Data.Resources;
using FLS.Server.Service.Email;
using NLog;

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
        private readonly SettingService _settingService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public PlanningDayNotificationJob(ClubService clubService, PlanningDayService planningDayService,
            AircraftReservationService aircraftReservationService, PlanningDayEmailBuildService planningDayEmailService,
            SettingService settingService)
        {
            _clubService = clubService;
            _planningDayService = planningDayService;
            _aircraftReservationService = aircraftReservationService;
            _planningDayEmailService = planningDayEmailService;
            _settingService = settingService;
        }

        /// <summary>
        /// Every time when the scheduler executes a job this method is called.
        /// </summary>
        public void Execute()
        {
            try
            {
                Logger.Info("Executing planning day notification job");

                var clubs = _clubService.GetClubs();

                foreach (var club in clubs.Where(c => string.IsNullOrEmpty(c.SendPlanningDayInfoMailTo) == false))
                {
                    try
                    {
                        bool useClubPlanningDayWithoutReservations = false;
                        _settingService.TryGetSettingValue(SettingKey.ClubUsePlanningDayWithoutReservations, club.ClubId, null, out useClubPlanningDayWithoutReservations);

                        var planningDays = _planningDayService.GetPlanningDayOverview(DateTime.Now.Date.AddDays(1), club.ClubId);

                        bool foundPlanningDays = false;

                        foreach (var planningDay in planningDays.Where(planningDay => planningDay.Day.Date == DateTime.Now.Date.AddDays(1)))
                        {
                            try
                            {
                                foundPlanningDays = true;
                                MailMessage message = null;

                                var reservations =
                                        _aircraftReservationService.GetAircraftReservationsByPlanningDayId(
                                            planningDay.PlanningDayId);

                                if (reservations.Any())
                                {
                                    //create mail with reservations
                                    message = _planningDayEmailService.CreatePlanningDayTakesPlaceEmail(
                                        planningDay, reservations, club.SendPlanningDayInfoMailTo, club.ClubId);
                                }
                                else if (useClubPlanningDayWithoutReservations)
                                {
                                    //create OK mail even without reservations
                                    message = _planningDayEmailService.CreatePlanningDayTakesPlaceEmail(
                                        planningDay, new List<AircraftReservationOverview>(), club.SendPlanningDayInfoMailTo, club.ClubId);

                                }
                                else
                                {
                                    //create cancel planning day email as no reservations have been done
                                    message =
                                        _planningDayEmailService.CreatePlanningDayNoReservationsEmail(planningDay,
                                            club.SendPlanningDayInfoMailTo, club.ClubId);
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
                                    if (string.IsNullOrWhiteSpace(planningDayPlanningDayAssignment.AssignedPerson.EmailAddressForCommunication))
                                    {
                                        Logger.Info($"No email address for sending an email to person {planningDayPlanningDayAssignment.AssignedPerson.DisplayName} for planning day: {planningDay.Day} in PlanningDayNotificationJob");
                                        continue;
                                    }

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
                                            $"Error while creating email message in planningDayEmailService for assigned person: {planningDayPlanningDayAssignment.AssignedPerson.DisplayName} in planning day: {planningDay.Day}");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Logger.Error(e,
                                        $"Error while trying to create a planningday assignment notification email for: {planningDayPlanningDayAssignment.AssignedPerson.DisplayName} in planning day: {planningDay.Day}. Error: {e.Message}");
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