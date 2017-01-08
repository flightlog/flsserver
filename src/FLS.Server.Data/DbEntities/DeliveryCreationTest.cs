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
    public partial class DeliveryCreationTest : IFLSMetaData
    {
        public Guid DeliveryCreationTestId { get; set; }

        public Guid ClubId { get; set; }

        public Guid FlightId { get; set; }

        public bool IsActive { get; set; }

        [StringLength(250)]
        public string DeliveryCreationTestName { get; set; }

        public string Description { get; set; }

        public string ExpectedDeliveryDetails { get; set; }

        public string ExpectedMatchedAccountingRuleFilterIds { get; set; }

        public bool MustNotCreateDeliveryForFlight { get; set; }

        public bool IgnoreRecipientName { get; set; }

        public bool IgnoreRecipientAddress { get; set; }

        public bool IgnoreRecipientPersonId { get; set; }

        public bool IgnoreRecipientClubMemberNumber { get; set; }

        public bool IgnoreDeliveryInformation { get; set; }

        public bool IgnoreAdditionalInformation { get; set; }

        public bool IgnoreItemPositioning { get; set; }

        public bool IgnoreItemText { get; set; }

        public bool IgnoreItemAdditionalInformation { get; set; }

        public DateTime? LastTestRunOn { get; set; }

        public bool? LastTestSuccessful { get; set; }

        public string LastTestResultMessage { get; set; }

        public string LastTestCreatedDeliveryDetails { get; set; }

        public string LastTestMatchedAccountingRuleFilterIds { get; set; }


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

        public Guid Id
        {
            get { return DeliveryCreationTestId; }
            set { DeliveryCreationTestId = value; }
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
