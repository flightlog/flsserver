CREATE TABLE [dbo].[AuditLogs] (
    [AuditLogId]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [UserName]     NVARCHAR (MAX) NULL,
    [EventDateUTC] DATETIME       NOT NULL,
    [EventType]    INT            NOT NULL,
    [TypeFullName] NVARCHAR (512) NOT NULL,
    [RecordId]     NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AuditLogs] PRIMARY KEY CLUSTERED ([AuditLogId] ASC),
    CONSTRAINT [FK_dbo.AuditLogs_dbo.AuditLogEventTypes_EventTypeId] FOREIGN KEY ([EventType]) REFERENCES [dbo].[AuditLogEventTypes] ([EventTypeId])
);

