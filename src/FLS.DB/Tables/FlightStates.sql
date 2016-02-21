CREATE TABLE [dbo].[FlightStates] (
    [FlightStateId]   INT            IDENTITY (1, 1) NOT NULL,
    [FlightStateName] NVARCHAR (50)  NOT NULL,
    [Comment]         NVARCHAR (200) NULL,
    [CreatedOn]       DATETIME2 (7)  NOT NULL,
    [ModifiedOn]      DATETIME2 (7)  NULL,
    CONSTRAINT [PK_FlightStates] PRIMARY KEY CLUSTERED ([FlightStateId] ASC)
);

