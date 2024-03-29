USE [FLSTest]
GO

DROP TABLE [dbo].[SystemData]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemData](
	[SystemId] [uniqueidentifier] NOT NULL,
	[BaseURL] [nvarchar](250) NOT NULL,
	[ReportSenderEmailAddress] [nvarchar](100) NOT NULL,
	[SystemSenderEmailAddress] [nvarchar](100) NOT NULL,
	[SmtpUsername] [nvarchar](100) NOT NULL,
	[SmtpPasswort] [nvarchar](100) NOT NULL,
	[SmtpServer] [nvarchar](100) NOT NULL,
	[SmtpPort] [int] NOT NULL,
	[MaxUserLoginAttempts] [int] NOT NULL,
	[Testmode] [bit] NOT NULL default(0),
	[TestmodeRecipientEmailAddresses] [nvarchar](250) NULL,
	[DebugMode] [bit] NOT NULL default(0),
	[SendToBccRecipients] [bit] NOT NULL default(0),
	[BccRecipientEmailAddresses] [nvarchar](250) NULL
 CONSTRAINT [PK_System] PRIMARY KEY CLUSTERED 
(
	[SystemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO