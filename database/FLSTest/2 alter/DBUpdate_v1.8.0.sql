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
           (2,1,8,0,0
           ,'1.7.0.0'
           ,SYSUTCDATETIME())
GO



PRINT 'Remove PersonMemberStates table and relations and add direct relation (personclub to memberstates)'
DROP TABLE [dbo].[PersonMemberStates]
GO

ALTER TABLE [dbo].[PersonClub]
	ADD [MemberStateId] [uniqueidentifier] NULL
GO

ALTER TABLE [dbo].[PersonClub]  WITH CHECK ADD  CONSTRAINT [FK_PersonClub_MemberState] FOREIGN KEY([MemberStateId])
REFERENCES [dbo].[MemberStates] ([MemberStateId])
GO
PRINT 'Finished Remove PersonMemberStates table and relations and add direct relation (personclub to memberstates)'



PRINT 'Change PersonPersonCategories table settings'
DROP TABLE [dbo].[PersonPersonCategories]
GO

/****** Object:  Table [dbo].[PersonPersonCategories] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersonPersonCategories](
	[PersonId] [uniqueidentifier] NOT NULL,
	[PersonCategoryId] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
 CONSTRAINT [PK_PersonPersonCategories] PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC,
	[PersonCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PersonPersonCategories]  WITH CHECK ADD  CONSTRAINT [FK_PersonPersonCategories_PersonCategories] FOREIGN KEY([PersonCategoryId])
REFERENCES [dbo].[PersonCategories] ([PersonCategoryId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonPersonCategories] CHECK CONSTRAINT [FK_PersonPersonCategories_PersonCategories]
GO
ALTER TABLE [dbo].[PersonPersonCategories]  WITH CHECK ADD  CONSTRAINT [FK_PersonPersonCategories_Persons] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([PersonId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PersonPersonCategories] CHECK CONSTRAINT [FK_PersonPersonCategories_Persons]
GO
PRINT 'Finished changing PersonPersonCategories table settings'



PRINT 'Update Roles'
ALTER TABLE [dbo].[Roles]
	ADD [SortIndicator] [int] NULL
GO

UPDATE [dbo].[Roles] SET SortIndicator = 1 where RoleApplicationKeyString = 'SystemAdministrator';
UPDATE [dbo].[Roles] SET SortIndicator = 2 where RoleApplicationKeyString = 'ClubAdministrator';
UPDATE [dbo].[Roles] SET SortIndicator = 3 where RoleApplicationKeyString = 'FlightOperator';

PRINT 'Finished updating Roles'



PRINT 'Update License Training States'
ALTER TABLE [dbo].[Persons] DROP CONSTRAINT [FK_Persons_LicenseTrainingStatesGlider]
GO
ALTER TABLE [dbo].[Persons] DROP CONSTRAINT [FK_Persons_LicenseTrainingStatesGliderPAX]
GO
ALTER TABLE [dbo].[Persons] DROP CONSTRAINT [FK_Persons_LicenseTrainingStatesGliderInstructor]
GO
ALTER TABLE [dbo].[Persons] DROP CONSTRAINT [FK_Persons_LicenseTrainingStatesMotor]
GO
ALTER TABLE [dbo].[Persons] DROP CONSTRAINT [FK_Persons_LicenseTrainingStatesTMG]
GO
ALTER TABLE [dbo].[Persons] DROP CONSTRAINT [FK_Persons_LicenseTrainingStatesTowing]
GO

ALTER TABLE [dbo].[Persons]
	DROP COLUMN [LicenseTrainingStateGlider],
	[LicenseTrainingStateGliderPAX],
	[LicenseTrainingStateGliderInstructor],
	[LicenseTrainingStateTowing],
	[LicenseTrainingStateTMG],
	[LicenseTrainingStateMotor]
GO

ALTER TABLE [dbo].[Persons]
	ADD [MedicalClass1ExpireDate] [datetime2](7) NULL,
	[MedicalClass2ExpireDate] [datetime2](7) NULL,
	[MedicalLaplExpireDate] [datetime2](7) NULL,
	[HasGliderTowingStartPermission] [bit] NOT NULL DEFAULT ((0)),
	[HasGliderSelfStartPermission] [bit] NOT NULL DEFAULT ((0)),
	[HasGliderWinchStartPermission] [bit] NOT NULL DEFAULT ((0))
GO
PRINT 'Finished updating License Training States'

PRINT 'Update Users table'
PRINT 'Create UsersCopy-Table'
/****** Object:  Table [dbo].[Users]    Script Date: 25.12.2015 02:28:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UsersCopy](
	[UserId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[FriendlyName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[PersonId] [uniqueidentifier] NULL,
	[NotificationEmail] [nvarchar](100) NOT NULL,
	[Remarks] [nvarchar](250) NULL,
	[FailedLoginCounts] [int] NOT NULL,
	[AccountState] [int] NOT NULL,
	[LastPasswordChangeOn] [datetime2](7) NULL,
	[ForcePasswordChangeNextLogon] [bit] NOT NULL DEFAULT ((0)),
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT ((0))
 CONSTRAINT [PK_UsersCopy] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_UsersCopy_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

PRINT 'Copy records from Users to UsersCopy'
INSERT INTO [dbo].[UsersCopy]
SELECT * FROM [dbo].[Users]
GO

PRINT 'Drop old Users-Table'
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_UserAccountStates]
GO

ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Persons]
GO

ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Club]
GO

ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_UserRole_Users]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 25.12.2015 02:31:29 ******/
DROP TABLE [dbo].[Users]
GO

