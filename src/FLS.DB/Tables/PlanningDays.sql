CREATE TABLE [dbo].[PlanningDays] (
    [PlanningDayId]    UNIQUEIDENTIFIER NOT NULL,
    [ClubId]           UNIQUEIDENTIFIER NOT NULL,
    [Day]              DATE             NOT NULL,
    [LocationId]       UNIQUEIDENTIFIER NOT NULL,
    [Remarks]          NVARCHAR (MAX)   NULL,
    [CreatedOn]        DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]  UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]       DATETIME2 (7)    NULL,
    [ModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [DeletedOn]        DATETIME2 (7)    NULL,
    [DeletedByUserId]  UNIQUEIDENTIFIER NULL,
    [RecordState]      INT              NULL,
    [OwnerId]          UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]    INT              NOT NULL,
    [IsDeleted]        BIT              NOT NULL,
    CONSTRAINT [PK_dbo.PlanningDays] PRIMARY KEY CLUSTERED ([PlanningDayId] ASC),
    CONSTRAINT [FK_dbo.PlanningDays_dbo.Clubs_ClubId] FOREIGN KEY ([ClubId]) REFERENCES [dbo].[Clubs] ([ClubId]),
    CONSTRAINT [FK_dbo.PlanningDays_dbo.Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Locations] ([LocationId]) ON DELETE CASCADE
);

