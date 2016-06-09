using System;
using System.ComponentModel.DataAnnotations;

namespace FLS.Data.WebApi.Aircraft
{
    public class AircraftDetails : FLSBaseData
    {
        /// <summary>
        /// Gets the Id of the object. The Id is set by the server.
        /// </summary>
        public Guid AircraftId { get; set; }

        public AircraftStateData AircraftStateData { get; set; }

        [StringLength(100)]
        public string ManufacturerName { get; set; }

        [StringLength(50)]
        public string AircraftModel { get; set; }

        [StringLength(250)]
        public string Comment { get; set; }

        [StringLength(5)]
        public string CompetitionSign { get; set; }

        public Nullable<int> DaecIndex { get; set; }

        [StringLength(50)]
        public string FLARMId { get; set; }

        [Required]
        [StringLength(15)]
        public string Immatriculation { get; set; }

        public int AircraftType { get; set; }

        public bool IsTowingAircraft { get; set; }

        public bool IsTowingOrWinchRequired { get; set; }

        public bool IsTowingstartAllowed { get; set; }

        public bool IsWinchstartAllowed { get; set; }

        public Nullable<int> NrOfSeats { get; set; }

        public Nullable<Guid> AircraftOwnerClubId { get; set; }

        public Nullable<Guid> AircraftOwnerPersonId { get; set; }

        [StringLength(20)]
        public string AircraftSerialNumber { get; set; }

        public Nullable<DateTime> YearOfManufacture { get; set; }

        [StringLength(1)]
        public string NoiseClass { get; set; }

        public Nullable<Decimal> NoiseLevel { get; set; }

        public Nullable<int> MTOM { get; set; }

        public int FlightDurationPrecision { get; set; }

        public int EngineOperatorCounterPrecision { get; set; }

        [StringLength(250)]
        public string SpotLink { get; set; }

        public override Guid Id
        {
            get { return AircraftId; }
            set { AircraftId = value; }
        }
    }
}
