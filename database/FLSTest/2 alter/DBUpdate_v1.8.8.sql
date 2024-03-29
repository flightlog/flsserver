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
           (10,1,8,8,0
           ,'1.8.7.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify club table part'

ALTER TABLE [dbo].[Clubs]
	ADD [ClubStateId] [int] NULL
GO

/****** Object:  Table [dbo].[ClubStates]    Script Date: 12.02.2016 19:14:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ClubStates](
	[ClubStateId] [int] IDENTITY(1,1) NOT NULL,
	[ClubStateName] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](200) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_ClubStates] PRIMARY KEY CLUSTERED 
(
	[ClubStateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Clubs]  WITH CHECK ADD  CONSTRAINT [FK_Clubs_ClubStates] FOREIGN KEY([ClubStateId])
REFERENCES [dbo].[ClubStates] ([ClubStateId])
GO

ALTER TABLE [dbo].[Clubs] CHECK CONSTRAINT [FK_Clubs_ClubStates]
GO


SET IDENTITY_INSERT [ClubStates] ON
INSERT INTO [dbo].[ClubStates] ([ClubStateId],[ClubStateName],[Comment],[CreatedOn])
     VALUES
           (0, 'System','System used club (will not be shown in club entities)', SYSUTCDATETIME())
INSERT INTO [dbo].[ClubStates] ([ClubStateId],[ClubStateName],[Comment],[CreatedOn])
     VALUES
           (1, 'Active club tenant','Active club tenant with users', SYSUTCDATETIME())
INSERT INTO [dbo].[ClubStates] ([ClubStateId],[ClubStateName],[Comment],[CreatedOn])
     VALUES
           (2, 'Passiv club','Club without tenant activities and no users (just information about the club)', SYSUTCDATETIME())
INSERT INTO [dbo].[ClubStates] ([ClubStateId],[ClubStateName],[Comment],[CreatedOn])
     VALUES
           (3, 'Inactive club','Club tenant which was active before', SYSUTCDATETIME())
SET IDENTITY_INSERT [ClubStates] OFF
GO

UPDATE [dbo].[Clubs] SET ClubStateId = 0 where ClubKey = 'SystemClub'
UPDATE [dbo].[Clubs] SET ClubStateId = 1 where ClubStateId IS NULL
GO

ALTER TABLE [dbo].[Clubs] ALTER COLUMN [ClubStateId] [int] NOT NULL
GO

PRINT 'Finished update to Version 1.8.8'