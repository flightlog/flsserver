CREATE TABLE [dbo].[PersonCategories] (
    [PersonCategoryId]       UNIQUEIDENTIFIER NOT NULL,
    [ClubId]                 UNIQUEIDENTIFIER NOT NULL,
    [CategoryName]           NVARCHAR (100)   NOT NULL,
    [Remarks]                NVARCHAR (250)   NULL,
    [ParentPersonCategoryId] UNIQUEIDENTIFIER NULL,
    [CreatedOn]              DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]        UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]             DATETIME2 (7)    NULL,
    [ModifiedByUserId]       UNIQUEIDENTIFIER NULL,
    [DeletedOn]              DATETIME2 (7)    NULL,
    [DeletedByUserId]        UNIQUEIDENTIFIER NULL,
    [RecordState]            INT              NULL,
    [OwnerId]                UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]          INT              NOT NULL,
    [IsDeleted]              BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PersonCategories] PRIMARY KEY CLUSTERED ([PersonCategoryId] ASC),
    CONSTRAINT [FK_PersonCategories_Club] FOREIGN KEY ([ClubId]) REFERENCES [dbo].[Clubs] ([ClubId]),
    CONSTRAINT [FK_PersonCategories_PersonCategories] FOREIGN KEY ([ParentPersonCategoryId]) REFERENCES [dbo].[PersonCategories] ([PersonCategoryId]),
    CONSTRAINT [UNIQUE_PersonCategories_ClubId_CategoryName_ParentId] UNIQUE NONCLUSTERED ([ClubId] ASC, [CategoryName] ASC, [ParentPersonCategoryId] ASC, [DeletedOn] ASC)
);

