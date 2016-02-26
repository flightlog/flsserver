CREATE TABLE [dbo].[AuditLogEventTypes] (
    [EventTypeId]   INT           NOT NULL,
    [EventTypeName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_dbo.AuditLogEventTypes] PRIMARY KEY CLUSTERED ([EventTypeId] ASC)
);

