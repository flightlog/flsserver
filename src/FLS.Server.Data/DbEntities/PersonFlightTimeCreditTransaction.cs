using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using FLS.Data.WebApi;

namespace FLS.Server.Data.DbEntities
{
    public class PersonFlightTimeCreditTransaction : IFLSMetaData
    {
        public PersonFlightTimeCreditTransaction()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonFlightTimeCredit"/> class, 
        /// sets the properties to the same values as the object given,
        /// except for Id, BalanceDateTime (UtcNow), IsCurrent and Metadata.
        /// </summary>
        /// <param name="personFlightTimeCreditTransaction">The person flight time credit.</param>
        public PersonFlightTimeCreditTransaction(PersonFlightTimeCreditTransaction personFlightTimeCreditTransaction)
        {
            PersonFlightTimeCreditId = personFlightTimeCreditTransaction.PersonFlightTimeCreditId;
            BalancedDeliveryId = personFlightTimeCreditTransaction.BalancedDeliveryId;
            BalanceDateTime = DateTime.UtcNow;
            NoFlightTimeLimit = personFlightTimeCreditTransaction.NoFlightTimeLimit;
            CurrentFlightTimeBalanceInSeconds = personFlightTimeCreditTransaction.CurrentFlightTimeBalanceInSeconds;
            OldFlightTimeBalanceInSeconds = personFlightTimeCreditTransaction.CurrentFlightTimeBalanceInSeconds;
            FlightTimeBalanceInSeconds = personFlightTimeCreditTransaction.FlightTimeBalanceInSeconds;
        }

        public Guid PersonFlightTimeCreditTransactionId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime BalanceDateTime { get; set; }

        public bool NoFlightTimeLimit { get; set; }

        public long? CurrentFlightTimeBalanceInSeconds { get; set; }

        public long FlightTimeBalanceInSeconds { get; set; }

        public long? OldFlightTimeBalanceInSeconds { get; set; }

        public bool IsCurrent { get; set; }
        
        public Guid PersonFlightTimeCreditId { get; set; }

        public Guid? BalancedDeliveryId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedByUserId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedByUserId { get; set; }

        public Guid Id 
        {
            get { return PersonFlightTimeCreditTransactionId; }
            private set { PersonFlightTimeCreditTransactionId = value; }
        }

        [Column(TypeName = "datetime2")]
        public DateTime? DeletedOn { get; set; }

        public Guid? DeletedByUserId { get; set; }

        public int? RecordState { get; set; }

        public Guid OwnerId { get; set; }

        public int OwnershipType { get; set; }

        public bool IsDeleted { get; set; }

        public virtual PersonFlightTimeCredit PersonFlightTimeCredit { get; set; }

        public virtual Delivery BalancedDelivery { get; set; }

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
