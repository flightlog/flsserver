using System;

namespace FLS.Data.WebApi.Location
{
    public class CountryOverview : FLSBaseData
    {

        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the country id iso.
        /// <remarks>Using ISO-3166 numeric coding</remarks>
        /// </summary>
        /// <value>
        /// The country number in ISO 3166 numeric format.
        /// </value>
        public int CountryIdIso { get; set; }

        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// <remarks>Using ISO-3166-Alpha-2 coding</remarks>
        /// </summary>
        /// <value>
        /// The country code in ISO 3166 Alpha-2 format.
        /// </value>
        public string CountryCode { get; set; }
        
        public override Guid Id
        {
            get { return CountryId; }
            set { CountryId = value; }
        }
    }
}
