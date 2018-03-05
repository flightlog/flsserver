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
           (41,1,9,27,0
           ,'1.9.26.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify Persons table'
ALTER TABLE [dbo].[Persons] ALTER COLUMN [MedicalClass1ExpireDate] date null
ALTER TABLE [dbo].[Persons] ALTER COLUMN [MedicalClass2ExpireDate] date null
ALTER TABLE [dbo].[Persons] ALTER COLUMN [MedicalLaplExpireDate] date null
ALTER TABLE [dbo].[Persons] ALTER COLUMN [GliderInstructorLicenceExpireDate] date null
ALTER TABLE [dbo].[Persons] ALTER COLUMN [MotorInstructorLicenceExpireDate] date null
ALTER TABLE [dbo].[Persons] ALTER COLUMN [PartMLicenceExpireDate] date null
GO

PRINT 'Finished update to Version 1.9.27'