USE [FLSTest]
GO

DELETE FROM SystemData

/****** Object:  Table [dbo].[SystemData]    Script Date: 05/30/2012 16:40:30 ******/
INSERT [dbo].[SystemData] ([SystemId], [BaseURL], [ReportSenderEmailAddress], [SystemSenderEmailAddress], [SmtpUsername], [SmtpPassword], [SmtpServer], [SmtpPort], [MaxUserLoginAttempts], [Testmode], [DebugMode], [SendToBccRecipients], [BccRecipientEmailAddresses], [UseSmtpAuthentication], [UseSSLforSmtpConnection]) 
VALUES (NEWID(), N'http://apitest.glider-fls.ch', N'test@glider-fls.ch', N'test@glider-fls.ch', NULL, NULL, 'smtp.gmail.com', 25, 10, 1, 1, 1, N'test@glider-fls.ch', 1, 0)
