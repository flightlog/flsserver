using FLS.Data.WebApi;
using FLS.Data.WebApi.Flight;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLS.Server.Data.DbEntities
{
    public class Movement : IFLSMetaData
    {
        public Movement()
        {
        }

        public Guid MovementId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime MovementDateTimeUtc { get; set; }

        [StringLength(20)]
        public string DeviceId { get; set; }

        [StringLength(15)]
        public string Immatriculation { get; set; }

        /// <summary>
        /// Gets or sets the aircraft type according the APRS message using enum <seealso cref="AprsAircraftType"/>
        /// </summary>
        public int AircraftType { get; set; }

        [StringLength(10)]
        public string LocationIcaoCode { get; set; }

        /// <summary>
        /// Gets or sets the movement type (take off or landing) according the FLS OGN Analyser.
        /// </summary>
        public int MovementType { get; set; }

        public Guid? FlightId { get; set; }

        /// <summary>
        /// JSON serialized FlightId's which has been found during processing TakeOff or Landing!
        /// Should be NULL or empty, as we can not handle multiple flights for one movement!
        /// </summary>
        public string FurtherFlightIdsFound { get; set; }

        public virtual Flight Flight { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedByUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedByUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletedOn { get; set; }

        public Guid? DeletedByUserId { get; set; }

        public int? RecordState { get; set; }

        public Guid OwnerId { get; set; }

        public int OwnershipType { get; set; }

        public bool IsDeleted { get; set; }


        public Guid Id
        {
            get { return MovementId; }
            set { MovementId = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [do not update meta data].
        /// Used for workflow processes to not create a modified user error when trying to save records.
        /// </summary>
        /// <value>
        /// <c>true</c> if [do not update meta data]; otherwise, <c>false</c>.
        /// </value>
        public bool DoNotUpdateMetaData { get; set; }
    }
}
