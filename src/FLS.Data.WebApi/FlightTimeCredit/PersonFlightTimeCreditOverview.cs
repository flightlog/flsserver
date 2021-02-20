using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FLS.Data.WebApi.FlightTimeCredit
{
    public class PersonFlightTimeCreditOverview : FLSBaseData
    {
        public Guid PersonFlightTimeCreditId { get; set; }

        public DateTime BalanceDateTime { get; set; }

        public bool NoFlightTimeLimit { get; set; }

        public long CurrentFlightTimeBalanceInSeconds { get; set; }

        public DateTime ValidUntil { get; set; }

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
