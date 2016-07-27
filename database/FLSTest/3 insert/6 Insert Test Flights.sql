		   
USE [FLSTest]
GO

-- config
DECLARE @insertUserId as uniqueidentifier
DECLARE @clubUserId as uniqueidentifier
DECLARE @clubFlightOperatorId as uniqueidentifier
DECLARE @systemAdminRoleId as uniqueidentifier
DECLARE @clientAdminRoleId as uniqueidentifier
DECLARE @flightOperatorRoleId as uniqueidentifier
DECLARE @ownerId as uniqueidentifier
DECLARE @locationTypeId as uniqueidentifier
DECLARE @memberStateId as uniqueidentifier
DECLARE @recordState as bigint
DECLARE @insertClubId as uniqueidentifier
DECLARE @clubEmailAddress as nvarchar
DECLARE @flightId as uniqueidentifier
DECLARE @personId as uniqueidentifier
DECLARE @countryId as uniqueidentifier
DECLARE @aircraftId as uniqueidentifier
DECLARE @towingAircraftId as uniqueidentifier
DECLARE @locationId as uniqueidentifier
DECLARE @flightTypeId as uniqueidentifier
DECLARE @towFlightTypeId as uniqueidentifier
DECLARE @startTypeId as int
DECLARE @TowFlightId as uniqueidentifier
DECLARE @Today DATETIME 
DECLARE @startDatetime DATETIME 
DECLARE @flightAirState as int
DECLARE @flightCrewType as int
DECLARE @flightAircraftType as int
DECLARE @OwnershipType as bigint

SET @OwnershipType = 2 --Club
SET @insertUserId = (SELECT TOP 1 userId FROM Users where Username = 'testclubuser')
SET @systemAdminRoleId = '56352545-2454-3453-2343-C742446534512'
SET @clientAdminRoleId = '92750A21-9BCD-FFFF-2343-23B44724019B'
SET @flightOperatorRoleId = '187A8729-92BC-2932-AC83-15F14724019B'
SET @recordState = 1
SET @clubEmailAddress = 'schuele@galaxy-net.ch'
SET @countryId = '77CC3BE6-95DB-11E0-B104-E7F04724019B'

-- Test-Club
SET @insertClubId = (SELECT TOP 1 ClubId FROM Clubs where ClubKey = 'TestClub')

SET @ownerId = @insertClubId 

SET @aircraftId = (SELECT TOP 1 aircraftId FROM Aircrafts where Immatriculation = 'HB-3407')
SET @towingAircraftId = (SELECT TOP 1 aircraftId FROM Aircrafts where Immatriculation = 'HB-KCB')
SET @locationId = (SELECT TOP 1 LocationId FROM Locations WHERE IcaoCode = 'LSZK')
SET @flightTypeId = (SELECT TOP 1  FlightTypeId FROM FlightTypes WHERE FlightCode = '63' AND ClubId = @insertClubId)
SET @towFlightTypeId = (SELECT TOP 1  FlightTypeId FROM FlightTypes WHERE FlightCode = '90' AND ClubId = @insertClubId)
SET @startTypeId = 1
SET @Today = DATEDIFF(dd, 0, SYSDATETIME()) 
SET @startDatetime = DATEADD(n,612, @Today)
SET @flightAirState = 10 --started
SET @flightCrewType = 1 --Pilot
SET @flightAircraftType = 2 --1=GliderFlight, 2=TowFlight, 3=MotorFlight

PRINT 'INSERT Test-Flights'
SET @TowFlightId = NULL
SET @flightId = NEWID()
SET @flightAirState = 20 --landed

INSERT INTO [dbo].[Flights] ([FlightId] ,[AircraftId] ,[StartDateTime] ,[LdgDateTime] ,[StartLocationId] ,[LdgLocationId]
           ,[StartRunway] ,[LdgRunway] ,[FlightTypeId] ,[StartType] ,[TowFlightId] ,[NrOfLdgs] ,[AirStateId] ,[FlightAircraftType] ,[Comment] ,[IncidentComment]
           ,[CouponNumber] ,[InvoicedOn] ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId]
           ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (@flightId ,@towingAircraftId ,@startDatetime ,DATEADD(n,14, @startDatetime) ,@locationId ,@locationId
           ,NULL ,NULL ,@towFlightTypeId ,@startTypeId ,@TowFlightId ,1 ,@flightAirState , @flightAircraftType ,'Towing flight' ,NULL
           ,NULL ,NULL ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)

