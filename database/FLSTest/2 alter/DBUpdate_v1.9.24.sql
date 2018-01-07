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
           (38,1,9,24,0
           ,'1.9.23.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify Deliveries table'
ALTER TABLE [dbo].[Deliveries] 
	ADD [IncludesTowFlightId] [uniqueidentifier] NULL
GO

ALTER TABLE [dbo].[Deliveries]  WITH CHECK ADD  CONSTRAINT [FK_Deliveries_IncludesTowFlight] FOREIGN KEY([IncludesTowFlightId])
REFERENCES [dbo].[Flights] ([FlightId])
GO

ALTER TABLE [dbo].[Deliveries] CHECK CONSTRAINT [FK_Deliveries_IncludesTowFlight]
GO

PRINT 'Update Process States'
INSERT [dbo].[FlightProcessStates] ([FlightProcessStateId], [FlightProcessStateName], [Comment], [CreatedOn]) 
	VALUES (45, N'Lieferschein-Fehler', N'Delivery seems to be incorrect (no items)', SYSUTCDATETIME())
GO
INSERT [dbo].[FlightProcessStates] ([FlightProcessStateId], [FlightProcessStateName], [Comment], [CreatedOn]) 
	VALUES (60, N'Lieferschein verbucht', N'Delivery is further processed external and is booked.', SYSUTCDATETIME())
GO
INSERT [dbo].[FlightProcessStates] ([FlightProcessStateId], [FlightProcessStateName], [Comment], [CreatedOn]) 
	VALUES (99, N'Ausschluss von Lieferschein-Prozess', N'No delivery will be created for this flight.', SYSUTCDATETIME())
GO

UPDATE [dbo].[FlightProcessStates]
   SET [Comment] = 'Delivery for flight created',
		[ModifiedOn] = SYSUTCDATETIME()
 WHERE [FlightProcessStateId] = 50

PRINT 'Finished update to Version 1.9.24'