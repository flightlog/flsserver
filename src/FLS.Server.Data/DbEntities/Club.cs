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
    public partial class Club : IFLSMetaData
    {
        public Club()
        {
            Aircrafts = new HashSet<Aircraft>();
            ClubExtensions = new HashSet<ClubExtension>();
            ExtensionParameterValues = new HashSet<ExtensionValue>();
            FlightTypes = new HashSet<FlightType>();
            MemberStates = new HashSet<MemberState>();
            PersonCategories = new HashSet<PersonCategory>();
            ClubPersons = new HashSet<PersonClub>();
            Users = new HashSet<User>();
            AircraftReservations = new HashSet<AircraftReservation>();
            EmailTemplates = new HashSet<EmailTemplate>();
            Articles = new HashSet<Article>();
        }

        public Guid ClubId { get; set; }

        [Required]
        [StringLength(100)]
        public string Clubname { get; set; }

        [Required]
        [StringLength(10)]
        public string ClubKey { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(10)]
        public string Zip { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        public Guid CountryId { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }

        [StringLength(30)]
        public string FaxNumber { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [StringLength(100)]
        public string WebPage { get; set; }

        [StringLength(100)]
        public string Contact { get; set; }

        public Guid? HomebaseId { get; set; }

        [Column("DefaultStartType")]
        public int? DefaultStartTypeId { get; set; }

        public Guid? DefaultGliderFlightTypeId { get; set; }

        public Guid? DefaultTowFlightTypeId { get; set; }

        public Guid? DefaultMotorFlightTypeId { get; set; }
        
        public string SendAircraftStatisticReportTo { get; set; }

        public string SendPlanningDayInfoMailTo { get; set; }

        public string SendInvoiceReportsTo { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastPersonSynchronisationOn { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastInvoiceExportOn { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastArticleSynchronisationOn { get; set; }

        public int ClubStateId { get; set; }

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

        public virtual ICollection<Aircraft> Aircrafts { get; set; }

        public virtual ICollection<ClubExtension> ClubExtensions { get; set; }

        public virtual Location Homebase { get; set; }

        public virtual Country Country { get; set; }

        public virtual FlightType DefaultGliderFlightType { get; set; }

        public virtual FlightType DefaultMotorFlightType { get; set; }

        public virtual StartType DefaultStartType { get; set; }

        public virtual FlightType DefaultTowFlightType { get; set; }

        public virtual ICollection<ExtensionValue> ExtensionParameterValues { get; set; }

        public virtual ICollection<FlightType> FlightTypes { get; set; }

        public virtual ICollection<MemberState> MemberStates { get; set; }

        public virtual ICollection<PersonCategory> PersonCategories { get; set; }

        public virtual ICollection<PersonClub> ClubPersons { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<PlanningDay> PlanningDays { get; set; }

        public virtual ICollection<PlanningDayAssignmentType> PlanningDayAssignmentTypes { get; set; }

        public virtual ICollection<AircraftReservation> AircraftReservations { get; set; }

        public virtual ICollection<EmailTemplate> EmailTemplates { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public virtual ClubState ClubState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [do not update meta data].
        /// Used for workflow processes to not create a modified user error when trying to save records.
        /// </summary>
        /// <value>
        /// <c>true</c> if [do not update meta data]; otherwise, <c>false</c>.
        /// </value>
        [NotMapped]
        public bool DoNotUpdateMetaData { get; set; }

        public Guid Id
        {
            get { return ClubId; }
            set { ClubId = value; }
        }

        public string HomebaseName
        {
            get
            {
                if (Homebase != null)
                {
                    return Homebase.LocationName;
                }

                return string.Empty;
            }
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