INSERT INTO [dbo].[FlightCrew] ([FlightCrewId] ,[FlightId] ,[PersonId] ,[FlightCrewType]
           ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId] ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (NEWID() ,@flightId ,(SELECT TOP 1 PersonId FROM Persons where HasTowPilotLicence = 1) ,@flightCrewType
           ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)

SET @flightAircraftType = 1
SET @TowFlightId = @flightId
SET @flightId = NEWID()
INSERT INTO [dbo].[Flights] ([FlightId] ,[AircraftId] ,[StartDateTime] ,[LdgDateTime] ,[StartLocationId] ,[LdgLocationId]
           ,[StartRunway] ,[LdgRunway] ,[FlightTypeId] ,[StartType] ,[TowFlightId] ,[NrOfLdgs] ,[AirStateId] ,[FlightAircraftType] ,[Comment] ,[IncidentComment]
           ,[CouponNumber] ,[InvoicedOn] ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId]
           ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (@flightId ,@aircraftId ,@startDatetime ,DATEADD(n,212, @startDatetime) ,@locationId ,@locationId
           ,NULL ,NULL ,@flightTypeId ,@startTypeId ,@TowFlightId ,1 ,@flightAirState , @flightAircraftType ,'PAX flight' ,NULL
           ,NULL ,NULL ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)

INSERT INTO [dbo].[FlightCrew] ([FlightCrewId] ,[FlightId] ,[PersonId] ,[FlightCrewType]
           ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId] ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (NEWID() ,@flightId ,(SELECT TOP 1 PersonId FROM Persons where HasGliderPilotLicence = 1) ,@flightCrewType
           ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)

SET @personId = NEWID()
INSERT INTO Persons (PersonId, Lastname, Firstname, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType, IsFastEntryRecord) 
VALUES (@personId, 'Schellenberg', 'Hansueli', SYSDATETIME(), @insertUserId, @recordState,@ownerId, @OwnershipType, 1)

SET @flightCrewType = 4 --passenger
INSERT INTO [dbo].[FlightCrew] ([FlightCrewId] ,[FlightId] ,[PersonId] ,[FlightCrewType]
           ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId] ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (NEWID() ,@flightId ,@personId ,@flightCrewType
           ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)


SET @startDatetime = DATEADD(n,20, @startDatetime)
SET @TowFlightId = NULL
SET @flightId = NEWID()
SET @flightAirState = 20 --landed
SET @flightCrewType = 1 --Pilot
SET @aircraftId = (SELECT TOP 1 aircraftId FROM Aircrafts where Immatriculation = 'HB-3256')
SET @flightTypeId = (SELECT TOP 1  FlightTypeId FROM FlightTypes WHERE FlightCode = '70' AND ClubId = @insertClubId)
SET @flightAircraftType = 2

INSERT INTO [dbo].[Flights] ([FlightId] ,[AircraftId] ,[StartDateTime] ,[LdgDateTime] ,[StartLocationId] ,[LdgLocationId]
           ,[StartRunway] ,[LdgRunway] ,[FlightTypeId] ,[StartType] ,[TowFlightId] ,[NrOfLdgs] ,[AirStateId] ,[FlightAircraftType] ,[Comment] ,[IncidentComment]
           ,[CouponNumber] ,[InvoicedOn] ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId]
           ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (@flightId ,@towingAircraftId ,@startDatetime ,DATEADD(n,6, @startDatetime) ,@locationId ,@locationId
           ,NULL ,NULL ,@towFlightTypeId ,@startTypeId ,@TowFlightId ,1 ,@flightAirState , @flightAircraftType ,'Towing flight' ,NULL
           ,NULL ,NULL ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)

INSERT INTO [dbo].[FlightCrew] ([FlightCrewId] ,[FlightId] ,[PersonId] ,[FlightCrewType]
           ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId] ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (NEWID() ,@flightId ,(SELECT TOP 1 PersonId FROM Persons where HasTowPilotLicence = 1) ,@flightCrewType
           ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)

SET @flightAircraftType = 1
SET @TowFlightId = @flightId
SET @flightId = NEWID()
INSERT INTO [dbo].[Flights] ([FlightId] ,[AircraftId] ,[StartDateTime] ,[LdgDateTime] ,[StartLocationId] ,[LdgLocationId]
           ,[StartRunway] ,[LdgRunway] ,[FlightTypeId] ,[StartType] ,[TowFlightId] ,[NrOfLdgs] ,[AirStateId] ,[FlightAircraftType] ,[Comment] ,[IncidentComment]
           ,[CouponNumber] ,[InvoicedOn] ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId]
           ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (@flightId ,@aircraftId ,@startDatetime ,DATEADD(n,22, @startDatetime) ,@locationId ,@locationId
           ,NULL ,NULL ,@flightTypeId ,@startTypeId ,@TowFlightId ,1 ,@flightAirState , @flightAircraftType ,'Schoolflight with instructor' ,NULL
           ,NULL ,NULL ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)


