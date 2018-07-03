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
           (45,1,10,6,0
           ,'1.10.5.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify Deliveries table'
ALTER TABLE [dbo].[Deliveries] 
	DROP COLUMN [RecipientDetails] 
GO

PRINT 'Finished update to Version 1.10.6'