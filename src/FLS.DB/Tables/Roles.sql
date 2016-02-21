CREATE TABLE [dbo].[Roles] (
    [RoleId]                   UNIQUEIDENTIFIER NOT NULL,
    [RoleName]                 NVARCHAR (100)   NOT NULL,
    [RoleApplicationKeyString] NVARCHAR (100)   NOT NULL,
    [CreatedOn]                DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]          UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]               DATETIME2 (7)    NULL,
    [ModifiedByUserId]         UNIQUEIDENTIFIER NULL,
    [DeletedOn]                DATETIME2 (7)    NULL,
    [DeletedByUserId]          UNIQUEIDENTIFIER NULL,
    [RecordState]              INT              NULL,
    [OwnerId]                  UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]            INT              NOT NULL,
    [IsDeleted]                BIT              DEFAULT ((0)) NOT NULL,
    [SortIndicator]            INT              NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);

