CREATE TABLE [dbo].[LengthUnitTypes] (
    [LengthUnitTypeId]        INT            IDENTITY (1, 1) NOT NULL,
    [LengthUnitTypeName]      NVARCHAR (50)  NOT NULL,
    [LengthUnitTypeKeyName]   NVARCHAR (50)  NOT NULL,
    [LengthUnitTypeShortName] NVARCHAR (20)  NULL,
    [Comment]                 NVARCHAR (200) NULL,
    [CreatedOn]               DATETIME2 (7)  NOT NULL,
    [ModifiedOn]              DATETIME2 (7)  NULL,
    CONSTRAINT [PK_LengthUnitTypes] PRIMARY KEY CLUSTERED ([LengthUnitTypeId] ASC),
    CONSTRAINT [UNIQUE_LengthUnitTypes_LengthUnitTypeKeyName] UNIQUE NONCLUSTERED ([LengthUnitTypeKeyName] ASC)
);

