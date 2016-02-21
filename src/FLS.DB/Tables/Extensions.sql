CREATE TABLE [dbo].[Extensions] (
    [ExtensionId]            UNIQUEIDENTIFIER NOT NULL,
    [ExtensionName]          NVARCHAR (50)    NOT NULL,
    [ExtensionClassName]     NVARCHAR (100)   NOT NULL,
    [ExtensionFullClassName] NVARCHAR (250)   NOT NULL,
    [ExtensionDllPublicKey]  NVARCHAR (MAX)   NULL,
    [ExtensionDllFilename]   NVARCHAR (250)   NULL,
    [ExtensionTypeId]        INT              NOT NULL,
    [IsPublic]               BIT              NOT NULL,
    [Comment]                NVARCHAR (MAX)   NULL,
    [CreatedOn]              DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]        UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]             DATETIME2 (7)    NULL,
    [ModifiedByUserId]       UNIQUEIDENTIFIER NULL,
    [DeletedOn]              DATETIME2 (7)    NULL,
    [DeletedByUserId]        UNIQUEIDENTIFIER NULL,
    [RecordState]            INT              NULL,
    [OwnerId]                UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]          INT              NOT NULL,
    [IsDeleted]              BIT              CONSTRAINT [DF_Extensions_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Extensions] PRIMARY KEY CLUSTERED ([ExtensionId] ASC),
    CONSTRAINT [FK_Extensions_ExtensionTypes] FOREIGN KEY ([ExtensionTypeId]) REFERENCES [dbo].[ExtensionTypes] ([ExtensionTypeId])
);

