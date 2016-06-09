using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using FLS.Data.WebApi;

namespace FLS.Server.Data.DbEntities
{
    [Table("PersonClub")]
    public partial class PersonClub : IFLSMetaData
    {
        [Key]
        [Column(Order = 0)]
        public Guid PersonId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid ClubId { get; set; }

        [StringLength(20)]
        public string MemberNumber { get; set; }

        [StringLength(40)]
        public string MemberKey { get; set; }

        public Guid? MemberStateId { get; set; }

        public bool IsMotorPilot { get; set; }

        public bool IsTowPilot { get; set; }

        public bool IsGliderInstructor { get; set; }

        public bool IsGliderPilot { get; set; }

        public bool IsGliderTrainee { get; set; }

        public bool IsPassenger { get; set; }

        public bool IsWinchOperator { get; set; }

        public bool IsMotorInstructor { get; set; }

        public bool ReceiveFlightReports { get; set; }

        public bool ReceiveAircraftReservationNotifications { get; set; }

        public bool ReceivePlanningDayRoleReminder { get; set; }

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

        public virtual Person Person { get; set; }

        public virtual MemberState MemberState { get; set; }

        public Guid Id
        {
            get { return Guid.Empty; }
            set
            {
                //Do nothing 
            }
        }

        public bool DoNotUpdateTimeStampsInMetaData { get; set; }

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
