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
           (12,1,8,10,0
           ,'1.8.9.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify FlightTypes table'

ALTER TABLE [dbo].[FlightTypes]
	ADD [MinNrOfAircraftSeatsRequired] [int] NULL
GO

PRINT 'Modify Flights table'
ALTER TABLE [dbo].[Flights]
	ADD [NrOfPassengers] [int] NULL
GO

PRINT 'Finished update to Version 1.8.10'