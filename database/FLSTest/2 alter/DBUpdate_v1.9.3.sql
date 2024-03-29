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
           (17,1,9,3,0
           ,'1.9.2.0'
           ,SYSUTCDATETIME())
GO


PRINT 'Refactor CounterUnitTypes tables'
ALTER TABLE [dbo].[AircraftOperatingCounters] DROP CONSTRAINT [FK_AircraftOperatingCounters_CounterUnitTypes_Engine]
GO
ALTER TABLE [dbo].[AircraftOperatingCounters] DROP CONSTRAINT [FK_AircraftOperatingCounters_CounterUnitTypes_Flight]
GO
ALTER TABLE [dbo].[AircraftOperatingCounters]
	DROP COLUMN [FlightOperatingCounter], [EngineOperatingCounter], [NextMaintenanceAtFlightOperatingCounter], 
	[NextMaintenanceAtEngineOperatingCounter], [FlightOperatingCounterUnitTypeId], [EngineOperatingCounterUnitTypeId]
GO

ALTER TABLE [dbo].[Aircrafts] DROP CONSTRAINT [FK_Aircrafts_CounterUnitTypes_Engine]
GO
ALTER TABLE [dbo].[Aircrafts] DROP CONSTRAINT [FK_Aircrafts_CounterUnitTypes_Flight]
GO
ALTER TABLE [dbo].[Flights] DROP CONSTRAINT [FK_Flights_CounterUnitTypes]
GO
ALTER TABLE [dbo].[Flights]
	DROP COLUMN [EngineOperatingCounterUnitTypeId]
GO

DROP TABLE [dbo].[CounterUnitTypes]
GO





SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CounterUnitTypes](
	[CounterUnitTypeId] [int] NOT NULL,
	[CounterUnitTypeName] [nvarchar](100) NOT NULL,
	[CounterUnitTypeKeyName] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](200) NULL,
	[IsActive] [bit] CONSTRAINT [DF__CounterUnitTypes__IsActive] DEFAULT((1)) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_CounterUnitTypes] PRIMARY KEY CLUSTERED 
(
	[CounterUnitTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_CounterUnitTypes_CounterUnitTypeKeyName] UNIQUE NONCLUSTERED 
(
	[CounterUnitTypeKeyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[CounterUnitTypes] ([CounterUnitTypeId], [CounterUnitTypeName], [CounterUnitTypeKeyName],[Comment],[IsActive],[CreatedOn])
     VALUES (1, 'Minutes','Min',NULL,1,SYSUTCDATETIME())
GO

INSERT INTO [dbo].[CounterUnitTypes] ([CounterUnitTypeId], [CounterUnitTypeName], [CounterUnitTypeKeyName],[Comment],[IsActive],[CreatedOn])
     VALUES (2, '2 decimals per hour','2decimalsperhour',NULL,1,SYSUTCDATETIME())
GO

PRINT 'Modify AircraftOperatingCounters table'
ALTER TABLE [dbo].[AircraftOperatingCounters]
	ADD [FlightOperatingCounterInSeconds] [bigint] NULL,
		[EngineOperatingCounterInSeconds] [bigint] NULL,
		[NextMaintenanceAtFlightOperatingCounterInSeconds] [bigint] NULL,
		[NextMaintenanceAtEngineOperatingCounterInSeconds] [bigint] NULL
GO

ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircrafts_CounterUnitTypes_Flight] FOREIGN KEY([FlightOperatingCounterUnitTypeId])
REFERENCES [dbo].[CounterUnitTypes] ([CounterUnitTypeId])
GO

ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircrafts_CounterUnitTypes_Flight]
GO

ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircrafts_CounterUnitTypes_Engine] FOREIGN KEY([EngineOperatingCounterUnitTypeId])
REFERENCES [dbo].[CounterUnitTypes] ([CounterUnitTypeId])
GO

ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircrafts_CounterUnitTypes_Engine]
GO



PRINT 'Modify Flights table'
ALTER TABLE [dbo].[Flights]
	ADD [EngineStartOperatingCounterInSeconds] [bigint] NULL,
		[EngineEndOperatingCounterInSeconds] [bigint] NULL
GO

UPDATE [dbo].[Flights] SET [EngineStartOperatingCounterInSeconds] = EngineStartOperatingCounter * 60, [EngineEndOperatingCounterInSeconds] = EngineEndOperatingCounter * 60
GO

ALTER TABLE [dbo].[Flights]
	DROP COLUMN [EngineStartOperatingCounter], [EngineEndOperatingCounter]
GO


PRINT 'Finished update to Version 1.9.3'