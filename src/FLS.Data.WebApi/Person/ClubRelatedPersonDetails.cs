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
        }
        
        public List<Guid> PersonCategoryIds { get; set; }

        public Guid? MemberStateId { get; set; }

        public string MemberNumber { get; set; }

        public string MemberKey { get; set; }

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
    }
}
