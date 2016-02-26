CREATE TABLE [dbo].[StartTypes] (
    [StartTypeId]        INT            IDENTITY (1, 1) NOT NULL,
    [StartTypeName]      NVARCHAR (100) NOT NULL,
    [IsForGliderFlights] BIT            NOT NULL,
    [IsForTowFlights]    BIT            NOT NULL,
    [IsForMotorFlights]  BIT            NOT NULL,
    [CreatedOn]          DATETIME2 (7)  NOT NULL,
    [ModifiedOn]         DATETIME2 (7)  NULL,
    CONSTRAINT [PK_StartTypes] PRIMARY KEY CLUSTERED ([StartTypeId] ASC)
);

