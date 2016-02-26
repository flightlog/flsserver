CREATE TABLE [dbo].[Languages] (
    [LanguageId]   INT            IDENTITY (1, 1) NOT NULL,
    [LanguageKey]  NVARCHAR (5)   NOT NULL,
    [LanguageName] NVARCHAR (50)  NOT NULL,
    [Remarks]      NVARCHAR (MAX) NULL,
    [CreatedOn]    DATETIME2 (7)  NOT NULL,
    [ModifiedOn]   DATETIME2 (7)  NULL,
    CONSTRAINT [PK_dbo.Languages] PRIMARY KEY CLUSTERED ([LanguageId] ASC)
);

