﻿using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Club
{
    public class ClubDetails : FLSBaseData
    {
        public Guid ClubId { get; set; }

        [Required]
        [StringLength(10)]
        public string ClubKey { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string ClubName { get; set; }

        [StringLength(100)]
        public string ContactName { get; set; }

        public Guid CountryId { get; set; }

        public Nullable<int> DefaultStartType { get; set; }

        public Nullable<Guid> DefaultGliderFlightTypeId { get; set; }

        public Nullable<Guid> DefaultTowFlightTypeId { get; set; }

        public Nullable<Guid> DefaultMotorFlightTypeId { get; set; }

        [StringLength(256)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [StringLength(30)]
        public string FaxNumber { get; set; }
        
        public Nullable<Guid> HomebaseId { get; set; }

        public Nullable<DateTime> LastDeliverySynchronisationOn { get; set; }

        public Nullable<DateTime> LastPersonSynchronisationOn { get; set; }

        public Nullable<DateTime> LastArticleSynchronisationOn { get; set; }

        [StringLength(30)]
        public string PhoneNumber { get; set; }

        [StringLength(100)]
        public string WebPage { get; set; }

        [StringLength(10)]
        public string ZipCode { get; set; }

        /// <summary>
        /// Gets or sets to which email addresses club owned aircraft statistic reports should be send
        /// </summary>
        [StringLength(250)]
        public string SendAircraftStatisticReportTo { get; set; }

        [StringLength(250)]
        public string SendPlanningDayInfoMailTo { get; set; }

        [StringLength(250)]
        public string SendDeliveryMailExportTo { get; set; }

        [StringLength(250)]
        public string SendTrialFlightRegistrationOperatorEmailTo { get; set; }

        [StringLength(250)]
        public string SendPassengerFlightRegistrationOperatorEmailTo { get; set; }

        [StringLength(250)]
        public string ReplyToEmailAddress { get; set; }

        public bool RunDeliveryCreationJob { get; set; }

        public bool RunDeliveryMailExportJob { get; set; }

        public bool IsClubMemberNumberReadonly { get; set; }

        public override Guid Id
        {
            get { return ClubId; }
            set { ClubId = value; }
        }
    }
}
