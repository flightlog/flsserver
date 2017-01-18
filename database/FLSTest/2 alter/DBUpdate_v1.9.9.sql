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
           (23,1,9,9,0
           ,'1.9.8.0'
           ,SYSUTCDATETIME())
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
PRINT 'Add FlightDate to flight table'
ALTER TABLE [dbo].[Flights]
	ADD [FlightDate] [date] NULL
GO

UPDATE [dbo].[Flights] SET [FlightDate] = CAST(StartDateTime AS DATE)
  FROM [FLSTest].[dbo].[Flights]
  WHERE StartDateTime IS NOT NULL AND FlightDate IS NULL

UPDATE [dbo].[Flights] SET [FlightDate] = CAST(LdgDateTime AS DATE)
  FROM [FLSTest].[dbo].[Flights]
  WHERE LdgDateTime IS NOT NULL AND FlightDate IS NULL