
USE [FLSTest]
GO

PRINT 'INSERT Static Data Started'

DECLARE @insertClubId as uniqueidentifier
-- System-Club
SET @insertClubId = 'A1DDE2CB-6326-4BB2-897D-7CFC118E842B'

DECLARE @recordState as bigint
SET @recordState = 1

DECLARE @OwnershipType as bigint
SET @OwnershipType = 2 --Club

DECLARE @insertUserId as uniqueidentifier
SET @insertUserId = '13731EE2-C1D8-455C-8AD1-C39399893FFF'

DECLARE @systemAdminRoleId as uniqueidentifier
SET @systemAdminRoleId = '56352545-2454-3453-2343-C742446534512'

DECLARE @clientAdminRoleId as uniqueidentifier
SET @clientAdminRoleId = '92750A21-9BCD-FFFF-2343-23B44724019B'

DECLARE @flightOperatorRoleId as uniqueidentifier
SET @flightOperatorRoleId = '187A8729-92BC-2932-AC83-15F14724019B'

DECLARE @SwitzerlandId as uniqueidentifier
SET @SwitzerlandId = '77CC3BE6-95DB-11E0-B104-E7F04724019B'

DECLARE @ownerId as uniqueidentifier
SET @ownerId = @insertClubId 

DECLARE @workflowUserId as uniqueidentifier
DECLARE @workflowRoleId as uniqueidentifier

PRINT 'DELETE Data'

UPDATE Clubs 
SET DefaultGliderFlightTypeId = null, 
DefaultMotorFlightTypeId = null, 
DefaultStartType = null, 
DefaultTowFlightTypeId = null,
HomebaseId = null

DELETE FROM EmailTemplates

DELETE FROM AircraftReservations
DELETE FROM AircraftReservationTypes
DELETE FROM AircraftOperatingCounters

DELETE FROM PlanningDayAssignments
DELETE FROM PlanningDays
DELETE FROM PlanningDayAssignmentTypes

DELETE FROM AuditLogDetails
DELETE FROM AuditLogs
DELETE FROM AuditLogEventTypes

DELETE FROM AircraftAircraftStates
DELETE FROM PersonPersonCategories
DELETE FROM PersonClub
DELETE FROM AircraftStates
DELETE FROM FlightCrew
DELETE FROM FlightCrewTypes
DELETE FROM Flights
DELETE FROM FlightStates
DELETE FROM Aircrafts
DELETE FROM UserRoles
DELETE FROM Users
DELETE FROM Persons
DELETE FROM PersonCategories
DELETE FROM InOutboundPoints
DELETE FROM Locations
DELETE FROM AircraftTypes
DELETE FROM FlightTypes
DELETE FROM StartTypes
DELETE FROM MemberStates
DELETE FROM LocationTypes
DELETE FROM ElevationUnitTypes
DELETE FROM LengthUnitTypes

DELETE FROM ClubExtensions
DELETE FROM ExtensionParameterValues
DELETE FROM ExtensionParameters
DELETE FROM ExtensionParameterTypes
DELETE FROM Extensions
DELETE FROM ExtensionTypes

DELETE FROM FlightCostBalanceTypes

DELETE FROM SystemLogs
DELETE FROM SystemData
-- DELETE FROM SystemVersion --do not delete version infos
DELETE FROM Clubs
DELETE FROM Roles
DELETE FROM UserAccountStates

DELETE FROM Countries

DELETE FROM LanguageTranslations
DELETE FROM Languages

DELETE FROM ClubStates

PRINT 'Insert ClubStates'
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

-- countries

PRINT 'INSERT Countries'
--SET IDENTITY_INSERT Countries ON

