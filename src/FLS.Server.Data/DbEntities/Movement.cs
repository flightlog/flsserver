using FLS.Data.WebApi;
using FLS.Data.WebApi.Flight;
using FLS.Server.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string DeviceId { get; set; }

        public string Immatriculation { get; set; }

        public AprsAircraftType AircraftType { get; set; }

        public string LocationIcaoCode { get; set; }

        public MovementType MovementType { get; set; }

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
    }
}
