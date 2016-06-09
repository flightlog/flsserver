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
           (14,1,9,0,0
           ,'1.8.11.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify Persons table'

ALTER TABLE [dbo].[Persons]
	ADD [EnableAddress] [bit] CONSTRAINT [DF__Persons__EnableAddress] DEFAULT((1)) NOT NULL,
	[HasMotorInstructorLicence] [bit] CONSTRAINT [DF__Persons__HasMotorInstrLic] DEFAULT((0)) NOT NULL
GO

PRINT 'Modify PersonClub table'
ALTER TABLE [dbo].[PersonClub]
	ADD [IsMotorInstructor] [bit] CONSTRAINT [DF__PersonClu__IsMotorInstr] DEFAULT((0)) NOT NULL
GO

PRINT 'Modify Aircrafts table'
ALTER TABLE [dbo].[Aircrafts]
	ADD [EngineOperatorCounterPrecision] [int] CONSTRAINT [DF__Aircrafts__EngineOperatorCounterPrecision] DEFAULT((1)) NOT NULL
GO

PRINT 'Modify Flights table'
ALTER TABLE [dbo].[Flights]
	ADD [NoStartTimeInformation] [bit] CONSTRAINT [DF__Flights__NoStartTimeInformation] DEFAULT((0)) NOT NULL,
	[NoLdgTimeInformation] [bit] CONSTRAINT [DF__Flights__NoLdgTimeInformation] DEFAULT((0)) NOT NULL,
	[NrOfLdgsOnStartLocation] [int] NULL
GO

PRINT 'Modify FlightCrew table'
ALTER TABLE [dbo].[FlightCrew]
	ADD [NrOfStarts] [int] NULL
GO

PRINT 'Finished update to Version 1.9.0'