DECLARE @countryId as uniqueidentifier
INSERT INTO Countries (CountryId, CountryIdIso, CountryName, CountryCodeIso2, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@SwitzerlandId, 756,		'Schweiz',		'CH',  SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)
SET @countryId = '8B7B4394-95DB-11E0-9BCD-06F14724019B'
INSERT INTO Countries (CountryId, CountryIdIso, CountryName, CountryCodeIso2, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@countryId, 276,		'Deutschland',		'DE',  SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)
SET @countryId = '9022F82E-95DB-11E0-8D18-07F14724019B'
INSERT INTO Countries (CountryId, CountryIdIso, CountryName, CountryCodeIso2, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@countryId, 250,		'Frankreich',		'FR', 	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)
SET @countryId = '95C48D24-95DB-11E0-A5E5-08F14724019B'
INSERT INTO Countries (CountryId, CountryIdIso, CountryName, CountryCodeIso2, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@countryId, 40,		'Österreich',		'AT', 	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)
SET @countryId = '9B2F555A-95DB-11E0-8E8C-15F14724019B'
INSERT INTO Countries (CountryId, CountryIdIso, CountryName, CountryCodeIso2, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@countryId, 380,		'Italien',		'IT', 	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)
SET @countryId = '9F5B2BE0-95DB-11E0-89E5-19F14724019B'
INSERT INTO Countries (CountryId, CountryIdIso, CountryName, CountryCodeIso2, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@countryId, 438,		'Liechtenstein',		'LI', 	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)
--SET IDENTITY_INSERT Countries OFF

PRINT 'INSERT ElevationUnitTypes'
SET IDENTITY_INSERT [ElevationUnitTypes] ON
INSERT INTO [ElevationUnitTypes] ([ElevationUnitTypeId], [ElevationUnitTypeName], [ElevationUnitTypeKeyName], 
	[ElevationUnitTypeShortName], [CreatedOn])
     VALUES (1, 'Meter über Meer', 'MeterAboveSeaLevel', 'm.ü.M.', SYSDATETIME())
INSERT INTO [ElevationUnitTypes] ([ElevationUnitTypeId], [ElevationUnitTypeName], [ElevationUnitTypeKeyName], 
	[ElevationUnitTypeShortName], [CreatedOn])
     VALUES (2, 'Fuss über Meer', 'FeetAboveSealevel', 'ft.ü.M.', SYSDATETIME())
SET IDENTITY_INSERT [ElevationUnitTypes] OFF

PRINT 'INSERT LengthUnitTypes'
SET IDENTITY_INSERT [LengthUnitTypes] ON
INSERT INTO [LengthUnitTypes] ([LengthUnitTypeId], [LengthUnitTypeName], [LengthUnitTypeKeyName], [LengthUnitTypeShortName],[CreatedOn])
     VALUES (1, 'Meter', 'Meter', 'm', SYSDATETIME())
INSERT INTO [LengthUnitTypes] ([LengthUnitTypeId], [LengthUnitTypeName], [LengthUnitTypeKeyName], [LengthUnitTypeShortName],[CreatedOn])
     VALUES (2, 'Fuss', 'Feet', 'ft', SYSDATETIME())
SET IDENTITY_INSERT [LengthUnitTypes] OFF


PRINT 'INSERT AircraftTypes'
SET IDENTITY_INSERT [AircraftTypes] ON
INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn])
     VALUES (0, 'Unbekannt', 'Flugzeugtyp ist nicht bekannt / nicht gesetzt', SYSDATETIME())

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn])
     VALUES (1, 'Segelflugzeug', 'Reines Segelflugzeug ohne Motor oder Turbo', SYSDATETIME())

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn])
     VALUES (2, 'Segelflugzeug mit Motor', 'Segelflugzeug mit Motor oder Turbo', SYSDATETIME())

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn])
     VALUES (4, 'Motorsegelflugzeug', 'Motorsegelflugzeug', SYSDATETIME())

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn])
     VALUES (8, 'Motorflugzeug', 'Motorflugzeug oder Schleppflugzeug', SYSDATETIME())

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn])
     VALUES (16, 'Multi-Motorflugzeug', 'Motorflugzeug mit mehr als einem Motor', SYSDATETIME())

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn])
     VALUES (32, 'Jet', 'Jet', SYSDATETIME())

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn])
     VALUES (64, 'Helikopter', 'Helikopter', SYSDATETIME())
SET IDENTITY_INSERT [AircraftTypes] OFF

PRINT 'INSERT FlightCrewTypes'
SET IDENTITY_INSERT [FlightCrewTypes] ON
INSERT INTO [FlightCrewTypes] ([FlightCrewTypeId], [FlightCrewTypeName], [Comment], [CreatedOn])
     VALUES (1, 'Pilot', 'Pilot oder Flugschüler', SYSDATETIME())

