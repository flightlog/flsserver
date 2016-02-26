CREATE TABLE [dbo].[LocationTypes] (
    [LocationTypeId]    UNIQUEIDENTIFIER NOT NULL,
    [LocationTypeName]  NVARCHAR (50)    NOT NULL,
    [LocationTypeCupId] INT              NULL,
    [IsAirfield]        BIT              NOT NULL,
    [CreatedOn]         DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]   UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]        DATETIME2 (7)    NULL,
    [ModifiedByUserId]  UNIQUEIDENTIFIER NULL,
    [DeletedOn]         DATETIME2 (7)    NULL,
    [DeletedByUserId]   UNIQUEIDENTIFIER NULL,
    [RecordState]       INT              NULL,
    [OwnerId]           UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]     INT              NOT NULL,
    [IsDeleted]         BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LocationTypes] PRIMARY KEY CLUSTERED ([LocationTypeId] ASC)
);

