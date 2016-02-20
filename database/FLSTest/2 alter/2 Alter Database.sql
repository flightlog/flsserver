USE [master]
GO
ALTER DATABASE [FLSTest] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FLSTest].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FLSTest] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FLSTest] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FLSTest] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FLSTest] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FLSTest] SET ARITHABORT OFF 
GO
ALTER DATABASE [FLSTest] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FLSTest] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [FLSTest] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FLSTest] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FLSTest] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FLSTest] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FLSTest] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FLSTest] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FLSTest] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FLSTest] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FLSTest] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FLSTest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FLSTest] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FLSTest] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FLSTest] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FLSTest] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FLSTest] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FLSTest] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FLSTest] SET RECOVERY FULL 
GO
ALTER DATABASE [FLSTest] SET MULTI_USER 
GO
ALTER DATABASE [FLSTest] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FLSTest] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FLSTest] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FLSTest] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [FLSTest]
GO
/****** Object:  Table [dbo].[AircraftAircraftStates]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftAircraftStates](
	[AircraftId] [uniqueidentifier] NOT NULL,
	[AircraftState] [int] NOT NULL,
	[ValidFrom] [datetime2](7) NOT NULL,
	[ValidTo] [datetime2](7) NULL,
	[NoticedByPersonId] [uniqueidentifier] NULL,
	[Remarks] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
 CONSTRAINT [PK_AircraftAircraftStates] PRIMARY KEY CLUSTERED 
(
	[AircraftId] ASC,
	[AircraftState] ASC,
	[ValidFrom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Aircrafts]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aircrafts](
	[AircraftId] [uniqueidentifier] NOT NULL,
	[ManufacturerName] [nvarchar](100) NULL,
	[AircraftModel] [nvarchar](50) NULL,
	[Immatriculation] [nvarchar](15) NOT NULL,
	[CompetitionSign] [nvarchar](5) NULL,
	[NrOfSeats] [int] NULL,
	[DaecIndex] [int] NULL,
	[Comment] [nvarchar](250) NULL,
	[AircraftType] [int] NOT NULL,
	[IsTowingOrWinchRequired] [bit] NOT NULL,
	[IsTowingstartAllowed] [bit] NOT NULL,
	[IsWinchstartAllowed] [bit] NOT NULL,
	[IsTowingAircraft] [bit] NOT NULL,
	[AircraftOwnerClubId] [uniqueidentifier] NULL,
	[AircraftOwnerPersonId] [uniqueidentifier] NULL,
	[FLARMId] [nvarchar](50) NULL,
	[AircraftSerialNumber] [nvarchar](20) NULL,
	[YearOfManufacture] [datetime2](7) NULL,
	[MaintenanceAssignmentId] [int] NULL,
	[NoiseClass] [nvarchar](1) NULL,
	[NoiseLevel] [numeric](18, 1) NULL,
	[MTOM] [int] NULL,
	[FlightDurationPrecision] [int] NOT NULL,
	[SpotLink] [nvarchar](250) NULL,
	[IsFastEntryRecord] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Aircrafts] PRIMARY KEY CLUSTERED 
(
	[AircraftId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_Aircrafts_Immatriculation] UNIQUE NONCLUSTERED 
(
	[Immatriculation] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AircraftStates]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftStates](
	[AircraftStateId] [int] IDENTITY(1,1) NOT NULL,
	[AircraftStateName] [nvarchar](50) NOT NULL,
	[IsAircraftFlyable] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_AircraftStates] PRIMARY KEY CLUSTERED 
(
	[AircraftStateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AircraftTypes]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AircraftTypes](
	[AircraftTypeId] [int] IDENTITY(1,1) NOT NULL,
	[AircraftTypeName] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](200) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_AircraftTypes] PRIMARY KEY CLUSTERED 
(
	[AircraftTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ClubExtensions]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClubExtensions](
	[ClubId] [uniqueidentifier] NOT NULL,
	[ExtensionId] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ClubExtensions] PRIMARY KEY CLUSTERED 
(
	[ClubId] ASC,
	[ExtensionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Clubs]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clubs](
	[ClubId] [uniqueidentifier] NOT NULL,
	[Clubname] [nvarchar](50) NOT NULL,
	[ClubKey] [nvarchar](10) NOT NULL,
	[Address] [nvarchar](100) NULL,
	[Zip] [nvarchar](10) NULL,
	[City] [nvarchar](100) NULL,
	[CountryId] [uniqueidentifier] NOT NULL,
	[Phone] [nvarchar](20) NULL,
	[FaxNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[WebPage] [nvarchar](100) NULL,
	[Contact] [nvarchar](100) NULL,
	[HomebaseId] [uniqueidentifier] NULL,
	[DefaultStartType] [int] NULL,
	[DefaultGliderFlightTypeId] [uniqueidentifier] NULL,
	[DefaultTowFlightTypeId] [uniqueidentifier] NULL,
	[DefaultMotorFlightTypeId] [uniqueidentifier] NULL,
	[IsInboundRouteRequired] [bit] NOT NULL,
	[IsOutboundRouteRequired] [bit] NOT NULL,
	[LastPersonSynchronisationOn] [datetime2](7) NULL,
	[LastInvoiceExportOn] [datetime2](7) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Clubs] PRIMARY KEY CLUSTERED 
(
	[ClubId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_Club_ClubKey] UNIQUE NONCLUSTERED 
(
	[ClubKey] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_Club_Clubname] UNIQUE NONCLUSTERED 
(
	[Clubname] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Countries]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Countries](
	[CountryId] [uniqueidentifier] NOT NULL,
	[CountryIdIso] [int] NOT NULL,
	[CountryName] [nvarchar](100) NOT NULL,
	[CountryFullName] [nvarchar](250) NULL,
	[CountryCodeIso2] [varchar](2) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_Countries_CountryCodeIso2] UNIQUE NONCLUSTERED 
(
	[CountryCodeIso2] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_Countries_CountryIdIso] UNIQUE NONCLUSTERED 
(
	[CountryIdIso] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_Countries_CountryName] UNIQUE NONCLUSTERED 
(
	[CountryName] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ElevationUnitTypes]    Script Date: 03.05.2014 13:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ElevationUnitTypes](
	[ElevationUnitTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ElevationUnitTypeName] [nvarchar](50) NOT NULL,
	[ElevationUnitTypeKeyName] [nvarchar](50) NOT NULL,
	[ElevationUnitTypeShortName] [nvarchar](20) NULL,
	[Comment] [nvarchar](200) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_ElevationUnitTypes] PRIMARY KEY CLUSTERED 
(
	[ElevationUnitTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_ElevationUnitTypes_ElevationUnitTypeKeyName] UNIQUE NONCLUSTERED 
(
	[ElevationUnitTypeKeyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExtensionParameters]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtensionParameters](
	[ExtensionParameterId] [uniqueidentifier] NOT NULL,
	[ExtensionId] [uniqueidentifier] NOT NULL,
	[ExtensionParameterName] [nvarchar](50) NOT NULL,
	[ExtensionParameterKeyString] [nvarchar](50) NOT NULL,
	[ExtensionParameterType] [int] NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ExtensionParameters] PRIMARY KEY CLUSTERED 
(
	[ExtensionParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExtensionParameterTypes]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtensionParameterTypes](
	[ExtensionParameterTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ExtensionParameterTypeName] [nvarchar](50) NOT NULL,
	[StoreValuesAsBinaryData] [bit] NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_ExtensionParameterTypes] PRIMARY KEY CLUSTERED 
(
	[ExtensionParameterTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExtensionParameterValues]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ExtensionParameterValues](
	[ExtensionParameterValueId] [uniqueidentifier] NOT NULL,
	[ExtensionParameterValue] [nvarchar](max) NULL,
	[ExtensionParameterBinaryValue] [varbinary](max) NULL,
	[ExtensionParameterId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NULL,
	[IsDefault] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ExtensionParameterValues] PRIMARY KEY CLUSTERED 
(
	[ExtensionParameterValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Extensions]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Extensions](
	[ExtensionId] [uniqueidentifier] NOT NULL,
	[ExtensionName] [nvarchar](50) NOT NULL,
	[ExtenstionClassName] [nvarchar](100) NOT NULL,
	[ExtenstionFullClassName] [nvarchar](250) NOT NULL,
	[ExtensionDllPublicKey] [nvarchar](max) NULL,
	[ExtensionDllFilename] [nvarchar](250) NULL,
	[ExtensionTypeId] [int] NOT NULL,
	[IsPublic] [bit] NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Extensions] PRIMARY KEY CLUSTERED 
(
	[ExtensionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExtensionTypes]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtensionTypes](
	[ExtensionTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ExtensionTypeName] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_ExtensionTypes_1] PRIMARY KEY CLUSTERED 
(
	[ExtensionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FlightCostBalanceTypes]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlightCostBalanceTypes](
	[FlightCostBalanceTypeId] [int] IDENTITY(1,1) NOT NULL,
	[FlightCostBalanceTypeName] [nvarchar](100) NOT NULL,
	[Comment] [nvarchar](500) NULL,
	[PersonForInvoiceRequired] [bit] NOT NULL,
	[IsForGliderFlights] [bit] NOT NULL,
	[IsForTowFlights] [bit] NOT NULL,
	[IsForMotorFlights] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_FlightCostBalanceTypes] PRIMARY KEY CLUSTERED 
(
	[FlightCostBalanceTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FlightCrew]    Script Date: 02.05.2014 21:50:59 ******/
