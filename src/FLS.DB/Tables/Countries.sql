CREATE TABLE [dbo].[Countries] (
    [CountryId]        UNIQUEIDENTIFIER NOT NULL,
    [CountryIdIso]     INT              NOT NULL,
    [CountryName]      NVARCHAR (100)   NOT NULL,
    [CountryFullName]  NVARCHAR (250)   NULL,
    [CountryCodeIso2]  VARCHAR (2)      NOT NULL,
    [CreatedOn]        DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]  UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]       DATETIME2 (7)    NULL,
    [ModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [DeletedOn]        DATETIME2 (7)    NULL,
    [DeletedByUserId]  UNIQUEIDENTIFIER NULL,
    [RecordState]      INT              NULL,
    [OwnerId]          UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]    INT              NOT NULL,
    [IsDeleted]        BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED ([CountryId] ASC),
    CONSTRAINT [UNIQUE_Countries_CountryCodeIso2] UNIQUE NONCLUSTERED ([CountryCodeIso2] ASC, [DeletedOn] ASC),
    CONSTRAINT [UNIQUE_Countries_CountryIdIso] UNIQUE NONCLUSTERED ([CountryIdIso] ASC, [DeletedOn] ASC),
    CONSTRAINT [UNIQUE_Countries_CountryName] UNIQUE NONCLUSTERED ([CountryName] ASC, [DeletedOn] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Country code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Countries', @level2type = N'COLUMN', @level2name = N'CountryCodeIso2';

