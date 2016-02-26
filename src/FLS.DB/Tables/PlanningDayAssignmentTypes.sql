CREATE TABLE [dbo].[PlanningDayAssignmentTypes] (
    [PlanningDayAssignmentTypeId]        UNIQUEIDENTIFIER NOT NULL,
    [AssignmentTypeName]                 NVARCHAR (MAX)   NULL,
    [ClubId]                             UNIQUEIDENTIFIER NOT NULL,
    [RequiredNrOfPlanningDayAssignments] INT              NOT NULL,
    [CreatedOn]                          DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]                    UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]                         DATETIME2 (7)    NULL,
    [ModifiedByUserId]                   UNIQUEIDENTIFIER NULL,
    [DeletedOn]                          DATETIME2 (7)    NULL,
    [DeletedByUserId]                    UNIQUEIDENTIFIER NULL,
    [RecordState]                        INT              NULL,
    [OwnerId]                            UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]                      INT              NOT NULL,
    [IsDeleted]                          BIT              NOT NULL,
    CONSTRAINT [PK_dbo.PlanningDayAssignmentTypes] PRIMARY KEY CLUSTERED ([PlanningDayAssignmentTypeId] ASC),
    CONSTRAINT [FK_dbo.PlanningDayAssignmentTypes_dbo.Clubs_ClubId] FOREIGN KEY ([ClubId]) REFERENCES [dbo].[Clubs] ([ClubId])
);

