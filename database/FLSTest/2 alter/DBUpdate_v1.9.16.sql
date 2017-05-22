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
           (30,1,9,16,0
           ,'1.9.15.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Add registrations table'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RegistrationTypes](
	[RegistrationTypeId] [int] NOT NULL,
	[RegistrationTypeName] [nvarchar](100) NOT NULL,
	[RegistrationTypeKeyName] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_RegistrationTypes] PRIMARY KEY CLUSTERED 
(
	[RegistrationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_RegistrationTypes_RegistrationTypeKeyName] UNIQUE NONCLUSTERED 
(
	[RegistrationTypeKeyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Registrations](
	[RegistrationId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[RegistrationTypeId] [int] NOT NULL,
	[Lastname] [nvarchar](100) NOT NULL,
	[Firstname] [nvarchar](100) NOT NULL,
	[AddressLine1] [nvarchar](200) NULL,
	[ZipCode] [nvarchar](10) NULL,
	[City] [nvarchar](100) NULL,
	[CountryId] [uniqueidentifier] NULL,
	[PrivatePhoneNumber] [nvarchar](30) NULL,
	[MobilePhoneNumber] [nvarchar](30) NULL,
	[BusinessPhoneNumber] [nvarchar](30) NULL,
	[PrivateEmail] [nvarchar](256) NULL,
	[InvoiceAddressIsSame] [bit] NOT NULL DEFAULT ((1)),
	[InvoiceToLastname] [nvarchar](100) NOT NULL,
	[InvoiceToFirstname] [nvarchar](100) NOT NULL,
	[InvoiceToAddressLine1] [nvarchar](200) NULL,
	[InvoiceToZipCode] [nvarchar](10) NULL,
	[InvoiceToCity] [nvarchar](100) NULL,
	[InvoiceToCountryId] [uniqueidentifier] NULL,
	[NotificationEmail] [nvarchar](256) NULL,
	[SelectedDay] [datetime2](7) NOT NULL,
	[SendCouponToInvoiceAddress] [bit] NOT NULL DEFAULT ((1)),
	[Remarks] [nvarchar](MAX) NULL,
	[CandidatePersonId] [uniqueidentifier] NULL,
	[InvoiceToPersonId] [uniqueidentifier] NULL,
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
 CONSTRAINT [PK_Registrations] PRIMARY KEY CLUSTERED 
(
	[RegistrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO

ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_Club]
GO

ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_RegistrationType] FOREIGN KEY([RegistrationTypeId])
REFERENCES [dbo].[RegistrationTypes] ([RegistrationTypeId])
GO

ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_RegistrationType]
GO

ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_CandidatePerson] FOREIGN KEY([CandidatePersonId])
REFERENCES [dbo].[Persons] ([PersonId])
GO

ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_CandidatePerson]
GO

ALTER TABLE [dbo].[Registrations]  WITH CHECK ADD  CONSTRAINT [FK_Registrations_InvoiceToPerson] FOREIGN KEY([InvoiceToPersonId])
REFERENCES [dbo].[Persons] ([PersonId])
GO

ALTER TABLE [dbo].[Registrations] CHECK CONSTRAINT [FK_Registrations_InvoiceToPerson]
GO

INSERT INTO [dbo].[RegistrationTypes]
           ([RegistrationTypeId],[RegistrationTypeName], [RegistrationTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (10,'TrialFlightRegistration', 'TrialFlight', SYSDATETIME(), null)

INSERT INTO [dbo].[RegistrationTypes]
           ([RegistrationTypeId],[RegistrationTypeName], [RegistrationTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (20,'PassengerFlightRegistration', 'PassengerFlight', SYSDATETIME(), null)

GO

PRINT 'Finished update to Version 1.9.16'