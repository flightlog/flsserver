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
           (29,1,9,15,0
           ,'1.9.14.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Extend flights table'

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[Flights]
	ADD [FlightReportSentOn] [datetime2](7) NULL
GO

UPDATE [dbo].[Flights] SET
	[ValidatedOn] = DATEADD(hour, 23, cast(cast([CreatedOn] as date) as datetime2)),
	[FlightReportSentOn] = DATEADD(hour, 23, cast(cast([CreatedOn] as date) as datetime2))
 where ValidatedOn is null
  and IsDeleted = 0
  and ProcessStateId > 0

UPDATE [dbo].[Flights] SET
	[FlightReportSentOn] = DATEADD(hour, 23, cast(cast([ValidatedOn] as date) as datetime2))
  where ValidatedOn is not null
  and FlightReportSentOn is null

PRINT 'Finished update to Version 1.9.15'