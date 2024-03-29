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
           (32,1,9,18,0
           ,'1.9.17.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Extend aircraft table'

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Aircrafts]
	ADD [HomebaseId] [uniqueidentifier] NULL
GO

ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircraft_Homebase] FOREIGN KEY([HomebaseId])
REFERENCES [dbo].[Locations] ([LocationId])
GO

ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircraft_Homebase]
GO

PRINT 'Modify InOutboundPoints table'
ALTER TABLE [dbo].[InOutboundPoints]
	DROP COLUMN [SortIndicatorInboundPoint], [SortIndicatorOutboundPoint]
GO

PRINT 'Finished update to Version 1.9.18'