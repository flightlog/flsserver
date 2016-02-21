CREATE TABLE [dbo].[EmailTemplates] (
    [EmailTemplateId]      UNIQUEIDENTIFIER NOT NULL,
    [ClubId]               UNIQUEIDENTIFIER NULL,
    [EmailTemplateName]    NVARCHAR (100)   NOT NULL,
    [EmailTemplateKeyName] NVARCHAR (100)   NOT NULL,
    [Description]          NVARCHAR (MAX)   NULL,
    [FromAddress]          NVARCHAR (256)   NOT NULL,
    [ReplyToAddresses]     NVARCHAR (256)   NULL,
    [Subject]              NVARCHAR (256)   NOT NULL,
    [IsSystemTemplate]     BIT              DEFAULT ((0)) NOT NULL,
    [CreatedOn]            DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]      UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]           DATETIME2 (7)    NULL,
    [ModifiedByUserId]     UNIQUEIDENTIFIER NULL,
    [DeletedOn]            DATETIME2 (7)    NULL,
    [DeletedByUserId]      UNIQUEIDENTIFIER NULL,
    [RecordState]          INT              NULL,
    [OwnerId]              UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]        INT              NOT NULL,
    [IsDeleted]            BIT              NOT NULL,
    [HtmlBody]             NVARCHAR (MAX)   NULL,
    [TextBody]             NVARCHAR (MAX)   NULL,
    [IsCustomizable]       BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_EmailTemplates] PRIMARY KEY CLUSTERED ([EmailTemplateId] ASC),
    CONSTRAINT [FK_EmailTemplates_Club] FOREIGN KEY ([ClubId]) REFERENCES [dbo].[Clubs] ([ClubId])
);


GO
CREATE NONCLUSTERED INDEX [IX_EmailTemplatesKeyName]
    ON [dbo].[EmailTemplates]([EmailTemplateKeyName] ASC);

