CREATE TABLE [dbo].[AuditLogDetails] (
    [Id]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [PropertyName]  NVARCHAR (256) NOT NULL,
    [OriginalValue] NVARCHAR (MAX) NULL,
    [NewValue]      NVARCHAR (MAX) NULL,
    [AuditLogId]    BIGINT         NOT NULL,
    CONSTRAINT [PK_dbo.AuditLogDetails] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AuditLogDetails_dbo.AuditLogs_AuditLogId] FOREIGN KEY ([AuditLogId]) REFERENCES [dbo].[AuditLogs] ([AuditLogId]) ON DELETE CASCADE
);

