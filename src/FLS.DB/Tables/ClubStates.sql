CREATE TABLE [dbo].[ClubStates] (
    [ClubStateId]   INT            IDENTITY (1, 1) NOT NULL,
    [ClubStateName] NVARCHAR (50)  NOT NULL,
    [Comment]       NVARCHAR (200) NULL,
    [CreatedOn]     DATETIME2 (7)  NOT NULL,
    [ModifiedOn]    DATETIME2 (7)  NULL,
    CONSTRAINT [PK_ClubStates] PRIMARY KEY CLUSTERED ([ClubStateId] ASC)
);

