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
           (16,1,9,2,0
           ,'1.9.1.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify PersonClub table'

/****** Object:  Index [UNIQUE_PersonClub_PersonId_ClubId_MemberNumber]    Script Date: 12.09.2016 21:36:44 ******/
ALTER TABLE [dbo].[PersonClub] DROP CONSTRAINT [UNIQUE_PersonClub_PersonId_ClubId_MemberNumber]
GO

CREATE UNIQUE INDEX UNIQUE_PersonClub_ClubId_MemberNumber ON [dbo].[PersonClub](ClubId, MemberNumber) 
WHERE MemberNumber IS NOT NULL
GO

PRINT 'Make some invoice payment types inactive'
UPDATE [dbo].[FlightCostBalanceTypes]
   SET [IsForGliderFlights] = 0
      ,[IsForTowFlights] = 0
      ,[IsForMotorFlights] = 0
      ,[ModifiedOn] = SYSUTCDATETIME()
 WHERE [FlightCostBalanceTypeId] in (2,3)
GO

PRINT 'Refactor Extension tables'
DROP TABLE [dbo].[ExtensionParameterValues]
GO

DROP TABLE [dbo].[ExtensionParameters]
GO

DROP TABLE [dbo].[ExtensionParameterTypes]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ExtensionValues](
	[ExtensionValueId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NULL,
	[ExtensionValueName] [nvarchar](100) NOT NULL,
	[ExtensionValueKeyName] [nvarchar](100) NOT NULL,
	[ExtensionStringValue] [nvarchar](max) NULL,
	[ExtensionBinaryValue] [varbinary](max) NULL,
	[IsDefault] [bit] NOT NULL,
	[Comment] [nvarchar](max) NULL,
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
 CONSTRAINT [PK_ExtensionValues] PRIMARY KEY CLUSTERED 
(
	[ExtensionValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ExtensionValues] ADD  CONSTRAINT [DF_ExtensionValues_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[ExtensionValues]  WITH CHECK ADD  CONSTRAINT [FK_ExtensionValues_Clubs] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO

ALTER TABLE [dbo].[ExtensionValues] CHECK CONSTRAINT [FK_ExtensionValues_Clubs]
GO

PRINT 'Finished update to Version 1.9.2'