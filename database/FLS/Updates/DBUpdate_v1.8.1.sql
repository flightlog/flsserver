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
           (3,1,8,1,0
           ,'1.8.0.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Cleaning licence tables'
DROP TABLE [dbo].[LicenseTrainingStates]
GO
ALTER TABLE [dbo].[Persons] DROP CONSTRAINT [FK_Persons_LicenseTypes]
GO
ALTER TABLE [dbo].[Persons]
	DROP COLUMN [LicenseTypeId]
GO
DROP TABLE [dbo].[LicenseTypes]
GO
ALTER TABLE [dbo].[Persons]
	ADD [LicenceNumber] [nvarchar](20) NULL
GO
UPDATE [dbo].[Persons] SET [LicenceNumber] = [LicenseNumber]
GO
ALTER TABLE [dbo].[Persons]
	DROP COLUMN [LicenseNumber]
GO
PRINT 'Finished cleaning licence tables'


PRINT 'Add InstructorExpireDate Field'
ALTER TABLE [dbo].[Persons]
	ADD [GliderInstructorLicenceExpireDate] [datetime2](7) NULL
GO
PRINT 'Finished adding InstructorExpireDate Field'


PRINT 'Change DB-Schema for Flight-Time-Splitting-Handling and instruction time'
ALTER TABLE [dbo].[FlightCrew]
	ADD [BeginFlightDateTime] [datetime2](7) NULL,
	[EndFlightDateTime] [datetime2](7) NULL,
	[BeginInstructionDateTime] [datetime2](7) NULL,
	[EndInstructionDateTime] [datetime2](7) NULL,
	[NrOfLdgs] [int] NULL
GO
UPDATE [dbo].[FlightCrew] SET [BeginFlightDateTime] = [dbo].[Flights].[StartDateTime], 
	[EndFlightDateTime] = [dbo].[Flights].[LdgDateTime],
	[NrOfLdgs] = [dbo].[Flights].[NrOfLdgs]
	FROM [dbo].[Flights] INNER JOIN FlightCrew ON [dbo].[Flights].FlightId = [dbo].[FlightCrew].FlightId
	WHERE [dbo].[FlightCrew].FlightCrewType = 1 --Pilots
GO
UPDATE [dbo].[FlightCrew] SET [BeginFlightDateTime] = [dbo].[Flights].[StartDateTime], 
	[EndFlightDateTime] = [dbo].[Flights].[LdgDateTime],
	[BeginInstructionDateTime] = [dbo].[Flights].[StartDateTime], 
	[EndInstructionDateTime] = [dbo].[Flights].[LdgDateTime],
	[NrOfLdgs] = [dbo].[Flights].[NrOfLdgs]
	FROM [dbo].[Flights] INNER JOIN FlightCrew ON [dbo].[Flights].FlightId = [dbo].[FlightCrew].FlightId
	WHERE [dbo].[FlightCrew].FlightCrewType = 3 --Instructors
GO
PRINT 'Finished change DB-Schema for Flight-Time-Splitting-Handling and instruction time'

PRINT 'Finished update to Version 1.8.1'