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
           (11,1,8,9,0
           ,'1.8.8.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify EmailTemplates table'

ALTER TABLE [dbo].[EmailTemplates]
	ADD [IsCustomizable] [bit] NOT NULL default(0)
GO

UPDATE [dbo].[EmailTemplates] SET [IsCustomizable] = 1 where EmailTemplateKeyName = 'emailconfirmation' 
OR EmailTemplateKeyName = 'lostpassword' OR EmailTemplateKeyName = 'planningday-ok' OR EmailTemplateKeyName = 'planningday-cancel'  
GO

PRINT 'Finished update to Version 1.8.9'