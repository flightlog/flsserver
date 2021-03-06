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
           (1,1,7,0,0
           ,'1.6.0.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Cleanup SystemData (SmtpUsername and SmtpPassword)'
ALTER TABLE [dbo].[SystemData] 
	ADD [TempSmtpPassword] [nvarchar](100) NULL,
		[TempSmtpUsername] [nvarchar](100) NULL
GO

UPDATE [dbo].[SystemData] SET [TempSmtpPassword] = [SmtpPasswort], [TempSmtpUsername] = [SmtpUsername]
GO

ALTER TABLE [dbo].[SystemData] DROP COLUMN [SmtpPasswort]
GO
ALTER TABLE [dbo].[SystemData] DROP COLUMN [SmtpUsername]
GO

ALTER TABLE [dbo].[SystemData] 
	ADD [SmtpUsername] [nvarchar](100) NULL
GO
ALTER TABLE [dbo].[SystemData] 
	ADD [SmtpPassword] [nvarchar](100) NULL
GO

UPDATE [dbo].[SystemData] SET [SmtpPassword] = [TempSmtpPassword], [SmtpUsername] = [TempSmtpUsername]
GO

ALTER TABLE [dbo].[SystemData] DROP COLUMN [TempSmtpPassword]
GO
ALTER TABLE [dbo].[SystemData] DROP COLUMN [TempSmtpUsername]
GO
PRINT 'Finished cleanup SystemData (SmtpUsername and SmtpPassword)'


PRINT 'Cleanup Extensions'

DROP TABLE [dbo].[ClubExtensions]
GO
DROP TABLE [dbo].[ExtensionParameterValues]
GO
DROP TABLE [dbo].[ExtensionParameters]
GO
DROP TABLE [dbo].[Extensions]
GO
DROP TABLE [dbo].[ExtensionParameterTypes]
GO
DROP TABLE [dbo].[ExtensionTypes]
GO

/****** Object:  Table [dbo].[ClubExtensions]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClubExtensions](
	[ClubId] [uniqueidentifier] NOT NULL,
	[ExtensionId] [uniqueidentifier] NOT NULL,
	[IsActive] [bit] NOT NULL,
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
 CONSTRAINT [PK_ClubExtensions] PRIMARY KEY CLUSTERED 
(
	[ClubId] ASC,
	[ExtensionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ExtensionParameters]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtensionParameters](
	[ExtensionParameterId] [uniqueidentifier] NOT NULL,
	[ExtensionId] [uniqueidentifier] NOT NULL,
	[ExtensionParameterName] [nvarchar](50) NOT NULL,
	[ExtensionParameterKeyString] [nvarchar](50) NOT NULL,
	[ExtensionParameterType] [int] NOT NULL,
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
 CONSTRAINT [PK_ExtensionParameters] PRIMARY KEY CLUSTERED 
(
	[ExtensionParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExtensionParameterTypes]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtensionParameterTypes](
	[ExtensionParameterTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ExtensionParameterTypeName] [nvarchar](50) NOT NULL,
	[StoreValuesAsBinaryData] [bit] NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_ExtensionParameterTypes] PRIMARY KEY CLUSTERED 
(
	[ExtensionParameterTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExtensionParameterValues]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ExtensionParameterValues](
	[ExtensionParameterValueId] [uniqueidentifier] NOT NULL,
	[ExtensionParameterValue] [nvarchar](max) NULL,
	[ExtensionParameterBinaryValue] [varbinary](max) NULL,
	[ExtensionParameterId] [uniqueidentifier] NOT NULL,
	[ClubId] [uniqueidentifier] NULL,
	[IsDefault] [bit] NOT NULL,
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
 CONSTRAINT [PK_ExtensionParameterValues] PRIMARY KEY CLUSTERED 
(
	[ExtensionParameterValueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Extensions]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Extensions](
	[ExtensionId] [uniqueidentifier] NOT NULL,
	[ExtensionName] [nvarchar](50) NOT NULL,
	[ExtensionClassName] [nvarchar](100) NOT NULL,
	[ExtensionFullClassName] [nvarchar](250) NOT NULL,
	[ExtensionDllPublicKey] [nvarchar](max) NULL,
	[ExtensionDllFilename] [nvarchar](250) NULL,
	[ExtensionTypeId] [int] NOT NULL,
	[IsPublic] [bit] NOT NULL,
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
 CONSTRAINT [PK_Extensions] PRIMARY KEY CLUSTERED 
(
	[ExtensionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ExtensionTypes]    Script Date: 02.05.2014 21:50:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExtensionTypes](
	[ExtensionTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ExtensionTypeName] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_ExtensionTypes_1] PRIMARY KEY CLUSTERED 
(
	[ExtensionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[ClubExtensions] ADD  CONSTRAINT [DF_ClubExtensions_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ExtensionParameters] ADD  CONSTRAINT [DF_ExtensionParameters_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ExtensionParameterValues] ADD  CONSTRAINT [DF_ExtensionParameterValues_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Extensions] ADD  CONSTRAINT [DF_Extensions_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[ClubExtensions]  WITH CHECK ADD  CONSTRAINT [FK_ClubExtensions_Clubs] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[ClubExtensions] CHECK CONSTRAINT [FK_ClubExtensions_Clubs]
GO
ALTER TABLE [dbo].[ClubExtensions]  WITH CHECK ADD  CONSTRAINT [FK_ClubExtensions_Extensions] FOREIGN KEY([ExtensionId])
REFERENCES [dbo].[Extensions] ([ExtensionId])
GO
ALTER TABLE [dbo].[ClubExtensions] CHECK CONSTRAINT [FK_ClubExtensions_Extensions]
GO
ALTER TABLE [dbo].[ExtensionParameters]  WITH CHECK ADD  CONSTRAINT [FK_ExtensionParameters_ExtensionParameterTypes] FOREIGN KEY([ExtensionParameterType])
REFERENCES [dbo].[ExtensionParameterTypes] ([ExtensionParameterTypeId])
GO
ALTER TABLE [dbo].[ExtensionParameters] CHECK CONSTRAINT [FK_ExtensionParameters_ExtensionParameterTypes]
GO
ALTER TABLE [dbo].[ExtensionParameters]  WITH CHECK ADD  CONSTRAINT [FK_ExtensionParameters_Extensions] FOREIGN KEY([ExtensionId])
REFERENCES [dbo].[Extensions] ([ExtensionId])
GO
ALTER TABLE [dbo].[ExtensionParameters] CHECK CONSTRAINT [FK_ExtensionParameters_Extensions]
GO
ALTER TABLE [dbo].[ExtensionParameterValues]  WITH CHECK ADD  CONSTRAINT [FK_ExtensionParameterValues_Clubs] FOREIGN KEY([ClubId])
REFERENCES [dbo].[Clubs] ([ClubId])
GO
ALTER TABLE [dbo].[ExtensionParameterValues] CHECK CONSTRAINT [FK_ExtensionParameterValues_Clubs]
GO
ALTER TABLE [dbo].[ExtensionParameterValues]  WITH CHECK ADD  CONSTRAINT [FK_ExtensionParameterValues_ExtensionParameters] FOREIGN KEY([ExtensionParameterId])
REFERENCES [dbo].[ExtensionParameters] ([ExtensionParameterId])
GO
ALTER TABLE [dbo].[ExtensionParameterValues] CHECK CONSTRAINT [FK_ExtensionParameterValues_ExtensionParameters]
GO
ALTER TABLE [dbo].[Extensions]  WITH CHECK ADD  CONSTRAINT [FK_Extensions_ExtensionTypes] FOREIGN KEY([ExtensionTypeId])
REFERENCES [dbo].[ExtensionTypes] ([ExtensionTypeId])
GO
ALTER TABLE [dbo].[Extensions] CHECK CONSTRAINT [FK_Extensions_ExtensionTypes]
GO

PRINT 'Finished cleanup Extensions'
