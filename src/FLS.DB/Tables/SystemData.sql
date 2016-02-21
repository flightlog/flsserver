CREATE TABLE [dbo].[SystemData] (
    [SystemId]                     UNIQUEIDENTIFIER NOT NULL,
    [BaseURL]                      NVARCHAR (250)   NOT NULL,
    [ReportSenderEmailAddress]     NVARCHAR (100)   NOT NULL,
    [SystemSenderEmailAddress]     NVARCHAR (100)   NOT NULL,
    [SmtpServer]                   NVARCHAR (100)   NOT NULL,
    [SmtpPort]                     INT              NOT NULL,
    [MaxUserLoginAttempts]         INT              NOT NULL,
    [Testmode]                     BIT              DEFAULT ((0)) NOT NULL,
    [DebugMode]                    BIT              DEFAULT ((0)) NOT NULL,
    [SendToBccRecipients]          BIT              DEFAULT ((0)) NOT NULL,
    [BccRecipientEmailAddresses]   NVARCHAR (250)   NULL,
    [UseSmtpAuthentication]        BIT              DEFAULT ((1)) NOT NULL,
    [UseSSLforSmtpConnection]      BIT              DEFAULT ((1)) NOT NULL,
    [SmtpUsername]                 NVARCHAR (100)   NULL,
    [SmtpPassword]                 NVARCHAR (100)   NULL,
    [TestmodeEmailPickupDirectory] NVARCHAR (100)   NULL,
    CONSTRAINT [PK_System] PRIMARY KEY CLUSTERED ([SystemId] ASC)
);