/****** Allow same pilot and instructor or observable person for self teaching flights (include FlightCrewType to unique constraint ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlightCrew](
	[FlightCrewId] [uniqueidentifier] NOT NULL,
	[FlightId] [uniqueidentifier] NOT NULL,
	[PersonId] [uniqueidentifier] NOT NULL,
	[FlightCrewType] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_FlightCrew] PRIMARY KEY CLUSTERED 
(
	[FlightCrewId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_FlightCrews_Person_Flight_FlightCrewType] UNIQUE NONCLUSTERED 
(
	[FlightId] ASC,
	[PersonId] ASC,
	[FlightCrewType] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FlightCrewTypes]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlightCrewTypes](
	[FlightCrewTypeId] [int] IDENTITY(1,1) NOT NULL,
	[FlightCrewTypeName] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](200) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_FlightCrewTypes] PRIMARY KEY CLUSTERED 
(
	[FlightCrewTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Flights]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Flights](
	[FlightId] [uniqueidentifier] NOT NULL,
	[AircraftId] [uniqueidentifier] NOT NULL,
	[StartPosition] [int] NULL,
	[StartDateTime] [datetime2](7) NULL,
	[LdgDateTime] [datetime2](7) NULL,
	[EngineTime] [datetime2](7) NULL,
	[BlockStartDateTime] [datetime2](7) NULL,
	[BlockEndDateTime] [datetime2](7) NULL,
	[StartLocationId] [uniqueidentifier] NULL,
	[LdgLocationId] [uniqueidentifier] NULL,
	[StartRunway] [nvarchar](5) NULL,
	[LdgRunway] [nvarchar](5) NULL,
	[OutboundRoute] [nvarchar](50) NULL,
	[InboundRoute] [nvarchar](50) NULL,
	[FlightTypeId] [uniqueidentifier] NULL,
	[IsSoloFlight] [bit] NOT NULL,
	[StartType] [int] NULL,
	[TowFlightId] [uniqueidentifier] NULL,
	[NrOfLdgs] [int] NULL,
	[FlightState] [int] NOT NULL,
	[FlightAircraftType] [int] NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[IncidentComment] [nvarchar](max) NULL,
	[PaxGiftCouponNumber] [nvarchar](20) NULL,
	[FlightCostBalanceType] [int] NULL,
	[InvoicedOn] [datetime2](7) NULL,
	[InvoiceNumber] [nvarchar](100) NULL,
	[DeliveryNumber] [nvarchar](100) NULL,
	[InvoicePaidOn] [datetime2](7) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Flights] PRIMARY KEY CLUSTERED 
(
	[FlightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FlightStates]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlightStates](
	[FlightStateId] [int] IDENTITY(1,1) NOT NULL,
	[FlightStateName] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](200) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_FlightStates] PRIMARY KEY CLUSTERED 
(
	[FlightStateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FlightTypes]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FlightTypes](
	[FlightTypeId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[FlightTypeName] [nvarchar](100) NOT NULL,
	[FlightCode] [nvarchar](30) NULL,
	[InstructorRequired] [bit] NOT NULL,
	[ObserverPilotOrInstructorRequired] [bit] NOT NULL,
	[IsCheckFlight] [bit] NOT NULL,
	[IsPassengerFlight] [bit] NOT NULL,
	[IsSoloFlight] [bit] NOT NULL,
	[IsSummarizedSystemFlight] [bit] NOT NULL,
	[IsForGliderFlights] [bit] NOT NULL,
	[IsForTowFlights] [bit] NOT NULL,
	[IsForMotorFlights] [bit] NOT NULL,
	[IsFlightCostBalanceSelectable] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_FlightTypes] PRIMARY KEY CLUSTERED 
(
	[FlightTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_FlightTypes_FlightTypeName] UNIQUE NONCLUSTERED 
(
	[FlightTypeName] ASC,
	[ClubId] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[InOutboundPoints]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InOutboundPoints](
	[InOutboundPointId] [uniqueidentifier] NOT NULL,
	[LocationId] [uniqueidentifier] NOT NULL,
	[InOutboundPointName] [nvarchar](50) NOT NULL,
	[IsInboundPoint] [bit] NOT NULL,
	[IsOutboundPoint] [bit] NOT NULL,
	[SortIndicatorInboundPoint] [int] NOT NULL,
	[SortIndicatorOutboundPoint] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_InOutboundPoints] PRIMARY KEY CLUSTERED 
(
	[InOutboundPointId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LengthUnitTypes]    Script Date: 03.05.2014 13:15:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LengthUnitTypes](
	[LengthUnitTypeId] [int] IDENTITY(1,1) NOT NULL,
	[LengthUnitTypeName] [nvarchar](50) NOT NULL,
	[LengthUnitTypeKeyName] [nvarchar](50) NOT NULL,
	[LengthUnitTypeShortName] [nvarchar](20) NULL,
	[Comment] [nvarchar](200) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_LengthUnitTypes] PRIMARY KEY CLUSTERED 
(
	[LengthUnitTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_LengthUnitTypes_LengthUnitTypeKeyName] UNIQUE NONCLUSTERED 
(
	[LengthUnitTypeKeyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LicenseTrainingStates]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LicenseTrainingStates](
	[LicenseTrainingStateId] [int] IDENTITY(1,1) NOT NULL,
	[LicenseTrainingStateName] [nvarchar](50) NOT NULL,
	[CanFly] [bit] NULL,
	[RequiresInstructor] [bit] NULL,
	[Comment] [nvarchar](200) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_LicenseTrainingStates] PRIMARY KEY CLUSTERED 
(
	[LicenseTrainingStateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LicenseTypes]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LicenseTypes](
	[LicenseTypeId] [uniqueidentifier] NOT NULL,
	[LicenseTypeName] [nvarchar](50) NOT NULL,
	[Expires] [bit] NOT NULL,
	[ExpiresAfterMonths] [int] NULL,
	[Comment] [nvarchar](200) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_LicenseTypes] PRIMARY KEY CLUSTERED 
(
	[LicenseTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Locations]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[LocationId] [uniqueidentifier] NOT NULL,
	[LocationName] [nvarchar](100) NOT NULL,
	[LocationShortName] [nvarchar](50) NULL,
	[CountryId] [uniqueidentifier] NOT NULL,
	[LocationTypeId] [uniqueidentifier] NOT NULL,
	[IcaoCode] [nvarchar](10) NULL,
	[Latitude] [nvarchar](10) NULL,
	[Longitude] [nvarchar](10) NULL,
	[Elevation] [int] NULL,
	[ElevationUnitType] [int] NULL,
	[RunwayDirection] [nvarchar](50) NULL,
	[RunwayLength] [int] NULL,
	[RunwayLengthUnitType] [int] NULL,
	[AirportFrequency] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[SortIndicator] [int] NULL,
	[IsFastEntryRecord] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[LocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_Locations_LocationName] UNIQUE NONCLUSTERED 
(
	[LocationName] ASC,
	[CountryId] ASC,
	[LocationTypeId] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LocationTypes]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationTypes](
	[LocationTypeId] [uniqueidentifier] NOT NULL,
	[LocationTypeName] [nvarchar](50) NOT NULL,
	[LocationTypeCupId] [int] NULL,
	[IsAirfield] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_LocationTypes] PRIMARY KEY CLUSTERED 
(
	[LocationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MemberStates]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberStates](
	[MemberStateId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[MemberStateName] [nvarchar](50) NOT NULL,
	[Remarks] [nvarchar](250) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_MemberStates] PRIMARY KEY CLUSTERED 
(
	[MemberStateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_MemberStates_MemberStateName] UNIQUE NONCLUSTERED 
(
	[MemberStateName] ASC,
	[ClubId] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[PermissionId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[IsSuperUser] [bit] NOT NULL,
	[CanCreateAircrafts] [bit] NOT NULL,
	[CanReadOwnedAircrafts] [bit] NOT NULL,
	[CanReadClubOwnedAircrafts] [bit] NOT NULL,
	[CanReadOtherAircrafts] [bit] NOT NULL,
	[CanUpdateOwnedAircrafts] [bit] NOT NULL,
	[CanUpdateClubOwnedAircrafts] [bit] NOT NULL,
	[CanUpdateOtherAircrafts] [bit] NOT NULL,
	[CanDeleteOwnedAircrafts] [bit] NOT NULL,
	[CanDeleteClubOwnedAircrafts] [bit] NOT NULL,
	[CanDeleteOtherAircrafts] [bit] NOT NULL,
	[CanImportAircrafts] [bit] NOT NULL,
	[CanExportOwnedAircrafts] [bit] NOT NULL,
	[CanExportClubOwnedAircrafts] [bit] NOT NULL,
	[CanExportOtherAircrafts] [bit] NOT NULL,
	[CanUpdateStateOfOwnedAircrafts] [bit] NOT NULL,
	[CanUpdateStateOfClubOwnedAircrafts] [bit] NOT NULL,
	[CanUpdateStateOfOtherAircrafts] [bit] NOT NULL,
	[CanCreateClubs] [bit] NOT NULL,
	[CanReadOwnedClubs] [bit] NOT NULL,
	[CanReadOwnClub] [bit] NOT NULL,
	[CanReadOtherClubs] [bit] NOT NULL,
	[CanUpdateOwnedClubs] [bit] NOT NULL,
	[CanUpdateOwnClub] [bit] NOT NULL,
	[CanUpdateOtherClubs] [bit] NOT NULL,
	[CanDeleteOwnedClubs] [bit] NOT NULL,
	[CanDeleteOwnClub] [bit] NOT NULL,
	[CanDeleteOtherClubs] [bit] NOT NULL,
	[CanImportClubs] [bit] NOT NULL,
	[CanExportOwnedClubs] [bit] NOT NULL,
	[CanExportOwnClub] [bit] NOT NULL,
	[CanExportOtherClubs] [bit] NOT NULL,
	[CanCreateLocations] [bit] NOT NULL,
	[CanReadOwnedLocations] [bit] NOT NULL,
	[CanReadClubOwnedLocations] [bit] NOT NULL,
	[CanReadOtherLocations] [bit] NOT NULL,
	[CanUpdateOwnedLocations] [bit] NOT NULL,
	[CanUpdateClubOwnedLocations] [bit] NOT NULL,
	[CanUpdateOtherLocations] [bit] NOT NULL,
	[CanDeleteOwnedLocations] [bit] NOT NULL,
	[CanDeleteClubOwnedLocations] [bit] NOT NULL,
	[CanDeleteOtherLocations] [bit] NOT NULL,
	[CanImportLocations] [bit] NOT NULL,
	[CanExportOwnedLocations] [bit] NOT NULL,
	[CanExportClubOwnedLocations] [bit] NOT NULL,
	[CanExportOtherLocations] [bit] NOT NULL,
	[CanCreateCountries] [bit] NOT NULL,
	[CanReadOwnedCountries] [bit] NOT NULL,
	[CanReadClubOwnedCountries] [bit] NOT NULL,
	[CanReadOtherCountries] [bit] NOT NULL,
	[CanUpdateOwnedCountries] [bit] NOT NULL,
	[CanUpdateClubOwnedCountries] [bit] NOT NULL,
	[CanUpdateOtherCountries] [bit] NOT NULL,
	[CanDeleteOwnedCountries] [bit] NOT NULL,
	[CanDeleteClubOwnedCountries] [bit] NOT NULL,
	[CanDeleteOtherCountries] [bit] NOT NULL,
	[CanImportCountries] [bit] NOT NULL,
	[CanExportOwnedCountries] [bit] NOT NULL,
	[CanExportClubOwnedCountries] [bit] NOT NULL,
	[CanExportOtherCountries] [bit] NOT NULL,
	[CanCreateFlightTypes] [bit] NOT NULL,
	[CanReadOwnedFlightTypes] [bit] NOT NULL,
	[CanReadClubOwnedFlightTypes] [bit] NOT NULL,
	[CanReadOtherFlightTypes] [bit] NOT NULL,
	[CanUpdateOwnedFlightTypes] [bit] NOT NULL,
	[CanUpdateClubOwnedFlightTypes] [bit] NOT NULL,
	[CanUpdateOtherFlightTypes] [bit] NOT NULL,
	[CanDeleteOwnedFlightTypes] [bit] NOT NULL,
	[CanDeleteClubOwnedFlightTypes] [bit] NOT NULL,
	[CanDeleteOtherFlightTypes] [bit] NOT NULL,
	[CanImportFlightTypes] [bit] NOT NULL,
	[CanExportOwnedFlightTypes] [bit] NOT NULL,
	[CanExportClubOwnedFlightTypes] [bit] NOT NULL,
	[CanExportOtherFlightTypes] [bit] NOT NULL,
	[CanCreateMemberStates] [bit] NOT NULL,
	[CanReadOwnedMemberStates] [bit] NOT NULL,
	[CanReadClubOwnedMemberStates] [bit] NOT NULL,
	[CanReadOtherMemberStates] [bit] NOT NULL,
	[CanUpdateOwnedMemberStates] [bit] NOT NULL,
	[CanUpdateClubOwnedMemberStates] [bit] NOT NULL,
	[CanUpdateOtherMemberStates] [bit] NOT NULL,
	[CanDeleteOwnedMemberStates] [bit] NOT NULL,
	[CanDeleteClubOwnedMemberStates] [bit] NOT NULL,
	[CanDeleteOtherMemberStates] [bit] NOT NULL,
	[CanImportMemberStates] [bit] NOT NULL,
	[CanExportOwnedMemberStates] [bit] NOT NULL,
	[CanExportClubOwnedMemberStates] [bit] NOT NULL,
	[CanExportOtherMemberStates] [bit] NOT NULL,
	[CanCreatePersonCategories] [bit] NOT NULL,
	[CanReadOwnedPersonCategories] [bit] NOT NULL,
	[CanReadClubOwnedPersonCategories] [bit] NOT NULL,
	[CanReadOtherPersonCategories] [bit] NOT NULL,
	[CanUpdateOwnedPersonCategories] [bit] NOT NULL,
	[CanUpdateClubOwnedPersonCategories] [bit] NOT NULL,
	[CanUpdateOtherPersonCategories] [bit] NOT NULL,
	[CanDeleteOwnedPersonCategories] [bit] NOT NULL,
	[CanDeleteClubOwnedPersonCategories] [bit] NOT NULL,
	[CanDeleteOtherPersonCategories] [bit] NOT NULL,
	[CanImportPersonCategories] [bit] NOT NULL,
	[CanExportOwnedPersonCategories] [bit] NOT NULL,
	[CanExportClubOwnedPersonCategories] [bit] NOT NULL,
	[CanExportOtherPersonCategories] [bit] NOT NULL,
	[CanCreateFlights] [bit] NOT NULL,
	[CanReadOwnedFlights] [bit] NOT NULL,
	[CanReadClubOwnedFlights] [bit] NOT NULL,
	[CanReadOtherFlights] [bit] NOT NULL,
	[CanUpdateOwnedFlights] [bit] NOT NULL,
	[CanUpdateClubOwnedFlights] [bit] NOT NULL,
	[CanUpdateOtherFlights] [bit] NOT NULL,
	[CanDeleteOwnedFlights] [bit] NOT NULL,
	[CanDeleteClubOwnedFlights] [bit] NOT NULL,
	[CanDeleteOtherFlights] [bit] NOT NULL,
	[CanImportFlights] [bit] NOT NULL,
	[CanExportOwnedFlights] [bit] NOT NULL,
	[CanExportClubOwnedFlights] [bit] NOT NULL,
	[CanExportOtherFlights] [bit] NOT NULL,
	[CanSetLandingDataOfOtherFlights] [bit] NOT NULL,
	[CanUnlockOwnedFlights] [bit] NOT NULL,
	[CanUnlockClubOwnedFlights] [bit] NOT NULL,
	[CanUnlockOtherFlights] [bit] NOT NULL,
	[CanCreateSystemFlights] [bit] NOT NULL,
	[CanReadOwnedSystemFlights] [bit] NOT NULL,
	[CanReadClubOwnedSystemFlights] [bit] NOT NULL,
	[CanReadOtherSystemFlights] [bit] NOT NULL,
	[CanUpdateOwnedSystemFlights] [bit] NOT NULL,
	[CanUpdateClubOwnedSystemFlights] [bit] NOT NULL,
	[CanUpdateOtherSystemFlights] [bit] NOT NULL,
	[CanDeleteOwnedSystemFlights] [bit] NOT NULL,
	[CanDeleteClubOwnedSystemFlights] [bit] NOT NULL,
	[CanDeleteOtherSystemFlights] [bit] NOT NULL,
	[CanImportSystemFlights] [bit] NOT NULL,
	[CanExportOwnedSystemFlights] [bit] NOT NULL,
	[CanExportClubOwnedSystemFlights] [bit] NOT NULL,
	[CanExportOtherSystemFlights] [bit] NOT NULL,
	[CanCreatePersons] [bit] NOT NULL,
	[CanReadOwnedPersons] [bit] NOT NULL,
	[CanReadClubOwnedPersons] [bit] NOT NULL,
	[CanReadOtherPersons] [bit] NOT NULL,
	[CanUpdateOwnedPersons] [bit] NOT NULL,
	[CanUpdateClubOwnedPersons] [bit] NOT NULL,
	[CanUpdateOtherPersons] [bit] NOT NULL,
	[CanDeleteOwnedPersons] [bit] NOT NULL,
	[CanDeleteClubOwnedPersons] [bit] NOT NULL,
	[CanDeleteOtherPersons] [bit] NOT NULL,
	[CanImportPersons] [bit] NOT NULL,
	[CanExportOwnedPersons] [bit] NOT NULL,
	[CanExportClubOwnedPersons] [bit] NOT NULL,
	[CanExportOtherPersons] [bit] NOT NULL,
	[CanReadOwnedPersonsSensitiveData] [bit] NOT NULL,
	[CanReadClubOwnedPersonsSensitiveData] [bit] NOT NULL,
	[CanReadOtherPersonsSensitiveData] [bit] NOT NULL,
	[CanUpdateOwnedPersonsSensitiveData] [bit] NOT NULL,
	[CanUpdateClubOwnedPersonsSensitiveData] [bit] NOT NULL,
	[CanUpdateOtherPersonsSensitiveData] [bit] NOT NULL,
	[CanExportOwnedPersonsSensitiveData] [bit] NOT NULL,
	[CanExportClubOwnedPersonsSensitiveData] [bit] NOT NULL,
	[CanExportOtherPersonsSensitiveData] [bit] NOT NULL,
	[CanCreateUsers] [bit] NOT NULL,
	[CanReadOwnedUsers] [bit] NOT NULL,
	[CanReadClubOwnedUsers] [bit] NOT NULL,
	[CanReadOtherUsers] [bit] NOT NULL,
	[CanUpdateOwnedUsers] [bit] NOT NULL,
	[CanUpdateClubOwnedUsers] [bit] NOT NULL,
	[CanUpdateOtherUsers] [bit] NOT NULL,
	[CanDeleteOwnedUsers] [bit] NOT NULL,
	[CanDeleteClubOwnedUsers] [bit] NOT NULL,
	[CanDeleteOtherUsers] [bit] NOT NULL,
	[CanImportUsers] [bit] NOT NULL,
	[CanExportOwnedUsers] [bit] NOT NULL,
	[CanExportClubOwnedUsers] [bit] NOT NULL,
	[CanExportOtherUsers] [bit] NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PersonCategories]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonCategories](
	[PersonCategoryId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[Remarks] [nvarchar](250) NULL,
	[ParentPersonCategoryId] [uniqueidentifier] NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_PersonCategories] PRIMARY KEY CLUSTERED 
(
	[PersonCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_PersonCategories_ClubId_CategoryName_ParentId] UNIQUE NONCLUSTERED 
(
	[ClubId] ASC,
	[CategoryName] ASC,
	[ParentPersonCategoryId] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PersonClub]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonClub](
	[PersonId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[MemberNumber] [nvarchar](20) NULL,
	[MemberKey] [nvarchar](40) NULL,
	[IsMotorPilot] [bit] NOT NULL,
	[IsTowPilot] [bit] NOT NULL,
	[IsGliderInstructor] [bit] NOT NULL,
	[IsGliderPilot] [bit] NOT NULL,
	[IsGliderTrainee] [bit] NOT NULL,
	[IsPassenger] [bit] NOT NULL,
	[IsWinchOperator] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_PersonClub] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC,
	[ClubId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_PersonClub_PersonId_ClubId_MemberKey] UNIQUE NONCLUSTERED 
(
	[PersonId] ASC,
	[ClubId] ASC,
	[MemberKey] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_PersonClub_PersonId_ClubId_MemberNumber] UNIQUE NONCLUSTERED 
(
	[PersonId] ASC,
	[ClubId] ASC,
	[MemberNumber] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PersonMemberStates]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonMemberStates](
	[PersonId] [uniqueidentifier] NOT NULL,
	[MemberStateId] [uniqueidentifier] NOT NULL,
	[ValidFrom] [datetime2](7) NOT NULL,
	[ValidTo] [datetime2](7) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
 CONSTRAINT [PK_PersonMemberStates] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC,
	[MemberStateId] ASC,
	[ValidFrom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PersonPersonCategories]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonPersonCategories](
	[PersonId] [uniqueidentifier] NOT NULL,
	[PersonCategoryId] [uniqueidentifier] NOT NULL,
	[ValidFrom] [datetime2](7) NOT NULL,
	[ValidTo] [datetime2](7) NULL,
	[SortIndicator] [int] NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
 CONSTRAINT [PK_PersonPersonCategories] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC,
	[PersonCategoryId] ASC,
	[ValidFrom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Persons]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Persons](
	[PersonId] [uniqueidentifier] NOT NULL,
	[Lastname] [nvarchar](100) NOT NULL,
	[Firstname] [nvarchar](100) NOT NULL,
	[Midname] [nvarchar](100) NULL,
	[CompanyName] [nvarchar](100) NULL,
	[AddressLine1] [nvarchar](200) NULL,
	[AddressLine2] [nvarchar](200) NULL,
	[Zip] [nvarchar](10) NULL,
	[City] [nvarchar](100) NULL,
	[Region] [nvarchar](100) NULL,
	[CountryId] [uniqueidentifier] NULL,
	[PrivatePhone] [nvarchar](20) NULL,
	[MobilePhone] [nvarchar](20) NULL,
	[BusinessPhone] [nvarchar](20) NULL,
	[FaxNumber] [nvarchar](20) NULL,
	[EmailPrivate] [nvarchar](100) NULL,
	[EmailBusiness] [nvarchar](100) NULL,
	[PreferMailToBusinessMail] [bit] NOT NULL,
	[Birthday] [date] NULL,
	[HasMotorPilotLicence] [bit] NOT NULL,
	[HasTowPilotLicence] [bit] NOT NULL,
	[HasGliderInstructorLicence] [bit] NOT NULL,
	[HasGliderPilotLicence] [bit] NOT NULL,
	[HasGliderTraineeLicence] [bit] NOT NULL,
	[HasGliderPAXLicence] [bit] NOT NULL,
	[HasTMGLicence] [bit] NOT NULL,
	[HasWinchOperatorLicence] [bit] NOT NULL,
	[LicenseNumber] [nvarchar](20) NULL,
	[LicenseTypeId] [uniqueidentifier] NULL,
	[LicenseTrainingStateGlider] [int] NULL,
	[LicenseTrainingStateGliderPAX] [int] NULL,
	[LicenseTrainingStateGliderInstructor] [int] NULL,
	[LicenseTrainingStateTowing] [int] NULL,
	[LicenseTrainingStateTMG] [int] NULL,
	[LicenseTrainingStateMotor] [int] NULL,
	[SpotLink] [nvarchar](250) NULL,
	[IsFastEntryRecord] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Persones] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](100) NOT NULL,
	[RoleApplicationKeyString] [nvarchar](100) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StartTypes]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StartTypes](
	[StartTypeId] [int] IDENTITY(1,1) NOT NULL,
	[StartTypeName] [nvarchar](100) NOT NULL,
	[IsForGliderFlights] [bit] NOT NULL,
	[IsForTowFlights] [bit] NOT NULL,
	[IsForMotorFlights] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_StartTypes] PRIMARY KEY CLUSTERED 
(
	[StartTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemData]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemData](
	[SystemId] [uniqueidentifier] NOT NULL,
	[ReportSenderEmailAddress] [nvarchar](100) NOT NULL,
	[SystemSenderEmailAddress] [nvarchar](100) NOT NULL,
	[SmtpUsername] [nvarchar](100) NOT NULL,
	[SmtpPasswort] [nvarchar](100) NOT NULL,
	[SmtpServer] [nvarchar](100) NOT NULL,
	[SmtpPort] [int] NOT NULL,
	[WorkflowStartsOn] [datetime2](7) NOT NULL,
	[MaxUserLoginAttempts] [int] NOT NULL,
	[UrlToFlightReport] [nvarchar](255) NULL,
	[UrlToBAZLReport] [nvarchar](255) NULL,
	[UrlToInvoiceReport] [nvarchar](255) NULL,
	[SystemPathToReports] [nvarchar](255) NULL,
 CONSTRAINT [PK_System] PRIMARY KEY CLUSTERED 
(
	[SystemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemLogs]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemLogs](
	[LogId] [bigint] IDENTITY(1,1) NOT NULL,
	[EventDateTime] [datetime2](7) NOT NULL,
	[Application] [nvarchar](200) NULL,
	[LogLevel] [nvarchar](100) NOT NULL,
	[EventType] [bigint] NULL,
	[Logger] [nvarchar](max) NULL,
	[Message] [nvarchar](max) NULL,
	[UserName] [nvarchar](100) NULL,
	[ComputerName] [nvarchar](50) NULL,
	[CallSite] [nvarchar](max) NULL,
	[Thread] [nvarchar](100) NULL,
	[Exception] [varchar](max) NULL,
	[Stacktrace] [varchar](max) NULL,
	[SqlString] [varchar](max) NULL,
	[Version] [nvarchar](50) NULL,
 CONSTRAINT [PK_SystemLogs] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SystemVersion]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemVersion](
	[VersionId] [bigint] NOT NULL,
	[MajorVersion] [bigint] NOT NULL,
	[MinorVersion] [bigint] NOT NULL,
	[BuildVersion] [bigint] NOT NULL,
	[RevisionVersion] [bigint] NOT NULL,
	[UpgradeFromVersion] [nvarchar](50) NULL,
	[UpgradeDateTime] [datetime2](7) NULL,
 CONSTRAINT [PK_SystemVersion] PRIMARY KEY CLUSTERED 
(
	[VersionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserAccountStates]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccountStates](
	[UserAccountStateId] [int] IDENTITY(1,1) NOT NULL,
	[UserAccountStateName] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](200) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_UserAccountStates] PRIMARY KEY CLUSTERED 
(
	[UserAccountStateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[FriendlyName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[PersonId] [uniqueidentifier] NULL,
	[NotificationEmail] [nvarchar](100) NOT NULL,
	[Remarks] [nvarchar](250) NULL,
	[FailedLoginCounts] [int] NOT NULL,
	[AccountState] [int] NOT NULL,
	[LastPasswordChangeOn] [datetime2](7) NULL,
	[ForcePasswordChangeNextLogon] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_Users_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__Aircr__33D4B598]  DEFAULT ((0)) FOR [AircraftType]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__IsTow__34C8D9D1]  DEFAULT ((0)) FOR [IsTowingOrWinchRequired]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__IsTow__35BCFE0A]  DEFAULT ((0)) FOR [IsTowingstartAllowed]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__IsWin__36B12243]  DEFAULT ((0)) FOR [IsWinchstartAllowed]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__IsTow__37A5467C]  DEFAULT ((0)) FOR [IsTowingAircraft]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF_Aircrafts_FlightDurationPrecision]  DEFAULT ((1)) FOR [FlightDurationPrecision]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__IsFas__38996AB5]  DEFAULT ((1)) FOR [IsFastEntryRecord]
GO
ALTER TABLE [dbo].[Aircrafts] ADD  CONSTRAINT [DF__Aircrafts__IsDel__398D8EEE]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ClubExtensions] ADD  CONSTRAINT [DF_ClubExtensions_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Clubs] ADD  CONSTRAINT [DF_Clubs_IsInOutboundRouteRequired]  DEFAULT ((0)) FOR [IsInboundRouteRequired]
GO
ALTER TABLE [dbo].[Clubs] ADD  CONSTRAINT [DF_Clubs_IsInboundRouteRequired1]  DEFAULT ((0)) FOR [IsOutboundRouteRequired]
GO
ALTER TABLE [dbo].[Clubs] ADD  CONSTRAINT [DF__Clubs__IsDeleted__108B795B]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Countries] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ExtensionParameters] ADD  CONSTRAINT [DF_ExtensionParameters_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ExtensionParameterValues] ADD  CONSTRAINT [DF_ExtensionParameterValues_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Extensions] ADD  CONSTRAINT [DF_Extensions_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[FlightCostBalanceTypes] ADD  CONSTRAINT [DF__FlightCos__Perso__18B6AB08]  DEFAULT ((0)) FOR [PersonForInvoiceRequired]
GO
ALTER TABLE [dbo].[FlightCostBalanceTypes] ADD  CONSTRAINT [DF__FlightCos__IsFor__19AACF41]  DEFAULT ((1)) FOR [IsForGliderFlights]
GO
ALTER TABLE [dbo].[FlightCostBalanceTypes] ADD  CONSTRAINT [DF__FlightCos__IsFor__1A9EF37A]  DEFAULT ((0)) FOR [IsForTowFlights]
GO
ALTER TABLE [dbo].[FlightCostBalanceTypes] ADD  CONSTRAINT [DF__FlightCos__IsFor__1B9317B3]  DEFAULT ((0)) FOR [IsForMotorFlights]
GO
ALTER TABLE [dbo].[FlightCrew] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Flights] ADD  CONSTRAINT [DF__Flights__IsSoloF__5CD6CB2B]  DEFAULT ((0)) FOR [IsSoloFlight]
GO
ALTER TABLE [dbo].[Flights] ADD  CONSTRAINT [DF__Flights__FlightC__5DCAEF64]  DEFAULT ((1)) FOR [FlightCostBalanceType]
GO
ALTER TABLE [dbo].[Flights] ADD  CONSTRAINT [DF__Flights__IsDelet__5EBF139D]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[FlightTypes] ADD  DEFAULT ((0)) FOR [IsSoloFlight]
GO
ALTER TABLE [dbo].[FlightTypes] ADD  DEFAULT ((0)) FOR [IsSummarizedSystemFlight]
GO
ALTER TABLE [dbo].[FlightTypes] ADD  DEFAULT ((1)) FOR [IsForGliderFlights]
GO
ALTER TABLE [dbo].[FlightTypes] ADD  DEFAULT ((0)) FOR [IsForTowFlights]
GO
ALTER TABLE [dbo].[FlightTypes] ADD  DEFAULT ((0)) FOR [IsForMotorFlights]
GO
ALTER TABLE [dbo].[FlightTypes] ADD  DEFAULT ((1)) FOR [IsFlightCostBalanceSelectable]
GO
ALTER TABLE [dbo].[FlightTypes] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[InOutboundPoints] ADD  CONSTRAINT [DF_InOutboundPoints_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[LicenseTypes] ADD  CONSTRAINT [DF_LicenseTypes_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Locations] ADD  DEFAULT ((1)) FOR [ElevationUnitType]
GO
ALTER TABLE [dbo].[Locations] ADD  DEFAULT ((1)) FOR [RunwayLengthUnitType]
GO
ALTER TABLE [dbo].[Locations] ADD  DEFAULT ((1)) FOR [IsFastEntryRecord]
GO
ALTER TABLE [dbo].[Locations] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[LocationTypes] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[MemberStates] ADD  CONSTRAINT [DF_MemberStates_MemberStateId]  DEFAULT (newid()) FOR [MemberStateId]
GO
ALTER TABLE [dbo].[MemberStates] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [IsSuperUser]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanCreateAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadClubOwnedAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateClubOwnedAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnedAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteClubOwnedAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOtherAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanImportAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportClubOwnedAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateStateOfOwnedAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateStateOfClubOwnedAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateStateOfOtherAircrafts]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanCreateClubs]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedClubs]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnClub]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherClubs]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedClubs]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnClub]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherClubs]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnedClubs]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnClub]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOtherClubs]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanImportClubs]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedClubs]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnClub]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherClubs]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanCreateLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadClubOwnedLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateClubOwnedLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnedLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteClubOwnedLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOtherLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanImportLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportClubOwnedLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherLocations]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanCreateCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadClubOwnedCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateClubOwnedCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnedCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteClubOwnedCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOtherCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanImportCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportClubOwnedCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherCountries]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanCreateFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadClubOwnedFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateClubOwnedFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnedFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteClubOwnedFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOtherFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanImportFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportClubOwnedFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherFlightTypes]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanCreateMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadClubOwnedMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateClubOwnedMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnedMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteClubOwnedMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOtherMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanImportMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportClubOwnedMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherMemberStates]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanCreatePersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadClubOwnedPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateClubOwnedPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnedPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteClubOwnedPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOtherPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanImportPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportClubOwnedPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherPersonCategories]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanCreateFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadClubOwnedFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateClubOwnedFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnedFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteClubOwnedFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOtherFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanImportFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportClubOwnedFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanSetLandingDataOfOtherFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUnlockOwnedFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUnlockClubOwnedFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUnlockOtherFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanCreateSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadClubOwnedSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateClubOwnedSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnedSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteClubOwnedSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOtherSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanImportSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportClubOwnedSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherSystemFlights]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanCreatePersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadClubOwnedPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateClubOwnedPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnedPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteClubOwnedPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOtherPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanImportPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportClubOwnedPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherPersons]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedPersonsSensitiveData]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadClubOwnedPersonsSensitiveData]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherPersonsSensitiveData]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedPersonsSensitiveData]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateClubOwnedPersonsSensitiveData]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherPersonsSensitiveData]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedPersonsSensitiveData]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportClubOwnedPersonsSensitiveData]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherPersonsSensitiveData]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanCreateUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOwnedUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadClubOwnedUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanReadOtherUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOwnedUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateClubOwnedUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanUpdateOtherUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOwnedUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteClubOwnedUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanDeleteOtherUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanImportUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOwnedUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportClubOwnedUsers]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanExportOtherUsers]
GO
ALTER TABLE [dbo].[PersonCategories] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[PersonClub] ADD  DEFAULT ((0)) FOR [IsMotorPilot]
GO
ALTER TABLE [dbo].[PersonClub] ADD  DEFAULT ((0)) FOR [IsTowPilot]
GO
ALTER TABLE [dbo].[PersonClub] ADD  DEFAULT ((0)) FOR [IsGliderInstructor]
GO
ALTER TABLE [dbo].[PersonClub] ADD  DEFAULT ((0)) FOR [IsGliderPilot]
GO
ALTER TABLE [dbo].[PersonClub] ADD  DEFAULT ((0)) FOR [IsGliderTrainee]
GO
ALTER TABLE [dbo].[PersonClub] ADD  DEFAULT ((0)) FOR [IsWinchOperator]
GO
ALTER TABLE [dbo].[PersonClub] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF__Persons__PreferM__4CA06362]  DEFAULT ((0)) FOR [PreferMailToBusinessMail]
GO
ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF__Persons__HasMoto__4D94879B]  DEFAULT ((0)) FOR [HasMotorPilotLicence]
GO
ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF__Persons__HasTowP__4E88ABD4]  DEFAULT ((0)) FOR [HasTowPilotLicence]
GO
ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF__Persons__HasGlid__4F7CD00D]  DEFAULT ((0)) FOR [HasGliderInstructorLicence]
GO
ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF__Persons__HasGlid__5070F446]  DEFAULT ((0)) FOR [HasGliderPilotLicence]
GO
ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF__Persons__HasGlid__5165187F]  DEFAULT ((0)) FOR [HasGliderTraineeLicence]
GO
ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF__Persons__HasGlid__52593CB8]  DEFAULT ((0)) FOR [HasGliderPAXLicence]
GO
ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF__Persons__HasTMGL__534D60F1]  DEFAULT ((0)) FOR [HasTMGLicence]
GO
ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF__Persons__HasWinc__5441852A]  DEFAULT ((0)) FOR [HasWinchOperatorLicence]
GO
ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF__Persons__IsFastE__5535A963]  DEFAULT ((1)) FOR [IsFastEntryRecord]
GO
ALTER TABLE [dbo].[Persons] ADD  CONSTRAINT [DF__Persons__IsDelet__5629CD9C]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Roles] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[UserRoles] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [ForcePasswordChangeNextLogon]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[AircraftAircraftStates]  WITH CHECK ADD  CONSTRAINT [FK_AircraftAircraftStates_Aircraft] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[Aircrafts] ([AircraftId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AircraftAircraftStates] CHECK CONSTRAINT [FK_AircraftAircraftStates_Aircraft]
GO
ALTER TABLE [dbo].[AircraftAircraftStates]  WITH CHECK ADD  CONSTRAINT [FK_AircraftAircraftStates_AircraftStates] FOREIGN KEY([AircraftState])
REFERENCES [dbo].[AircraftStates] ([AircraftStateId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AircraftAircraftStates] CHECK CONSTRAINT [FK_AircraftAircraftStates_AircraftStates]
GO
ALTER TABLE [dbo].[AircraftAircraftStates]  WITH CHECK ADD  CONSTRAINT [FK_AircraftAircraftStates_Persons] FOREIGN KEY([NoticedByPersonId])
REFERENCES [dbo].[Persons] ([PersonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AircraftAircraftStates] CHECK CONSTRAINT [FK_AircraftAircraftStates_Persons]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircrafts_AircraftTypes] FOREIGN KEY([AircraftType])
REFERENCES [dbo].[AircraftTypes] ([AircraftTypeId])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircrafts_AircraftTypes]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircrafts_OwnerClub] FOREIGN KEY([AircraftOwnerClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircrafts_OwnerClub]
GO
ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircrafts_OwnerPerson] FOREIGN KEY([AircraftOwnerPersonId])
REFERENCES [dbo].[Persons] ([PersonId])
GO
ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircrafts_OwnerPerson]
GO
ALTER TABLE [dbo].[ClubExtensions]  WITH CHECK ADD  CONSTRAINT [FK_ClubExtensions_Clubs] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[ClubExtensions] CHECK CONSTRAINT [FK_ClubExtensions_Clubs]
GO
ALTER TABLE [dbo].[ClubExtensions]  WITH CHECK ADD  CONSTRAINT [FK_ClubExtensions_Extensions] FOREIGN KEY([ExtensionId])
REFERENCES [dbo].[Extensions] ([ExtensionId])
GO
ALTER TABLE [dbo].[ClubExtensions] CHECK CONSTRAINT [FK_ClubExtensions_Extensions]
GO
ALTER TABLE [dbo].[Clubs]  WITH CHECK ADD  CONSTRAINT [FK_Club_Homebase] FOREIGN KEY([HomebaseId])
REFERENCES [dbo].[Locations] ([LocationId])
GO
ALTER TABLE [dbo].[Clubs] CHECK CONSTRAINT [FK_Club_Homebase]
GO
ALTER TABLE [dbo].[Clubs]  WITH CHECK ADD  CONSTRAINT [FK_Clubs_Country] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([CountryId])
GO
ALTER TABLE [dbo].[Clubs] CHECK CONSTRAINT [FK_Clubs_Country]
GO
ALTER TABLE [dbo].[Clubs]  WITH CHECK ADD  CONSTRAINT [FK_Clubs_DefaultGliderFlightTypeId] FOREIGN KEY([DefaultGliderFlightTypeId])
REFERENCES [dbo].[FlightTypes] ([FlightTypeId])
GO
ALTER TABLE [dbo].[Clubs] CHECK CONSTRAINT [FK_Clubs_DefaultGliderFlightTypeId]
GO
ALTER TABLE [dbo].[Clubs]  WITH CHECK ADD  CONSTRAINT [FK_Clubs_DefaultMotorFlightTypeId] FOREIGN KEY([DefaultMotorFlightTypeId])
REFERENCES [dbo].[FlightTypes] ([FlightTypeId])
GO
ALTER TABLE [dbo].[Clubs] CHECK CONSTRAINT [FK_Clubs_DefaultMotorFlightTypeId]
GO
ALTER TABLE [dbo].[Clubs]  WITH CHECK ADD  CONSTRAINT [FK_Clubs_DefaultStartType] FOREIGN KEY([DefaultStartType])
REFERENCES [dbo].[StartTypes] ([StartTypeId])
GO
ALTER TABLE [dbo].[Clubs] CHECK CONSTRAINT [FK_Clubs_DefaultStartType]
GO
ALTER TABLE [dbo].[Clubs]  WITH CHECK ADD  CONSTRAINT [FK_Clubs_DefaultTowFlightTypeId] FOREIGN KEY([DefaultTowFlightTypeId])
REFERENCES [dbo].[FlightTypes] ([FlightTypeId])
GO
ALTER TABLE [dbo].[Clubs] CHECK CONSTRAINT [FK_Clubs_DefaultTowFlightTypeId]
GO
ALTER TABLE [dbo].[ExtensionParameters]  WITH CHECK ADD  CONSTRAINT [FK_ExtensionParameters_ExtensionParameterTypes] FOREIGN KEY([ExtensionParameterType])
REFERENCES [dbo].[ExtensionParameterTypes] ([ExtensionParameterTypeId])
GO
ALTER TABLE [dbo].[ExtensionParameters] CHECK CONSTRAINT [FK_ExtensionParameters_ExtensionParameterTypes]
GO
ALTER TABLE [dbo].[ExtensionParameters]  WITH CHECK ADD  CONSTRAINT [FK_ExtensionParameters_Extensions] FOREIGN KEY([ExtensionId])
REFERENCES [dbo].[Extensions] ([ExtensionId])
GO
ALTER TABLE [dbo].[ExtensionParameters] CHECK CONSTRAINT [FK_ExtensionParameters_Extensions]
GO
ALTER TABLE [dbo].[ExtensionParameterValues]  WITH CHECK ADD  CONSTRAINT [FK_ExtensionParameterValues_Clubs] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[ExtensionParameterValues] CHECK CONSTRAINT [FK_ExtensionParameterValues_Clubs]
GO
ALTER TABLE [dbo].[ExtensionParameterValues]  WITH CHECK ADD  CONSTRAINT [FK_ExtensionParameterValues_ExtensionParameters] FOREIGN KEY([ExtensionParameterId])
REFERENCES [dbo].[ExtensionParameters] ([ExtensionParameterId])
GO
ALTER TABLE [dbo].[ExtensionParameterValues] CHECK CONSTRAINT [FK_ExtensionParameterValues_ExtensionParameters]
GO
ALTER TABLE [dbo].[Extensions]  WITH CHECK ADD  CONSTRAINT [FK_Extensions_ExtensionTypes] FOREIGN KEY([ExtensionTypeId])
REFERENCES [dbo].[ExtensionTypes] ([ExtensionTypeId])
GO
ALTER TABLE [dbo].[Extensions] CHECK CONSTRAINT [FK_Extensions_ExtensionTypes]
GO
ALTER TABLE [dbo].[FlightCrew]  WITH CHECK ADD  CONSTRAINT [FK_FlightCrew_FlightCrewTypes] FOREIGN KEY([FlightCrewType])
REFERENCES [dbo].[FlightCrewTypes] ([FlightCrewTypeId])
GO
ALTER TABLE [dbo].[FlightCrew] CHECK CONSTRAINT [FK_FlightCrew_FlightCrewTypes]
GO
ALTER TABLE [dbo].[FlightCrew]  WITH CHECK ADD  CONSTRAINT [FK_FlightCrew_Flights] FOREIGN KEY([FlightId])
REFERENCES [dbo].[Flights] ([FlightId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FlightCrew] CHECK CONSTRAINT [FK_FlightCrew_Flights]
GO
ALTER TABLE [dbo].[FlightCrew]  WITH CHECK ADD  CONSTRAINT [FK_FlightCrew_Persons] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([PersonId])
GO
ALTER TABLE [dbo].[FlightCrew] CHECK CONSTRAINT [FK_FlightCrew_Persons]
GO
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_Aircraft] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[Aircrafts] ([AircraftId])
GO
ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_Aircraft]
GO
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_FlightCostBalanceTypes] FOREIGN KEY([FlightCostBalanceType])
REFERENCES [dbo].[FlightCostBalanceTypes] ([FlightCostBalanceTypeId])
GO
ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_FlightCostBalanceTypes]
GO
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_Flights] FOREIGN KEY([TowFlightId])
REFERENCES [dbo].[Flights] ([FlightId])
GO
ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_Flights]
GO
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_FlightStates] FOREIGN KEY([FlightState])
REFERENCES [dbo].[FlightStates] ([FlightStateId])
GO
ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_FlightStates]
GO
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_FlightTypes] FOREIGN KEY([FlightTypeId])
REFERENCES [dbo].[FlightTypes] ([FlightTypeId])
GO
ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_FlightTypes]
GO
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_Ldg_Locations] FOREIGN KEY([LdgLocationId])
REFERENCES [dbo].[Locations] ([LocationId])
GO
ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_Ldg_Locations]
GO
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_Start_Locations] FOREIGN KEY([StartLocationId])
REFERENCES [dbo].[Locations] ([LocationId])
GO
ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_Start_Locations]
GO
ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_StartTypes] FOREIGN KEY([StartType])
REFERENCES [dbo].[StartTypes] ([StartTypeId])
GO
ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_StartTypes]
GO
ALTER TABLE [dbo].[FlightTypes]  WITH CHECK ADD  CONSTRAINT [FK_FlightTypes_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[FlightTypes] CHECK CONSTRAINT [FK_FlightTypes_Club]
GO
ALTER TABLE [dbo].[InOutboundPoints]  WITH CHECK ADD  CONSTRAINT [FK_InOutboundPoints_Locations] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([LocationId])
GO
ALTER TABLE [dbo].[InOutboundPoints] CHECK CONSTRAINT [FK_InOutboundPoints_Locations]
GO
ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([CountryId])
GO
ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_Countries]
GO
ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_ElevationUnitTypes] FOREIGN KEY([ElevationUnitType])
REFERENCES [dbo].[ElevationUnitTypes] ([ElevationUnitTypeId])
GO
ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_ElevationUnitTypes]
GO
ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_LengthUnitTypes] FOREIGN KEY([RunwayLengthUnitType])
REFERENCES [dbo].[LengthUnitTypes] ([LengthUnitTypeId])
GO
ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_LengthUnitTypes]
GO
ALTER TABLE [dbo].[Locations]  WITH CHECK ADD  CONSTRAINT [FK_Locations_LocationTypes] FOREIGN KEY([LocationTypeId])
REFERENCES [dbo].[LocationTypes] ([LocationTypeId])
GO
ALTER TABLE [dbo].[Locations] CHECK CONSTRAINT [FK_Locations_LocationTypes]
GO
ALTER TABLE [dbo].[MemberStates]  WITH CHECK ADD  CONSTRAINT [FK_MemberStates_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[MemberStates] CHECK CONSTRAINT [FK_MemberStates_Club]
GO
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_Permissions_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_Permissions_Roles]
GO
ALTER TABLE [dbo].[PersonCategories]  WITH CHECK ADD  CONSTRAINT [FK_PersonCategories_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[PersonCategories] CHECK CONSTRAINT [FK_PersonCategories_Club]
GO
ALTER TABLE [dbo].[PersonCategories]  WITH CHECK ADD  CONSTRAINT [FK_PersonCategories_PersonCategories] FOREIGN KEY([ParentPersonCategoryId])
REFERENCES [dbo].[PersonCategories] ([PersonCategoryId])
GO
ALTER TABLE [dbo].[PersonCategories] CHECK CONSTRAINT [FK_PersonCategories_PersonCategories]
GO
ALTER TABLE [dbo].[PersonClub]  WITH CHECK ADD  CONSTRAINT [FK_PersonClub_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[PersonClub] CHECK CONSTRAINT [FK_PersonClub_Club]
GO
ALTER TABLE [dbo].[PersonClub]  WITH CHECK ADD  CONSTRAINT [FK_PersonClub_Person] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([PersonId])
GO
ALTER TABLE [dbo].[PersonClub] CHECK CONSTRAINT [FK_PersonClub_Person]
GO
ALTER TABLE [dbo].[PersonMemberStates]  WITH CHECK ADD  CONSTRAINT [FK_PersonMemberStates_MemberStates] FOREIGN KEY([MemberStateId])
REFERENCES [dbo].[MemberStates] ([MemberStateId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonMemberStates] CHECK CONSTRAINT [FK_PersonMemberStates_MemberStates]
GO
ALTER TABLE [dbo].[PersonMemberStates]  WITH CHECK ADD  CONSTRAINT [FK_PersonMemberStates_Persons] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([PersonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonMemberStates] CHECK CONSTRAINT [FK_PersonMemberStates_Persons]
GO
ALTER TABLE [dbo].[PersonPersonCategories]  WITH CHECK ADD  CONSTRAINT [FK_PersonPersonCategories_PersonCategories] FOREIGN KEY([PersonCategoryId])
REFERENCES [dbo].[PersonCategories] ([PersonCategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonPersonCategories] CHECK CONSTRAINT [FK_PersonPersonCategories_PersonCategories]
GO
ALTER TABLE [dbo].[PersonPersonCategories]  WITH CHECK ADD  CONSTRAINT [FK_PersonPersonCategories_Persons] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([PersonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonPersonCategories] CHECK CONSTRAINT [FK_PersonPersonCategories_Persons]
GO
ALTER TABLE [dbo].[Persons]  WITH CHECK ADD  CONSTRAINT [FK_Persons_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([CountryId])
GO
ALTER TABLE [dbo].[Persons] CHECK CONSTRAINT [FK_Persons_Countries]
GO
ALTER TABLE [dbo].[Persons]  WITH CHECK ADD  CONSTRAINT [FK_Persons_LicenseTrainingStatesGlider] FOREIGN KEY([LicenseTrainingStateGlider])
REFERENCES [dbo].[LicenseTrainingStates] ([LicenseTrainingStateId])
GO
ALTER TABLE [dbo].[Persons] CHECK CONSTRAINT [FK_Persons_LicenseTrainingStatesGlider]
GO
ALTER TABLE [dbo].[Persons]  WITH CHECK ADD  CONSTRAINT [FK_Persons_LicenseTrainingStatesGliderInstructor] FOREIGN KEY([LicenseTrainingStateGliderInstructor])
REFERENCES [dbo].[LicenseTrainingStates] ([LicenseTrainingStateId])
GO
ALTER TABLE [dbo].[Persons] CHECK CONSTRAINT [FK_Persons_LicenseTrainingStatesGliderInstructor]
GO
ALTER TABLE [dbo].[Persons]  WITH CHECK ADD  CONSTRAINT [FK_Persons_LicenseTrainingStatesGliderPAX] FOREIGN KEY([LicenseTrainingStateGliderPAX])
REFERENCES [dbo].[LicenseTrainingStates] ([LicenseTrainingStateId])
GO
ALTER TABLE [dbo].[Persons] CHECK CONSTRAINT [FK_Persons_LicenseTrainingStatesGliderPAX]
GO
ALTER TABLE [dbo].[Persons]  WITH CHECK ADD  CONSTRAINT [FK_Persons_LicenseTrainingStatesMotor] FOREIGN KEY([LicenseTrainingStateMotor])
REFERENCES [dbo].[LicenseTrainingStates] ([LicenseTrainingStateId])
GO
ALTER TABLE [dbo].[Persons] CHECK CONSTRAINT [FK_Persons_LicenseTrainingStatesMotor]
GO
ALTER TABLE [dbo].[Persons]  WITH CHECK ADD  CONSTRAINT [FK_Persons_LicenseTrainingStatesTMG] FOREIGN KEY([LicenseTrainingStateTMG])
REFERENCES [dbo].[LicenseTrainingStates] ([LicenseTrainingStateId])
GO
ALTER TABLE [dbo].[Persons] CHECK CONSTRAINT [FK_Persons_LicenseTrainingStatesTMG]
GO
ALTER TABLE [dbo].[Persons]  WITH CHECK ADD  CONSTRAINT [FK_Persons_LicenseTrainingStatesTowing] FOREIGN KEY([LicenseTrainingStateTowing])
REFERENCES [dbo].[LicenseTrainingStates] ([LicenseTrainingStateId])
GO
ALTER TABLE [dbo].[Persons] CHECK CONSTRAINT [FK_Persons_LicenseTrainingStatesTowing]
GO
ALTER TABLE [dbo].[Persons]  WITH CHECK ADD  CONSTRAINT [FK_Persons_LicenseTypes] FOREIGN KEY([LicenseTypeId])
REFERENCES [dbo].[LicenseTypes] ([LicenseTypeId])
GO
ALTER TABLE [dbo].[Persons] CHECK CONSTRAINT [FK_Persons_LicenseTypes]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRole_Roles]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRole_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Club]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Persons] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([PersonId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Persons]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserAccountStates] FOREIGN KEY([AccountState])
REFERENCES [dbo].[UserAccountStates] ([UserAccountStateId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserAccountStates]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Country code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Countries', @level2type=N'COLUMN',@level2name=N'CountryCodeIso2'
GO
USE [master]
GO
ALTER DATABASE [FLSTest] SET  READ_WRITE 
GO
