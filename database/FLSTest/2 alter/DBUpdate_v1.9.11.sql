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
           (25,1,9,11,0
           ,'1.9.10.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Remove MemberNumber unique constraint'
DROP INDEX [UNIQUE_PersonClub_ClubId_MemberNumber] ON [dbo].[PersonClub]
GO
