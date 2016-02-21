CREATE TABLE [dbo].[LanguageTranslations] (
    [LanguageTranslationId] UNIQUEIDENTIFIER NOT NULL,
    [TranslationKey]        NVARCHAR (250)   NOT NULL,
    [TranslationValue]      NVARCHAR (MAX)   NOT NULL,
    [LanguageId]            INT              NOT NULL,
    [CreatedOn]             DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]       UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]            DATETIME2 (7)    NULL,
    [ModifiedByUserId]      UNIQUEIDENTIFIER NULL,
    [DeletedOn]             DATETIME2 (7)    NULL,
    [DeletedByUserId]       UNIQUEIDENTIFIER NULL,
    [RecordState]           INT              NULL,
    [OwnerId]               UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]         INT              NOT NULL,
    [IsDeleted]             BIT              NOT NULL,
    CONSTRAINT [PK_dbo.LanguageTranslations] PRIMARY KEY CLUSTERED ([LanguageTranslationId] ASC),
    CONSTRAINT [FK_dbo.LanguageTranslations_dbo.Languages_LanguageId] FOREIGN KEY ([LanguageId]) REFERENCES [dbo].[Languages] ([LanguageId])
);

