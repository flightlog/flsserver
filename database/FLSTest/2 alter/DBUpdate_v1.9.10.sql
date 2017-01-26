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
           (24,1,9,10,0
           ,'1.9.9.0'
           ,SYSUTCDATETIME())
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
PRINT 'Add FlightDate to flight table'
ALTER TABLE [dbo].[PersonClub]
	ADD [IsActive] [bit] NOT NULL CONSTRAINT [DF__PersonClub__IsActive]  DEFAULT ((1))
GO

UPDATE [dbo].[PersonClub] SET [IsActive] = 1
  WHERE [IsDeleted] = 0

UPDATE [dbo].[PersonClub] SET [IsActive] = 0
  WHERE [IsDeleted] = 1