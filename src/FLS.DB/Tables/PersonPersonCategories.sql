CREATE TABLE [dbo].[PersonPersonCategories] (
    [PersonId]         UNIQUEIDENTIFIER NOT NULL,
    [PersonCategoryId] UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]        DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]  UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]       DATETIME2 (7)    NULL,
    [ModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [DeletedOn]        DATETIME2 (7)    NULL,
    [DeletedByUserId]  UNIQUEIDENTIFIER NULL,
    [RecordState]      INT              NULL,
    [OwnerId]          UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]    INT              NOT NULL,
    CONSTRAINT [PK_PersonPersonCategories] PRIMARY KEY CLUSTERED ([PersonId] ASC, [PersonCategoryId] ASC),
    CONSTRAINT [FK_PersonPersonCategories_PersonCategories] FOREIGN KEY ([PersonCategoryId]) REFERENCES [dbo].[PersonCategories] ([PersonCategoryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_PersonPersonCategories_Persons] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[Persons] ([PersonId]) ON DELETE CASCADE
);

