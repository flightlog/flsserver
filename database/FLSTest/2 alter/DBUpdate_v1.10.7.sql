USE [FLSTest]
GO

PRINT 'Update SystemVersion Information'
INSERT INTO [dbo].[SystemVersion]
           ([VersionId]
		   ,[MajorVersion]
           ,[MinorVersion]
           ,[BuildVersion]
           ,[RevisionVersion]
           ,[UpgradeFromVersion]
           ,[UpgradeDateTime])
     VALUES
           (46,1,10,7,0
           ,'1.10.6.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Update club table'
UPDATE [dbo].[Clubs] 
	SET [OwnerId] = [ClubId]
GO

PRINT 'Modify club table'
ALTER TABLE [dbo].[Clubs]
	ADD [IsClubMemberNumberReadonly] [bit] NOT NULL CONSTRAINT [DF__Clubs__IsClubMemberNumberReadonly]  DEFAULT ((1)),
	[ReplyToEmailAddress] [nvarchar](250) NULL
GO

UPDATE [dbo].[Clubs] 
	SET [ReplyToEmailAddress] = (SELECT TOP 1 [SystemSenderEmailAddress] FROM [dbo].[SystemData])
GO
PRINT 'Finished update to Version 1.10.7'