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
           (42,1,9,28,0
           ,'1.9.27.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify FlightCostBalanceTypes table'
ALTER TABLE [dbo].[FlightCostBalanceTypes] 
	ADD [IsActive] [bit] NOT NULL CONSTRAINT [DF__FlightCostBalanceTypes__IsActive]  DEFAULT ((1))
GO

UPDATE [dbo].[FlightCostBalanceTypes]
   SET [IsActive] = 1
GO

UPDATE [dbo].[FlightCostBalanceTypes]
   SET [IsActive] = 0
   WHERE FlightCostBalanceTypeId in (2,3)
GO

PRINT 'Finished update to Version 1.9.28'