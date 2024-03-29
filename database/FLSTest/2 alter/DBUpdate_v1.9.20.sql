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
           (34,1,9,20,0
           ,'1.9.19.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Remove UNIQUE_Articles_ArticleName'
ALTER TABLE [dbo].[Articles] DROP CONSTRAINT [UNIQUE_Articles_ArticleName]
GO


PRINT 'Finished update to Version 1.9.20'