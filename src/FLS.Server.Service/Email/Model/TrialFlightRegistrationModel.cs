﻿namespace FLS.Server.Service.Email.Model
{
    public class TrialFlightRegistrationModel
    {
        public string RecipientName { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string AddressLine1 { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string PrivateEmail { get; set; }

        public string MobilePhoneNumber { get; set; }

        public string PrivatePhoneNumber { get; set; }

        public string BusinessPhoneNumber { get; set; }

        public string SelectedTrialFlightDate { get; set; }

        public string InvoiceToFirstname { get; set; }

        public string InvoiceToLastname { get; set; }

        public string InvoiceToAddressLine1 { get; set; }

        public string InvoiceToZipCode { get; set; }

        public string InvoiceToCity { get; set; }

        public string InvoiceToCountryName { get; set; }

        public string SendCouponToInformation { get; set; }

        public string WillBeContactedOnDate { get; set; }

        public string LocationName { get; set; }

        public string Remarks { get; set; }

        public string ReservationInformation { get; set; }
    }
}
