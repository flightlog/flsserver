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
           (8,1,8,6,0
           ,'1.8.5.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify Clubs table'

ALTER TABLE [dbo].[Clubs] DROP CONSTRAINT [DF__Clubs__SendPlann__595B4002]
GO

ALTER TABLE [dbo].[Clubs]
	DROP COLUMN [SendPlanningDayInfoMailToClubMembers]
GO

PRINT 'Modify EmailTemplates table'

ALTER TABLE [dbo].[EmailTemplates]
	ADD [HtmlBody] [nvarchar](max) NULL,
	[TextBody] [nvarchar](max) NULL
GO

UPDATE [dbo].[EmailTemplates] SET [HtmlBody] = [Body]
GO

ALTER TABLE [dbo].[EmailTemplates]
	DROP COLUMN [Body]
GO

PRINT 'PLEASE RUN SQL-Script "90 Insert EmailTemplates"'

PRINT 'Finished update to Version 1.8.6'