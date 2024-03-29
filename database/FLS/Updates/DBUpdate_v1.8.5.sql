USE [FLS]
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
           (7,1,8,5,0
           ,'1.8.4.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify FlightType table'

ALTER TABLE [dbo].[FlightTypes] DROP CONSTRAINT [DF__FlightTyp__IsSum__02084FDA]
GO

ALTER TABLE [dbo].[FlightTypes]
	DROP COLUMN [IsSummarizedSystemFlight]
GO

PRINT 'Modify Aircraft table'

ALTER TABLE [dbo].[Aircrafts]
	DROP COLUMN [MaintenanceAssignmentId]
GO

PRINT 'Modify Aircraft types table'
ALTER TABLE [dbo].[AircraftTypes]
	ADD [HasEngine] [bit] NULL,
	[RequiresTowingInfo] [bit] NULL,
	[MayBeTowingAircraft] [bit] NULL
GO

UPDATE [dbo].[AircraftTypes] SET [HasEngine] = 0, [RequiresTowingInfo] = 1, [MayBeTowingAircraft] = 0 WHERE [AircraftTypeId] = 1
UPDATE [dbo].[AircraftTypes] SET [HasEngine] = 1, [RequiresTowingInfo] = 1, [MayBeTowingAircraft] = 0 WHERE [AircraftTypeId] = 2
UPDATE [dbo].[AircraftTypes] SET [HasEngine] = 1, [RequiresTowingInfo] = 0, [MayBeTowingAircraft] = 1 WHERE [AircraftTypeId] = 4
UPDATE [dbo].[AircraftTypes] SET [HasEngine] = 1, [RequiresTowingInfo] = 0, [MayBeTowingAircraft] = 1 WHERE [AircraftTypeId] = 8
UPDATE [dbo].[AircraftTypes] SET [HasEngine] = 1, [RequiresTowingInfo] = 0, [MayBeTowingAircraft] = 0 WHERE [AircraftTypeId] > 8
GO

PRINT 'Finished update to Version 1.8.5'