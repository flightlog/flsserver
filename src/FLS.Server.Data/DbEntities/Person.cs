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
    [Table("Persons")]
    public partial class Person : IFLSMetaData
    {
        public Person()
        {
            ReportedAircraftAircraftStates = new HashSet<AircraftAircraftState>();
            OwnedAircrafts = new HashSet<Aircraft>();
            FlightCrews = new HashSet<FlightCrew>();
            PersonClubs = new HashSet<PersonClub>();
            PersonPersonCategories = new HashSet<PersonPersonCategory>();
            Users = new HashSet<User>();
            AircraftReservations = new HashSet<AircraftReservation>();
            SecondCrewAssignedAircraftReservations = new HashSet<AircraftReservation>();
            PlanningDayAssignments = new HashSet<PlanningDayAssignment>();
        }

        public Guid PersonId { get; set; }

        [Required]
        [StringLength(100)]
        public string Lastname { get; set; }

        [Required]
        [StringLength(100)]
        public string Firstname { get; set; }

        [StringLength(100)]
        public string Midname { get; set; }

        [StringLength(100)]
        public string CompanyName { get; set; }

        [StringLength(200)]
        public string AddressLine1 { get; set; }

        [StringLength(200)]
        public string AddressLine2 { get; set; }

        [StringLength(10)]
        public string Zip { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string Region { get; set; }

        public Guid? CountryId { get; set; }

        [StringLength(30)]
        public string PrivatePhone { get; set; }

        [StringLength(30)]
        public string MobilePhone { get; set; }

        [StringLength(30)]
        public string BusinessPhone { get; set; }

        [StringLength(30)]
        public string FaxNumber { get; set; }

        [StringLength(256)]
        public string EmailPrivate { get; set; }

        [StringLength(256)]
        public string EmailBusiness { get; set; }

        public bool PreferMailToBusinessMail { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }

        public bool HasMotorPilotLicence { get; set; }

        public bool HasTowPilotLicence { get; set; }

        public bool HasGliderInstructorLicence { get; set; }

        public bool HasGliderPilotLicence { get; set; }

        public bool HasGliderTraineeLicence { get; set; }

        public bool HasGliderPAXLicence { get; set; }

        public bool HasTMGLicence { get; set; }

        public bool HasWinchOperatorLicence { get; set; }

        public bool HasMotorInstructorLicence { get; set; }

        public bool HasPartMLicence { get; set; }

        [StringLength(20)]
        public string LicenceNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MedicalClass1ExpireDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MedicalClass2ExpireDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MedicalLaplExpireDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GliderInstructorLicenceExpireDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MotorInstructorLicenceExpireDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PartMLicenceExpireDate { get; set; }

        public bool HasGliderTowingStartPermission { get; set; }

        public bool HasGliderSelfStartPermission { get; set; }

        public bool HasGliderWinchStartPermission { get; set; }
        
        [StringLength(250)]
        public string SpotLink { get; set; }

        public bool ReceiveOwnedAircraftStatisticReports { get; set; }

        public bool EnableAddress { get; set; }

        public bool IsFastEntryRecord { get; set; }

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

        public virtual ICollection<AircraftAircraftState> ReportedAircraftAircraftStates { get; set; }

        public virtual ICollection<Aircraft> OwnedAircrafts { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<FlightCrew> FlightCrews { get; set; }

        public virtual ICollection<PersonClub> PersonClubs { get; set; }

        public virtual ICollection<PersonPersonCategory> PersonPersonCategories { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<AircraftReservation> AircraftReservations { get; set; }

        public virtual ICollection<AircraftReservation> SecondCrewAssignedAircraftReservations { get; set; }

        public virtual ICollection<PlanningDayAssignment> PlanningDayAssignments { get; set; }

        /// <summary>
        /// returns if this person has a connection to given club.
        /// </summary>
        /// <param name="clubId">id of club to search connection</param>
        /// <returns>if person has connection to club</returns>
        public bool HasConnectionToClub(Guid clubId)
        {
            bool hasConnectionInClubRating =
                PersonClubs.Any(pc => pc.ClubId.Equals(clubId));
            bool hasConnectionInPersonCategories = PersonPersonCategories.Any(
                ppc => ppc.PersonCategory.ClubId.Equals(clubId));
            return hasConnectionInClubRating || hasConnectionInPersonCategories;
        }

        public string DisplayName
        {
            get { return Lastname + " " + Firstname; }
        }
        
        public string EmailAddressForCommunication
        {
            get
            {
                if (PreferMailToBusinessMail)
                {
                    if (string.IsNullOrWhiteSpace(EmailBusiness)
                        && string.IsNullOrWhiteSpace(EmailPrivate) == false)
                    {
                        //return private email address when no business email address is available
                        return EmailPrivate;
                    }

                    return EmailBusiness;
                }

                if (string.IsNullOrWhiteSpace(EmailPrivate)
                        && string.IsNullOrWhiteSpace(EmailBusiness) == false)
                {
                    //return business email address when no private email address is available
                    return EmailBusiness;
                }

                return EmailPrivate;
            }
        }

        public Guid Id
        {
            get { return PersonId; }
            set { PersonId = value; }
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
