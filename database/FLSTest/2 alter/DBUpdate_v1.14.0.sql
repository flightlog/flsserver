USE [FLSTest]
GO

PRINT 'Update SystemVersion Information'
INSERT INTO [dbo].[SystemVersion]
           ([VersionId]
		   ,[MajorVersion]
           ,[MinorVersion]
           ,[BuildVersion]
           ,[RevisionVersion]
           ,[UpgradeFromVersion]
           ,[UpgradeDateTime])
     VALUES
           (49,1,14,0,0
           ,'1.12.0.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Insert Movement table'
/****** Object:  Table [dbo].[Movements] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movements](
	[MovementId] [uniqueidentifier] NOT NULL,
	[MovementDateTimeUtc] [datetime2](7) NOT NULL,
	[DeviceId] [nvarchar](20) NOT NULL,
	[Immatriculation] [nvarchar](15) NOT NULL,
	[AircraftType] [int] NOT NULL,
	[LocationIcaoCode] [nvarchar](10) NOT NULL,
	[MovementType] [int] NOT NULL,
	[FlightId] [uniqueidentifier] NULL,
	[FurtherFlightIdsFound] [nvarchar](max) NULL,
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
 CONSTRAINT [PK_dbo.Movements] PRIMARY KEY CLUSTERED 
(
	[MovementId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Movements]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Movements_dbo.Flights_FlightId] FOREIGN KEY([FlightId])
REFERENCES [dbo].[Flight] ([FlightId])
GO

PRINT 'Finished update to Version 1.14.0'