INSERT INTO [FlightCrewTypes] ([FlightCrewTypeId], [FlightCrewTypeName], [Comment], [CreatedOn])
     VALUES (2, 'Copilot', 'Copilot / 2. Besatzung', SYSDATETIME())

INSERT INTO [FlightCrewTypes] ([FlightCrewTypeId], [FlightCrewTypeName], [Comment], [CreatedOn])
     VALUES (3, 'Fluglehrer', 'Fluglehrer / Experte / Instructor', SYSDATETIME())

INSERT INTO [FlightCrewTypes] ([FlightCrewTypeId], [FlightCrewTypeName], [Comment], [CreatedOn])
     VALUES (4, 'Passagier', 'Passagier', SYSDATETIME())

INSERT INTO [FlightCrewTypes] ([FlightCrewTypeId], [FlightCrewTypeName], [Comment], [CreatedOn])
     VALUES (5, 'Windenführer', 'Windenführer', SYSDATETIME())

INSERT INTO [FlightCrewTypes] ([FlightCrewTypeId], [FlightCrewTypeName], [Comment], [CreatedOn])
     VALUES (6, 'Observer', 'Überwachender Pilot oder Fluglehrer', SYSDATETIME())

INSERT INTO [FlightCrewTypes] ([FlightCrewTypeId], [FlightCrewTypeName], [Comment], [CreatedOn])
     VALUES (10, 'Rechnungsempfänger', 'Rechnungsempfänger der Flugkosten / Invoice Recipient', SYSDATETIME())
SET IDENTITY_INSERT [FlightCrewTypes] OFF



PRINT 'INSERT UserAccountStates'
SET IDENTITY_INSERT [UserAccountStates] ON
INSERT INTO [UserAccountStates] ([UserAccountStateId], [UserAccountStateName], [Comment], [CreatedOn])
     VALUES (1, 'Aktiv', 'Benutzer aktiv', SYSDATETIME())

INSERT INTO [UserAccountStates] ([UserAccountStateId], [UserAccountStateName], [Comment], [CreatedOn])
     VALUES (2, 'Gesperrt', 'Benutzer gesperrt (zu viele Loginversuche)', SYSDATETIME())

INSERT INTO [UserAccountStates] ([UserAccountStateId], [UserAccountStateName], [Comment], [CreatedOn])
     VALUES (10, 'Deaktiviert', 'Benutzer ist deaktiviert', SYSDATETIME())
SET IDENTITY_INSERT [UserAccountStates] OFF



PRINT 'INSERT FlightCostBalanceTypes'
-- flight cost balance types
SET IDENTITY_INSERT [FlightCostBalanceTypes] ON
INSERT INTO [FlightCostBalanceTypes] ([FlightCostBalanceTypeId],[FlightCostBalanceTypeName]
           ,[Comment],
		   [PersonForInvoiceRequired],[IsForGliderFlights],[IsForTowFlights],[IsForMotorFlights],[CreatedOn])
     VALUES
           (1,'Pilot bezahlt sämtliche Kosten', 
		   'Rechnung wird mit sämtlichen anfallenden Segelflugkosten, Landetaxen an Pilot versendet.'
           ,0, 1, 1, 1
           ,SYSDATETIME())

INSERT INTO [FlightCostBalanceTypes] ([FlightCostBalanceTypeId],[FlightCostBalanceTypeName]
           ,[Comment],
		   [PersonForInvoiceRequired],[IsForGliderFlights],[IsForTowFlights],[IsForMotorFlights],[CreatedOn])
     VALUES
           (2,'50:50 Pilot/Copilot',
		   'Rechnung wird an Pilot und Copilot ausgestellt im Kostenverhältnis 50:50'
           ,0, 1, 1, 1
           ,SYSDATETIME())


