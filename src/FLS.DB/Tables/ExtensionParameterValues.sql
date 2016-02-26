CREATE TABLE [dbo].[ExtensionParameterValues] (
    [ExtensionParameterValueId]     UNIQUEIDENTIFIER NOT NULL,
    [ExtensionParameterValue]       NVARCHAR (MAX)   NULL,
    [ExtensionParameterBinaryValue] VARBINARY (MAX)  NULL,
    [ExtensionParameterId]          UNIQUEIDENTIFIER NOT NULL,
    [ClubId]                        UNIQUEIDENTIFIER NULL,
    [IsDefault]                     BIT              NOT NULL,
    [CreatedOn]                     DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]               UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]                    DATETIME2 (7)    NULL,
    [ModifiedByUserId]              UNIQUEIDENTIFIER NULL,
    [DeletedOn]                     DATETIME2 (7)    NULL,
    [DeletedByUserId]               UNIQUEIDENTIFIER NULL,
    [RecordState]                   INT              NULL,
    [OwnerId]                       UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]                 INT              NOT NULL,
    [IsDeleted]                     BIT              CONSTRAINT [DF_ExtensionParameterValues_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ExtensionParameterValues] PRIMARY KEY CLUSTERED ([ExtensionParameterValueId] ASC),
    CONSTRAINT [FK_ExtensionParameterValues_Clubs] FOREIGN KEY ([ClubId]) REFERENCES [dbo].[Clubs] ([ClubId]),
    CONSTRAINT [FK_ExtensionParameterValues_ExtensionParameters] FOREIGN KEY ([ExtensionParameterId]) REFERENCES [dbo].[ExtensionParameters] ([ExtensionParameterId])
);

