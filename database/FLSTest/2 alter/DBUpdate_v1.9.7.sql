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
           (21,1,9,7,0
           ,'1.9.6.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Make FlightTypeCodes unique'
ALTER TABLE [dbo].[FlightTypes] ADD  CONSTRAINT [UNIQUE_FlightTypes_FlightCode] UNIQUE NONCLUSTERED 
(
	[FlightCode] ASC,
	[ClubId] ASC,
	[DeletedOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO


PRINT 'Add delivery tables'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Deliveries](
	[DeliveryId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NOT NULL,
	[FlightId] [uniqueidentifier] NULL,
	[RecipientDetails] [nvarchar](max) NOT NULL,
	[DeliveryInformation] [nvarchar](250) NULL,
	[AdditionalInformation] [nvarchar](250) NULL,
	[DeliveryNumber] [nvarchar](100) NULL,
	[DeliveredOn] [datetime2](7) NULL,
	[IsFurtherProcessed] [bit] NOT NULL CONSTRAINT [DF__Deliveries__IsFurtherProcessed]  DEFAULT ((0)),
	[BatchId] [bigint] NULL,
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
 CONSTRAINT [PK_Deliveries] PRIMARY KEY CLUSTERED 
(
	[DeliveryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Deliveries]  WITH CHECK ADD  CONSTRAINT [FK_Deliveries_Club] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO

ALTER TABLE [dbo].[Deliveries] CHECK CONSTRAINT [FK_Deliveries_Club]
GO

ALTER TABLE [dbo].[Deliveries]  WITH CHECK ADD  CONSTRAINT [FK_Deliveries_Flight] FOREIGN KEY([FlightId])
REFERENCES [dbo].[Flights] ([FlightId])
GO

ALTER TABLE [dbo].[Deliveries] CHECK CONSTRAINT [FK_Deliveries_Flight]
GO


CREATE TABLE [dbo].[DeliveryItems](
	[DeliveryItemId] [uniqueidentifier] NOT NULL,
	[DeliveryId] [uniqueidentifier] NOT NULL,
	[Position] [int] NOT NULL,
	[ArticleNumber] [nvarchar](50) NOT NULL,
	[ItemText] [nvarchar](250) NULL,
	[AdditionalInformation] [nvarchar](250) NULL,
	[Quantity] decimal(18, 3) NOT NULL,
	[DiscountInPercent] [int] NOT NULL CONSTRAINT [DF__DeliveryItems__DiscountInPercent]  DEFAULT ((0)),
	[UnitType] [nvarchar](250) NOT NULL,
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
 CONSTRAINT [PK_DeliveryItems] PRIMARY KEY CLUSTERED 
(
	[DeliveryItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] 

GO

ALTER TABLE [dbo].[DeliveryItems]  WITH CHECK ADD  CONSTRAINT [FK_DeliveryItems_Delivery] FOREIGN KEY([DeliveryId])
REFERENCES [dbo].[Deliveries] ([DeliveryId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[DeliveryItems] CHECK CONSTRAINT [FK_DeliveryItems_Delivery]
GO


PRINT 'Redesign flight table'
ALTER TABLE [dbo].[Flights]
	ADD [DeliveryCreatedOn] [datetime2](7) NULL
GO

UPDATE [dbo].[Flights]
   SET [DeliveryCreatedOn] = [InvoicedOn]
 WHERE [InvoicedOn] is not null
GO

UPDATE [dbo].[Flights]
   SET [ProcessStateId] = 50
 WHERE [ProcessStateId] > 50 OR [ProcessStateId] = 45
GO

ALTER TABLE [dbo].[Flights]
	DROP COLUMN [InvoicedOn], [InvoicePaidOn], [InvoiceNumber], [DeliveryNumber], [DeliveredOn]
GO

UPDATE [dbo].[FlightProcessStates]
   SET [FlightProcessStateName] = 'Lieferschein erstellt'
      ,[Comment] = 'Flug-Lieferschein / Rechnung wurde erstellt und Flug kann nicht mehr editiert werden'
      ,[ModifiedOn] = SYSUTCDATETIME()
 WHERE [FlightProcessStateId] = 50
GO

DELETE FROM [dbo].[FlightProcessStates]
      WHERE FlightProcessStateId > 50 OR FlightProcessStateId = 45
GO

PRINT 'Modify club table'
ALTER TABLE [dbo].[Clubs]
	ADD [RunDeliveryCreationJob] [bit] NOT NULL CONSTRAINT [DF__Clubs__RunDeliveryCreationJob]  DEFAULT ((0)),
	[RunDeliveryMailExportJob] [bit] NOT NULL CONSTRAINT [DF__Clubs__RunDeliveryMailExportJob]  DEFAULT ((0)),
	[SendDeliveryMailExportTo] [nvarchar](250) NULL,
	[LastDeliverySynchronisationOn] [datetime2](7) NULL
GO

UPDATE [dbo].[Clubs]
   SET [SendDeliveryMailExportTo] = [SendInvoiceReportsTo]
      ,[LastDeliverySynchronisationOn] = [LastInvoiceExportOn]
GO

ALTER TABLE [dbo].[Clubs]
	DROP COLUMN [SendInvoiceReportsTo], [LastInvoiceExportOn]
GO