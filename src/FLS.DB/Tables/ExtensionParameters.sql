CREATE TABLE [dbo].[ExtensionParameters] (
    [ExtensionParameterId]        UNIQUEIDENTIFIER NOT NULL,
    [ExtensionId]                 UNIQUEIDENTIFIER NOT NULL,
    [ExtensionParameterName]      NVARCHAR (50)    NOT NULL,
    [ExtensionParameterKeyString] NVARCHAR (50)    NOT NULL,
    [ExtensionParameterType]      INT              NOT NULL,
    [Comment]                     NVARCHAR (MAX)   NULL,
    [CreatedOn]                   DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]             UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]                  DATETIME2 (7)    NULL,
    [ModifiedByUserId]            UNIQUEIDENTIFIER NULL,
    [DeletedOn]                   DATETIME2 (7)    NULL,
    [DeletedByUserId]             UNIQUEIDENTIFIER NULL,
    [RecordState]                 INT              NULL,
    [OwnerId]                     UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]               INT              NOT NULL,
    [IsDeleted]                   BIT              CONSTRAINT [DF_ExtensionParameters_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ExtensionParameters] PRIMARY KEY CLUSTERED ([ExtensionParameterId] ASC),
    CONSTRAINT [FK_ExtensionParameters_ExtensionParameterTypes] FOREIGN KEY ([ExtensionParameterType]) REFERENCES [dbo].[ExtensionParameterTypes] ([ExtensionParameterTypeId]),
    CONSTRAINT [FK_ExtensionParameters_Extensions] FOREIGN KEY ([ExtensionId]) REFERENCES [dbo].[Extensions] ([ExtensionId])
);

