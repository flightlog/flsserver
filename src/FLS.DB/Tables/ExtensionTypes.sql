CREATE TABLE [dbo].[ExtensionTypes] (
    [ExtensionTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [ExtensionTypeName] NVARCHAR (50)  NOT NULL,
    [Comment]           NVARCHAR (MAX) NULL,
    [CreatedOn]         DATETIME2 (7)  NOT NULL,
    [ModifiedOn]        DATETIME2 (7)  NULL,
    CONSTRAINT [PK_ExtensionTypes_1] PRIMARY KEY CLUSTERED ([ExtensionTypeId] ASC)
);

