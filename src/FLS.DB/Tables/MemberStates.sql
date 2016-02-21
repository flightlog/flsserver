CREATE TABLE [dbo].[MemberStates] (
    [MemberStateId]    UNIQUEIDENTIFIER CONSTRAINT [DF_MemberStates_MemberStateId] DEFAULT (newid()) NOT NULL,
    [ClubId]           UNIQUEIDENTIFIER NOT NULL,
    [MemberStateName]  NVARCHAR (50)    NOT NULL,
    [Remarks]          NVARCHAR (250)   NULL,
    [CreatedOn]        DATETIME2 (7)    NOT NULL,
    [CreatedByUserId]  UNIQUEIDENTIFIER NOT NULL,
    [ModifiedOn]       DATETIME2 (7)    NULL,
    [ModifiedByUserId] UNIQUEIDENTIFIER NULL,
    [DeletedOn]        DATETIME2 (7)    NULL,
    [DeletedByUserId]  UNIQUEIDENTIFIER NULL,
    [RecordState]      INT              NULL,
    [OwnerId]          UNIQUEIDENTIFIER NOT NULL,
    [OwnershipType]    INT              NOT NULL,
    [IsDeleted]        BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MemberStates] PRIMARY KEY CLUSTERED ([MemberStateId] ASC),
    CONSTRAINT [FK_MemberStates_Club] FOREIGN KEY ([ClubId]) REFERENCES [dbo].[Clubs] ([ClubId]),
    CONSTRAINT [UNIQUE_MemberStates_MemberStateName] UNIQUE NONCLUSTERED ([MemberStateName] ASC, [ClubId] ASC, [DeletedOn] ASC)
);

