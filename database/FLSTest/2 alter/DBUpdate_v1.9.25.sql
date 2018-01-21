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
           (39,1,9,25,0
           ,'1.9.24.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify FlightTypes table'
ALTER TABLE [dbo].[FlightTypes]  WITH CHECK ADD  CONSTRAINT [CK_FlightTypes_InstructorRequiredXORObserverPilotRequired] CHECK  (([InstructorRequired]=(0) AND [ObserverPilotOrInstructorRequired]=(0) OR [InstructorRequired]=(0) AND [ObserverPilotOrInstructorRequired]=(1) OR [InstructorRequired]=(1) AND [ObserverPilotOrInstructorRequired]=(0)))
GO

ALTER TABLE [dbo].[FlightTypes] CHECK CONSTRAINT [CK_FlightTypes_InstructorRequiredXORObserverPilotRequired]
GO

PRINT 'Modify Persons table'
ALTER TABLE [dbo].[Persons] 
	ADD [MotorInstructorLicenceExpireDate] [datetime2](7) NULL,
	[HasPartMLicence] [bit] NOT NULL CONSTRAINT [DF__Persons__HasPartMLicence]  DEFAULT ((0)),
	[PartMLicenceExpireDate] [datetime2](7) NULL
GO

PRINT 'Finished update to Version 1.9.25'