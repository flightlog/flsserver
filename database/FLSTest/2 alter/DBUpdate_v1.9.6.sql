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
           (20,1,9,6,0
           ,'1.9.5.0'
           ,SYSUTCDATETIME())
GO


PRINT 'Add invoice rule filter tables'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AccountingRuleFilterTypes](
	[AccountingRuleFilterTypeId] [int] NOT NULL,
	[AccountingRuleFilterTypeName] [nvarchar](100) NOT NULL,
	[AccountingRuleFilterTypeKeyName] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_AccountingRuleFilterTypes] PRIMARY KEY CLUSTERED 
(
	[AccountingRuleFilterTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_AccountingRuleFilterTypes_AccountingRuleFilterTypeKeyName] UNIQUE NONCLUSTERED 
(
	[AccountingRuleFilterTypeKeyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[AccountingRuleFilters](
	[AccountingRuleFilterId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[AccountingRuleFilterTypeId] [int] NOT NULL,
	[RuleFilterName] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__AccountingRuleFilters__IsActive]  DEFAULT ((1)),
	[SortIndicator] [int] NULL,
	[ArticleTarget] [nvarchar](max) NULL,
	[RecipientTarget] [nvarchar](max) NULL,
	[IsRuleForSelfstartedGliderFlights] [bit] NOT NULL,
	[IsRuleForGliderFlights] [bit] NOT NULL,
	[IsRuleForTowingFlights] [bit] NOT NULL,
	[IsRuleForMotorFlights] [bit] NOT NULL,
	[UseRuleForAllAircraftsExceptListed] [bit] NOT NULL,
	[MatchedAircraftImmatriculations] [nvarchar](max) NULL,
	[UseRuleForAllFlightTypesExceptListed] [bit] NOT NULL,
	[MatchedFlightTypeCodes] [nvarchar](max) NULL,
	[ExtendMatchingFlightTypeCodesToGliderAndTowFlight] [bit] NOT NULL,
	[UseRuleForAllStartLocationsExceptListed] [bit] NOT NULL,
	[MatchedStartLocations] [nvarchar](max) NULL,
	[UseRuleForAllLdgLocationsExceptListed] [bit] NOT NULL,
	[MatchedLdgLocations] [nvarchar](max) NULL,
	[UseRuleForAllClubMemberNumbersExceptListed] [bit] NOT NULL,
	[MatchedClubMemberNumbers] [nvarchar](max) NULL,
	[UseRuleForAllFlightCrewTypesExceptListed] [bit] NOT NULL,
	[MatchedFlightCrewTypes] [nvarchar](max) NULL,
	[IsChargedToClubInternal] [bit] NULL,
	[MinFlightTimeMatchingValue] [int] NULL,
	[MaxFlightTimeMatchingValue] [int] NULL,
	[IncludeThresholdText] [bit] NULL,
	[ThresholdText] [nvarchar](250) NULL,
	[IncludeFlightTypeName] [bit] NULL,
	[NoLandingTaxForGlider] [bit] NULL,
	[NoLandingTaxForTowingAircraft] [bit] NULL,
	[NoLandingTaxForAircraft] [bit] NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL
 CONSTRAINT [PK_AccountingRuleFilters] PRIMARY KEY CLUSTERED 
(
	[AccountingRuleFilterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[AccountingRuleFilters]  WITH CHECK ADD  CONSTRAINT [FK_AccountingRuleFilters_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO

ALTER TABLE [dbo].[AccountingRuleFilters] CHECK CONSTRAINT [FK_AccountingRuleFilters_Club]
GO

ALTER TABLE [dbo].[AccountingRuleFilters]  WITH CHECK ADD  CONSTRAINT [FK_AccountingRuleFilters_AccountingRuleFilterTypes] FOREIGN KEY([AccountingRuleFilterTypeId])
REFERENCES [dbo].[AccountingRuleFilterTypes] ([AccountingRuleFilterTypeId])
GO

ALTER TABLE [dbo].[AccountingRuleFilters] CHECK CONSTRAINT [FK_AccountingRuleFilters_AccountingRuleFilterTypes]
GO

INSERT INTO [dbo].[AccountingRuleFilterTypes]
           ([AccountingRuleFilterTypeId],[AccountingRuleFilterTypeName], [AccountingRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (10,'Recipient invoice rule filter', 'RecipientInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[AccountingRuleFilterTypes]
           ([AccountingRuleFilterTypeId],[AccountingRuleFilterTypeName], [AccountingRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (20,'No landing tax invoice rule filter', 'NoLandingTaxInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[AccountingRuleFilterTypes]
           ([AccountingRuleFilterTypeId],[AccountingRuleFilterTypeName], [AccountingRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (30,'Aircraft invoice rule filter', 'AircraftInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[AccountingRuleFilterTypes]
           ([AccountingRuleFilterTypeId],[AccountingRuleFilterTypeName], [AccountingRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (40,'Instructor fee invoice rule filter', 'InstructorFeeInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[AccountingRuleFilterTypes]
           ([AccountingRuleFilterTypeId],[AccountingRuleFilterTypeName], [AccountingRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (50,'Additional fuel fee invoice rule filter', 'AdditionalFuelFeeInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[AccountingRuleFilterTypes]
           ([AccountingRuleFilterTypeId],[AccountingRuleFilterTypeName], [AccountingRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (60,'Landing tax invoice rule filter', 'LandingTaxInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[AccountingRuleFilterTypes]
           ([AccountingRuleFilterTypeId],[AccountingRuleFilterTypeName], [AccountingRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (70,'VSF fee invoice rule filter', 'VsfFeeInvoiceRuleFilter', SYSDATETIME(), null)
GO