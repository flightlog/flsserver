CREATE TABLE [dbo].[AircraftReservationTypes] (
    [AircraftReservationTypeId]   INT            IDENTITY (1, 1) NOT NULL,
    [AircraftReservationTypeName] NVARCHAR (50)  NOT NULL,
    [Remarks]                     NVARCHAR (MAX) NULL,
    [CreatedOn]                   DATETIME2 (7)  NOT NULL,
    [ModifiedOn]                  DATETIME2 (7)  NULL,
    [IsInstructorRequired]        BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.AircraftReservationTypes] PRIMARY KEY CLUSTERED ([AircraftReservationTypeId] ASC)
);