INSERT INTO [FlightCostBalanceTypes] ([FlightCostBalanceTypeId],[FlightCostBalanceTypeName]
           ,[Comment],
		   [PersonForInvoiceRequired],[IsForGliderFlights],[IsForTowFlights],[IsForMotorFlights],[CreatedOn])
     VALUES
           (3,'Schlepppilot übernimmt Schleppkosten',
		   'Rechnung mit den Segelflugkosten inkl. Landetaxen geht an den Segelflugpiloten, Schleppkosten werden dem Schlepppiloten in Rechnung gestellt.'
           ,0, 1, 0, 0
           ,SYSDATETIME())

INSERT INTO [FlightCostBalanceTypes] ([FlightCostBalanceTypeId],[FlightCostBalanceTypeName]
           ,[Comment],
		   [PersonForInvoiceRequired],[IsForGliderFlights],[IsForTowFlights],[IsForMotorFlights],[CreatedOn])
     VALUES
           (4,'Kein Fluglehrerhonorar verrechnen',
		   'Dem Piloten/Schüler wird kein Fluglehrerhonorar in Rechnung gestellt.'
           ,0, 1, 1, 1
           ,SYSDATETIME())

INSERT INTO [FlightCostBalanceTypes] ([FlightCostBalanceTypeId],[FlightCostBalanceTypeName]
           ,[Comment],
		   [PersonForInvoiceRequired],[IsForGliderFlights],[IsForTowFlights],[IsForMotorFlights],[CreatedOn])
     VALUES
           (5,'Rechnung an ...',
		   'Die Rechnung geht an die entsprechend angegebene Person'
           ,1, 1, 1, 1
           ,SYSDATETIME())

SET IDENTITY_INSERT [FlightCostBalanceTypes] OFF


PRINT 'INSERT FlightStates'
SET IDENTITY_INSERT [FlightStates] ON
INSERT INTO [FlightStates] ([FlightStateId],[FlightStateName],[Comment],[CreatedOn])
     VALUES
           (0, 'Neu', 'Neuer Flug / Noch nicht gestartet'
           ,SYSDATETIME())

INSERT INTO [FlightStates] ([FlightStateId],[FlightStateName],[Comment],[CreatedOn])
     VALUES
           (5, 'Flugplan eröffnet', 'Flugplan eröffnet'
           ,SYSDATETIME())
INSERT INTO [FlightStates] ([FlightStateId],[FlightStateName],[Comment],[CreatedOn])
     VALUES
           (10, 'Gestartet', 'Flugzeug gestartet / in der Luft'
           ,SYSDATETIME())

INSERT INTO [FlightStates] ([FlightStateId],[FlightStateName],[Comment],[CreatedOn])
     VALUES
           (20, 'Gelandet', 'Flugzeug gelandet'
           ,SYSDATETIME())

INSERT INTO [FlightStates] ([FlightStateId],[FlightStateName],[Comment],[CreatedOn])
     VALUES
           (25, 'Geschlossen', 'Flug/Flugplan geschlossen'
           ,SYSDATETIME())

INSERT INTO [FlightStates] ([FlightStateId],[FlightStateName],[Comment],[CreatedOn])
     VALUES
           (28, 'Ungültig', 'Flug wurde validiert, Angaben sind aber ungültig oder nicht plausibel'
           ,SYSDATETIME())

INSERT INTO [FlightStates] ([FlightStateId],[FlightStateName],[Comment],[CreatedOn])
     VALUES
           (30, 'Gültig', 'Flug wurde validiert und Angaben zum Flug sind gültig'
           ,SYSDATETIME())

INSERT INTO [FlightStates] ([FlightStateId],[FlightStateName],[Comment],[CreatedOn])
     VALUES
           (40, 'Gesperrt', 'Flug kann nicht mehr editiert werden und ist für Verrechnung bereit'
           ,SYSDATETIME())

INSERT INTO [FlightStates] ([FlightStateId],[FlightStateName],[Comment],[CreatedOn])
     VALUES
           (50, 'Verrechnet', 'Flug wurde verrechnet und kann nicht mehr editiert werden'
           ,SYSDATETIME())

INSERT INTO [FlightStates] ([FlightStateId],[FlightStateName],[Comment],[CreatedOn])
     VALUES
           (55, 'Teilweise bezahlt', 'Flug wurde verrechnet und einen Teil der Rechnung(en) wurde bezahlt.'
           ,SYSDATETIME())

