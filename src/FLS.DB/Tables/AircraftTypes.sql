CREATE TABLE [dbo].[AircraftTypes] (
    [AircraftTypeId]      INT            IDENTITY (1, 1) NOT NULL,
    [AircraftTypeName]    NVARCHAR (50)  NOT NULL,
    [Comment]             NVARCHAR (200) NOT NULL,
    [CreatedOn]           DATETIME2 (7)  NOT NULL,
    [ModifiedOn]          DATETIME2 (7)  NULL,
    [HasEngine]           BIT            NULL,
    [RequiresTowingInfo]  BIT            NULL,
    [MayBeTowingAircraft] BIT            NULL,
    CONSTRAINT [PK_AircraftTypes] PRIMARY KEY CLUSTERED ([AircraftTypeId] ASC)
);

