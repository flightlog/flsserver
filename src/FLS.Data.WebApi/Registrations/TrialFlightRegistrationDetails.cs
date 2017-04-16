using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Registrations
{
    public class TrialFlightRegistrationDetails
    {
        [Required]
        [StringLength(10)]
        public string ClubKey { get; set; }

        [Required]
        [StringLength(100)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(100)]
        public string Lastname { get; set; }
        
        [StringLength(200)]
        public string AddressLine1 { get; set; }

        [StringLength(10)]
        public string ZipCode { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        public Guid? CountryId { get; set; }

        [StringLength(256)]
        [EmailAddress]
        public string PrivateEmail { get; set; }

        [StringLength(30)]
        public string MobilePhoneNumber { get; set; }

        [StringLength(30)]
        public string PrivatePhoneNumber { get; set; }

        [StringLength(30)]
        public string BusinessPhoneNumber { get; set; }

        public bool InvoiceAddressIsSame { get; set; }

        [StringLength(100)]
        public string InvoiceToFirstname { get; set; }

        [StringLength(100)]
        public string InvoiceToLastname { get; set; }

        [StringLength(200)]
        public string InvoiceToAddressLine1 { get; set; }

        [StringLength(10)]
        public string InvoiceToZipCode { get; set; }

        [StringLength(100)]
        public string InvoiceToCity { get; set; }

        public Guid? InvoiceToCountryId { get; set; }

        public DateTime SelectedDay { get; set; }

        public bool SendCouponToInvoiceAddress { get; set; }

        public string Remarks { get; set; }
    }
}
