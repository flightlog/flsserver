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
           (28,1,9,14,0
           ,'1.9.13.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Extend Accounting rule filters table'

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccountingUnitTypes](
	[AccountingUnitTypeId] [int] NOT NULL,
	[AccountingUnitTypeName] [nvarchar](100) NOT NULL,
	[AccountingUnitTypeKeyName] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_AccountingUnitTypes] PRIMARY KEY CLUSTERED 
(
	[AccountingUnitTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_AccountingUnitTypes_AccountingUnitTypeKeyName] UNIQUE NONCLUSTERED 
(
	[AccountingUnitTypeKeyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [dbo].[AccountingRuleFilters]
	ADD [UseRuleForAllStartTypesExceptListed] [bit] NOT NULL CONSTRAINT [DF__AccountingRuleFilters__UseRuleForAllStartTypes]  DEFAULT ((1)),
	[MatchedStartTypes] [nvarchar](max) NULL,
	[StopRuleEngineWhenRuleApplied] [bit] NOT NULL CONSTRAINT [DF__AccountingRuleFilters__StopRuleEngine]  DEFAULT ((0)),
	[MinFlightTimeInSecondsMatchingValue] [bigint] NULL,
	[MaxFlightTimeInSecondsMatchingValue] [bigint] NULL,
	[MinEngineTimeInSecondsMatchingValue] [bigint] NULL,
	[MaxEngineTimeInSecondsMatchingValue] [bigint] NULL,
	[AccountingUnitTypeId] [int] NULL
GO

UPDATE [dbo].[AccountingRuleFilters] SET 
	[MinFlightTimeInSecondsMatchingValue] = [MinFlightTimeMatchingValue] * 60,
	[MaxFlightTimeInSecondsMatchingValue] = [MaxFlightTimeMatchingValue] * 60,
	[MinEngineTimeInSecondsMatchingValue] = 0,
	[MaxEngineTimeInSecondsMatchingValue] = 9223372036854775807
GO

UPDATE [dbo].[AccountingRuleFilters] SET 
	[MaxFlightTimeInSecondsMatchingValue] = 9223372036854775807
	WHERE [MaxFlightTimeInSecondsMatchingValue] = 2147483647 or [MaxFlightTimeInSecondsMatchingValue] = 128849018820
GO

ALTER TABLE [dbo].[AccountingRuleFilters]
	DROP COLUMN [IsRuleForSelfstartedGliderFlights],
	[MinFlightTimeMatchingValue],
	[MaxFlightTimeMatchingValue]
GO

ALTER TABLE [dbo].[AccountingRuleFilters]  WITH CHECK ADD  CONSTRAINT [FK_AccountingRuleFilters_AccountingUnitTypes] FOREIGN KEY([AccountingUnitTypeId])
REFERENCES [dbo].[AccountingUnitTypes] ([AccountingUnitTypeId])
GO

ALTER TABLE [dbo].[AccountingRuleFilters] CHECK CONSTRAINT [FK_AccountingRuleFilters_AccountingUnitTypes]
GO

INSERT INTO [dbo].[AccountingUnitTypes]
           ([AccountingUnitTypeId],[AccountingUnitTypeName], [AccountingUnitTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (10,'Minuten', 'Min', SYSDATETIME(), null)

INSERT INTO [dbo].[AccountingUnitTypes]
           ([AccountingUnitTypeId],[AccountingUnitTypeName], [AccountingUnitTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (20,'Sekunden', 'Sec', SYSDATETIME(), null)

INSERT INTO [dbo].[AccountingUnitTypes]
           ([AccountingUnitTypeId],[AccountingUnitTypeName], [AccountingUnitTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (30,'Landungen', 'Ldgs', SYSDATETIME(), null)

INSERT INTO [dbo].[AccountingUnitTypes]
           ([AccountingUnitTypeId],[AccountingUnitTypeName], [AccountingUnitTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (40,'Start oder Flug', 'StartOrFlight', SYSDATETIME(), null)

UPDATE [dbo].[AccountingRuleFilters] SET 
	[AccountingUnitTypeId] = 10
	WHERE [AccountingRuleFilterTypeId] >= 30 AND [AccountingRuleFilterTypeId] <= 50 or [AccountingRuleFilterTypeId] = 80
GO

UPDATE [dbo].[AccountingRuleFilters] SET 
	[AccountingUnitTypeId] = 30
	WHERE [AccountingRuleFilterTypeId] = 60 or [AccountingRuleFilterTypeId] = 70
GO

PRINT 'Finished update to Version 1.9.14'