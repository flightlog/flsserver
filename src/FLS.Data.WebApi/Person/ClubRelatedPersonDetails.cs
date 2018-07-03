using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Person
{
    public class ClubRelatedPersonDetails
    {
        public ClubRelatedPersonDetails()
        {
            PersonCategoryIds = new List<Guid>();
            IsMemberNumberReadonly = false;
        }

        public ClubRelatedPersonDetails(ClubRelatedPersonDetails clubRelatedPersonDetails)
        {
            PersonCategoryIds = clubRelatedPersonDetails.PersonCategoryIds;
            MemberStateId = clubRelatedPersonDetails.MemberStateId;
            MemberNumber = clubRelatedPersonDetails.MemberNumber;
            IsGliderTrainee = clubRelatedPersonDetails.IsGliderTrainee;
            IsGliderInstructor = clubRelatedPersonDetails.IsGliderInstructor;
            IsMotorInstructor = clubRelatedPersonDetails.IsMotorInstructor;
            IsGliderPilot = clubRelatedPersonDetails.IsGliderPilot;
            IsPassenger = clubRelatedPersonDetails.IsPassenger;
            IsMotorPilot = clubRelatedPersonDetails.IsMotorPilot;
            IsTowPilot = clubRelatedPersonDetails.IsTowPilot;
            IsWinchOperator = clubRelatedPersonDetails.IsWinchOperator;
            ReceiveAircraftReservationNotifications = clubRelatedPersonDetails.ReceiveAircraftReservationNotifications;
            ReceiveFlightReports = clubRelatedPersonDetails.ReceiveFlightReports;
            ReceivePlanningDayRoleReminder = clubRelatedPersonDetails.ReceivePlanningDayRoleReminder;
            IsActive = clubRelatedPersonDetails.IsActive;
            IsMemberNumberReadonly = clubRelatedPersonDetails.IsMemberNumberReadonly;
        }
        
        public List<Guid> PersonCategoryIds { get; set; }

        public Guid? MemberStateId { get; set; }

        public string MemberNumber { get; set; }

        public bool IsGliderInstructor { get; set; }

        public bool IsGliderPilot { get; set; }

        public bool IsGliderTrainee { get; set; }

        public bool IsMotorPilot { get; set; }

        public bool IsPassenger { get; set; }

        public bool IsTowPilot { get; set; }

        public bool IsWinchOperator { get; set; }

        public bool IsMotorInstructor { get; set; }

        public bool ReceiveFlightReports { get; set; }

        public bool ReceiveAircraftReservationNotifications { get; set; }

        public bool ReceivePlanningDayRoleReminder { get; set; }

        public bool IsActive { get; set; }

        public bool IsMemberNumberReadonly { get; set; }
    }
}
