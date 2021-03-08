using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using FLS.Data.WebApi;
using FLS.Data.WebApi.Accounting;

namespace FLS.Server.Data.DbEntities
{
    public partial class Delivery : IFLSMetaData
    {
        public Delivery()
        {
            DeliveryItems = new HashSet<DeliveryItem>();
            PersonFlightTimeCreditTransactions = new HashSet<PersonFlightTimeCreditTransaction>();
        }

        public Guid DeliveryId { get; set; }

        public Guid ClubId { get; set; }

        public Guid? FlightId { get; set; }
       
        public Guid? RecipientPersonId { get; set; }

        [StringLength(250)]
        public string RecipientName { get; set; }

        [StringLength(100)]
        public string RecipientFirstname { get; set; }

        [StringLength(100)]
        public string RecipientLastname { get; set; }

        [StringLength(200)]
        public string RecipientAddressLine1 { get; set; }

        [StringLength(200)]
        public string RecipientAddressLine2 { get; set; }

        [StringLength(10)]
        public string RecipientZipCode { get; set; }

        [StringLength(100)]
        public string RecipientCity { get; set; }

        [StringLength(100)]
        public string RecipientCountryName { get; set; }

        [StringLength(20)]
        public string RecipientPersonClubMemberNumber { get; set; }

        [StringLength(250)]
        public string DeliveryInformation { get; set; }

        [StringLength(250)]
        public string AdditionalInformation { get; set; }

        public string DeliveryNumber { get; set; }

        /// <summary>
        /// Delivery date and in case of a flight, the flight date.
        /// </summary>
        public DateTime? DeliveredOn { get; set; }

        public bool IsFurtherProcessed { get; set; }

        public long BatchId { get; set; }

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

        public virtual Club Club { get; set; }

        public virtual Flight Flight { get; set; }

        public virtual ICollection<DeliveryItem> DeliveryItems { get; set; }

        public virtual ICollection<PersonFlightTimeCreditTransaction> PersonFlightTimeCreditTransactions { get; set; }

        public Guid Id
        {
            get { return DeliveryId; }
            set { DeliveryId = value; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Type type = GetType();
            sb.Append("[");
            sb.Append(type.Name);
            sb.Append(" -> ");
            foreach (FieldInfo info in type.GetFields())
            {
                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this)));
            }

            Type tColl = typeof(ICollection<>);
            foreach (PropertyInfo info in type.GetProperties())
            {
                Type t = info.PropertyType;
                if (t.IsGenericType && tColl.IsAssignableFrom(t.GetGenericTypeDefinition()) ||
                    t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl)
                    || (t.Namespace != null && t.Namespace.Contains("FLS.Server.Data.DbEntities")))
                {
                    continue;
                }

                sb.Append(string.Format("{0}: {1}, ", info.Name, info.GetValue(this, null)));
            }

            sb.Append(" <- ");
            sb.Append(type.Name);
            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}
