CREATE TABLE [dbo].[AircraftStates] (
    [AircraftStateId]   INT           IDENTITY (1, 1) NOT NULL,
    [AircraftStateName] NVARCHAR (50) NOT NULL,
    [IsAircraftFlyable] BIT           NOT NULL,
    [CreatedOn]         DATETIME2 (7) NOT NULL,
    [ModifiedOn]        DATETIME2 (7) NULL,
    CONSTRAINT [PK_AircraftStates] PRIMARY KEY CLUSTERED ([AircraftStateId] ASC)
);

