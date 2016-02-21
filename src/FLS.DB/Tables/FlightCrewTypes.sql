CREATE TABLE [dbo].[FlightCrewTypes] (
    [FlightCrewTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [FlightCrewTypeName] NVARCHAR (50)  NOT NULL,
    [Comment]            NVARCHAR (200) NULL,
    [CreatedOn]          DATETIME2 (7)  NOT NULL,
    [ModifiedOn]         DATETIME2 (7)  NULL,
    CONSTRAINT [PK_FlightCrewTypes] PRIMARY KEY CLUSTERED ([FlightCrewTypeId] ASC)
);

