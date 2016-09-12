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
           (16,1,9,2,0
           ,'1.9.1.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify PersonClub table'

/****** Object:  Index [UNIQUE_PersonClub_PersonId_ClubId_MemberNumber]    Script Date: 12.09.2016 21:36:44 ******/
ALTER TABLE [dbo].[PersonClub] DROP CONSTRAINT [UNIQUE_PersonClub_PersonId_ClubId_MemberNumber]
GO

CREATE UNIQUE INDEX UNIQUE_PersonClub_ClubId_MemberNumber ON [dbo].[PersonClub](ClubId, MemberNumber) 
WHERE MemberNumber IS NOT NULL
GO

UPDATE [dbo].[FlightCostBalanceTypes]
   SET [IsForGliderFlights] = 0
      ,[IsForTowFlights] = 0
      ,[IsForMotorFlights] = 0
      ,[ModifiedOn] = SYSUTCDATETIME()
 WHERE [FlightCostBalanceTypeId] in (2,3)
GO

PRINT 'Finished update to Version 1.9.2'