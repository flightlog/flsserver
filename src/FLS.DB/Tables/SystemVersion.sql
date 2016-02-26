CREATE TABLE [dbo].[SystemVersion] (
    [VersionId]          BIGINT        NOT NULL,
    [MajorVersion]       BIGINT        NOT NULL,
    [MinorVersion]       BIGINT        NOT NULL,
    [BuildVersion]       BIGINT        NOT NULL,
    [RevisionVersion]    BIGINT        NOT NULL,
    [UpgradeFromVersion] NVARCHAR (50) NULL,
    [UpgradeDateTime]    DATETIME2 (7) NULL,
    CONSTRAINT [PK_SystemVersion] PRIMARY KEY CLUSTERED ([VersionId] ASC)
);

