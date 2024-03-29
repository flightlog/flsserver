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
           (14,1,9,0,0
           ,'1.8.11.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify Persons table'

ALTER TABLE [dbo].[Persons]
	ADD [EnableAddress] [bit] CONSTRAINT [DF__Persons__EnableAddress] DEFAULT((1)) NOT NULL,
	[HasMotorInstructorLicence] [bit] CONSTRAINT [DF__Persons__HasMotorInstrLic] DEFAULT((0)) NOT NULL
GO

PRINT 'Modify PersonClub table'
ALTER TABLE [dbo].[PersonClub]
	ADD [IsMotorInstructor] [bit] CONSTRAINT [DF__PersonClu__IsMotorInstr] DEFAULT((0)) NOT NULL
GO

PRINT 'Modify Flights table'
ALTER TABLE [dbo].[Flights]
	ADD [NoStartTimeInformation] [bit] CONSTRAINT [DF__Flights__NoStartTimeInformation] DEFAULT((0)) NOT NULL,
	[NoLdgTimeInformation] [bit] CONSTRAINT [DF__Flights__NoLdgTimeInformation] DEFAULT((0)) NOT NULL,
	[NrOfLdgsOnStartLocation] [int] NULL
GO

PRINT 'Modify FlightCrew table'
ALTER TABLE [dbo].[FlightCrew]
	ADD [NrOfStarts] [int] NULL
GO


PRINT 'Create CounterUnitTypes table'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CounterUnitTypes](
	[CounterUnitTypeId] [int] IDENTITY(1,1) NOT NULL,
	[CounterUnitTypeName] [nvarchar](50) NOT NULL,
	[CounterUnitTypeKeyName] [nvarchar](50) NOT NULL,
	[Comment] [nvarchar](200) NULL,
	[IsActive] [bit] CONSTRAINT [DF__CounterUnitTypes__IsActive] DEFAULT((1)) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NULL,
 CONSTRAINT [PK_CounterUnitTypes] PRIMARY KEY CLUSTERED 
(
	[CounterUnitTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UNIQUE_CounterUnitTypes_CounterUnitTypeKeyName] UNIQUE NONCLUSTERED 
(
	[CounterUnitTypeKeyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO [dbo].[CounterUnitTypes] ([CounterUnitTypeName], [CounterUnitTypeKeyName],[Comment],[IsActive],[CreatedOn])
     VALUES ('Minutes','Min',NULL,1,SYSUTCDATETIME())
GO

INSERT INTO [dbo].[CounterUnitTypes] ([CounterUnitTypeName], [CounterUnitTypeKeyName],[Comment],[IsActive],[CreatedOn])
     VALUES ('100 minutes per hour','100Min',NULL,1,SYSUTCDATETIME())
GO

INSERT INTO [dbo].[CounterUnitTypes] ([CounterUnitTypeName], [CounterUnitTypeKeyName],[Comment],[IsActive],[CreatedOn])
     VALUES ('Seconds' ,'Sec' ,NULL , 1 ,SYSUTCDATETIME())
GO

INSERT INTO [dbo].[CounterUnitTypes] ([CounterUnitTypeName], [CounterUnitTypeKeyName],[Comment],[IsActive],[CreatedOn])
     VALUES ('1/10 minutes' ,'6Sec' ,NULL , 1 ,SYSUTCDATETIME())
GO

PRINT 'Modify AircraftOperatingCounters table'
ALTER TABLE [dbo].[AircraftOperatingCounters]
	ADD [FlightOperatingCounter] [bigint] NULL,
		[EngineOperatingCounter] [bigint] NULL,
		[NextMaintenanceAtFlightOperatingCounter] [bigint] NULL,
		[NextMaintenanceAtEngineOperatingCounter] [bigint] NULL,
		[FlightOperatingCounterUnitTypeId] [int] NULL,
		[EngineOperatingCounterUnitTypeId] [int] NULL
GO

ALTER TABLE [dbo].[AircraftOperatingCounters]  WITH CHECK ADD  CONSTRAINT [FK_AircraftOperatingCounters_CounterUnitTypes_Flight] FOREIGN KEY([FlightOperatingCounterUnitTypeId])
REFERENCES [dbo].[CounterUnitTypes] ([CounterUnitTypeId])
GO

ALTER TABLE [dbo].[AircraftOperatingCounters] CHECK CONSTRAINT [FK_AircraftOperatingCounters_CounterUnitTypes_Flight]
GO

ALTER TABLE [dbo].[AircraftOperatingCounters]  WITH CHECK ADD  CONSTRAINT [FK_AircraftOperatingCounters_CounterUnitTypes_Engine] FOREIGN KEY([EngineOperatingCounterUnitTypeId])
REFERENCES [dbo].[CounterUnitTypes] ([CounterUnitTypeId])
GO

ALTER TABLE [dbo].[AircraftOperatingCounters] CHECK CONSTRAINT [FK_AircraftOperatingCounters_CounterUnitTypes_Engine]
GO

ALTER TABLE [dbo].[AircraftOperatingCounters]
	DROP COLUMN [FlightOperatingCounterInMinutes]
GO

ALTER TABLE [dbo].[AircraftOperatingCounters]
	DROP COLUMN [EngineOperatingCounterInMinutes]
GO

ALTER TABLE [dbo].[AircraftOperatingCounters]
	DROP COLUMN [NextMaintenanceAtFlightOperatingCounterInMinutes]
GO

ALTER TABLE [dbo].[AircraftOperatingCounters]
	DROP COLUMN [NextMaintenanceAtEngineOperatingCounterInMinutes]
GO

PRINT 'Modify Aircrafts table'
ALTER TABLE [dbo].[Aircrafts]
	ADD [FlightOperatingCounterUnitTypeId] [int] NULL,
		[EngineOperatingCounterUnitTypeId] [int] NULL
GO

ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircrafts_CounterUnitTypes_Flight] FOREIGN KEY([FlightOperatingCounterUnitTypeId])
REFERENCES [dbo].[CounterUnitTypes] ([CounterUnitTypeId])
GO

ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircrafts_CounterUnitTypes_Flight]
GO

ALTER TABLE [dbo].[Aircrafts]  WITH CHECK ADD  CONSTRAINT [FK_Aircrafts_CounterUnitTypes_Engine] FOREIGN KEY([EngineOperatingCounterUnitTypeId])
REFERENCES [dbo].[CounterUnitTypes] ([CounterUnitTypeId])
GO

ALTER TABLE [dbo].[Aircrafts] CHECK CONSTRAINT [FK_Aircrafts_CounterUnitTypes_Engine]
GO

ALTER TABLE [dbo].[Aircrafts] DROP CONSTRAINT [DF_Aircrafts_FlightDurationPrecision]
GO

ALTER TABLE [dbo].[Aircrafts]
	DROP COLUMN [FlightDurationPrecision]
GO



PRINT 'Modify Flights table'
ALTER TABLE [dbo].[Flights]
	ADD [EngineStartOperatingCounter] [bigint] NULL,
		[EngineEndOperatingCounter] [bigint] NULL,
		[EngineOperatingCounterUnitTypeId] [int] NULL
GO

ALTER TABLE [dbo].[Flights]  WITH CHECK ADD  CONSTRAINT [FK_Flights_CounterUnitTypes] FOREIGN KEY([EngineOperatingCounterUnitTypeId])
REFERENCES [dbo].[CounterUnitTypes] ([CounterUnitTypeId])
GO

ALTER TABLE [dbo].[Flights] CHECK CONSTRAINT [FK_Flights_CounterUnitTypes]
GO

DECLARE @CounterUnitTypeId as int
SET @CounterUnitTypeId = (SELECT TOP 1 CounterUnitTypeId FROM CounterUnitTypes where CounterUnitTypeKeyName = 'Min')

UPDATE [dbo].[Flights] SET EngineStartOperatingCounter = EngineStartOperatingCounterInMinutes, EngineEndOperatingCounter = EngineEndOperatingCounterInMinutes, EngineOperatingCounterUnitTypeId = @CounterUnitTypeId

UPDATE [dbo].[Aircrafts] SET [FlightOperatingCounterUnitTypeId] = @CounterUnitTypeId, [EngineOperatingCounterUnitTypeId] = @CounterUnitTypeId
GO

ALTER TABLE [dbo].[Flights]
	DROP COLUMN [EngineStartOperatingCounterInMinutes]
GO

ALTER TABLE [dbo].[Flights]
	DROP COLUMN [EngineEndOperatingCounterInMinutes]
GO

ALTER TABLE [dbo].[Flights]
	DROP COLUMN [EngineTime]
GO


PRINT 'Add new email templates'
INSERT [dbo].[EmailTemplates] ([EmailTemplateId], [ClubId], [EmailTemplateName], [EmailTemplateKeyName], [Description], [FromAddress], [ReplyToAddresses], [Subject], [IsSystemTemplate], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted], [HtmlBody], [TextBody]) 
VALUES (NEWID(), NULL, N'Planning day assignment notification', N'planningday-assignment-notification', N'Notifies the persons who are assigned to a planning day as towpilot, flight operator or instructor, etc. a week before the event.', N'fls@glider-fls.ch', N'noreply@glider-fls.ch', N'Erinnerung für {planningDayInfoModel.Date} in {planningDayInfoModel.LocationName} als {planningDayInfoModel.AssignmentTypeName}', 1, SYSUTCDATETIME(), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0, N'<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
<meta content="en-gb" http-equiv="Content-Language" />
<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
<title>Erinnerung für $PlanningDayInfoModel.Date in $PlanningDayInfoModel.LocationName als $PlanningDayInfoModel.AssignmentTypeName</title>
<style type="text/css">

body {
    font-family: Arial, Helvetica, Sans-Serif;
    font-size: 14px;
}
</style>
</head>

<body>
<p>
Hallo $PlanningDayInfoModel.PersonFirstname
</p>
<p>
Dies ist ein Erinnerungsmail für den $PlanningDayInfoModel.Date in $PlanningDayInfoModel.LocationName. Du bist dann als $PlanningDayInfoModel.AssignmentTypeName eingeteilt.</p>

<p>
Falls in der Zwischenzeit etwas dazwischengekommen ist und du den Einsatz nicht wahrnehmen kannst, dann suche eine Ersatzperson und aktualisiere die Angaben im Flight Logging System.</p>

<p>Herzliche Grüsse</p>
<p>Flight Logging System</p>

</body>

</html>
', NULL)
GO

PRINT 'Finished update to Version 1.9.0'