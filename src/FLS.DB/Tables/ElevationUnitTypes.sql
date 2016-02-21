CREATE TABLE [dbo].[ElevationUnitTypes] (
    [ElevationUnitTypeId]        INT            IDENTITY (1, 1) NOT NULL,
    [ElevationUnitTypeName]      NVARCHAR (50)  NOT NULL,
    [ElevationUnitTypeKeyName]   NVARCHAR (50)  NOT NULL,
    [ElevationUnitTypeShortName] NVARCHAR (20)  NULL,
    [Comment]                    NVARCHAR (200) NULL,
    [CreatedOn]                  DATETIME2 (7)  NOT NULL,
    [ModifiedOn]                 DATETIME2 (7)  NULL,
    CONSTRAINT [PK_ElevationUnitTypes] PRIMARY KEY CLUSTERED ([ElevationUnitTypeId] ASC),
    CONSTRAINT [UNIQUE_ElevationUnitTypes_ElevationUnitTypeKeyName] UNIQUE NONCLUSTERED ([ElevationUnitTypeKeyName] ASC)
);