INSERT INTO [FlightStates] ([FlightStateId],[FlightStateName],[Comment],[CreatedOn])
     VALUES
           (60, 'Bezahlt', 'Flug wurde bezahlt.'
           ,SYSDATETIME())

SET IDENTITY_INSERT [FlightStates] OFF


PRINT 'INSERT SystemClub'
INSERT INTO [Clubs] ([ClubID], [ClubName], [ClubKey], [Address], [Zip], [City], [CountryId], [Phone], [FaxNumber], [Email], [WebPage], [Contact], 
			[CreatedOn], [CreatedByUserId], [RecordState], [OwnerId], [OwnershipType], [ClubStateId])
     VALUES
           (@insertClubId,'System-Verein', 'SystemClub',null,null,null,@SwitzerlandId,null,null,null,null,null
           ,SYSDATETIME(),@insertClubId,@recordState,@insertClubId
           ,0, 0)


-- Users
-- Password for User s is "s"
PRINT 'INSERT System users'
INSERT INTO Users (UserId, ClubId, Username, FriendlyName, PasswordHash, EmailConfirmed, SecurityStamp, PersonId, NotificationEmail, AccountState, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@insertUserId, @insertClubId, 's', 'Default System User', 'AG3i8UWZYzlQMoA7jS58oJCWKJUhe+MR6nInBRAHfFc2YtoL+eiOuTYZd46urgf+ZA==', 1, NEWID(), null, 'test@glider-fls.ch', 1,  SYSDATETIME(),		 @insertUserId, @recordState, @insertUserId, @ownershipType)

-- Password for User s is "Workflow@2015$$"
SET @workflowUserId = NEWID()
INSERT INTO Users (UserId, ClubId, Username, FriendlyName, PasswordHash, EmailConfirmed, SecurityStamp, PersonId, NotificationEmail, AccountState, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@workflowUserId, @insertClubId, 'workflow', 'Workflow User', 'ADNpUPPVvaI8GpQIMN4QNa9QrOJr+Zk/P5zNFGkp0Vnviw9297cslKAoLrJaB+sDgw==', 1, NEWID(), null, 'test@glider-fls.ch', 1,  SYSDATETIME(),		 @insertUserId, @recordState, @insertUserId, @ownershipType)




-- Roles
PRINT 'INSERT Roles'

INSERT INTO Roles (RoleId, RoleName, RoleApplicationKeyString, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@systemAdminRoleId, 'System Administrator', 'SystemAdministrator', SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, 0)

SET @workflowRoleId = NEWID()
INSERT INTO Roles (RoleId, RoleName, RoleApplicationKeyString, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@workflowRoleId, 'Workflow Executor', 'WorkflowExecutor', SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, 0)

INSERT INTO Roles (RoleId, RoleName, RoleApplicationKeyString, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@flightOperatorRoleId, 'Segelflugleiter', 'FlightOperator', SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, 0)

INSERT INTO Roles (RoleId, RoleName, RoleApplicationKeyString, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@clientAdminRoleId, 'Vereins-Administrator', 'ClubAdministrator', SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, 0)



-- UserRoles

PRINT 'INSERT UserRoles'
INSERT INTO UserRoles (UserId, RoleId)
VALUES	(@insertUserId, @systemAdminRoleId)

INSERT INTO UserRoles (UserId, RoleId)
VALUES	(@workflowUserId, @workflowRoleId)




