CREATE TABLE [dbo].[SystemLogs] (
    [LogId]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [EventDateTime] DATETIME2 (7)  NOT NULL,
    [Application]   NVARCHAR (200) NULL,
    [LogLevel]      NVARCHAR (100) NOT NULL,
    [EventType]     BIGINT         NULL,
    [Logger]        NVARCHAR (MAX) NULL,
    [Message]       NVARCHAR (MAX) NULL,
    [UserName]      NVARCHAR (100) NULL,
    [ComputerName]  NVARCHAR (50)  NULL,
    [CallSite]      NVARCHAR (MAX) NULL,
    [Thread]        NVARCHAR (100) NULL,
    [Exception]     VARCHAR (MAX)  NULL,
    [Stacktrace]    VARCHAR (MAX)  NULL,
    CONSTRAINT [PK_SystemLogs] PRIMARY KEY CLUSTERED ([LogId] ASC)
);


GO
GRANT INSERT
    ON OBJECT::[dbo].[SystemLogs] TO [flstest_logwriter]
    AS [dbo];

