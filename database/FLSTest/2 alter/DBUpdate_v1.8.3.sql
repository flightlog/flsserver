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
           (5,1,8,3,0
           ,'1.8.2.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Creating AircraftOperatingCounters tables'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AircraftOperatingCounters](
	[AircraftOperatingCounterId] [uniqueidentifier] NOT NULL,
	[AircraftId] [uniqueidentifier] NOT NULL,
	[AtDateTime] [datetime2](7) NOT NULL,
	[TotalTowedGliderStarts] [int] NULL,
	[TotalWinchLaunchStarts] [int] NULL,
	[TotalSelfStarts] [int] NULL,
	[FlightOperatingCounterInMinutes] [numeric](18, 3) NULL,
	[EngineOperatingCounterInMinutes] [numeric](18, 3) NULL,
	[NextMaintenanceAtFlightOperatingCounterInMinutes] [numeric](18, 3) NULL,
	[NextMaintenanceAtEngineOperatingCounterInMinutes] [numeric](18, 3) NULL,
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
 CONSTRAINT [PK_AircraftOperatingCounters] PRIMARY KEY CLUSTERED 
(
	[AircraftOperatingCounterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_Aircrafts_SameTime_OpsCounters] UNIQUE NONCLUSTERED 
(
	[AircraftId] ASC,
	[AtDateTime] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AircraftOperatingCounters]  WITH CHECK ADD  CONSTRAINT [FK_AircraftOperatingCounters_Aircraft] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[Aircrafts] ([AircraftId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AircraftOperatingCounters] CHECK CONSTRAINT [FK_AircraftOperatingCounters_Aircraft]
GO

PRINT 'Finished creating AircraftOperatingCounters tables'

PRINT 'Finished update to Version 1.8.3'