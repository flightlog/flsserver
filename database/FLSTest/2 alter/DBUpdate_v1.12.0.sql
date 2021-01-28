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
           (48,1,12,0,0
           ,'1.11.0.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Insert Person Flight Time Credit table'
/****** Object:  Table [dbo].[PersonFlightTimeCredits] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].PersonFlightTimeCredits(
	[PersonFlightTimeCreditId] [uniqueidentifier] NOT NULL,
	[BalanceDateTime] [datetime2](7) NOT NULL,
	[NoFlightTimeLimit] [bit] NOT NULL,
	[CurrentFlightTimeBalanceInSeconds] [bigint] NOT NULL,
	[ValidUntil] [datetime2](7) NULL,
	[PersonId] [uniqueidentifier] NOT NULL,
	[UseRuleForAllAircraftsExceptListed] [bit] NOT NULL,
	[MatchedAircraftImmatriculations] [nvarchar](max) NULL,
	[DiscountInPercent] [int] NOT NULL CONSTRAINT [DF__PersonFlightTimeCredits__DiscountInPercent]  DEFAULT ((0)),
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.PersonFlightTimeCredits] PRIMARY KEY CLUSTERED 
(
	[PersonFlightTimeCreditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[PersonFlightTimeCredits]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PersonFlightTimeCredits_dbo.Persons_PersonId] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([PersonId])
GO
PRINT 'Finished update to Version 1.12.0'