INSERT INTO [dbo].[FlightCrew] ([FlightCrewId] ,[FlightId] ,[PersonId] ,[FlightCrewType]
           ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId] ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (NEWID() ,@flightId ,(SELECT TOP 1 PersonId FROM Persons where HasGliderTraineeLicence = 1) ,@flightCrewType
           ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)

SET @flightCrewType = 3 --Teacher

INSERT INTO [dbo].[FlightCrew] ([FlightCrewId] ,[FlightId] ,[PersonId] ,[FlightCrewType]
           ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId] ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (NEWID() ,@flightId ,(SELECT TOP 1 PersonId FROM Persons where HasGliderInstructorLicence = 1 AND Zip <> '2333') ,@flightCrewType
           ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)




SET @startDatetime = DATEADD(n,23, @startDatetime)
SET @TowFlightId = NULL
SET @flightId = NEWID()
SET @flightAirState = 10 --started
SET @flightCrewType = 1 --Pilot
SET @aircraftId = (SELECT TOP 1 aircraftId FROM Aircrafts where Immatriculation = 'HB-2464')
SET @flightTypeId = (SELECT TOP 1  FlightTypeId FROM FlightTypes WHERE FlightCode = '60' AND ClubId = @insertClubId)
SET @startTypeId = 3

SET @flightAircraftType = 1
SET @TowFlightId = NULL
SET @flightId = NEWID()
INSERT INTO [dbo].[Flights] ([FlightId] ,[AircraftId] ,[StartDateTime] ,[LdgDateTime] ,[StartLocationId] ,[LdgLocationId]
           ,[StartRunway] ,[LdgRunway] ,[FlightTypeId] ,[StartType] ,[TowFlightId] ,[NrOfLdgs] ,[AirStateId] ,[FlightAircraftType] ,[Comment] ,[IncidentComment]
           ,[CouponNumber] ,[InvoicedOn] ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId]
           ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (@flightId ,@aircraftId ,@startDatetime ,NULL ,@locationId ,@locationId
           ,NULL ,NULL ,@flightTypeId ,@startTypeId ,@TowFlightId ,0 ,@flightAirState , @flightAircraftType ,'Selfstarter' ,NULL
           ,NULL ,NULL ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)


INSERT INTO [dbo].[FlightCrew] ([FlightCrewId] ,[FlightId] ,[PersonId] ,[FlightCrewType]
           ,[CreatedOn] ,[CreatedByUserId] ,[ModifiedOn] ,[ModifiedByUserId] ,[DeletedOn] ,[DeletedByUserId] ,[RecordState] ,[OwnerId] ,[OwnershipType])
     VALUES
           (NEWID() ,@flightId ,(SELECT TOP 1 PersonId FROM Persons where HasGliderPilotLicence = 1) ,@flightCrewType
           ,SYSDATETIME() ,@insertUserId ,NULL ,NULL ,NULL ,NULL ,@recordState ,@ownerId, @OwnershipType)

GO

UPDATE [dbo].[FlightCrew] SET [BeginFlightDateTime] = [dbo].[Flights].[StartDateTime], 
	[EndFlightDateTime] = [dbo].[Flights].[LdgDateTime],
	[NrOfLdgs] = [dbo].[Flights].[NrOfLdgs]
	FROM [dbo].[Flights] INNER JOIN FlightCrew ON [dbo].[Flights].FlightId = [dbo].[FlightCrew].FlightId
	WHERE [dbo].[FlightCrew].FlightCrewType = 1 --Pilots
GO
UPDATE [dbo].[FlightCrew] SET [BeginFlightDateTime] = [dbo].[Flights].[StartDateTime], 
	[EndFlightDateTime] = [dbo].[Flights].[LdgDateTime],
	[BeginInstructionDateTime] = [dbo].[Flights].[StartDateTime], 
	[EndInstructionDateTime] = [dbo].[Flights].[LdgDateTime],
	[NrOfLdgs] = [dbo].[Flights].[NrOfLdgs]
	FROM [dbo].[Flights] INNER JOIN FlightCrew ON [dbo].[Flights].FlightId = [dbo].[FlightCrew].FlightId
	WHERE [dbo].[FlightCrew].FlightCrewType = 3 --Instructors
GO