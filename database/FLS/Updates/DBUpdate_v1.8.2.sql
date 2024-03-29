USE [FLS]
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
           (4,1,8,2,0
           ,'1.8.1.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Cleaning SystemLogs table'
ALTER TABLE [dbo].[SystemLogs]
	DROP COLUMN [SqlString],
	[Version]
GO
PRINT 'Finished cleaning SystemLogs table'


PRINT 'Creating AuditLogs tables'
/****** Object:  Table [dbo].[AuditLogs]    Script Date: 20.01.2016 14:02:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AuditLogs](
	[AuditLogId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[EventDateUTC] [datetime] NOT NULL,
	[EventType] [int] NOT NULL,
	[TypeFullName] [nvarchar](512) NOT NULL,
	[RecordId] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AuditLogs] PRIMARY KEY CLUSTERED 
(
	[AuditLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AuditLogDetails]    Script Date: 20.01.2016 14:04:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AuditLogDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[PropertyName] [nvarchar](256) NOT NULL,
	[OriginalValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
	[AuditLogId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.AuditLogDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AuditLogDetails]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AuditLogDetails_dbo.AuditLogs_AuditLogId] FOREIGN KEY([AuditLogId])
REFERENCES [dbo].[AuditLogs] ([AuditLogId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AuditLogDetails] CHECK CONSTRAINT [FK_dbo.AuditLogDetails_dbo.AuditLogs_AuditLogId]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AuditLogEventTypes](
	[EventTypeId] [int] NOT NULL,
	[EventTypeName] [nvarchar](50) NOT NULL
 CONSTRAINT [PK_dbo.AuditLogEventTypes] PRIMARY KEY CLUSTERED 
(
	[EventTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[AuditLogEventTypes]
           ([EventTypeId],[EventTypeName])
     VALUES
           (0, 'Added')

INSERT INTO [dbo].[AuditLogEventTypes]
           ([EventTypeId],[EventTypeName])
     VALUES
           (1, 'Deleted')

INSERT INTO [dbo].[AuditLogEventTypes]
           ([EventTypeId],[EventTypeName])
     VALUES
           (2, 'Modified')

INSERT INTO [dbo].[AuditLogEventTypes]
           ([EventTypeId],[EventTypeName])
     VALUES
           (3, 'Soft-Deleted')

INSERT INTO [dbo].[AuditLogEventTypes]
           ([EventTypeId],[EventTypeName])
     VALUES
           (4, 'Undeleted')
GO

ALTER TABLE [dbo].[AuditLogs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AuditLogs_dbo.AuditLogEventTypes_EventTypeId] FOREIGN KEY([EventType])
REFERENCES [dbo].[AuditLogEventTypes] ([EventTypeId])
GO

ALTER TABLE [dbo].[AuditLogs] CHECK CONSTRAINT [FK_dbo.AuditLogs_dbo.AuditLogEventTypes_EventTypeId]
GO

PRINT 'Finished creating AuditLogs tables'

PRINT 'Finished update to Version 1.8.1'