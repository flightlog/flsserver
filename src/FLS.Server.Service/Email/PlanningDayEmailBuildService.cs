using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using Alpinely.TownCrier;
using FLS.Common.Extensions;
using FLS.Common.Validators;
using FLS.Data.WebApi.AircraftReservation;
using FLS.Data.WebApi.PlanningDay;
using FLS.Server.Data.DbEntities;
using FLS.Server.Service.Email.Model;

namespace FLS.Server.Service.Email
{
    public class PlanningDayEmailBuildService : EmailBuildService
    {
        public PlanningDayEmailBuildService(DataAccessService dataAccessService, 
            IEmailSendService emailSendService, TemplateService templateService) 
            : base(dataAccessService, emailSendService, templateService)
        {
        }

        public MailMessage CreatePlanningDayTakesPlaceEmail(PlanningDayOverview planningDayOverview, 
            List<AircraftReservationOverview> reservations, string recipientMailAddresses, Guid clubId)
        {
            try
            {
                planningDayOverview.ArgumentNotNull("planningDayOverview");
                reservations.ArgumentNotNull("reservations");

                var factory = new MergedEmailFactory(new VelocityTemplateParser("PlanningDayInfoModel"));

                var planningDayInfoModel = planningDayOverview.ToPlanningDayInfoModel(SystemData, reservations);

                var tokenValues = new Dictionary<string, object>
                {
                    {"PlanningDayInfoModel", planningDayInfoModel}
                };

                return BuildEmail("planningday-ok", factory, tokenValues, recipientMailAddresses.FormatMultipleEmailAddresses(), clubId);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error while trying to create planning day reservation email. Error: {e.Message}");
                throw;
            }
        }

        public MailMessage CreatePlanningDayNoReservationsEmail(PlanningDayOverview planningDayOverview, string recipientMailAddresses, Guid clubId)
        {
            try
            {
                planningDayOverview.ArgumentNotNull("planningDay");

                var factory = new MergedEmailFactory(new VelocityTemplateParser("PlanningDayInfoModel"));

                var planningDayInfoModel = planningDayOverview.ToPlanningDayInfoModel(SystemData, null);

                var tokenValues = new Dictionary<string, object>
                {
                    {"PlanningDayInfoModel", planningDayInfoModel}
                };

                return BuildEmail("planningday-cancel", factory, tokenValues, recipientMailAddresses.FormatMultipleEmailAddresses(), clubId);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error while trying to create planning day no reservation email. Error: {e.Message}");
                throw;
            }
        }

        public MailMessage CreatePlanningDayAssignmentNotificationEmail(PlanningDayAssignment planningDayAssignment, Guid clubId)
        {
            try
            {
                planningDayAssignment.ArgumentNotNull("planningDayAssignment");

                var factory = new MergedEmailFactory(new VelocityTemplateParser("PlanningDayAssignmentModel"));

                var planningDayAssignmentModel = new PlanningDayAssignmentModel()
                {
                    Date = planningDayAssignment.AssignedPlanningDay.Day.ToShortDateString(),
                    LocationName = planningDayAssignment.AssignedPlanningDay.Location.LocationName,
                    Remarks = planningDayAssignment.AssignedPlanningDay.Remarks,
                    AssignedPersonName = planningDayAssignment.AssignedPerson.Firstname,
                    AssignmentTypeName = planningDayAssignment.AssignmentType.AssignmentTypeName,
                    FLSUrl = SystemData.BaseURL,
                    SenderName = SystemData.SystemSenderEmailAddress
                };

                var tokenValues = new Dictionary<string, object>
                {
                    {"PlanningDayAssignmentModel", planningDayAssignmentModel}
                };

                return BuildEmail("planningday-assignment-notification", factory, tokenValues, planningDayAssignment.AssignedPerson.EmailAddressForCommunication.FormatMultipleEmailAddresses(), clubId);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"Error while trying to create planning day assignment notification email. Error: {e.Message}");
                throw;
            }
        }
    }
}
