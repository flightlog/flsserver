﻿CREATE TABLE [dbo].[Locations] (
    [LocationId]              UNIQUEIDENTIFIER NOT NULL,
    [LocationName]            NVARCHAR (100)   NOT NULL,
    [LocationShortName]       NVARCHAR (50)    NULL,
    [CountryId]               UNIQUEIDENTIFIER NOT NULL,
    [LocationTypeId]          UNIQUEIDENTIFIER NOT NULL,
    [IcaoCode]                NVARCHAR (10)    NULL,
    [Latitude]                NVARCHAR (10)    NULL,
    [Longitude]               NVARCHAR (10)    NULL,
    [Elevation]               INT              NULL,
    [ElevationUnitType]       INT              DEFAULT ((1)) NULL,
    [RunwayDirection]         NVARCHAR (50)    NULL,
    [RunwayLength]            INT              NULL,
    [RunwayLengthUnitType]    INT              DEFAULT ((1)) NULL,
    [AirportFrequency]        NVARCHAR (50)    NULL,
    [Description]             NVARCHAR (MAX)   NULL,
    [SortIndicator]           INT              NULL,
    [IsFastEntryRecord]       BIT              DEFAULT ((1)) NOT NULL,
    [CreatedOn]               DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]         UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]              DATETIME2 (7)    NULL,
    [ModifiedByUserId]        UNIQUEIDENTIFIER NULL,
    [DeletedOn]               DATETIME2 (7)    NULL,
    [DeletedByUserId]         UNIQUEIDENTIFIER NULL,
    [RecordState]             INT              NULL,
    [OwnerId]                 UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]           INT              NOT NULL,
    [IsDeleted]               BIT              DEFAULT ((0)) NOT NULL,
    [IsInboundRouteRequired]  BIT              DEFAULT ((0)) NOT NULL,
    [IsOutboundRouteRequired] BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED ([LocationId] ASC),
    CONSTRAINT [FK_Locations_Countries] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Countries] ([CountryId]),
    CONSTRAINT [FK_Locations_ElevationUnitTypes] FOREIGN KEY ([ElevationUnitType]) REFERENCES [dbo].[ElevationUnitTypes] ([ElevationUnitTypeId]),
    CONSTRAINT [FK_Locations_LengthUnitTypes] FOREIGN KEY ([RunwayLengthUnitType]) REFERENCES [dbo].[LengthUnitTypes] ([LengthUnitTypeId]),
    CONSTRAINT [FK_Locations_LocationTypes] FOREIGN KEY ([LocationTypeId]) REFERENCES [dbo].[LocationTypes] ([LocationTypeId]),
    CONSTRAINT [UNIQUE_Locations_LocationName] UNIQUE NONCLUSTERED ([LocationName] ASC, [CountryId] ASC, [LocationTypeId] ASC, [DeletedOn] ASC)
);

