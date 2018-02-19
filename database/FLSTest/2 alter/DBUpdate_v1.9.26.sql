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
           (40,1,9,26,0
           ,'1.9.25.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify Deliveries table'
ALTER TABLE [dbo].[Deliveries] DROP CONSTRAINT [FK_Deliveries_IncludesTowFlight]
GO

ALTER TABLE [dbo].[Deliveries] 
	DROP COLUMN [IncludesTowFlightId]
GO

PRINT 'Finished update to Version 1.9.26'