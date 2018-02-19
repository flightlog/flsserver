USE [FLSTest]
GO

PRINT 'Update Process States of flights'
UPDATE [dbo].[Flights]
	SET [ProcessStateId] = 60
	WHERE [FlightId] in ((SELECT FlightId FROM [dbo].[Deliveries] WHERE IsDeleted = 0 AND IsFurtherProcessed = 1))
GO

UPDATE [dbo].[Flights]
	SET [ProcessStateId] = 60
	WHERE [FlightId] in (SELECT TowFlightId FROM [dbo].[Flights] WHERE FlightId in (SELECT FlightId FROM [dbo].[Deliveries] WHERE IsDeleted = 0 AND IsFurtherProcessed = 1) AND TowFlightId IS NOT NULL)
GO

PRINT 'Finished update to Version 1.9.24p1'