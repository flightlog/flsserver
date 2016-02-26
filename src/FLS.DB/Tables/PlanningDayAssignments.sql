CREATE TABLE [dbo].[PlanningDayAssignments] (
    [PlanningDayAssignmentId] UNIQUEIDENTIFIER NOT NULL,
    [AssignedPlanningDayId]   UNIQUEIDENTIFIER NOT NULL,
    [AssignedPersonId]        UNIQUEIDENTIFIER NOT NULL,
    [AssignmentTypeId]        UNIQUEIDENTIFIER NOT NULL,
    [Remarks]                 NVARCHAR (MAX)   NULL,
    [CreatedOn]               DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]         UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]              DATETIME2 (7)    NULL,
    [ModifiedByUserId]        UNIQUEIDENTIFIER NULL,
    [DeletedOn]               DATETIME2 (7)    NULL,
    [DeletedByUserId]         UNIQUEIDENTIFIER NULL,
    [RecordState]             INT              NULL,
    [OwnerId]                 UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]           INT              NOT NULL,
    [IsDeleted]               BIT              NOT NULL,
    CONSTRAINT [PK_dbo.PlanningDayAssignments] PRIMARY KEY CLUSTERED ([PlanningDayAssignmentId] ASC),
    CONSTRAINT [FK_dbo.PlanningDayAssignments_dbo.Persons_AssignedPersonId] FOREIGN KEY ([AssignedPersonId]) REFERENCES [dbo].[Persons] ([PersonId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.PlanningDayAssignments_dbo.PlanningDayAssignmentTypes_AssignmentTypeId] FOREIGN KEY ([AssignmentTypeId]) REFERENCES [dbo].[PlanningDayAssignmentTypes] ([PlanningDayAssignmentTypeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.PlanningDayAssignments_dbo.PlanningDays_AssignedPlanningDayId] FOREIGN KEY ([AssignedPlanningDayId]) REFERENCES [dbo].[PlanningDays] ([PlanningDayId]) ON DELETE CASCADE
);

