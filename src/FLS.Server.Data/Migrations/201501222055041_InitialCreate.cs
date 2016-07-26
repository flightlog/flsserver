namespace FLS.Server.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AircraftAircraftStates",
                c => new
                    {
                        AircraftId = c.Guid(nullable: false),
                        AircraftState = c.Int(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(precision: 7, storeType: "datetime2"),
                        NoticedByPersonId = c.Guid(),
                        Remarks = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AircraftId, t.AircraftState, t.ValidFrom })
                .ForeignKey("dbo.Aircrafts", t => t.AircraftId, cascadeDelete: true)
                .ForeignKey("dbo.Persons", t => t.NoticedByPersonId, cascadeDelete: true)
                .ForeignKey("dbo.AircraftStates", t => t.AircraftState, cascadeDelete: true)
                .Index(t => t.AircraftId)
                .Index(t => t.AircraftState)
                .Index(t => t.NoticedByPersonId);
            
            CreateTable(
                "dbo.Aircrafts",
                c => new
                    {
                        AircraftId = c.Guid(nullable: false),
                        ManufacturerName = c.String(maxLength: 100),
                        AircraftModel = c.String(maxLength: 50),
                        Immatriculation = c.String(nullable: false, maxLength: 15),
                        CompetitionSign = c.String(maxLength: 5),
                        NrOfSeats = c.Int(),
                        DaecIndex = c.Int(),
                        Comment = c.String(maxLength: 250),
                        AircraftType = c.Int(nullable: false),
                        IsTowingOrWinchRequired = c.Boolean(nullable: false),
                        IsTowingstartAllowed = c.Boolean(nullable: false),
                        IsWinchstartAllowed = c.Boolean(nullable: false),
                        IsTowingAircraft = c.Boolean(nullable: false),
                        AircraftOwnerClubId = c.Guid(),
                        AircraftOwnerPersonId = c.Guid(),
                        FLARMId = c.String(maxLength: 50),
                        AircraftSerialNumber = c.String(maxLength: 20),
                        YearOfManufacture = c.DateTime(precision: 7, storeType: "datetime2"),
                        NoiseClass = c.String(maxLength: 1),
                        NoiseLevel = c.Decimal(precision: 18, scale: 1, storeType: "numeric"),
                        MTOM = c.Int(),
                        FlightDurationPrecision = c.Int(nullable: false),
                        SpotLink = c.String(maxLength: 250),
                        IsFastEntryRecord = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AircraftId)
                .ForeignKey("dbo.Persons", t => t.AircraftOwnerPersonId)
                .ForeignKey("dbo.Clubs", t => t.AircraftOwnerClubId)
                .ForeignKey("dbo.AircraftTypes", t => t.AircraftType)
                .Index(t => t.AircraftType)
                .Index(t => t.AircraftOwnerClubId)
                .Index(t => t.AircraftOwnerPersonId);
            
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        ClubId = c.Guid(nullable: false),
                        Clubname = c.String(nullable: false, maxLength: 50),
                        ClubKey = c.String(nullable: false, maxLength: 10),
                        Address = c.String(maxLength: 100),
                        Zip = c.String(maxLength: 10),
                        City = c.String(maxLength: 100),
                        CountryId = c.Guid(nullable: false),
                        Phone = c.String(maxLength: 20),
                        FaxNumber = c.String(maxLength: 20),
                        Email = c.String(maxLength: 100),
                        WebPage = c.String(maxLength: 100),
                        Contact = c.String(maxLength: 100),
                        HomebaseId = c.Guid(),
                        DefaultStartType = c.Int(),
                        DefaultGliderFlightTypeId = c.Guid(),
                        DefaultTowFlightTypeId = c.Guid(),
                        DefaultMotorFlightTypeId = c.Guid(),
                        IsInboundRouteRequired = c.Boolean(nullable: false),
                        IsOutboundRouteRequired = c.Boolean(nullable: false),
                        LastPersonSynchronisationOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        LastInvoiceExportOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClubId)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .ForeignKey("dbo.Locations", t => t.HomebaseId)
                .ForeignKey("dbo.FlightTypes", t => t.DefaultGliderFlightTypeId)
                .ForeignKey("dbo.FlightTypes", t => t.DefaultMotorFlightTypeId)
                .ForeignKey("dbo.FlightTypes", t => t.DefaultTowFlightTypeId)
                .ForeignKey("dbo.StartTypes", t => t.DefaultStartType)
                .Index(t => t.CountryId)
                .Index(t => t.HomebaseId)
                .Index(t => t.DefaultStartType)
                .Index(t => t.DefaultGliderFlightTypeId)
                .Index(t => t.DefaultTowFlightTypeId)
                .Index(t => t.DefaultMotorFlightTypeId);
            
            CreateTable(
                "dbo.AircraftReservations",
                c => new
                    {
                        AircraftReservationId = c.Guid(nullable: false),
                        Start = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        End = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsAllDayReservation = c.Boolean(nullable: false),
                        AircraftId = c.Guid(nullable: false),
                        PilotPersonId = c.Guid(nullable: false),
                        InstructorPersonId = c.Guid(),
                        ReservationTypeId = c.Int(nullable: false),
                        Remarks = c.String(),
                        ClubId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AircraftReservationId)
                .ForeignKey("dbo.Persons", t => t.PilotPersonId)
                .ForeignKey("dbo.Persons", t => t.InstructorPersonId)
                .ForeignKey("dbo.AircraftReservationTypes", t => t.ReservationTypeId)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .ForeignKey("dbo.Aircrafts", t => t.AircraftId)
                .Index(t => t.AircraftId)
                .Index(t => t.InstructorPersonId)
                .Index(t => t.ReservationTypeId)
                .Index(t => t.ClubId)
                .Index(t => t.PilotPersonId);
            
            CreateTable(
                "dbo.Persons",
                c => new
                    {
                        PersonId = c.Guid(nullable: false),
                        Lastname = c.String(nullable: false, maxLength: 100),
                        Firstname = c.String(nullable: false, maxLength: 100),
                        Midname = c.String(maxLength: 100),
                        CompanyName = c.String(maxLength: 100),
                        AddressLine1 = c.String(maxLength: 200),
                        AddressLine2 = c.String(maxLength: 200),
                        Zip = c.String(maxLength: 10),
                        City = c.String(maxLength: 100),
                        Region = c.String(maxLength: 100),
                        CountryId = c.Guid(),
                        PrivatePhone = c.String(maxLength: 20),
                        MobilePhone = c.String(maxLength: 20),
                        BusinessPhone = c.String(maxLength: 20),
                        FaxNumber = c.String(maxLength: 20),
                        EmailPrivate = c.String(maxLength: 100),
                        EmailBusiness = c.String(maxLength: 100),
                        PreferMailToBusinessMail = c.Boolean(nullable: false),
                        Birthday = c.DateTime(storeType: "date"),
                        HasMotorPilotLicence = c.Boolean(nullable: false),
                        HasTowPilotLicence = c.Boolean(nullable: false),
                        HasGliderInstructorLicence = c.Boolean(nullable: false),
                        HasGliderPilotLicence = c.Boolean(nullable: false),
                        HasGliderTraineeLicence = c.Boolean(nullable: false),
                        HasGliderPAXLicence = c.Boolean(nullable: false),
                        HasTMGLicence = c.Boolean(nullable: false),
                        HasWinchOperatorLicence = c.Boolean(nullable: false),
                        LicenceNumber = c.String(maxLength: 20),
                        LicenseTypeId = c.Guid(),
                        LicenseTrainingStateGlider = c.Int(),
                        LicenseTrainingStateGliderPAX = c.Int(),
                        LicenseTrainingStateGliderInstructor = c.Int(),
                        LicenseTrainingStateTowing = c.Int(),
                        LicenseTrainingStateTMG = c.Int(),
                        LicenseTrainingStateMotor = c.Int(),
                        SpotLink = c.String(maxLength: 250),
                        IsFastEntryRecord = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PersonId)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .ForeignKey("dbo.LicenseTrainingStates", t => t.LicenseTrainingStateGliderInstructor)
                .ForeignKey("dbo.LicenseTrainingStates", t => t.LicenseTrainingStateGliderPAX)
                .ForeignKey("dbo.LicenseTrainingStates", t => t.LicenseTrainingStateGlider)
                .ForeignKey("dbo.LicenseTrainingStates", t => t.LicenseTrainingStateMotor)
                .ForeignKey("dbo.LicenseTrainingStates", t => t.LicenseTrainingStateTMG)
                .ForeignKey("dbo.LicenseTrainingStates", t => t.LicenseTrainingStateTowing)
                .ForeignKey("dbo.LicenseTypes", t => t.LicenseTypeId)
                .Index(t => t.CountryId)
                .Index(t => t.LicenseTypeId)
                .Index(t => t.LicenseTrainingStateGlider)
                .Index(t => t.LicenseTrainingStateGliderPAX)
                .Index(t => t.LicenseTrainingStateGliderInstructor)
                .Index(t => t.LicenseTrainingStateTowing)
                .Index(t => t.LicenseTrainingStateTMG)
                .Index(t => t.LicenseTrainingStateMotor);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        CountryId = c.Guid(nullable: false),
                        CountryIdIso = c.Int(nullable: false),
                        CountryName = c.String(nullable: false, maxLength: 100),
                        CountryFullName = c.String(maxLength: 250),
                        CountryCodeIso2 = c.String(nullable: false, maxLength: 2, unicode: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CountryId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Guid(nullable: false),
                        LocationName = c.String(nullable: false, maxLength: 100),
                        LocationShortName = c.String(maxLength: 50),
                        CountryId = c.Guid(nullable: false),
                        LocationTypeId = c.Guid(nullable: false),
                        IcaoCode = c.String(maxLength: 10),
                        Latitude = c.String(maxLength: 10),
                        Longitude = c.String(maxLength: 10),
                        Elevation = c.Int(),
                        ElevationUnitType = c.Int(),
                        RunwayDirection = c.String(maxLength: 50),
                        RunwayLength = c.Int(),
                        RunwayLengthUnitType = c.Int(),
                        AirportFrequency = c.String(maxLength: 50),
                        Description = c.String(),
                        SortIndicator = c.Int(),
                        IsFastEntryRecord = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.ElevationUnitTypes", t => t.ElevationUnitType)
                .ForeignKey("dbo.LengthUnitTypes", t => t.RunwayLengthUnitType)
                .ForeignKey("dbo.LocationTypes", t => t.LocationTypeId)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .Index(t => t.CountryId)
                .Index(t => t.LocationTypeId)
                .Index(t => t.ElevationUnitType)
                .Index(t => t.RunwayLengthUnitType);
            
            CreateTable(
                "dbo.ElevationUnitTypes",
                c => new
                    {
                        ElevationUnitTypeId = c.Int(nullable: false, identity: true),
                        ElevationUnitTypeName = c.String(nullable: false, maxLength: 50),
                        ElevationUnitTypeKeyName = c.String(nullable: false, maxLength: 50),
                        ElevationUnitTypeShortName = c.String(maxLength: 20),
                        Comment = c.String(maxLength: 200),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ElevationUnitTypeId);
            
            CreateTable(
                "dbo.InOutboundPoints",
                c => new
                    {
                        InOutboundPointId = c.Guid(nullable: false),
                        LocationId = c.Guid(nullable: false),
                        InOutboundPointName = c.String(nullable: false, maxLength: 50),
                        IsInboundPoint = c.Boolean(nullable: false),
                        IsOutboundPoint = c.Boolean(nullable: false),
                        SortIndicatorInboundPoint = c.Int(nullable: false),
                        SortIndicatorOutboundPoint = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.InOutboundPointId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Flights",
                c => new
                    {
                        FlightId = c.Guid(nullable: false),
                        AircraftId = c.Guid(nullable: false),
                        StartPosition = c.Int(),
                        StartDateTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        LdgDateTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        BlockStartDateTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        BlockEndDateTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        StartLocationId = c.Guid(),
                        LdgLocationId = c.Guid(),
                        StartRunway = c.String(maxLength: 5),
                        LdgRunway = c.String(maxLength: 5),
                        OutboundRoute = c.String(maxLength: 50),
                        InboundRoute = c.String(maxLength: 50),
                        FlightTypeId = c.Guid(),
                        IsSoloFlight = c.Boolean(nullable: false),
                        StartType = c.Int(),
                        TowFlightId = c.Guid(),
                        NrOfLdgs = c.Int(),
                        FlightState = c.Int(nullable: false),
                        FlightAircraftType = c.Int(nullable: false),
                        Comment = c.String(),
                        IncidentComment = c.String(),
                        CouponNumber = c.String(maxLength: 20),
                        FlightCostBalanceType = c.Int(),
                        InvoicedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        InvoiceNumber = c.String(maxLength: 100),
                        DeliveryNumber = c.String(maxLength: 100),
                        InvoicePaidOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FlightId)
                .ForeignKey("dbo.FlightCostBalanceTypes", t => t.FlightCostBalanceType)
                .ForeignKey("dbo.FlightStates", t => t.FlightState)
                .ForeignKey("dbo.FlightTypes", t => t.FlightTypeId)
                .ForeignKey("dbo.StartTypes", t => t.StartType)
                .ForeignKey("dbo.Flights", t => t.TowFlightId)
                .ForeignKey("dbo.Locations", t => t.LdgLocationId)
                .ForeignKey("dbo.Locations", t => t.StartLocationId)
                .ForeignKey("dbo.Aircrafts", t => t.AircraftId)
                .Index(t => t.AircraftId)
                .Index(t => t.StartLocationId)
                .Index(t => t.LdgLocationId)
                .Index(t => t.FlightTypeId)
                .Index(t => t.StartType)
                .Index(t => t.TowFlightId)
                .Index(t => t.FlightState)
                .Index(t => t.FlightCostBalanceType);
            
            CreateTable(
                "dbo.FlightCostBalanceTypes",
                c => new
                    {
                        FlightCostBalanceTypeId = c.Int(nullable: false, identity: true),
                        FlightCostBalanceTypeName = c.String(nullable: false, maxLength: 100),
                        Comment = c.String(maxLength: 500),
                        PersonForInvoiceRequired = c.Boolean(nullable: false),
                        IsForGliderFlights = c.Boolean(nullable: false),
                        IsForTowFlights = c.Boolean(nullable: false),
                        IsForMotorFlights = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.FlightCostBalanceTypeId);
            
            CreateTable(
                "dbo.FlightCrew",
                c => new
                    {
                        FlightCrewId = c.Guid(nullable: false),
                        FlightId = c.Guid(nullable: false),
                        PersonId = c.Guid(nullable: false),
                        FlightCrewType = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FlightCrewId)
                .ForeignKey("dbo.Flights", t => t.FlightId, cascadeDelete: true)
                .ForeignKey("dbo.FlightCrewTypes", t => t.FlightCrewType)
                .ForeignKey("dbo.Persons", t => t.PersonId)
                .Index(t => t.FlightId)
                .Index(t => t.PersonId)
                .Index(t => t.FlightCrewType);
            
            CreateTable(
                "dbo.FlightCrewTypes",
                c => new
                    {
                        FlightCrewTypeId = c.Int(nullable: false, identity: true),
                        FlightCrewTypeName = c.String(nullable: false, maxLength: 50),
                        Comment = c.String(maxLength: 200),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.FlightCrewTypeId);
            
            CreateTable(
                "dbo.FlightStates",
                c => new
                    {
                        FlightStateId = c.Int(nullable: false, identity: true),
                        FlightStateName = c.String(nullable: false, maxLength: 50),
                        Comment = c.String(maxLength: 200),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.FlightStateId);
            
            CreateTable(
                "dbo.FlightTypes",
                c => new
                    {
                        FlightTypeId = c.Guid(nullable: false),
                        ClubId = c.Guid(nullable: false),
                        FlightTypeName = c.String(nullable: false, maxLength: 100),
                        FlightCode = c.String(maxLength: 30),
                        InstructorRequired = c.Boolean(nullable: false),
                        ObserverPilotOrInstructorRequired = c.Boolean(nullable: false),
                        IsCheckFlight = c.Boolean(nullable: false),
                        IsPassengerFlight = c.Boolean(nullable: false),
                        IsSoloFlight = c.Boolean(nullable: false),
                        IsSummarizedSystemFlight = c.Boolean(nullable: false),
                        IsForGliderFlights = c.Boolean(nullable: false),
                        IsForTowFlights = c.Boolean(nullable: false),
                        IsForMotorFlights = c.Boolean(nullable: false),
                        IsFlightCostBalanceSelectable = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.FlightTypeId)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.StartTypes",
                c => new
                    {
                        StartTypeId = c.Int(nullable: false, identity: true),
                        StartTypeName = c.String(nullable: false, maxLength: 100),
                        IsForGliderFlights = c.Boolean(nullable: false),
                        IsForTowFlights = c.Boolean(nullable: false),
                        IsForMotorFlights = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.StartTypeId);
            
            CreateTable(
                "dbo.LengthUnitTypes",
                c => new
                    {
                        LengthUnitTypeId = c.Int(nullable: false, identity: true),
                        LengthUnitTypeName = c.String(nullable: false, maxLength: 50),
                        LengthUnitTypeKeyName = c.String(nullable: false, maxLength: 50),
                        LengthUnitTypeShortName = c.String(maxLength: 20),
                        Comment = c.String(maxLength: 200),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.LengthUnitTypeId);
            
            CreateTable(
                "dbo.LocationTypes",
                c => new
                    {
                        LocationTypeId = c.Guid(nullable: false),
                        LocationTypeName = c.String(nullable: false, maxLength: 50),
                        LocationTypeCupId = c.Int(),
                        IsAirfield = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LocationTypeId);
            
            CreateTable(
                "dbo.PlanningDays",
                c => new
                    {
                        PlanningDayId = c.Guid(nullable: false),
                        ClubId = c.Guid(nullable: false),
                        Day = c.DateTime(nullable: false, storeType: "date"),
                        LocationId = c.Guid(nullable: false),
                        Remarks = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PlanningDayId)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .Index(t => t.ClubId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.PlanningDayAssignments",
                c => new
                    {
                        PlanningDayAssignmentId = c.Guid(nullable: false),
                        AssignedPlanningDayId = c.Guid(nullable: false),
                        AssignedPersonId = c.Guid(nullable: false),
                        AssignmentTypeId = c.Guid(nullable: false),
                        Remarks = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PlanningDayAssignmentId)
                .ForeignKey("dbo.PlanningDayAssignmentTypes", t => t.AssignmentTypeId, cascadeDelete: true)
                .ForeignKey("dbo.PlanningDays", t => t.AssignedPlanningDayId, cascadeDelete: true)
                .ForeignKey("dbo.Persons", t => t.AssignedPersonId, cascadeDelete: true)
                .Index(t => t.AssignedPlanningDayId)
                .Index(t => t.AssignedPersonId)
                .Index(t => t.AssignmentTypeId);
            
            CreateTable(
                "dbo.PlanningDayAssignmentTypes",
                c => new
                    {
                        PlanningDayAssignmentTypeId = c.Guid(nullable: false),
                        AssignmentTypeName = c.String(),
                        ClubId = c.Guid(nullable: false),
                        RequiredNrOfPlanningDayAssignments = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PlanningDayAssignmentTypeId)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.LicenseTrainingStates",
                c => new
                    {
                        LicenseTrainingStateId = c.Int(nullable: false, identity: true),
                        LicenseTrainingStateName = c.String(nullable: false, maxLength: 50),
                        CanFly = c.Boolean(),
                        RequiresInstructor = c.Boolean(),
                        Comment = c.String(nullable: false, maxLength: 200),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.LicenseTrainingStateId);
            
            CreateTable(
                "dbo.LicenseTypes",
                c => new
                    {
                        LicenseTypeId = c.Guid(nullable: false),
                        LicenseTypeName = c.String(nullable: false, maxLength: 50),
                        Expires = c.Boolean(nullable: false),
                        ExpiresAfterMonths = c.Int(),
                        Comment = c.String(nullable: false, maxLength: 200),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.LicenseTypeId);
            
            CreateTable(
                "dbo.PersonClub",
                c => new
                    {
                        PersonId = c.Guid(nullable: false),
                        ClubId = c.Guid(nullable: false),
                        MemberNumber = c.String(maxLength: 20),
                        MemberKey = c.String(maxLength: 40),
                        IsMotorPilot = c.Boolean(nullable: false),
                        IsTowPilot = c.Boolean(nullable: false),
                        IsGliderInstructor = c.Boolean(nullable: false),
                        IsGliderPilot = c.Boolean(nullable: false),
                        IsGliderTrainee = c.Boolean(nullable: false),
                        IsPassenger = c.Boolean(nullable: false),
                        IsWinchOperator = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.ClubId })
                .ForeignKey("dbo.Persons", t => t.PersonId)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .Index(t => t.PersonId)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.PersonMemberStates",
                c => new
                    {
                        PersonId = c.Guid(nullable: false),
                        MemberStateId = c.Guid(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.MemberStateId, t.ValidFrom })
                .ForeignKey("dbo.MemberStates", t => t.MemberStateId, cascadeDelete: true)
                .ForeignKey("dbo.Persons", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.MemberStateId);
            
            CreateTable(
                "dbo.MemberStates",
                c => new
                    {
                        MemberStateId = c.Guid(nullable: false),
                        ClubId = c.Guid(nullable: false),
                        MemberStateName = c.String(nullable: false, maxLength: 50),
                        Remarks = c.String(maxLength: 250),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MemberStateId)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.PersonPersonCategories",
                c => new
                    {
                        PersonId = c.Guid(nullable: false),
                        PersonCategoryId = c.Guid(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(precision: 7, storeType: "datetime2"),
                        SortIndicator = c.Int(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PersonId, t.PersonCategoryId, t.ValidFrom })
                .ForeignKey("dbo.Persons", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.PersonCategories", t => t.PersonCategoryId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.PersonCategoryId);
            
            CreateTable(
                "dbo.PersonCategories",
                c => new
                    {
                        PersonCategoryId = c.Guid(nullable: false),
                        ClubId = c.Guid(nullable: false),
                        CategoryName = c.String(nullable: false, maxLength: 100),
                        Remarks = c.String(maxLength: 250),
                        ParentPersonCategoryId = c.Guid(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PersonCategoryId)
                .ForeignKey("dbo.PersonCategories", t => t.ParentPersonCategoryId)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .Index(t => t.ClubId)
                .Index(t => t.ParentPersonCategoryId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ClubId = c.Guid(nullable: false),
                        Username = c.String(nullable: false, maxLength: 50),
                        FriendlyName = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 255),
                        PersonId = c.Guid(),
                        NotificationEmail = c.String(nullable: false, maxLength: 100),
                        Remarks = c.String(maxLength: 250),
                        FailedLoginCounts = c.Int(nullable: false),
                        AccountState = c.Int(nullable: false),
                        LastPasswordChangeOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ForcePasswordChangeNextLogon = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Persons", t => t.PersonId)
                .ForeignKey("dbo.UserAccountStates", t => t.AccountState)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .Index(t => t.ClubId)
                .Index(t => t.PersonId)
                .Index(t => t.AccountState);
            
            CreateTable(
                "dbo.UserAccountStates",
                c => new
                    {
                        UserAccountStateId = c.Int(nullable: false, identity: true),
                        UserAccountStateName = c.String(nullable: false, maxLength: 50),
                        Comment = c.String(nullable: false, maxLength: 200),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.UserAccountStateId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Guid(nullable: false),
                        RoleName = c.String(nullable: false, maxLength: 100),
                        RoleApplicationKeyString = c.String(nullable: false, maxLength: 100),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        PermissionId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        IsSuperUser = c.Boolean(nullable: false),
                        CanCreateAircrafts = c.Boolean(nullable: false),
                        CanReadOwnedAircrafts = c.Boolean(nullable: false),
                        CanReadClubOwnedAircrafts = c.Boolean(nullable: false),
                        CanReadOtherAircrafts = c.Boolean(nullable: false),
                        CanUpdateOwnedAircrafts = c.Boolean(nullable: false),
                        CanUpdateClubOwnedAircrafts = c.Boolean(nullable: false),
                        CanUpdateOtherAircrafts = c.Boolean(nullable: false),
                        CanDeleteOwnedAircrafts = c.Boolean(nullable: false),
                        CanDeleteClubOwnedAircrafts = c.Boolean(nullable: false),
                        CanDeleteOtherAircrafts = c.Boolean(nullable: false),
                        CanImportAircrafts = c.Boolean(nullable: false),
                        CanExportOwnedAircrafts = c.Boolean(nullable: false),
                        CanExportClubOwnedAircrafts = c.Boolean(nullable: false),
                        CanExportOtherAircrafts = c.Boolean(nullable: false),
                        CanUpdateStateOfOwnedAircrafts = c.Boolean(nullable: false),
                        CanUpdateStateOfClubOwnedAircrafts = c.Boolean(nullable: false),
                        CanUpdateStateOfOtherAircrafts = c.Boolean(nullable: false),
                        CanCreateClubs = c.Boolean(nullable: false),
                        CanReadOwnedClubs = c.Boolean(nullable: false),
                        CanReadOwnClub = c.Boolean(nullable: false),
                        CanReadOtherClubs = c.Boolean(nullable: false),
                        CanUpdateOwnedClubs = c.Boolean(nullable: false),
                        CanUpdateOwnClub = c.Boolean(nullable: false),
                        CanUpdateOtherClubs = c.Boolean(nullable: false),
                        CanDeleteOwnedClubs = c.Boolean(nullable: false),
                        CanDeleteOwnClub = c.Boolean(nullable: false),
                        CanDeleteOtherClubs = c.Boolean(nullable: false),
                        CanImportClubs = c.Boolean(nullable: false),
                        CanExportOwnedClubs = c.Boolean(nullable: false),
                        CanExportOwnClub = c.Boolean(nullable: false),
                        CanExportOtherClubs = c.Boolean(nullable: false),
                        CanCreateLocations = c.Boolean(nullable: false),
                        CanReadOwnedLocations = c.Boolean(nullable: false),
                        CanReadClubOwnedLocations = c.Boolean(nullable: false),
                        CanReadOtherLocations = c.Boolean(nullable: false),
                        CanUpdateOwnedLocations = c.Boolean(nullable: false),
                        CanUpdateClubOwnedLocations = c.Boolean(nullable: false),
                        CanUpdateOtherLocations = c.Boolean(nullable: false),
                        CanDeleteOwnedLocations = c.Boolean(nullable: false),
                        CanDeleteClubOwnedLocations = c.Boolean(nullable: false),
                        CanDeleteOtherLocations = c.Boolean(nullable: false),
                        CanImportLocations = c.Boolean(nullable: false),
                        CanExportOwnedLocations = c.Boolean(nullable: false),
                        CanExportClubOwnedLocations = c.Boolean(nullable: false),
                        CanExportOtherLocations = c.Boolean(nullable: false),
                        CanCreateCountries = c.Boolean(nullable: false),
                        CanReadOwnedCountries = c.Boolean(nullable: false),
                        CanReadClubOwnedCountries = c.Boolean(nullable: false),
                        CanReadOtherCountries = c.Boolean(nullable: false),
                        CanUpdateOwnedCountries = c.Boolean(nullable: false),
                        CanUpdateClubOwnedCountries = c.Boolean(nullable: false),
                        CanUpdateOtherCountries = c.Boolean(nullable: false),
                        CanDeleteOwnedCountries = c.Boolean(nullable: false),
                        CanDeleteClubOwnedCountries = c.Boolean(nullable: false),
                        CanDeleteOtherCountries = c.Boolean(nullable: false),
                        CanImportCountries = c.Boolean(nullable: false),
                        CanExportOwnedCountries = c.Boolean(nullable: false),
                        CanExportClubOwnedCountries = c.Boolean(nullable: false),
                        CanExportOtherCountries = c.Boolean(nullable: false),
                        CanCreateFlightTypes = c.Boolean(nullable: false),
                        CanReadOwnedFlightTypes = c.Boolean(nullable: false),
                        CanReadClubOwnedFlightTypes = c.Boolean(nullable: false),
                        CanReadOtherFlightTypes = c.Boolean(nullable: false),
                        CanUpdateOwnedFlightTypes = c.Boolean(nullable: false),
                        CanUpdateClubOwnedFlightTypes = c.Boolean(nullable: false),
                        CanUpdateOtherFlightTypes = c.Boolean(nullable: false),
                        CanDeleteOwnedFlightTypes = c.Boolean(nullable: false),
                        CanDeleteClubOwnedFlightTypes = c.Boolean(nullable: false),
                        CanDeleteOtherFlightTypes = c.Boolean(nullable: false),
                        CanImportFlightTypes = c.Boolean(nullable: false),
                        CanExportOwnedFlightTypes = c.Boolean(nullable: false),
                        CanExportClubOwnedFlightTypes = c.Boolean(nullable: false),
                        CanExportOtherFlightTypes = c.Boolean(nullable: false),
                        CanCreateMemberStates = c.Boolean(nullable: false),
                        CanReadOwnedMemberStates = c.Boolean(nullable: false),
                        CanReadClubOwnedMemberStates = c.Boolean(nullable: false),
                        CanReadOtherMemberStates = c.Boolean(nullable: false),
                        CanUpdateOwnedMemberStates = c.Boolean(nullable: false),
                        CanUpdateClubOwnedMemberStates = c.Boolean(nullable: false),
                        CanUpdateOtherMemberStates = c.Boolean(nullable: false),
                        CanDeleteOwnedMemberStates = c.Boolean(nullable: false),
                        CanDeleteClubOwnedMemberStates = c.Boolean(nullable: false),
                        CanDeleteOtherMemberStates = c.Boolean(nullable: false),
                        CanImportMemberStates = c.Boolean(nullable: false),
                        CanExportOwnedMemberStates = c.Boolean(nullable: false),
                        CanExportClubOwnedMemberStates = c.Boolean(nullable: false),
                        CanExportOtherMemberStates = c.Boolean(nullable: false),
                        CanCreatePersonCategories = c.Boolean(nullable: false),
                        CanReadOwnedPersonCategories = c.Boolean(nullable: false),
                        CanReadClubOwnedPersonCategories = c.Boolean(nullable: false),
                        CanReadOtherPersonCategories = c.Boolean(nullable: false),
                        CanUpdateOwnedPersonCategories = c.Boolean(nullable: false),
                        CanUpdateClubOwnedPersonCategories = c.Boolean(nullable: false),
                        CanUpdateOtherPersonCategories = c.Boolean(nullable: false),
                        CanDeleteOwnedPersonCategories = c.Boolean(nullable: false),
                        CanDeleteClubOwnedPersonCategories = c.Boolean(nullable: false),
                        CanDeleteOtherPersonCategories = c.Boolean(nullable: false),
                        CanImportPersonCategories = c.Boolean(nullable: false),
                        CanExportOwnedPersonCategories = c.Boolean(nullable: false),
                        CanExportClubOwnedPersonCategories = c.Boolean(nullable: false),
                        CanExportOtherPersonCategories = c.Boolean(nullable: false),
                        CanCreateFlights = c.Boolean(nullable: false),
                        CanReadOwnedFlights = c.Boolean(nullable: false),
                        CanReadClubOwnedFlights = c.Boolean(nullable: false),
                        CanReadOtherFlights = c.Boolean(nullable: false),
                        CanUpdateOwnedFlights = c.Boolean(nullable: false),
                        CanUpdateClubOwnedFlights = c.Boolean(nullable: false),
                        CanUpdateOtherFlights = c.Boolean(nullable: false),
                        CanDeleteOwnedFlights = c.Boolean(nullable: false),
                        CanDeleteClubOwnedFlights = c.Boolean(nullable: false),
                        CanDeleteOtherFlights = c.Boolean(nullable: false),
                        CanImportFlights = c.Boolean(nullable: false),
                        CanExportOwnedFlights = c.Boolean(nullable: false),
                        CanExportClubOwnedFlights = c.Boolean(nullable: false),
                        CanExportOtherFlights = c.Boolean(nullable: false),
                        CanSetLandingDataOfOtherFlights = c.Boolean(nullable: false),
                        CanUnlockOwnedFlights = c.Boolean(nullable: false),
                        CanUnlockClubOwnedFlights = c.Boolean(nullable: false),
                        CanUnlockOtherFlights = c.Boolean(nullable: false),
                        CanCreateSystemFlights = c.Boolean(nullable: false),
                        CanReadOwnedSystemFlights = c.Boolean(nullable: false),
                        CanReadClubOwnedSystemFlights = c.Boolean(nullable: false),
                        CanReadOtherSystemFlights = c.Boolean(nullable: false),
                        CanUpdateOwnedSystemFlights = c.Boolean(nullable: false),
                        CanUpdateClubOwnedSystemFlights = c.Boolean(nullable: false),
                        CanUpdateOtherSystemFlights = c.Boolean(nullable: false),
                        CanDeleteOwnedSystemFlights = c.Boolean(nullable: false),
                        CanDeleteClubOwnedSystemFlights = c.Boolean(nullable: false),
                        CanDeleteOtherSystemFlights = c.Boolean(nullable: false),
                        CanImportSystemFlights = c.Boolean(nullable: false),
                        CanExportOwnedSystemFlights = c.Boolean(nullable: false),
                        CanExportClubOwnedSystemFlights = c.Boolean(nullable: false),
                        CanExportOtherSystemFlights = c.Boolean(nullable: false),
                        CanCreatePersons = c.Boolean(nullable: false),
                        CanReadOwnedPersons = c.Boolean(nullable: false),
                        CanReadClubOwnedPersons = c.Boolean(nullable: false),
                        CanReadOtherPersons = c.Boolean(nullable: false),
                        CanUpdateOwnedPersons = c.Boolean(nullable: false),
                        CanUpdateClubOwnedPersons = c.Boolean(nullable: false),
                        CanUpdateOtherPersons = c.Boolean(nullable: false),
                        CanDeleteOwnedPersons = c.Boolean(nullable: false),
                        CanDeleteClubOwnedPersons = c.Boolean(nullable: false),
                        CanDeleteOtherPersons = c.Boolean(nullable: false),
                        CanImportPersons = c.Boolean(nullable: false),
                        CanExportOwnedPersons = c.Boolean(nullable: false),
                        CanExportClubOwnedPersons = c.Boolean(nullable: false),
                        CanExportOtherPersons = c.Boolean(nullable: false),
                        CanReadOwnedPersonsSensitiveData = c.Boolean(nullable: false),
                        CanReadClubOwnedPersonsSensitiveData = c.Boolean(nullable: false),
                        CanReadOtherPersonsSensitiveData = c.Boolean(nullable: false),
                        CanUpdateOwnedPersonsSensitiveData = c.Boolean(nullable: false),
                        CanUpdateClubOwnedPersonsSensitiveData = c.Boolean(nullable: false),
                        CanUpdateOtherPersonsSensitiveData = c.Boolean(nullable: false),
                        CanExportOwnedPersonsSensitiveData = c.Boolean(nullable: false),
                        CanExportClubOwnedPersonsSensitiveData = c.Boolean(nullable: false),
                        CanExportOtherPersonsSensitiveData = c.Boolean(nullable: false),
                        CanCreateUsers = c.Boolean(nullable: false),
                        CanReadOwnedUsers = c.Boolean(nullable: false),
                        CanReadClubOwnedUsers = c.Boolean(nullable: false),
                        CanReadOtherUsers = c.Boolean(nullable: false),
                        CanUpdateOwnedUsers = c.Boolean(nullable: false),
                        CanUpdateClubOwnedUsers = c.Boolean(nullable: false),
                        CanUpdateOtherUsers = c.Boolean(nullable: false),
                        CanDeleteOwnedUsers = c.Boolean(nullable: false),
                        CanDeleteClubOwnedUsers = c.Boolean(nullable: false),
                        CanDeleteOtherUsers = c.Boolean(nullable: false),
                        CanImportUsers = c.Boolean(nullable: false),
                        CanExportOwnedUsers = c.Boolean(nullable: false),
                        CanExportClubOwnedUsers = c.Boolean(nullable: false),
                        CanExportOtherUsers = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PermissionId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AircraftReservationTypes",
                c => new
                    {
                        AircraftReservationTypeId = c.Int(nullable: false, identity: true),
                        AircraftReservationTypeName = c.String(nullable: false, maxLength: 50),
                        Remarks = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.AircraftReservationTypeId);
            
            CreateTable(
                "dbo.ClubExtensions",
                c => new
                    {
                        ClubId = c.Guid(nullable: false),
                        ExtensionId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.ClubId, t.ExtensionId })
                .ForeignKey("dbo.Extensions", t => t.ExtensionId)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .Index(t => t.ClubId)
                .Index(t => t.ExtensionId);
            
            CreateTable(
                "dbo.Extensions",
                c => new
                    {
                        ExtensionId = c.Guid(nullable: false),
                        ExtensionName = c.String(nullable: false, maxLength: 50),
                        ExtenstionClassName = c.String(nullable: false, maxLength: 100),
                        ExtenstionFullClassName = c.String(nullable: false, maxLength: 250),
                        ExtensionDllPublicKey = c.String(),
                        ExtensionDllFilename = c.String(maxLength: 250),
                        ExtensionTypeId = c.Int(nullable: false),
                        IsPublic = c.Boolean(nullable: false),
                        Comment = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ExtensionId)
                .ForeignKey("dbo.ExtensionTypes", t => t.ExtensionTypeId)
                .Index(t => t.ExtensionTypeId);
            
            CreateTable(
                "dbo.ExtensionParameters",
                c => new
                    {
                        ExtensionParameterId = c.Guid(nullable: false),
                        ExtensionId = c.Guid(nullable: false),
                        ExtensionParameterName = c.String(nullable: false, maxLength: 50),
                        ExtensionParameterKeyString = c.String(nullable: false, maxLength: 50),
                        ExtensionParameterType = c.Int(nullable: false),
                        Comment = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ExtensionParameterId)
                .ForeignKey("dbo.ExtensionParameterTypes", t => t.ExtensionParameterType)
                .ForeignKey("dbo.Extensions", t => t.ExtensionId)
                .Index(t => t.ExtensionId)
                .Index(t => t.ExtensionParameterType);
            
            CreateTable(
                "dbo.ExtensionParameterTypes",
                c => new
                    {
                        ExtensionParameterTypeId = c.Int(nullable: false, identity: true),
                        ExtensionParameterTypeName = c.String(nullable: false, maxLength: 50),
                        StoreValuesAsBinaryData = c.Boolean(nullable: false),
                        Comment = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ExtensionParameterTypeId);
            
            CreateTable(
                "dbo.ExtensionParameterValues",
                c => new
                    {
                        ExtensionParameterValueId = c.Guid(nullable: false),
                        ExtensionParameterValue = c.String(),
                        ExtensionParameterBinaryValue = c.Binary(),
                        ExtensionParameterId = c.Guid(nullable: false),
                        ClubId = c.Guid(),
                        IsDefault = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedByUserId = c.Guid(nullable: false),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        ModifiedByUserId = c.Guid(),
                        DeletedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                        DeletedByUserId = c.Guid(),
                        RecordState = c.Int(),
                        OwnerId = c.Guid(nullable: false),
                        OwnershipType = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ExtensionParameterValueId)
                .ForeignKey("dbo.Clubs", t => t.ClubId)
                .ForeignKey("dbo.ExtensionParameters", t => t.ExtensionParameterId)
                .Index(t => t.ExtensionParameterId)
                .Index(t => t.ClubId);
            
            CreateTable(
                "dbo.ExtensionTypes",
                c => new
                    {
                        ExtensionTypeId = c.Int(nullable: false, identity: true),
                        ExtensionTypeName = c.String(nullable: false, maxLength: 50),
                        Comment = c.String(),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ExtensionTypeId);
            
            CreateTable(
                "dbo.AircraftTypes",
                c => new
                    {
                        AircraftTypeId = c.Int(nullable: false, identity: true),
                        AircraftTypeName = c.String(nullable: false, maxLength: 50),
                        Comment = c.String(nullable: false, maxLength: 200),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.AircraftTypeId);
            
            CreateTable(
                "dbo.AircraftStates",
                c => new
                    {
                        AircraftStateId = c.Int(nullable: false, identity: true),
                        AircraftStateName = c.String(nullable: false, maxLength: 50),
                        IsAircraftFlyable = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedOn = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.AircraftStateId);
            
            CreateTable(
                "dbo.SystemData",
                c => new
                    {
                        SystemId = c.Guid(nullable: false),
                        ReportSenderEmailAddress = c.String(nullable: false, maxLength: 100),
                        SystemSenderEmailAddress = c.String(nullable: false, maxLength: 100),
                        SmtpUsername = c.String(nullable: false, maxLength: 100),
                        SmtpPasswort = c.String(nullable: false, maxLength: 100),
                        SmtpServer = c.String(nullable: false, maxLength: 100),
                        SmtpPort = c.Int(nullable: false),
                        WorkflowStartsOn = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        MaxUserLoginAttempts = c.Int(nullable: false),
                        UrlToFlightReport = c.String(maxLength: 255),
                        UrlToBAZLReport = c.String(maxLength: 255),
                        UrlToInvoiceReport = c.String(maxLength: 255),
                        SystemPathToReports = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.SystemId);
            
            CreateTable(
                "dbo.SystemLogs",
                c => new
                    {
                        LogId = c.Long(nullable: false, identity: true),
                        EventDateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Application = c.String(maxLength: 200),
                        LogLevel = c.String(nullable: false, maxLength: 100),
                        EventType = c.Long(),
                        Logger = c.String(),
                        Message = c.String(),
                        UserName = c.String(maxLength: 100),
                        ComputerName = c.String(maxLength: 50),
                        CallSite = c.String(),
                        Thread = c.String(maxLength: 100),
                        Exception = c.String(unicode: false),
                        Stacktrace = c.String(unicode: false),
                        SqlString = c.String(unicode: false),
                        Version = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.LogId);
            
            CreateTable(
                "dbo.SystemVersion",
                c => new
                    {
                        VersionId = c.Long(nullable: false),
                        MajorVersion = c.Long(nullable: false),
                        MinorVersion = c.Long(nullable: false),
                        BuildVersion = c.Long(nullable: false),
                        RevisionVersion = c.Long(nullable: false),
                        UpgradeFromVersion = c.String(maxLength: 50),
                        UpgradeDateTime = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.VersionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AircraftAircraftStates", "AircraftState", "dbo.AircraftStates");
            DropForeignKey("dbo.Flights", "AircraftId", "dbo.Aircrafts");
            DropForeignKey("dbo.Aircrafts", "AircraftType", "dbo.AircraftTypes");
            DropForeignKey("dbo.AircraftReservations", "AircraftId", "dbo.Aircrafts");
            DropForeignKey("dbo.Users", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.PlanningDays", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.PlanningDayAssignmentTypes", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.PersonCategories", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.MemberStates", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.FlightTypes", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.PersonClub", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.ClubExtensions", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.Extensions", "ExtensionTypeId", "dbo.ExtensionTypes");
            DropForeignKey("dbo.ExtensionParameters", "ExtensionId", "dbo.Extensions");
            DropForeignKey("dbo.ExtensionParameterValues", "ExtensionParameterId", "dbo.ExtensionParameters");
            DropForeignKey("dbo.ExtensionParameterValues", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.ExtensionParameters", "ExtensionParameterType", "dbo.ExtensionParameterTypes");
            DropForeignKey("dbo.ClubExtensions", "ExtensionId", "dbo.Extensions");
            DropForeignKey("dbo.Aircrafts", "AircraftOwnerClubId", "dbo.Clubs");
            DropForeignKey("dbo.AircraftReservations", "ClubId", "dbo.Clubs");
            DropForeignKey("dbo.AircraftReservations", "ReservationTypeId", "dbo.AircraftReservationTypes");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Permissions", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Users", "AccountState", "dbo.UserAccountStates");
            DropForeignKey("dbo.Users", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.AircraftAircraftStates", "NoticedByPersonId", "dbo.Persons");
            DropForeignKey("dbo.PlanningDayAssignments", "AssignedPersonId", "dbo.Persons");
            DropForeignKey("dbo.PersonPersonCategories", "PersonCategoryId", "dbo.PersonCategories");
            DropForeignKey("dbo.PersonCategories", "ParentPersonCategoryId", "dbo.PersonCategories");
            DropForeignKey("dbo.PersonPersonCategories", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.PersonMemberStates", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.PersonMemberStates", "MemberStateId", "dbo.MemberStates");
            DropForeignKey("dbo.PersonClub", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Aircrafts", "AircraftOwnerPersonId", "dbo.Persons");
            DropForeignKey("dbo.Persons", "LicenseTypeId", "dbo.LicenseTypes");
            DropForeignKey("dbo.Persons", "LicenseTrainingStateTowing", "dbo.LicenseTrainingStates");
            DropForeignKey("dbo.Persons", "LicenseTrainingStateTMG", "dbo.LicenseTrainingStates");
            DropForeignKey("dbo.Persons", "LicenseTrainingStateMotor", "dbo.LicenseTrainingStates");
            DropForeignKey("dbo.Persons", "LicenseTrainingStateGlider", "dbo.LicenseTrainingStates");
            DropForeignKey("dbo.Persons", "LicenseTrainingStateGliderPAX", "dbo.LicenseTrainingStates");
            DropForeignKey("dbo.Persons", "LicenseTrainingStateGliderInstructor", "dbo.LicenseTrainingStates");
            DropForeignKey("dbo.AircraftReservations", "InstructorPersonId", "dbo.Persons");
            DropForeignKey("dbo.FlightCrew", "PersonId", "dbo.Persons");
            DropForeignKey("dbo.Persons", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Locations", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Flights", "StartLocationId", "dbo.Locations");
            DropForeignKey("dbo.PlanningDays", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.PlanningDayAssignments", "AssignedPlanningDayId", "dbo.PlanningDays");
            DropForeignKey("dbo.PlanningDayAssignments", "AssignmentTypeId", "dbo.PlanningDayAssignmentTypes");
            DropForeignKey("dbo.Locations", "LocationTypeId", "dbo.LocationTypes");
            DropForeignKey("dbo.Locations", "RunwayLengthUnitType", "dbo.LengthUnitTypes");
            DropForeignKey("dbo.Flights", "LdgLocationId", "dbo.Locations");
            DropForeignKey("dbo.Flights", "TowFlightId", "dbo.Flights");
            DropForeignKey("dbo.Flights", "StartType", "dbo.StartTypes");
            DropForeignKey("dbo.Clubs", "DefaultStartType", "dbo.StartTypes");
            DropForeignKey("dbo.Flights", "FlightTypeId", "dbo.FlightTypes");
            DropForeignKey("dbo.Clubs", "DefaultTowFlightTypeId", "dbo.FlightTypes");
            DropForeignKey("dbo.Clubs", "DefaultMotorFlightTypeId", "dbo.FlightTypes");
            DropForeignKey("dbo.Clubs", "DefaultGliderFlightTypeId", "dbo.FlightTypes");
            DropForeignKey("dbo.Flights", "FlightState", "dbo.FlightStates");
            DropForeignKey("dbo.FlightCrew", "FlightCrewType", "dbo.FlightCrewTypes");
            DropForeignKey("dbo.FlightCrew", "FlightId", "dbo.Flights");
            DropForeignKey("dbo.Flights", "FlightCostBalanceType", "dbo.FlightCostBalanceTypes");
            DropForeignKey("dbo.InOutboundPoints", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Locations", "ElevationUnitType", "dbo.ElevationUnitTypes");
            DropForeignKey("dbo.Clubs", "HomebaseId", "dbo.Locations");
            DropForeignKey("dbo.Clubs", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.AircraftReservations", "PilotPersonId", "dbo.Persons");
            DropForeignKey("dbo.AircraftAircraftStates", "AircraftId", "dbo.Aircrafts");
            DropIndex("dbo.ExtensionParameterValues", new[] { "ClubId" });
            DropIndex("dbo.ExtensionParameterValues", new[] { "ExtensionParameterId" });
            DropIndex("dbo.ExtensionParameters", new[] { "ExtensionParameterType" });
            DropIndex("dbo.ExtensionParameters", new[] { "ExtensionId" });
            DropIndex("dbo.Extensions", new[] { "ExtensionTypeId" });
            DropIndex("dbo.ClubExtensions", new[] { "ExtensionId" });
            DropIndex("dbo.ClubExtensions", new[] { "ClubId" });
            DropIndex("dbo.Permissions", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "AccountState" });
            DropIndex("dbo.Users", new[] { "PersonId" });
            DropIndex("dbo.Users", new[] { "ClubId" });
            DropIndex("dbo.PersonCategories", new[] { "ParentPersonCategoryId" });
            DropIndex("dbo.PersonCategories", new[] { "ClubId" });
            DropIndex("dbo.PersonPersonCategories", new[] { "PersonCategoryId" });
            DropIndex("dbo.PersonPersonCategories", new[] { "PersonId" });
            DropIndex("dbo.MemberStates", new[] { "ClubId" });
            DropIndex("dbo.PersonMemberStates", new[] { "MemberStateId" });
            DropIndex("dbo.PersonMemberStates", new[] { "PersonId" });
            DropIndex("dbo.PersonClub", new[] { "ClubId" });
            DropIndex("dbo.PersonClub", new[] { "PersonId" });
            DropIndex("dbo.PlanningDayAssignmentTypes", new[] { "ClubId" });
            DropIndex("dbo.PlanningDayAssignments", new[] { "AssignmentTypeId" });
            DropIndex("dbo.PlanningDayAssignments", new[] { "AssignedPersonId" });
            DropIndex("dbo.PlanningDayAssignments", new[] { "AssignedPlanningDayId" });
            DropIndex("dbo.PlanningDays", new[] { "LocationId" });
            DropIndex("dbo.PlanningDays", new[] { "ClubId" });
            DropIndex("dbo.FlightTypes", new[] { "ClubId" });
            DropIndex("dbo.FlightCrew", new[] { "FlightCrewType" });
            DropIndex("dbo.FlightCrew", new[] { "PersonId" });
            DropIndex("dbo.FlightCrew", new[] { "FlightId" });
            DropIndex("dbo.Flights", new[] { "FlightCostBalanceType" });
            DropIndex("dbo.Flights", new[] { "FlightState" });
            DropIndex("dbo.Flights", new[] { "TowFlightId" });
            DropIndex("dbo.Flights", new[] { "StartType" });
            DropIndex("dbo.Flights", new[] { "FlightTypeId" });
            DropIndex("dbo.Flights", new[] { "LdgLocationId" });
            DropIndex("dbo.Flights", new[] { "StartLocationId" });
            DropIndex("dbo.Flights", new[] { "AircraftId" });
            DropIndex("dbo.InOutboundPoints", new[] { "LocationId" });
            DropIndex("dbo.Locations", new[] { "RunwayLengthUnitType" });
            DropIndex("dbo.Locations", new[] { "ElevationUnitType" });
            DropIndex("dbo.Locations", new[] { "LocationTypeId" });
            DropIndex("dbo.Locations", new[] { "CountryId" });
            DropIndex("dbo.Persons", new[] { "LicenseTrainingStateMotor" });
            DropIndex("dbo.Persons", new[] { "LicenseTrainingStateTMG" });
            DropIndex("dbo.Persons", new[] { "LicenseTrainingStateTowing" });
            DropIndex("dbo.Persons", new[] { "LicenseTrainingStateGliderInstructor" });
            DropIndex("dbo.Persons", new[] { "LicenseTrainingStateGliderPAX" });
            DropIndex("dbo.Persons", new[] { "LicenseTrainingStateGlider" });
            DropIndex("dbo.Persons", new[] { "LicenseTypeId" });
            DropIndex("dbo.Persons", new[] { "CountryId" });
            DropIndex("dbo.AircraftReservations", new[] { "PilotPersonId" });
            DropIndex("dbo.AircraftReservations", new[] { "ClubId" });
            DropIndex("dbo.AircraftReservations", new[] { "ReservationTypeId" });
            DropIndex("dbo.AircraftReservations", new[] { "InstructorPersonId" });
            DropIndex("dbo.AircraftReservations", new[] { "AircraftId" });
            DropIndex("dbo.Clubs", new[] { "DefaultMotorFlightTypeId" });
            DropIndex("dbo.Clubs", new[] { "DefaultTowFlightTypeId" });
            DropIndex("dbo.Clubs", new[] { "DefaultGliderFlightTypeId" });
            DropIndex("dbo.Clubs", new[] { "DefaultStartType" });
            DropIndex("dbo.Clubs", new[] { "HomebaseId" });
            DropIndex("dbo.Clubs", new[] { "CountryId" });
            DropIndex("dbo.Aircrafts", new[] { "AircraftOwnerPersonId" });
            DropIndex("dbo.Aircrafts", new[] { "AircraftOwnerClubId" });
            DropIndex("dbo.Aircrafts", new[] { "AircraftType" });
            DropIndex("dbo.AircraftAircraftStates", new[] { "NoticedByPersonId" });
            DropIndex("dbo.AircraftAircraftStates", new[] { "AircraftState" });
            DropIndex("dbo.AircraftAircraftStates", new[] { "AircraftId" });
            DropTable("dbo.SystemVersion");
            DropTable("dbo.SystemLogs");
            DropTable("dbo.SystemData");
            DropTable("dbo.AircraftStates");
            DropTable("dbo.AircraftTypes");
            DropTable("dbo.ExtensionTypes");
            DropTable("dbo.ExtensionParameterValues");
            DropTable("dbo.ExtensionParameterTypes");
            DropTable("dbo.ExtensionParameters");
            DropTable("dbo.Extensions");
            DropTable("dbo.ClubExtensions");
            DropTable("dbo.AircraftReservationTypes");
            DropTable("dbo.Permissions");
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserAccountStates");
            DropTable("dbo.Users");
            DropTable("dbo.PersonCategories");
            DropTable("dbo.PersonPersonCategories");
            DropTable("dbo.MemberStates");
            DropTable("dbo.PersonMemberStates");
            DropTable("dbo.PersonClub");
            DropTable("dbo.LicenseTypes");
            DropTable("dbo.LicenseTrainingStates");
            DropTable("dbo.PlanningDayAssignmentTypes");
            DropTable("dbo.PlanningDayAssignments");
            DropTable("dbo.PlanningDays");
            DropTable("dbo.LocationTypes");
            DropTable("dbo.LengthUnitTypes");
            DropTable("dbo.StartTypes");
            DropTable("dbo.FlightTypes");
            DropTable("dbo.FlightStates");
            DropTable("dbo.FlightCrewTypes");
            DropTable("dbo.FlightCrew");
            DropTable("dbo.FlightCostBalanceTypes");
            DropTable("dbo.Flights");
            DropTable("dbo.InOutboundPoints");
            DropTable("dbo.ElevationUnitTypes");
            DropTable("dbo.Locations");
            DropTable("dbo.Countries");
            DropTable("dbo.Persons");
            DropTable("dbo.AircraftReservations");
            DropTable("dbo.Clubs");
            DropTable("dbo.Aircrafts");
            DropTable("dbo.AircraftAircraftStates");
        }
    }
}
