CREATE TABLE [dbo].[AircraftAircraftStates] (
    [AircraftId]        UNIQUEIDENTIFIER NOT NULL,
    [AircraftState]     INT              NOT NULL,
    [ValidFrom]         DATETIME2 (7)    NOT NULL,
    [ValidTo]           DATETIME2 (7)    NULL,
    [NoticedByPersonId] UNIQUEIDENTIFIER NULL,
    [Remarks]           NVARCHAR (MAX)   NULL,
    [CreatedOn]         DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]   UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]        DATETIME2 (7)    NULL,
    [ModifiedByUserId]  UNIQUEIDENTIFIER NULL,
    [DeletedOn]         DATETIME2 (7)    NULL,
    [DeletedByUserId]   UNIQUEIDENTIFIER NULL,
    [RecordState]       INT              NULL,
    [OwnerId]           UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]     INT              NOT NULL,
    CONSTRAINT [PK_AircraftAircraftStates] PRIMARY KEY CLUSTERED ([AircraftId] ASC, [AircraftState] ASC, [ValidFrom] ASC),
    CONSTRAINT [FK_AircraftAircraftStates_Aircraft] FOREIGN KEY ([AircraftId]) REFERENCES [dbo].[Aircrafts] ([AircraftId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AircraftAircraftStates_AircraftStates] FOREIGN KEY ([AircraftState]) REFERENCES [dbo].[AircraftStates] ([AircraftStateId]) ON DELETE CASCADE,
    CONSTRAINT [FK_AircraftAircraftStates_Persons] FOREIGN KEY ([NoticedByPersonId]) REFERENCES [dbo].[Persons] ([PersonId]) ON DELETE CASCADE
);