-- LocationTypes
PRINT 'INSERT LocationTypes'
DECLARE @locationTypeId as uniqueidentifier
SET @locationTypeId = NEWID()
INSERT INTO LocationTypes (LocationTypeId, LocationTypeName, LocationTypeCupId, IsAirfield, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@locationTypeId, 'Wegpunkt', 1, 0,	 SYSDATETIME(), @insertUserId, @recordState, @ownerId, @ownershipType)
SET @locationTypeId = NEWID()
INSERT INTO LocationTypes (LocationTypeId, LocationTypeName, LocationTypeCupId, IsAirfield, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@locationTypeId, 'Flugplatz mit Graspiste', 2, 1 , SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @ownershipType)
SET @locationTypeId = NEWID()
INSERT INTO LocationTypes (LocationTypeId, LocationTypeName, LocationTypeCupId, IsAirfield, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@locationTypeId, 'Aussenlandefeld', 3, 0 , SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @ownershipType)
SET @locationTypeId = NEWID()
INSERT INTO LocationTypes (LocationTypeId, LocationTypeName, LocationTypeCupId, IsAirfield, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@locationTypeId, 'Segelflugplatz (nur)', 4, 1,	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @ownershipType)
SET @locationTypeId = NEWID()
INSERT INTO LocationTypes (LocationTypeId, LocationTypeName, LocationTypeCupId, IsAirfield, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@locationTypeId, 'Flugplatz mit Hartbelagpiste', 5, 1,	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @ownershipType)
SET @locationTypeId = NEWID()
INSERT INTO LocationTypes (LocationTypeId, LocationTypeName, LocationTypeCupId, IsAirfield, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@locationTypeId, 'Anderer Lokationstyp', 99, 0,	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @ownershipType)
-- weitere LocationTypes noch nicht erfasst (Pässe, Berge, Sender, Tower, Tunnel, Brücken, etc.)
-- siehe SeeYou Waypoint CUP file format description




