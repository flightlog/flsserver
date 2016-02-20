using System;
using System.Collections.Generic;

namespace FLS.Server.Service.Invoicing
{
    public class AdditionalFuelFeeRule
    {
        public bool UseRuleForAllAircraftsExceptListed { get; set; }
        public List<Guid> AircraftIds { get; set; }
        public Guid AircraftId { get; set; }
        //public string AircraftImmatriculation { get; set; }
        public int SortIndicator { get; set; }
        public string ERPArticleNumber { get; set; }
        public string InvoiceLineText { get; set; }
        public bool UseRuleForAllStartLocationsExceptListed { get; set; }
        public List<Guid> MatchedStartLocations { get; set; }

        public AdditionalFuelFeeRule()
        {
            MatchedStartLocations = new List<Guid>();
        }
    }
}
