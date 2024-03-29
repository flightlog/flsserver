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
           (19,1,9,5,0
           ,'1.9.4.0'
           ,SYSUTCDATETIME())
GO


PRINT 'Add DeliveredOn column to Flights table'
ALTER TABLE [dbo].[Flights]
	ADD [DeliveredOn] [datetime2](7) NULL
GO

INSERT [dbo].[FlightProcessStates] ([FlightProcessStateId], [FlightProcessStateName], [Comment], [CreatedOn]) 
VALUES (45, N'Geliefert', N'Flug wurde mit Lieferschein abgebucht und kann nicht mehr editiert werden', SYSUTCDATETIME())
PRINT 'Finished update to Version 1.9.4'