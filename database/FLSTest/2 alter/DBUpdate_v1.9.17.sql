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
           (31,1,9,17,0
           ,'1.9.16.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify accountingrulefilter table'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

UPDATE [dbo].[AccountingRuleFilters]
   SET [MaxFlightTimeInSecondsMatchingValue] = 2147483647
 WHERE [MaxFlightTimeInSecondsMatchingValue] > 2147483647

UPDATE [dbo].[AccountingRuleFilters]
   SET [MaxEngineTimeInSecondsMatchingValue] = 2147483647
 WHERE [MaxEngineTimeInSecondsMatchingValue] > 2147483647

GO

ALTER TABLE [dbo].[AccountingRuleFilters] ALTER COLUMN [MinFlightTimeInSecondsMatchingValue] [int] NULL
ALTER TABLE [dbo].[AccountingRuleFilters] ALTER COLUMN [MaxFlightTimeInSecondsMatchingValue] [int] NULL
ALTER TABLE [dbo].[AccountingRuleFilters] ALTER COLUMN [MinEngineTimeInSecondsMatchingValue] [int] NULL
ALTER TABLE [dbo].[AccountingRuleFilters] ALTER COLUMN [MaxEngineTimeInSecondsMatchingValue] [int] NULL

GO

PRINT 'Finished update to Version 1.9.17'