USE [FLSTest]
GO

ALTER TABLE [dbo].[SystemData] 
	ADD [UseSmtpAuthentication] [bit] NOT NULL default(1),
		[UseSSLforSmtpConnection] [bit] NOT NULL default(1)
GO
