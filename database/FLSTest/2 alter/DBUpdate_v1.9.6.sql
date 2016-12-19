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

CREATE TABLE [dbo].[InvoiceRuleFilterTypes](
	[InvoiceRuleFilterTypeId] [int] NOT NULL,
	[InvoiceRuleFilterTypeName] [nvarchar](100) NOT NULL,
	[InvoiceRuleFilterTypeKeyName] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_InvoiceRuleFilterTypes] PRIMARY KEY CLUSTERED 
(
	[InvoiceRuleFilterTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_InvoiceRuleFilterTypes_InvoiceRuleFilterTypeKeyName] UNIQUE NONCLUSTERED 
(
	[InvoiceRuleFilterTypeKeyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


CREATE TABLE [dbo].[InvoiceRuleFilters](
	[InvoiceRuleFilterId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[InvoiceRuleFilterTypeId] [int] NOT NULL,
	[RuleFilterName] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__InvoiceRuleFilters__IsActive]  DEFAULT ((1)),
	[SortIndicator] [int] NULL,
	[ArticleTarget] [nvarchar](max) NULL,
	[RecipientTarget] [nvarchar](max) NULL,
	[IsRuleForSelfstartedGliderFlights] [bit] NOT NULL,
	[IsRuleForGliderFlights] [bit] NOT NULL,
	[IsRuleForTowingFlights] [bit] NOT NULL,
	[IsRuleForMotorFlights] [bit] NOT NULL,
	[UseRuleForAllAircraftsExceptListed] [bit] NOT NULL,
	[AircraftImmatriculations] [nvarchar](max) NULL,
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
	[IsInvoicedToClubInternal] [bit] NULL,
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
 CONSTRAINT [PK_InvoiceRuleFilters] PRIMARY KEY CLUSTERED 
(
	[InvoiceRuleFilterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[InvoiceRuleFilters]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceRuleFilters_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO

ALTER TABLE [dbo].[InvoiceRuleFilters] CHECK CONSTRAINT [FK_InvoiceRuleFilters_Club]
GO

ALTER TABLE [dbo].[InvoiceRuleFilters]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceRuleFilters_InvoiceRuleFilterTypes] FOREIGN KEY([InvoiceRuleFilterTypeId])
REFERENCES [dbo].[InvoiceRuleFilterTypes] ([InvoiceRuleFilterTypeId])
GO

ALTER TABLE [dbo].[InvoiceRuleFilters] CHECK CONSTRAINT [FK_InvoiceRuleFilters_InvoiceRuleFilterTypes]
GO

