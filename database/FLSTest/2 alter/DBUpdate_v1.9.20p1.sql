USE [FLSTest]
GO

PRINT 'Update FlightAircraftType from 3 to 4'
UPDATE [dbo].[Flights] SET [FlightAircraftType] = 4
  WHERE [FlightAircraftType] = 3


PRINT 'Finished update to Version 1.9.20p1'