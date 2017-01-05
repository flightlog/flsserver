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
           (22,1,9,8,0
           ,'1.9.7.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Add delivery creation test table'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliveryCreationTests](
	[DeliveryCreationTestId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[FlightId] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL CONSTRAINT [DF__DeliveryCreationTests__IsActive]  DEFAULT ((1)),
	[DeliveryCreationTestName] [nvarchar](250) NULL,
	[Description] [nvarchar](max) NULL,
	[ExpectedDeliveryDetails] [nvarchar](max) NOT NULL,
	[IgnoreRecipientName] [bit] NOT NULL CONSTRAINT [DF__DeliveryCreationTests__IgnoreRecipientName]  DEFAULT ((0)),
	[IgnoreRecipientAddress] [bit] NOT NULL CONSTRAINT [DF__DeliveryCreationTests__IgnoreRecipientAddress]  DEFAULT ((0)),
	[IgnoreDeliveryInformation] [bit] NOT NULL CONSTRAINT [DF__DeliveryCreationTests__IgnoreDeliveryInformation]  DEFAULT ((0)),
	[IgnoreAdditionalInformation] [bit] NOT NULL CONSTRAINT [DF__DeliveryCreationTests__IgnoreAdditionalInformation]  DEFAULT ((0)),
	[IgnoreItemPositioning] [bit] NOT NULL CONSTRAINT [DF__DeliveryCreationTests__IgnoreItemPositioning]  DEFAULT ((0)),
	[IgnoreItemText] [bit] NOT NULL CONSTRAINT [DF__DeliveryCreationTests__IgnoreItemText]  DEFAULT ((0)),
	[IgnoreItemAdditionalInformation] [bit] NOT NULL CONSTRAINT [DF__DeliveryCreationTests__IgnoreItemAdditionalInformation]  DEFAULT ((0)),
	[LastTestRunOn] [datetime2](7) NULL,
	[LastTestSuccessful] [bit] NULL,
	[ResultMessage] [nvarchar](max) NULL,
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
 CONSTRAINT [PK_DeliveryCreationTests] PRIMARY KEY CLUSTERED 
(
	[DeliveryCreationTestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[DeliveryCreationTests]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryCreationTests_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO

ALTER TABLE [dbo].[DeliveryCreationTests] CHECK CONSTRAINT [FK_DeliveryCreationTests_Club]
GO

ALTER TABLE [dbo].[DeliveryCreationTests]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryCreationTests_Flight] FOREIGN KEY([FlightId])
REFERENCES [dbo].[Flights] ([FlightId])
GO

ALTER TABLE [dbo].[DeliveryCreationTests] CHECK CONSTRAINT [FK_DeliveryCreationTests_Flight]
GO