-- Locations
PRINT 'INSERT Locations'
DECLARE @locationId as uniqueidentifier
SET @locationId = NEWID()
SET @locationTypeId = (SELECT LocationTypeId FROM LocationTypes WHERE LocationTypeCupId = 2)
INSERT INTO Locations (LocationId, LocationName, CountryId, LocationTypeId, IcaoCode, Elevation, ElevationUnitType, RunwayLength, RunwayLengthUnitType, IsFastEntryRecord, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (@locationId, 'Speck Fehraltorf', @SwitzerlandId, @locationTypeId, 'LSZK', 536, 1, 600, 1, 0, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @ownershipType)
SET @locationId = NEWID()
INSERT INTO Locations (LocationId, LocationName, CountryId, LocationTypeId, IcaoCode, Elevation, ElevationUnitType, RunwayLength, RunwayLengthUnitType, IsFastEntryRecord, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (@locationId, 'Montricher', @SwitzerlandId, @locationTypeId, 'LSTR', 614, 1, 1000, 1, 0, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @ownershipType)
SET @locationId = NEWID()
SET @locationTypeId = (SELECT LocationTypeId FROM LocationTypes WHERE LocationTypeCupId = 5)
INSERT INTO Locations (LocationId, LocationName, CountryId, LocationTypeId, IcaoCode, Elevation, ElevationUnitType, RunwayLength, RunwayLengthUnitType, IsFastEntryRecord, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (@locationId, 'Schänis', @SwitzerlandId, @locationTypeId, 'LSZX', 416, 1, 500, 1, 0, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @ownershipType)
SET @locationId = NEWID()
INSERT INTO Locations (LocationId, LocationName, CountryId, LocationTypeId, IcaoCode, Elevation, ElevationUnitType, RunwayLength, RunwayLengthUnitType, IsFastEntryRecord, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (@locationId, 'Saanen', @SwitzerlandId, @locationTypeId, 'LSGK', 1008, 1, 1400, 1, 0, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @ownershipType)



-- start types
PRINT 'INSERT StartTypes'
SET IDENTITY_INSERT StartTypes ON
INSERT INTO StartTypes (StartTypeId, StartTypeName, IsForGliderFlights, IsForTowFlights, IsForMotorFlights, CreatedOn)
VALUES	(1, 'Flugzeugschlepp', 1, 1, 0, SYSDATETIME())
INSERT INTO StartTypes (StartTypeId, StartTypeName, IsForGliderFlights, IsForTowFlights, IsForMotorFlights, CreatedOn)
VALUES	(2, 'Windenstart', 1, 0, 0, SYSDATETIME())
INSERT INTO StartTypes (StartTypeId, StartTypeName, IsForGliderFlights, IsForTowFlights, IsForMotorFlights, CreatedOn)
VALUES	(3, 'Eigenstart', 1, 0, 0, SYSDATETIME())
INSERT INTO StartTypes (StartTypeId, StartTypeName, IsForGliderFlights, IsForTowFlights, IsForMotorFlights, CreatedOn)
VALUES	(4, 'Externer Start', 1, 1, 1, SYSDATETIME())
INSERT INTO StartTypes (StartTypeId, StartTypeName, IsForGliderFlights, IsForTowFlights, IsForMotorFlights, CreatedOn)
VALUES	(5, 'Motorflugstart', 0, 1, 1, SYSDATETIME())
SET IDENTITY_INSERT StartTypes OFF



PRINT 'INSERT AircraftStates'
SET IDENTITY_INSERT [AircraftStates] ON
INSERT INTO [AircraftStates] ([AircraftStateId],[AircraftStateName],[IsAircraftFlyable],[CreatedOn])
VALUES (0 ,'Unbekannt' ,1 ,SYSDATETIME())
INSERT INTO [AircraftStates] ([AircraftStateId],[AircraftStateName],[IsAircraftFlyable],[CreatedOn])
VALUES (1 ,'OK' ,1 ,SYSDATETIME())
INSERT INTO [AircraftStates] ([AircraftStateId],[AircraftStateName],[IsAircraftFlyable],[CreatedOn])
VALUES (2 ,'Information' ,1 ,SYSDATETIME())
INSERT INTO [AircraftStates] ([AircraftStateId],[AircraftStateName],[IsAircraftFlyable],[CreatedOn])
VALUES (3 ,'Vorsicht' ,1 ,SYSDATETIME())
INSERT INTO [AircraftStates] ([AircraftStateId],[AircraftStateName],[IsAircraftFlyable],[CreatedOn])
VALUES (4 ,'Defekt' , 0,SYSDATETIME())
INSERT INTO [AircraftStates] ([AircraftStateId],[AircraftStateName],[IsAircraftFlyable],[CreatedOn])
VALUES (5 ,'Wartung' ,0 ,SYSDATETIME())
INSERT INTO [AircraftStates] ([AircraftStateId],[AircraftStateName],[IsAircraftFlyable],[CreatedOn])
VALUES (6 ,'Nicht eingelöst' , 0, SYSDATETIME())
INSERT INTO [AircraftStates] ([AircraftStateId],[AircraftStateName],[IsAircraftFlyable],[CreatedOn])
VALUES (99 ,'Ausrangiert' ,0 ,SYSDATETIME())
SET IDENTITY_INSERT [AircraftStates] OFF

SET IDENTITY_INSERT [AircraftReservationTypes] ON
INSERT INTO [dbo].[AircraftReservationTypes]
           ([AircraftReservationTypeId],[AircraftReservationTypeName],[Remarks]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (1,'Charter-Flug',null, SYSDATETIME(), null)

INSERT INTO [dbo].[AircraftReservationTypes]
           ([AircraftReservationTypeId],[AircraftReservationTypeName],[Remarks]
           ,[CreatedOn],[ModifiedOn], [IsInstructorRequired])
     VALUES
           (2,'Check-Flug',null, SYSDATETIME(), null, 1)
SET IDENTITY_INSERT [AircraftReservationTypes] OFF

SET IDENTITY_INSERT [Languages] ON
INSERT INTO [dbo].[Languages]
           ([LanguageId],[LanguageKey], [LanguageName], [Remarks]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (1,'DE','Deutsch', null, SYSDATETIME(), null)

SET IDENTITY_INSERT [Languages] OFF

GO

PRINT 'Insert AuditLogEventTypes'
INSERT INTO [dbo].[AuditLogEventTypes]
           ([EventTypeId],[EventTypeName])
     VALUES
           (0, 'Added')

INSERT INTO [dbo].[AuditLogEventTypes]
           ([EventTypeId],[EventTypeName])
     VALUES
           (1, 'Deleted')

INSERT INTO [dbo].[AuditLogEventTypes]
           ([EventTypeId],[EventTypeName])
     VALUES
           (2, 'Modified')

INSERT INTO [dbo].[AuditLogEventTypes]
           ([EventTypeId],[EventTypeName])
     VALUES
           (3, 'Soft-Deleted')

INSERT INTO [dbo].[AuditLogEventTypes]
           ([EventTypeId],[EventTypeName])
     VALUES
           (4, 'Undeleted')
GO

PRINT 'INSERT Static Data Finished'