CREATE TABLE [dbo].[InOutboundPoints] (
    [InOutboundPointId]          UNIQUEIDENTIFIER NOT NULL,
    [LocationId]                 UNIQUEIDENTIFIER NOT NULL,
    [InOutboundPointName]        NVARCHAR (50)    NOT NULL,
    [IsInboundPoint]             BIT              NOT NULL,
    [IsOutboundPoint]            BIT              NOT NULL,
    [SortIndicatorInboundPoint]  INT              NOT NULL,
    [SortIndicatorOutboundPoint] INT              NOT NULL,
    [CreatedOn]                  DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]            UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]                 DATETIME2 (7)    NULL,
    [ModifiedByUserId]           UNIQUEIDENTIFIER NULL,
    [DeletedOn]                  DATETIME2 (7)    NULL,
    [DeletedByUserId]            UNIQUEIDENTIFIER NULL,
    [RecordState]                INT              NULL,
    [OwnerId]                    UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]              INT              NOT NULL,
    [IsDeleted]                  BIT              CONSTRAINT [DF_InOutboundPoints_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_InOutboundPoints] PRIMARY KEY CLUSTERED ([InOutboundPointId] ASC),
    CONSTRAINT [FK_InOutboundPoints_Locations] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Locations] ([LocationId])
);

