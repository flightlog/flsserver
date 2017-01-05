using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using FLS.Common.Extensions;
using FLS.Data.WebApi;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Enums;
using NLog;
using TrackerEnabledDbContext;
using TrackerEnabledDbContext.Common.Configuration;

namespace FLS.Server.Data
{
    public partial class FLSDataEntities : TrackerContext
    {
        private readonly IIdentityService _identityService;
        private Logger _logger = LogManager.GetCurrentClassLogger();

        protected Logger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }
        
        public FLSDataEntities(IIdentityService identityService)
            : base("name=FLSDataEntities")
        {
            _identityService = identityService;
            //tried to fix the lazy loading issue with this configuration on 5.1.2014 PAS, but without success
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        #region DbSet Entity Properties
        public virtual DbSet<AccountingRuleFilter> AccountingRuleFilters { get; set; }
        public virtual DbSet<AccountingRuleFilterType> AccountingRuleFilterTypes { get; set; }
        public virtual DbSet<AircraftAircraftState> AircraftAircraftStates { get; set; }
        public virtual DbSet<Aircraft> Aircrafts { get; set; }
        public virtual DbSet<AircraftOperatingCounter> AircraftOperatingCounters { get; set; }
        public virtual DbSet<AircraftReservation> AircraftReservations { get; set; }
        public virtual DbSet<AircraftReservationType> AircraftReservationTypes { get; set; }
        public virtual DbSet<AircraftState> AircraftStates { get; set; }
        public virtual DbSet<AircraftType> AircraftTypes { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ClubExtension> ClubExtensions { get; set; }
        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<ClubState> ClubStates { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<CounterUnitType> CounterUnitTypes { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<DeliveryCreationTest> DeliveryCreationTests { get; set; }
        public virtual DbSet<DeliveryItem> DeliveryItems { get; set; }
        public virtual DbSet<ElevationUnitType> ElevationUnitTypes { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }
        public virtual DbSet<ExtensionValue> ExtensionValues { get; set; }
        public virtual DbSet<Extension> Extensions { get; set; }
        public virtual DbSet<ExtensionType> ExtensionTypes { get; set; }
        public virtual DbSet<FlightCostBalanceType> FlightCostBalanceTypes { get; set; }
        public virtual DbSet<FlightCrew> FlightCrews { get; set; }
        public virtual DbSet<FlightCrewType> FlightCrewTypes { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<FlightAirState> FlightAirStates { get; set; }
        public virtual DbSet<FlightValidationState> FlightValidationStates { get; set; }
        public virtual DbSet<FlightProcessState> FlightProcessStates { get; set; }
        public virtual DbSet<FlightType> FlightTypes { get; set; }
        public virtual DbSet<InOutboundPoint> InOutboundPoints { get; set; }
        public virtual DbSet<LanguageTranslation> LanguageTranslations { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<LengthUnitType> LengthUnitTypes { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationType> LocationTypes { get; set; }
        public virtual DbSet<MemberState> MemberStates { get; set; }
        public virtual DbSet<PersonCategory> PersonCategories { get; set; }
        public virtual DbSet<PersonClub> PersonClubs { get; set; }
        public virtual DbSet<PersonPersonCategory> PersonPersonCategories { get; set; }
        public virtual DbSet<PlanningDay> PlanningDays { get; set; }
        public virtual DbSet<PlanningDayAssignment> PlanningDayAssignments { get; set; }
        public virtual DbSet<PlanningDayAssignmentType> PlanningDayAssignmentTypes { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<StartType> StartTypes { get; set; }
        public virtual DbSet<SystemData> SystemDatas { get; set; }
        public virtual DbSet<SystemLog> SystemLogs { get; set; }
        public virtual DbSet<SystemVersion> SystemVersions { get; set; }
        public virtual DbSet<UserAccountState> UserAccountStates { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        #endregion DbSet Entity Properties

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aircraft>().Ignore(t => t.Id);
            modelBuilder.Entity<Aircraft>().Ignore(t => t.CurrentAircraftAircraftState);
            modelBuilder.Entity<Aircraft>().Ignore(t => t.HasEngine);
            modelBuilder.Entity<Aircraft>().Ignore(t => t.CurrentAircraftOperatingCounter);

            modelBuilder.Entity<AccountingRuleFilter>().Ignore(t => t.Id);
            modelBuilder.Entity<AircraftAircraftState>().Ignore(t => t.Id);
            modelBuilder.Entity<AircraftReservation>().Ignore(t => t.Id);
            modelBuilder.Entity<AircraftOperatingCounter>().Ignore(t => t.Id);
            modelBuilder.Entity<AircraftState>().Ignore(t => t.Id);
            modelBuilder.Entity<Article>().Ignore(t => t.Id);
            modelBuilder.Entity<Club>().Ignore(t => t.Id);
            modelBuilder.Entity<Club>().Ignore(t => t.HomebaseName);
            modelBuilder.Entity<Country>().Ignore(t => t.Id);
            modelBuilder.Entity<Delivery>().Ignore(t => t.Id);
            modelBuilder.Entity<DeliveryCreationTest>().Ignore(t => t.Id);
            modelBuilder.Entity<DeliveryItem>().Ignore(t => t.Id);
            modelBuilder.Entity<EmailTemplate>().Ignore(t => t.Id);
            modelBuilder.Entity<ExtensionValue>().Ignore(t => t.Id);
            modelBuilder.Entity<Flight>().Ignore(t => t.Id);
            modelBuilder.Entity<Flight>().Ignore(t => t.Pilot);
            modelBuilder.Entity<Flight>().Ignore(t => t.CoPilot);
            modelBuilder.Entity<Flight>().Ignore(t => t.Passenger);
            modelBuilder.Entity<Flight>().Ignore(t => t.Passengers);
            modelBuilder.Entity<Flight>().Ignore(t => t.Instructor);
            modelBuilder.Entity<Flight>().Ignore(t => t.InvoiceRecipient);
            modelBuilder.Entity<Flight>().Ignore(t => t.WinchOperator);
            modelBuilder.Entity<Flight>().Ignore(t => t.ObserverPerson);
            modelBuilder.Entity<Flight>().Ignore(t => t.IsTowed);
            modelBuilder.Entity<Flight>().Ignore(t => t.IsGliderFlight);
            modelBuilder.Entity<Flight>().Ignore(t => t.IsTowFlight);
            modelBuilder.Entity<Flight>().Ignore(t => t.IsMotorFlight);
            modelBuilder.Entity<Flight>().Ignore(t => t.PilotDisplayName);
            modelBuilder.Entity<Flight>().Ignore(t => t.InstructorDisplayName);
            modelBuilder.Entity<Flight>().Ignore(t => t.CoPilotDisplayName);
            modelBuilder.Entity<Flight>().Ignore(t => t.PassengerDisplayName);
            modelBuilder.Entity<Flight>().Ignore(t => t.AircraftImmatriculation);
            modelBuilder.Entity<Flight>().Ignore(t => t.IsStarted);
            modelBuilder.Entity<Flight>().Ignore(t => t.FlightDate);
            modelBuilder.Entity<Flight>().Ignore(t => t.DoNotUpdateMetaData);

            modelBuilder.Entity<FlightCrew>().Ignore(t => t.Id);
            modelBuilder.Entity<FlightCrew>().Ignore(t => t.EntityState);
            modelBuilder.Entity<FlightCrew>().Ignore(t => t.HasPerson);

            modelBuilder.Entity<FlightType>().Ignore(t => t.Id);
            modelBuilder.Entity<Language>().Ignore(t => t.Id);
            modelBuilder.Entity<LanguageTranslation>().Ignore(t => t.Id);
            modelBuilder.Entity<Location>().Ignore(t => t.Id);
            modelBuilder.Entity<LocationType>().Ignore(t => t.Id);
            modelBuilder.Entity<MemberState>().Ignore(t => t.Id);
            modelBuilder.Entity<PlanningDay>().Ignore(t => t.Id);
            modelBuilder.Entity<PlanningDayAssignment>().Ignore(t => t.Id);
            modelBuilder.Entity<PlanningDayAssignment>().Ignore(t => t.EntityState);
            modelBuilder.Entity<PlanningDayAssignmentType>().Ignore(t => t.Id); 
            modelBuilder.Entity<Person>().Ignore(t => t.Id);
            modelBuilder.Entity<Person>().Ignore(t => t.DisplayName);
            modelBuilder.Entity<Person>().Ignore(t => t.EmailAddressForCommunication);
            modelBuilder.Entity<Person>().Ignore(t => t.DoNotUpdateTimeStampsInMetaData);

            modelBuilder.Entity<PersonCategory>().Ignore(t => t.Id);
            modelBuilder.Entity<PersonClub>().Ignore(t => t.Id);
            modelBuilder.Entity<PersonClub>().Ignore(t => t.DoNotUpdateTimeStampsInMetaData); 
            modelBuilder.Entity<PersonPersonCategory>().Ignore(t => t.Id);
            modelBuilder.Entity<Role>().Ignore(t => t.Id);
            modelBuilder.Entity<Role>().Ignore(t => t.Name);
            modelBuilder.Entity<StartType>().Ignore(t => t.Id);
            modelBuilder.Entity<User>().Ignore(t => t.Id);

            modelBuilder.Entity<Aircraft>()
                .Property(e => e.NoiseLevel)
                .HasPrecision(18, 1);

            modelBuilder.Entity<DeliveryItem>()
                .Property(e => e.Quantity)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Aircraft>()
                .HasMany(e => e.Flights)
                .WithRequired(e => e.Aircraft)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Aircraft>()
               .HasMany(e => e.AircraftReservations)
               .WithRequired(e => e.Aircraft)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Aircraft>()
               .HasMany(e => e.AircraftOperatingCounters)
               .WithRequired(e => e.Aircraft)
               .WillCascadeOnDelete();
            
            modelBuilder.Entity<AircraftReservationType>()
               .HasMany(e => e.AircraftReservations)
               .WithRequired(e => e.ReservationType)
               .HasForeignKey(e => e.ReservationTypeId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<AircraftState>()
                .HasMany(e => e.AircraftAircraftStates)
                .WithRequired(e => e.AircraftState)
                .HasForeignKey(e => e.AircraftStateId);

            modelBuilder.Entity<AircraftType>()
                .HasMany(e => e.Aircrafts)
                .WithRequired(e => e.AircraftType)
                .HasForeignKey(e => e.AircraftTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.Aircrafts)
                .WithOptional(e => e.AircraftOwnerClub)
                .HasForeignKey(e => e.AircraftOwnerClubId);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.AircraftReservations)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.Deliveries)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.DeliveryCreationTests)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.EmailTemplates)
                .WithOptional(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.AccountingRuleFilters)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.PlanningDays)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.PlanningDayAssignmentTypes)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.ClubExtensions)
                .WithRequired(e => e.Club)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.FlightTypes)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.Articles)
                .WithRequired(e => e.Club)
                .HasForeignKey(e => e.ClubId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.MemberStates)
                .WithRequired(e => e.Club)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.PersonCategories)
                .WithRequired(e => e.Club)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.ClubPersons)
                .WithRequired(e => e.Club)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Club>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Club)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ClubState>()
                .HasMany(e => e.Clubs)
                .WithRequired(e => e.ClubState)
                .HasForeignKey(e => e.ClubStateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .Property(e => e.CountryCodeIso2)
                .IsUnicode(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.Clubs)
                .WithRequired(e => e.Country)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.Locations)
                .WithRequired(e => e.Country)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CounterUnitType>()
                .HasMany(e => e.AircraftFlightOperatingCounters)
                .WithOptional(e => e.FlightOperatingCounterUnitType)
                .HasForeignKey(e => e.FlightOperatingCounterUnitTypeId);

            modelBuilder.Entity<CounterUnitType>()
                .HasMany(e => e.AircraftEngineOperatingCounters)
                .WithOptional(e => e.EngineOperatingCounterUnitType)
                .HasForeignKey(e => e.EngineOperatingCounterUnitTypeId);

            modelBuilder.Entity<Delivery>()
                .HasMany(e => e.DeliveryItems)
                .WithRequired(e => e.Delivery)
                .HasForeignKey(e => e.DeliveryId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ElevationUnitType>()
                .HasMany(e => e.Locations)
                .WithOptional(e => e.ElevationUnitType)
                .HasForeignKey(e => e.ElevationUnitTypeId);
            
            modelBuilder.Entity<Extension>()
                .HasMany(e => e.ClubExtensions)
                .WithRequired(e => e.Extension)
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<ExtensionType>()
                .HasMany(e => e.Extensions)
                .WithRequired(e => e.ExtensionType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountingRuleFilterType>()
                .HasMany(e => e.AccountingRuleFilters)
                .WithRequired(e => e.AccountingRuleFilterType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FlightCostBalanceType>()
                .HasMany(e => e.Flights)
                .WithOptional(e => e.FlightCostBalanceType)
                .HasForeignKey(e => e.FlightCostBalanceTypeId);

            modelBuilder.Entity<FlightCrewType>()
                .HasMany(e => e.FlightCrews)
                .WithRequired(e => e.FlightCrewType)
                .HasForeignKey(e => e.FlightCrewTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Flight>()
                .HasMany(e => e.FlightCrews)
                .WithRequired(e => e.Flight)
                .HasForeignKey(e => e.FlightId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Flight>()
                .HasMany(e => e.Deliveries)
                .WithOptional(e => e.Flight)
                .HasForeignKey(e => e.FlightId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Flight>()
                .HasMany(e => e.DeliveryCreationTests)
                .WithRequired(e => e.Flight)
                .HasForeignKey(e => e.FlightId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FlightCrew>()
                .HasRequired(e => e.Flight);
            
            modelBuilder.Entity<Flight>()
                .HasMany(e => e.TowedFlights)
                .WithOptional(e => e.TowFlight)
                .HasForeignKey(e => e.TowFlightId);
            
            modelBuilder.Entity<FlightAirState>()
                .HasMany(e => e.Flights)
                .WithRequired(e => e.FlightAirState)
                .HasForeignKey(e => e.AirStateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FlightValidationState>()
               .HasMany(e => e.Flights)
               .WithRequired(e => e.FlightValidationState)
               .HasForeignKey(e => e.ValidationStateId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<FlightProcessState>()
               .HasMany(e => e.Flights)
               .WithRequired(e => e.FlightProcessState)
               .HasForeignKey(e => e.ProcessStateId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<FlightType>()
                .HasMany(e => e.ClubsDefaultGliderFlightType)
                .WithOptional(e => e.DefaultGliderFlightType)
                .HasForeignKey(e => e.DefaultGliderFlightTypeId);

            modelBuilder.Entity<FlightType>()
                .HasMany(e => e.ClubsDefaultMotorFlightType)
                .WithOptional(e => e.DefaultMotorFlightType)
                .HasForeignKey(e => e.DefaultMotorFlightTypeId);

            modelBuilder.Entity<FlightType>()
                .HasMany(e => e.ClubsDefaultTowFlightType)
                .WithOptional(e => e.DefaultTowFlightType)
                .HasForeignKey(e => e.DefaultTowFlightTypeId);

            modelBuilder.Entity<Language>()
               .HasMany(e => e.LanguageTranslations)
               .WithRequired(e => e.Language)
               .HasForeignKey(e => e.LanguageId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<LengthUnitType>()
                .HasMany(e => e.Locations)
                .WithOptional(e => e.LengthUnitType)
                .HasForeignKey(e => e.RunwayLengthUnitType);
            
            modelBuilder.Entity<Location>()
                .HasMany(e => e.Clubs)
                .WithOptional(e => e.Homebase)
                .HasForeignKey(e => e.HomebaseId);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LandedFlights)
                .WithOptional(e => e.LdgLocation)
                .HasForeignKey(e => e.LdgLocationId);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.StartedFlights)
                .WithOptional(e => e.StartLocation)
                .HasForeignKey(e => e.StartLocationId);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.InOutboundPoints)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.PlanningDays)
                .WithRequired(e => e.Location)
                .HasForeignKey(e => e.LocationId);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.AircraftReservations)
                .WithRequired(e => e.Location)
                .HasForeignKey(e => e.LocationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LocationType>()
                .HasMany(e => e.Locations)
                .WithRequired(e => e.LocationType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MemberState>()
                .HasMany(e => e.PersonClubs)
                .WithOptional(e => e.MemberState)
                .HasForeignKey(e => e.MemberStateId);

            modelBuilder.Entity<PersonCategory>()
                .HasMany(e => e.ChildPersonCategories)
                .WithOptional(e => e.ParentPersonCategory)
                .HasForeignKey(e => e.ParentPersonCategoryId);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.ReportedAircraftAircraftStates)
                .WithOptional(e => e.NoticedByPerson)
                .HasForeignKey(e => e.NoticedByPersonId)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Person>()
                .HasMany(e => e.OwnedAircrafts)
                .WithOptional(e => e.AircraftOwnerPerson)
                .HasForeignKey(e => e.AircraftOwnerPersonId);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.FlightCrews)
                .WithRequired(e => e.Person)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.PersonClubs)
                .WithRequired(e => e.Person)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.AircraftReservations)
                .WithRequired(e => e.PilotPerson)
                .HasForeignKey(e => e.PilotPersonId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.InstructorAssignedAircraftReservations)
                .WithOptional(e => e.InstructorPerson)
                .HasForeignKey(e => e.InstructorPersonId);

            modelBuilder.Entity<Person>()
                .HasMany(e => e.PlanningDayAssignments)
                .WithRequired(e => e.AssignedPerson)
                .HasForeignKey(e => e.AssignedPersonId);

            modelBuilder.Entity<PlanningDay>()
                .HasMany(e => e.PlanningDayAssignments)
                .WithRequired(e => e.AssignedPlanningDay)
                .HasForeignKey(e => e.AssignedPlanningDayId);

            modelBuilder.Entity<PlanningDayAssignmentType>()
                .HasMany(e => e.PlanningDayAssignments)
                .WithRequired(e => e.AssignmentType)
                .HasForeignKey(e => e.AssignmentTypeId);

            modelBuilder.Entity<StartType>()
                .HasMany(e => e.Clubs)
                .WithOptional(e => e.DefaultStartType)
                .HasForeignKey(e => e.DefaultStartTypeId);

            modelBuilder.Entity<StartType>()
                .HasMany(e => e.Flights)
                .WithOptional(e => e.StartType)
                .HasForeignKey(e => e.StartTypeId);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.Exception)
                .IsUnicode(false);

            modelBuilder.Entity<SystemLog>()
                .Property(e => e.Stacktrace)
                .IsUnicode(false);

            modelBuilder.Entity<UserAccountState>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.UserAccountState)
                .HasForeignKey(e => e.AccountState)
                .WillCascadeOnDelete(false);

            SetIsDeletedMapping(modelBuilder, false);

            #region Audit Tracking Settings
            //For more details, see: https://github.com/bilal-fazlani/tracker-enabled-dbcontext/wiki

            EntityTracker.TrackAllProperties<AircraftAircraftState>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<Aircraft>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<AircraftOperatingCounter>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<AircraftReservation>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<Article>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<Club>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<Delivery>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<DeliveryCreationTest>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<DeliveryItem>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<EmailTemplate>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<FlightCrew>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<Flight>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState)
                .And(x => x.FlightAircraftType);
            EntityTracker.TrackAllProperties<FlightType>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<InOutboundPoint>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<AccountingRuleFilter>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<Location>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<MemberState>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<PersonCategory>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<PersonClub>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<PersonPersonCategory>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<PlanningDay>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<PlanningDayAssignment>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<PlanningDayAssignmentType>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<Person>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<Role>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);
            EntityTracker.TrackAllProperties<UserRole>();
            EntityTracker.TrackAllProperties<User>().Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);

            #endregion Audit Tracking Settings
        }

        /// <summary>
        /// Sets the deleted filter to not return deleted objects to the object mapper.
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="isDeleted"></param>
        protected virtual void SetIsDeletedMapping(DbModelBuilder modelBuilder, bool isDeleted)
        {
            #region Soft-Delete settings
            //Soft-Delete
            modelBuilder.Entity<Aircraft>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<AircraftOperatingCounter>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<AircraftReservation>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<Article>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<Club>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<ClubExtension>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<Country>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<Delivery>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<DeliveryItem>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<EmailTemplate>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<Extension>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<ExtensionValue>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<Flight>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<FlightCrew>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<FlightType>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<InOutboundPoint>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<AccountingRuleFilter>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<LanguageTranslation>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<Location>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<LocationType>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<MemberState>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<PlanningDay>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<PlanningDayAssignment>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<PlanningDayAssignmentType>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<Person>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<PersonCategory>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<PersonClub>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<Role>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);
            modelBuilder.Entity<User>()
                        .Map(m => m.Requires("IsDeleted").HasValue(isDeleted))
                        .Ignore(m => m.IsDeleted);

            #endregion Soft-Delete settings
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                PrepareEntitiesForSave();

                return base.SaveChangesAsync(_identityService.CurrentAuthenticatedFLSUser?.UserName);
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                Logger.Error(ex, $"DbEntityValidationException: {sb}");
                throw new DbEntityValidationException($"Entity Validation Failed - errors follow:\n{sb}", ex); // Add the original exception as the innerException
            }
            catch (Exception ex)
            {
                var message = ex.Message;

                if (ex.InnerException != null)
                {
                    message += $" InnerException: {ex.InnerException.Message}";

                    if (ex.InnerException.InnerException != null)
                    {
                        message += $" InnerException: {ex.InnerException.InnerException.Message}";
                    }
                }

                Logger.Error(ex, $"Error while trying to save entity changes: {message}");

                throw;
            }
        }
        
        public override int SaveChanges()
        {
            int changes = 0;

            try
            {
                PrepareEntitiesForSave();

                changes = base.SaveChanges(_identityService.CurrentAuthenticatedFLSUser?.UserName);
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                Logger.Error(ex, $"DbEntityValidationException: {sb}");
                throw new DbEntityValidationException($"Entity Validation Failed - errors follow:\n{sb}", ex); // Add the original exception as the innerException
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var innerException = ex.InnerException;

                while (innerException != null)
                {
                    message += $" InnerException: {innerException.Message}";
                    innerException = ex.InnerException;
                }

                Logger.Error(ex, $"Error while trying to save entity changes: {message}");

                throw;
            }

            return changes;
        }

        private void PrepareEntitiesForSave()
        {
            foreach (var entry in ChangeTracker.Entries().Where(p =>
                p.State == EntityState.Added || p.State == EntityState.Modified ||
                p.State == EntityState.Deleted))
            {
                if ((entry.Entity.GetType() == typeof(Flight)
                     && ((Flight)entry.Entity).DoNotUpdateMetaData)
                    || (entry.Entity.GetType() == typeof(User)
                        && ((User)entry.Entity).DoNotUpdateMetaData))
                {
                    //check next entry
                }
                else
                {
                    //we found an entry which requires the UserId, so we check the authentication values
                    if (_identityService.CurrentAuthenticatedFLSUser == null)
                    {
                        throw new AuthenticationException("Current authenticated FLS user is null");
                    }

                    break;
                }
            }

            //https://lostechies.com/jimmybogard/2014/05/08/missing-ef-feature-workarounds-cascade-delete-orphans/
            foreach (var orphan in FlightCrews.Local.Where(a => a.Flight == null).ToList())
            {
                FlightCrews.Remove(orphan);
            }

            foreach (var orphan in PlanningDayAssignments.Local.Where(a => a.AssignedPlanningDay == null).ToList())
            {
                PlanningDayAssignments.Remove(orphan);
            }

            foreach (var orphan in PersonClubs.Local.Where(a => a.Person == null).ToList())
            {
                PersonClubs.Remove(orphan);
            }

            #region Added Entities
            foreach (var entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Added))
            {
                try
                {
                    var flsEntity = entry.Entity as IFLSMetaData;

                    if (flsEntity != null)
                    {
                        if (flsEntity.Id == Guid.Empty)
                        {
                            flsEntity.SetPropertyValue("Id", Guid.NewGuid());
                        }

                        if ((entry.Entity.GetType() == typeof(Person)
                            && ((Person)entry.Entity).DoNotUpdateTimeStampsInMetaData)
                            || (entry.Entity.GetType() == typeof(PersonClub)
                            && ((PersonClub)entry.Entity).DoNotUpdateTimeStampsInMetaData))
                        {
                            //don't update created on metadata
                        }
                        else
                        {
                            flsEntity.SetPropertyValue("CreatedOn", DateTime.UtcNow);
                        }

                        flsEntity.SetPropertyValue("CreatedByUserId", _identityService.CurrentAuthenticatedFLSUser.UserId);
                        flsEntity.SetPropertyValue("OwnerId", _identityService.CurrentAuthenticatedFLSUser.ClubId);
                        flsEntity.SetPropertyValue("OwnershipType", (int)OwnershipType.Club);
                        flsEntity.SetPropertyValue("RecordState", (int)EntityRecordState.Active);

                        Logger.Debug($"Insert {entry.Entity.GetType().Name} with data: {entry.Entity}");
                    }
                    else if (entry.Entity is UserRole)
                    {
                        Logger.Debug($"Insert {entry.Entity.GetType().Name} with data: {entry.Entity}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"Error while trying to save new FLSDataEntities. Error: {ex.Message}");
                    throw;
                }
            }
            #endregion Added Entities

            #region Modified Entities
            foreach (var entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Modified))
            {
                try
                {
                    var flsEntity = entry.Entity as IFLSMetaData;

                    if (flsEntity != null)
                    {
                        if ((entry.Entity.GetType() == typeof(Flight)
                             && ((Flight)entry.Entity).DoNotUpdateMetaData)
                            || (entry.Entity.GetType() == typeof(User)
                            && ((User)entry.Entity).DoNotUpdateMetaData)
                            || (entry.Entity.GetType() == typeof(Club)
                            && ((Club)entry.Entity).DoNotUpdateMetaData))
                        {
                            //don't update metadata when workflow process set flag "DoNotUpdateMetaData" on flight
                            //or when user resets passwords
                            flsEntity.SetPropertyValue("RecordState", (int)EntityRecordState.Active);

                            Logger.Debug($"Update {entry.Entity.GetType().Name} with data: {entry.Entity}");

                            continue;
                        }

                        var userId = _identityService.CurrentAuthenticatedFLSUser.UserId;

                        if ((entry.Entity.GetType() == typeof(Person)
                             && ((Person)entry.Entity).DoNotUpdateTimeStampsInMetaData)
                            || (entry.Entity.GetType() == typeof(PersonClub)
                                && ((PersonClub)entry.Entity).DoNotUpdateTimeStampsInMetaData))
                        {
                            //don't update modified on metadata
                        }
                        else
                        {
                            flsEntity.SetPropertyValue("ModifiedOn", DateTime.UtcNow);
                        }

                        flsEntity.SetPropertyValue("ModifiedByUserId", userId);
                        flsEntity.SetPropertyValue("RecordState", (int)EntityRecordState.Active);

                        Logger.Debug($"Update {entry.Entity.GetType().Name} with data: {entry.Entity}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"Error while trying to update FLSDataEntities. Error: {ex.Message}");
                    throw;
                }
            }
            #endregion Modified Entities

            #region Deleted Entities
            foreach (var entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted))
            {
                try
                {
                    var flsEntity = entry.Entity as IFLSMetaData;
                    if (flsEntity != null)
                    {
                        var userId = _identityService.CurrentAuthenticatedFLSUser.UserId;

                        if ((entry.Entity.GetType() == typeof(Person)
                             && ((Person)entry.Entity).DoNotUpdateTimeStampsInMetaData)
                            || (entry.Entity.GetType() == typeof(PersonClub)
                                && ((PersonClub)entry.Entity).DoNotUpdateTimeStampsInMetaData))
                        {
                            //don't update deleted on metadata
                        }
                        else
                        {
                            flsEntity.SetPropertyValue("DeletedOn", DateTime.UtcNow);
                        }

                        flsEntity.SetPropertyValue("DeletedByUserId", userId);
                        flsEntity.SetPropertyValue("RecordState", (int)EntityRecordState.Deleted);
                        Logger.Debug($"Delete {entry.Entity.GetType().Name} with Data: {entry.Entity}");
                    }
                    else if (entry.Entity is UserRole)
                    {
                        Logger.Debug($"Delete {entry.Entity.GetType().Name} with data: {entry.Entity}");
                    }

                    SoftDelete(entry);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, $"Error while trying to delete FLSDataEntities. Error: {ex.Message}");
                    throw;
                }
            }
            #endregion Deleted Entities

        }

        #region SoftDelete
        private void SoftDelete(DbEntityEntry entry)
        {
            Type entryEntityType = entry.Entity.GetType();

            if (entryEntityType.Name == "UserRole")
            {
                //Hard delete user role entries, as we do not have unique key for re-insert a same role
                return;
            }

            if (entry.Entity.HasProperty("IsDeleted"))
            {
                string tableName = GetTableName(entryEntityType);
                string primaryKeyName = GetPrimaryKeyName(entryEntityType);
                DateTime? deletedOn = null;
                Guid? deletedByUserId = null;

                string sql =
                    string.Format(
                        "UPDATE {0} SET DeletedOn = @deletedOn, DeletedByUserId = @deletedByUserId, RecordState = @recordState, IsDeleted = 1 WHERE {1} = @id",
                        tableName, primaryKeyName);

                var flsEntity = entry.Entity as IFLSMetaData;
                if (flsEntity != null)
                {
                    deletedOn = flsEntity.DeletedOn;
                    deletedByUserId = flsEntity.DeletedByUserId;
                }

                Database.ExecuteSqlCommand(
                    sql,
                    new SqlParameter("@id", entry.OriginalValues[primaryKeyName]),
                    new SqlParameter("@deletedOn", deletedOn),
                    new SqlParameter("@deletedByUserId", deletedByUserId),
                    new SqlParameter("@recordState", (int) EntityRecordState.Deleted));

                // prevent hard delete and change state to Detached (not modified as modified will reattach it back to the client
                entry.State = EntityState.Detached;
            }
        }

        private static Dictionary<Type, EntitySetBase> _mappingCache =
        new Dictionary<Type, EntitySetBase>();

        private string GetTableName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);

            return string.Format("[{0}].[{1}]",
                es.MetadataProperties["Schema"].Value,
                es.MetadataProperties["Table"].Value);
        }

        private string GetPrimaryKeyName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);

            return es.ElementType.KeyMembers[0].Name;
        }

        private EntitySetBase GetEntitySet(Type type)
        {
            if (!_mappingCache.ContainsKey(type))
            {
                ObjectContext octx = ((IObjectContextAdapter)this).ObjectContext;

                string typeName = ObjectContext.GetObjectType(type).Name;

                var es = octx.MetadataWorkspace
                                .GetItemCollection(DataSpace.SSpace)
                                .GetItems<EntityContainer>()
                                .SelectMany(c => c.BaseEntitySets
                                                .Where(e => e.Name == typeName))
                                .FirstOrDefault();

                if (es == null)
                    throw new ArgumentException("Entity type not found in GetTableName", typeName);

                _mappingCache.Add(type, es);
            }

            return _mappingCache[type];
        }

        #endregion SoftDelete
    }
}
