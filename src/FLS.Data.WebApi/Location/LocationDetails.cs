using System;
using System.ComponentModel.DataAnnotations;
using FLS.Common.Validators;

namespace FLS.Data.WebApi.Location
{
    public class LocationDetails : FLSBaseData
    {

        public Guid LocationId { get; set; }

        //[StringLength(50, ErrorMessage = ConstantUIStrings.MaxLength50)]
        //[Display(Name = ConstantUIStrings.AirportFrequency)]
        [StringLength(50)]
        public string AirportFrequency { get; set; }
        
        //[CustomValidation(typeof(LocationValidator), "ValidateLocationHasCountry")]
        //[Display(Name = ConstantUIStrings.Country)]
        [Required]
        [GuidNotEmptyValidator]
        public Guid CountryId { get; set; }

        public string Description { get; set; }

        //[Display(Name = ConstantUIStrings.Elevation)]
        public Nullable<int> Elevation { get; set; }

        public Nullable<int> ElevationUnitType { get; set; }

        //[StringLength(10, ErrorMessage = ConstantUIStrings.MaxLength10)]
        //[Display(Name = ConstantUIStrings.IcaoCode)]
        [StringLength(10)]
        public string IcaoCode { get; set; }

        //[RegularExpression("[0-9]{4}[.][0-9]{3}[S,N]", ErrorMessage = ConstantUIStrings.RegexLatitude)]
        //[StringLength(10, ErrorMessage = ConstantUIStrings.MaxLength10)]
        //[Display(Name = ConstantUIStrings.Latitude)]
        [StringLength(10)]
        public string Latitude { get; set; }

        //[Display(Name = ConstantUIStrings.LocationName)]
        //[Required(ErrorMessage = ConstantUIStrings.FieldRequired)]
        //[StringLength(100, ErrorMessage = ConstantUIStrings.MaxLength100)]
        [Required]
        [StringLength(100)]
        public string LocationName { get; set; }

        //[StringLength(5, ErrorMessage = ConstantUIStrings.MaxLength5)]
        [StringLength(50)]
        public string LocationShortName { get; set; }

        //[CustomValidation(typeof(LocationValidator), "ValidateLocationHasLocationType")]
        //[Display(Name = ConstantUIStrings.LocationType)]
        [Required]
        [GuidNotEmptyValidator]
        public Guid LocationTypeId { get; set; }

        //[RegularExpression("[0-9]{5}[.][0-9]{3}[E,W]", ErrorMessage = ConstantUIStrings.RegexLongitude)]
        //[StringLength(10, ErrorMessage = ConstantUIStrings.MaxLength10)]
        //[Display(Name = ConstantUIStrings.Longitude)]
        [StringLength(10)]
        public string Longitude { get; set; }

        //not implemented for the first version
        //public Nullable<int> OrderNumber { get; set; }

        //[StringLength(50, ErrorMessage = ConstantUIStrings.MaxLength50)]
        //[Display(Name = ConstantUIStrings.RunwayDirection)]
        [StringLength(50)]
        public string RunwayDirection { get; set; }

        //[Display(Name = ConstantUIStrings.RunwayLength)]
        public Nullable<int> RunwayLength { get; set; }

        public Nullable<int> RunwayLengthUnitType { get; set; }

        public bool IsInboundRouteRequired { get; set; }

        public bool IsOutboundRouteRequired { get; set; }

        public override Guid Id
        {
            get { return LocationId; }
            set { LocationId = value; }
        }
    }
}