PRINT 'Create new Users-Table'
/****** Object:  Table [dbo].[Users]    Script Date: 25.12.2015 02:31:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[UserId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](256) NOT NULL,
	[FriendlyName] [nvarchar](100) NOT NULL,
	[NotificationEmail] [nvarchar](256) NOT NULL,
	[EmailConfirmed] [bit] NOT NULL DEFAULT (0),
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PersonId] [uniqueidentifier] NULL,
	[Remarks] [nvarchar](max) NULL,
	[AccountState] [int] NOT NULL,
	[LastPasswordChangeOn] [datetime2](7) NULL,
	[ForcePasswordChangeNextLogon] [bit] NOT NULL DEFAULT (0),
	[PhoneNumber] [nvarchar](30) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL DEFAULT (0),
	[TwoFactorEnabled] [bit] NOT NULL DEFAULT (0),
	[LockoutEnabled] [bit] NOT NULL DEFAULT (1),
	[LockoutEndDateUtc] [datetime2](7) NULL,
	[AccessFailedCount] [int] NOT NULL DEFAULT (0),
	[CreatedOn] [datetime2](7) NOT NULL,
	[CreatedByUserId] [uniqueidentifier] NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
	[ModifiedByUserId] [uniqueidentifier] NULL,
	[DeletedOn] [datetime2](7) NULL,
	[DeletedByUserId] [uniqueidentifier] NULL,
	[RecordState] [int] NULL,
	[OwnerId] [uniqueidentifier] NOT NULL,
	[OwnershipType] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT ((0))
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_Users_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

PRINT 'Copy back users data'

INSERT INTO [dbo].[Users] ([UserId],[ClubId],[Username],[FriendlyName],[NotificationEmail],[EmailConfirmed],[PasswordHash],[SecurityStamp],[PersonId],
[Remarks],[AccountState],[LastPasswordChangeOn],[ForcePasswordChangeNextLogon],[LockoutEnabled],[CreatedOn],[CreatedByUserId],[ModifiedOn],[ModifiedByUserId],
[DeletedOn],[DeletedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
   SELECT [UserId]
		   ,[ClubId]
           ,[Username]
           ,[FriendlyName]
           ,[NotificationEmail]
		   ,1
		   ,[Password]
		   ,NEWID()
           ,[PersonId]
           ,[Remarks]
           ,[AccountState]
           ,[LastPasswordChangeOn]
           ,[ForcePasswordChangeNextLogon]
		   ,0
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId]
           ,[OwnershipType]
           ,[IsDeleted] FROM [dbo].[UsersCopy]

GO

PRINT 'Drop temp UsersCopy-Table'
DROP TABLE [dbo].[UsersCopy]
GO

PRINT 'Create Users-Constraints'
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Club]
GO

ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Persons] FOREIGN KEY([PersonId])
REFERENCES [dbo].[Persons] ([PersonId])
GO

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Persons]
GO

ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_UserAccountStates] FOREIGN KEY([AccountState])
REFERENCES [dbo].[UserAccountStates] ([UserAccountStateId])
GO

ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_UserAccountStates]
GO

ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRole_Users]
GO

PRINT 'Finished updating Users table'



PRINT 'Extend Clubname, Phonenumber and email address columns'
ALTER TABLE [dbo].[Clubs]
	ALTER COLUMN [Clubname] [nvarchar](100) not null
GO 
ALTER TABLE [dbo].[Clubs]
	ALTER COLUMN [Phone] [nvarchar](30) null
GO 
ALTER TABLE [dbo].[Clubs]
	ALTER COLUMN [FaxNumber] [nvarchar](30) null
GO 
ALTER TABLE [dbo].[Clubs]
	ALTER COLUMN [Email] [nvarchar](256) null
GO 

ALTER TABLE [dbo].[Persons]
	ALTER COLUMN [PrivatePhone] [nvarchar](30) null
GO 
ALTER TABLE [dbo].[Persons]
	ALTER COLUMN [MobilePhone] [nvarchar](30) null
GO 
ALTER TABLE [dbo].[Persons]
	ALTER COLUMN [BusinessPhone] [nvarchar](30) null
GO 
ALTER TABLE [dbo].[Persons]
	ALTER COLUMN [FaxNumber] [nvarchar](30) null
GO 
ALTER TABLE [dbo].[Persons]
	ALTER COLUMN [EmailPrivate] [nvarchar](256) null
GO 
ALTER TABLE [dbo].[Persons]
	ALTER COLUMN [EmailBusiness] [nvarchar](256) null
GO 
PRINT 'Finished extending Clubname, Phonenumber and email address columns'



PRINT 'Restructure of In-/Outbound-Routes bit'
ALTER TABLE [dbo].[Clubs] DROP CONSTRAINT [DF_Clubs_IsInboundRouteRequired1]
GO
ALTER TABLE [dbo].[Clubs] DROP CONSTRAINT [DF_Clubs_IsInOutboundRouteRequired]
GO
ALTER TABLE [dbo].[Clubs]
	DROP COLUMN [IsInboundRouteRequired],
	[IsOutboundRouteRequired]
GO
ALTER TABLE [dbo].[Locations]
ADD [IsInboundRouteRequired] [bit] NOT NULL DEFAULT (0),
	[IsOutboundRouteRequired] [bit] NOT NULL DEFAULT (0)
GO
PRINT 'Finished restructuring of In-/Outbound-Routes bit'



PRINT 'Create EmailTemplates table'
/****** Object:  Table [dbo].[ClubExtensions]    Script Date: 29.12.2015 10:35:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EmailTemplates](
	[EmailTemplateId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NULL,
	[EmailTemplateName] [nvarchar](100) NOT NULL,
	[EmailTemplateKeyName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[FromAddress] [nvarchar](256) NOT NULL,
	[ReplyToAddresses] [nvarchar](256) NULL,
	[Subject] [nvarchar](256) NOT NULL,
	[Body] [ntext] NOT NULL,
	[IsSystemTemplate] [bit] NOT NULL default (0),
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
 CONSTRAINT [PK_EmailTemplates] PRIMARY KEY CLUSTERED 
(
	[EmailTemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[EmailTemplates]  WITH CHECK ADD CONSTRAINT [FK_EmailTemplates_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO

ALTER TABLE [dbo].[EmailTemplates] CHECK CONSTRAINT [FK_EmailTemplates_Club]
GO

CREATE NONCLUSTERED INDEX [IX_EmailTemplatesKeyName] ON [dbo].[EmailTemplates]
(
	[EmailTemplateKeyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

PRINT 'Finished creating EmailTemplates table'

PRINT 'Redesign UserRoles-Table'
ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [DF__UserRoles__IsDel__467D75B8]
GO

ALTER TABLE [dbo].[UserRoles]
	DROP COLUMN [CreatedOn],
	[CreatedByUserId],
	[ModifiedOn],
	[ModifiedByUserId],
	[DeletedOn],
	[DeletedByUserId],
	[RecordState],
	[OwnerId],
	[OwnershipType],
	[IsDeleted]
GO
PRINT 'Finished redesigning UserRoles-Table'

PRINT 'Delete Permissions-Table'
DROP TABLE [dbo].[Permissions]
GO
PRINT 'Finished Deleting Permissions-Table'

PRINT 'Redesign Flights-Table for Engine Counter'
ALTER TABLE [dbo].[Flights]
ADD [EngineStartOperatingCounterInMinutes] [numeric](18, 3) NULL,
	[EngineEndOperatingCounterInMinutes] [numeric](18, 3) NULL
GO
PRINT 'Finished redesigning Flights-Table for Engine Counter'

PRINT 'Modify License Training States Table'
ALTER TABLE [dbo].[LicenseTrainingStates]
	ALTER COLUMN [Comment] [nvarchar](256) null
GO 

PRINT 'Finished update to Version 1.8'