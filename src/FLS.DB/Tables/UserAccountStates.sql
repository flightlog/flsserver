CREATE TABLE [dbo].[UserAccountStates] (
    [UserAccountStateId]   INT            IDENTITY (1, 1) NOT NULL,
    [UserAccountStateName] NVARCHAR (50)  NOT NULL,
    [Comment]              NVARCHAR (200) NOT NULL,
    [CreatedOn]            DATETIME2 (7)  NOT NULL,
    [ModifiedOn]           DATETIME2 (7)  NULL,
    CONSTRAINT [PK_UserAccountStates] PRIMARY KEY CLUSTERED ([UserAccountStateId] ASC)
);

