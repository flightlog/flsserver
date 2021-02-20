using System;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.FlightTimeCredit
{
    public class PersonFlightTimeCreditDetails : FLSBaseData
    {
        public Guid PersonFlightTimeCreditId { get; set; }

        public DateTime BalanceDateTime { get; set; }

        public bool NoFlightTimeLimit { get; set; }

        public long CurrentFlightTimeBalanceInSeconds { get; set; }

        public DateTime ValidUntil { get; set; }

        public Guid PersonId { get; set; }

        public bool UseRuleForAllAircraftsExceptListed { get; set; }

        public string MatchedAircraftImmatriculations { get; set; }

        public int DiscountInPercent { get; set; }

        public override Guid Id
        {
            get { return PersonFlightTimeCreditId; }
            set { PersonFlightTimeCreditId = value; }
        }
    }
}
