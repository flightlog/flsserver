CREATE TABLE [dbo].[ClubExtensions] (
    [ClubId]           UNIQUEIDENTIFIER NOT NULL,
    [ExtensionId]      UNIQUEIDENTIFIER NOT NULL,
    [IsActive]         BIT              NOT NULL,
    [CreatedOn]        DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]  UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]       DATETIME2 (7)    NULL,
    [ModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [DeletedOn]        DATETIME2 (7)    NULL,
    [DeletedByUserId]  UNIQUEIDENTIFIER NULL,
    [RecordState]      INT              NULL,
    [OwnerId]          UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]    INT              NOT NULL,
    [IsDeleted]        BIT              CONSTRAINT [DF_ClubExtensions_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ClubExtensions] PRIMARY KEY CLUSTERED ([ClubId] ASC, [ExtensionId] ASC),
    CONSTRAINT [FK_ClubExtensions_Clubs] FOREIGN KEY ([ClubId]) REFERENCES [dbo].[Clubs] ([ClubId]),
    CONSTRAINT [FK_ClubExtensions_Extensions] FOREIGN KEY ([ExtensionId]) REFERENCES [dbo].[Extensions] ([ExtensionId])
);

