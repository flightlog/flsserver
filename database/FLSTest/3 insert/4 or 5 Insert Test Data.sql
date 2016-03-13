
USE [FLSTest]
GO

PRINT 'INSERT Test Datas Started'

-- config
DECLARE @insertUserId as uniqueidentifier
SET @insertUserId = '13731EE2-C1D8-455C-8AD1-C39399893FFF'
DECLARE @clubUserId as uniqueidentifier
DECLARE @clubFlightOperatorId as uniqueidentifier

DECLARE @systemAdminRoleId as uniqueidentifier
SET @systemAdminRoleId = '56352545-2454-3453-2343-C742446534512'
DECLARE @clientAdminRoleId as uniqueidentifier
SET @clientAdminRoleId = '92750A21-9BCD-FFFF-2343-23B44724019B'
DECLARE @flightOperatorRoleId as uniqueidentifier
SET @flightOperatorRoleId = '187A8729-92BC-2932-AC83-15F14724019B'

DECLARE @locationTypeId as uniqueidentifier
DECLARE @memberStateId as uniqueidentifier
DECLARE @recordState as bigint
SET @recordState = 1

DECLARE @OwnershipType as bigint
SET @OwnershipType = 2 --Club

DECLARE @insertClubId as uniqueidentifier

-- System-Club
DECLARE @systemClubId as uniqueidentifier
SET @systemClubId = 'A1DDE2CB-6326-4BB2-897D-7CFC118E842B'

-- Test-Club
SET @insertClubId = '0FA7B76F-47BA-4138-8F96-671400FD7C83'

DECLARE @ownerId as uniqueidentifier
SET @ownerId = @insertClubId 

DECLARE @clubEmailAddress as nvarchar(100)
SET @clubEmailAddress = 'schuele@galaxy-net.ch'

DECLARE @personId as uniqueidentifier

DECLARE @countryId as uniqueidentifier
SET @countryId = '77CC3BE6-95DB-11E0-B104-E7F04724019B'
DECLARE @SwitzerlandId as uniqueidentifier
SET @SwitzerlandId = '77CC3BE6-95DB-11E0-B104-E7F04724019B'

DECLARE @locationId as uniqueidentifier


PRINT 'INSERT Clubs'
INSERT INTO Clubs
           ([ClubID]
           ,[ClubName]
		   ,[ClubKey]
           ,[Address]
           ,[Zip]
           ,[City]
			  ,[CountryId]
           ,[Phone]
           ,[FaxNumber]
           ,[Email]
           ,[WebPage]
           ,[Contact]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType], [ClubStateId], [SendInvoiceReportsTo])
     VALUES
           (@insertClubId
           ,'Test-Club'
		   ,'TestClub'
           ,'Flugplatz Test'
           ,'5556'
           ,'Zürich'
			  ,'77CC3BE6-95DB-11E0-B104-E7F04724019B'
           ,'044 333 44 55'
           ,'044 666 55 44'
           ,'info@club.ch'
           ,'www.club.ch'
           ,'Sekretariat'
           ,SYSDATETIME()
           ,@insertUserId
           ,null
           ,null
           ,null
           ,null
           ,1
           ,@systemClubId, @OwnershipType, 1
		   ,'test@glider-fls.ch')




PRINT 'INSERT Users'
-- Password for User clubuser is "s"
SET @clubUserId = NEWID()
INSERT INTO Users (UserId, ClubId, Username, FriendlyName, PasswordHash, EmailConfirmed, SecurityStamp, PersonId, NotificationEmail, AccountState, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@clubUserId, @insertClubId, 'testclubadmin', 'Test Club Admin', 'AG3i8UWZYzlQMoA7jS58oJCWKJUhe+MR6nInBRAHfFc2YtoL+eiOuTYZd46urgf+ZA==', 1, NEWID(), null, @clubEmailAddress, 1,  SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)

-- Password for User fl is "f"
SET @clubFlightOperatorId = NEWID()
INSERT INTO Users (UserId, ClubId, Username, FriendlyName, PasswordHash, EmailConfirmed, SecurityStamp, PersonId, NotificationEmail, AccountState, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@clubFlightOperatorId, @insertClubId, 'testclubuser', 'Test Club Segelflugleiter User', 'AH9pN6aVgsLdt93zMBetFxx97q+aYPHhEE36H0M++kz0U+PN+GSnOJEVeTox8WfcnQ==', 1, NEWID(), null, @clubEmailAddress, 1,  SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)



-- UserRoles

PRINT 'INSERT UserRoles'
INSERT INTO UserRoles (UserId, RoleId)
VALUES	(@clubUserId, @clientAdminRoleId)

INSERT INTO UserRoles (UserId, RoleId)
VALUES	(@clubFlightOperatorId, @flightOperatorRoleId)



-- Persons
PRINT 'INSERT Persons'
SET @personId = NEWID()
INSERT INTO Persons (PersonId, Lastname, Firstname, Midname, CompanyName, AddressLine1, AddressLine2, Zip, City, Region, CountryId, PrivatePhone, MobilePhone, BusinessPhone, FaxNumber, EmailPrivate, EmailBusiness, Birthday, HasMotorPilotLicence, HasTowPilotLicence, HasGliderInstructorLicence, HasGliderPilotLicence, HasGliderTraineeLicence, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType, IsFastEntryRecord) 
VALUES (@personId, 'Rieterman', 'Conny', 'TowPilot', null, 'Köchlistrasse 1', null,  '8004', 'Zürich', null, @countryId, '044 333 88 88', '079 999 88 77', '044 666 55 44', null, 'hans@muster.ch', null, null, 1, 1, 0, 0, 0, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType, 0)
INSERT INTO PersonClub (PersonId, ClubId, MemberNumber, IsMotorPilot, IsTowPilot, IsGliderInstructor, IsGliderPilot, IsGliderTrainee, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType, IsPassenger, IsWinchOperator) VALUES (@personId, @insertClubId, '103000', 1, 1, 0, 0, 0, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType, 0, 0)
SET @personId = NEWID()
INSERT INTO Persons (PersonId, Lastname, Firstname, Midname, CompanyName, AddressLine1, AddressLine2, Zip, City, Region, CountryId, PrivatePhone, MobilePhone, BusinessPhone, FaxNumber, EmailPrivate, EmailBusiness, Birthday, HasMotorPilotLicence, HasTowPilotLicence, HasGliderInstructorLicence, HasGliderPilotLicence, HasGliderTraineeLicence, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType, IsFastEntryRecord) 
VALUES (@personId, 'Stahel', 'Morli', 'GliderPilot', null, 'Köchlistrasse 1', null,  '1234', 'Uster', null, @countryId, '044 333 88 22', '079 999 88 44', '044 666 55 44', null, 'hans2@muster.ch', null, null, 0, 0, 0, 1, 0, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType, 0)
INSERT INTO PersonClub (PersonId, ClubId, MemberNumber, IsMotorPilot, IsTowPilot, IsGliderInstructor, IsGliderPilot, IsGliderTrainee, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType, IsPassenger, IsWinchOperator) VALUES (@personId, @insertClubId, '222323', 0, 0, 0, 1, 0, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType, 0, 0)
SET @personId = NEWID()
INSERT INTO Persons (PersonId, Lastname, Firstname, Midname, CompanyName, AddressLine1, AddressLine2, Zip, City, Region, CountryId, PrivatePhone, MobilePhone, BusinessPhone, FaxNumber, EmailPrivate, EmailBusiness, Birthday, HasMotorPilotLicence, HasTowPilotLicence, HasGliderInstructorLicence, HasGliderPilotLicence, HasGliderTraineeLicence, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType, IsFastEntryRecord) 
VALUES (@personId, 'Kellner', 'Hansli', 'Teacher', null, 'Köchlistrasse 1', null,  '9544', 'Bichelsee', null, @countryId, '044 333 44 88', '079 999 88 33', '044 666 55 44', null, 'hans3@muster.ch', null, null, 0, 0, 1, 1, 0, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType, 0)
INSERT INTO PersonClub (PersonId, ClubId, MemberNumber, MemberKey, IsMotorPilot, IsTowPilot, IsGliderInstructor, IsGliderPilot, IsGliderTrainee, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType, IsPassenger, IsWinchOperator) VALUES (@personId, @insertClubId, '536594', '536594', 0, 0, 1, 1, 0, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType, 0, 0)
SET @personId = NEWID()
INSERT INTO Persons (PersonId, Lastname, Firstname, Midname, CompanyName, AddressLine1, AddressLine2, Zip, City, Region, CountryId, PrivatePhone, MobilePhone, BusinessPhone, FaxNumber, EmailPrivate, EmailBusiness, Birthday, HasMotorPilotLicence, HasTowPilotLicence, HasGliderInstructorLicence, HasGliderPilotLicence, HasGliderTraineeLicence, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType, IsFastEntryRecord) 
VALUES (@personId, 'Bünzli', 'Lenni', 'Trainee', null, 'Köchlistrasse 1', null,  '3000', 'Bern', null, @countryId, '044 333 33 33', '+41 79 999 88 22', '044 666 55 44', null, 'hans4@muster.ch', null, null, 0, 0, 0, 0, 1, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType, 0)
INSERT INTO PersonClub (PersonId, ClubId, MemberNumber, IsMotorPilot, IsTowPilot, IsGliderInstructor, IsGliderPilot, IsGliderTrainee, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType, IsPassenger, IsWinchOperator) VALUES (@personId, @insertClubId, '205207', 0, 0, 0, 0, 1, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType, 0, 0)
SET @personId = NEWID()
INSERT INTO Persons (PersonId, Lastname, Firstname, Midname, CompanyName, AddressLine1, AddressLine2, Zip, City, Region, CountryId, PrivatePhone, MobilePhone, BusinessPhone, FaxNumber, EmailPrivate, EmailBusiness, Birthday, HasMotorPilotLicence, HasTowPilotLicence, HasGliderInstructorLicence, HasGliderPilotLicence, HasGliderTraineeLicence, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType, IsFastEntryRecord) 
VALUES (@personId, 'Egli', 'Elias', 'extern', null, 'Köchlistrasse 1', null,  '2333', 'Locarno', null, @countryId, '044 333 33 44', '079 999 88 11', '044 666 55 44', null, 'hans5@muster.ch', null, null, 1, 1, 1, 1, 0, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType, 0)


PRINT 'INSERT MemberStates'
SET @memberStateId = NEWID()
INSERT INTO MemberStates (MemberStateId, ClubId, MemberStateName, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@memberStateId, @insertClubId, 'Aktiv',	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)
SET @memberStateId = NEWID()
INSERT INTO MemberStates (MemberStateId, ClubId, MemberStateName, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@memberStateId, @insertClubId, 'Passiv',	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)
SET @memberStateId = NEWID()
INSERT INTO MemberStates (MemberStateId, ClubId, MemberStateName, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@memberStateId, @insertClubId, 'Schüler',	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)
SET @memberStateId = NEWID()
INSERT INTO MemberStates (MemberStateId, ClubId, MemberStateName, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@memberStateId, @insertClubId, 'Prov. Aktiv',	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)
SET @memberStateId = NEWID()
INSERT INTO MemberStates (MemberStateId, ClubId, MemberStateName, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@memberStateId, @insertClubId, 'Ausgetreten',	 SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType)





-- Aircrafts
PRINT 'INSERT Aircrafts'
INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'Maule Air', 'Maule MX-7', 8, 'HB-KCB', null, 4, 0, 0, 0, 1, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)
INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'Schempp-Hirth', 'Duo Discus', 1, 'HB-3407', 'ZO', 2, 1, 1, 1, 0, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)
INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'DG Flugzeugbau', 'DG-505 Orion', 1, 'HB-3256', 'FF', 2, 1, 1, 1, 0, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)
INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'DG Flugzeugbau', 'LS-4', 1, 'HB-1824', 'FG', 1, 1, 1, 1, 0, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)
INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'DG Flugzeugbau', 'DG-300', 1, 'HB-1841', 'TU', 1, 1, 1, 1, 0, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)
INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'Schempp-Hirth', 'Discus 2cT', 2, 'HB-2464', 'GZ', 1, 1, 1, 1, 0, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)

INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'ASK', 'ASK-21Mi', 2, 'HB-3254', 'MI', 2, 0, 1, 0, 0, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)

INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'DYN''AERO TECNOLOGIA AEROSPACIAL IBÉRICA S.A.', 'MCR-ULC', 8, 'HB-WAT', null, 2, 0, 0, 0, 1, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)

INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'VARGA AIRCRAFT CORPORATION', 'Kachina 2180', 8, 'HB-DGP', null, 2, 0, 0, 0, 1, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)

INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'AVIONS PIERRE ROBIN S.A.', 'ROBIN DR 400/200 R', 8, 'HB-KDO', null, 4, 0, 0, 0, 1, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)

INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'VARGA AIRCRAFT CORPORATION', 'Kachina 2180', 8, 'HB-DCU', null, 2, 0, 0, 0, 1, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)

INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'PIPER AIRCRAFT CORPORATION', 'PA-25-235', 8, 'HB-PFW', null, 1, 0, 0, 0, 1, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)

INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'MAULE AIRCRAFT CORPORATION', 'MX-7-235', 8, 'HB-KIO', null, 4, 0, 0, 0, 1, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)

INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'PIPER AIRCRAFT CORPORATION', 'PA-18-180M', 8, 'HB-PDL', null, 2, 0, 0, 0, 1, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)

INSERT INTO Aircrafts (AircraftId, ManufacturerName, AircraftModel, AircraftType, Immatriculation, CompetitionSign, NrOfSeats, IsTowingOrWinchRequired, 
IsTowingstartAllowed, IsWinchstartAllowed, IsTowingAircraft, 
AircraftOwnerClubId, AircraftOwnerPersonId, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType) 
VALUES (NEWID(), 'AVIONS PIERRE ROBIN S.A.', 'ROBIN DR 400/180 R', 8, 'HB-EQC', null, 4, 0, 0, 0, 1, @insertClubId, null, SYSDATETIME(), @insertUserId, @recordState, @ownerId, @OwnershipType)

UPDATE Aircrafts SET IsFastEntryRecord = 0;



INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Charter Clubflugzeug'
           ,'60'
           ,0
           ,0
           ,0
           ,0
           ,1
           ,0
		   ,1
		   ,1
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)
           
           
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
		   ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Charter Privatflugzeug'
           ,'61'
           ,0
           ,0
           ,0
           ,0
           ,1
           ,0
		   ,1
		   ,1
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)
           
          
          
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Passagierflug mit Gutschein'
           ,'62'
           ,0
           ,0
           ,0
           ,1
           ,1
           ,0
		   ,1
		   ,0
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)
           
           
           
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Passagierflug Bar'
           ,'63'
           ,0
           ,0
           ,0
           ,1
           ,1
           ,0
		   ,1
		   ,0
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)
           
     
     
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Passagierflug Privat'
           ,'65'
           ,0
           ,0
           ,0
           ,1
           ,1
           ,0
		   ,1
		   ,1
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)     
     
         
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Lufttaufe Bar'
           ,'66'
           ,0
           ,0
           ,0
           ,1
           ,1
           ,0
		   ,1
		   ,0
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)     


INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Schnupperflug Bar'
           ,'69'
           ,1
           ,0
           ,0
           ,0
           ,1
           ,0
		   ,1
		   ,0
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)     
		    
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Grundschulung Doppelsteuer'
           ,'70'
           ,1
           ,0
           ,0
           ,0
           ,1
           ,0
		   ,1
		   ,1
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)


		   

INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Weiterbildung Doppelsteuer'
           ,'77'
           ,1
           ,0
           ,0
           ,0
           ,1
           ,0
		   ,1
		   ,1
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)
           
           
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Umschulung neues Flugzeug'
           ,'55'
           ,0
           ,1
           ,0
           ,0
           ,1
           ,0
		   ,0
		   ,1
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)
    
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Jahres-Checkflug'
           ,'78'
           ,1
           ,0
           ,1
           ,0
           ,1
           ,1
		   ,0
		   ,1
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)
           
	
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
		   ,[IsSoloFlight]
		   ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Grundschulung Solo'
           ,'80'
           ,1
           ,0
           ,0
           ,0
           ,1
           ,0
		   ,1
		   ,1
		   ,1
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)
		   
		          
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
		   ,[IsSoloFlight]
		   ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Weiterbildung Solo'
           ,'88'
           ,1
           ,0
           ,0
           ,0
           ,1
           ,1
		   ,1
		   ,1
		   ,1
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)
                      

        
    
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Schleppflug'
           ,'90'
           ,0
           ,0
           ,0
           ,0
           ,0
           ,1
		   ,0
		   ,0
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)     

INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Schleppcheck'
           ,'91'
           ,1
           ,0
           ,1
           ,0
           ,0
           ,1
		   ,0
		   ,0
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)  
		   
		   
		   
INSERT INTO [FlightTypes]
           ([FlightTypeId]
           ,[ClubId]
           ,[FlightTypeName]
           ,[FlightCode]
           ,[InstructorRequired]
           ,[ObserverPilotOrInstructorRequired]
           ,[IsCheckFlight]
           ,[IsPassengerFlight]
           ,[IsForGliderFlights]
		   ,[IsForTowFlights]
		   ,[IsForMotorFlights]
		   ,[IsFlightCostBalanceSelectable]
           ,[CreatedOn]
           ,[CreatedByUserId]
           ,[ModifiedOn]
           ,[ModifiedByUserId]
           ,[DeletedOn]
           ,[DeletedByUserId]
           ,[RecordState]
           ,[OwnerId], [OwnershipType])
     VALUES
           (newid()
           ,@insertClubId
           ,'Marketingflug'
           ,'100'
           ,0
           ,0
           ,0
           ,1
           ,1
           ,0
		   ,1
		   ,0
           ,SYSDATETIME()
           ,@clubUserId
           ,null
           ,null
           ,null
           ,null
           ,0
           ,@ownerId, @OwnershipType)     
     
     
           

PRINT 'INSERT PlanningDayTypes for test club'
DECLARE @planningDayId as uniqueidentifier
SET @planningDayId = NEWID()
DECLARE @flightOperatorAssignmentTypeId as uniqueidentifier
SET @flightOperatorAssignmentTypeId = NEWID()
DECLARE @towingPilotAssignmentTypeId as uniqueidentifier
SET @towingPilotAssignmentTypeId = NEWID()

INSERT INTO [dbo].[PlanningDayAssignmentTypes]
           ([PlanningDayAssignmentTypeId],[AssignmentTypeName],[ClubId],[RequiredNrOfPlanningDayAssignments],
		   [CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (@flightOperatorAssignmentTypeId, 'Segelflugleiter', @insertClubId, 1, SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType, 0)

INSERT INTO [dbo].[PlanningDayAssignmentTypes]
           ([PlanningDayAssignmentTypeId],[AssignmentTypeName],[ClubId],[RequiredNrOfPlanningDayAssignments],
		   [CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (@towingPilotAssignmentTypeId, 'Schlepppilot', @insertClubId, 1, SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType, 0)

INSERT INTO [dbo].[PlanningDayAssignmentTypes]
           ([PlanningDayAssignmentTypeId],[AssignmentTypeName],[ClubId],[RequiredNrOfPlanningDayAssignments],
		   [CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(), 'Fluglehrer', @insertClubId, 1, SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType, 0)




PRINT 'INSERT PlanningDays'
SET @locationId = (SELECT TOP 1 LocationId FROM Locations WHERE IcaoCode = 'LSZK')
INSERT INTO [dbo].[PlanningDays]
           ([PlanningDayId], [ClubId],[Day],[LocationId],[Remarks],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
VALUES	(@planningDayId, @insertClubId, (GETDATE()+1), @locationId, 'Test', SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType, 0)

PRINT 'INSERT PlanningDay Assignments'
INSERT INTO [dbo].[PlanningDayAssignments]
           ([PlanningDayAssignmentId],[AssignedPlanningDayId],[AssignedPersonId],[AssignmentTypeId],[Remarks]
           ,[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@planningDayId
           ,(SELECT TOP 1 PersonId FROM Persons where HasTowPilotLicence = 1)
           ,@towingPilotAssignmentTypeId
           ,'Test for Towing pilot',
           SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType, 0)

INSERT INTO [dbo].[PlanningDayAssignments]
           ([PlanningDayAssignmentId],[AssignedPlanningDayId],[AssignedPersonId],[AssignmentTypeId],[Remarks]
           ,[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@planningDayId
           ,(SELECT TOP 1 PersonId FROM Persons where HasGliderPilotLicence = 1)
           ,@flightOperatorAssignmentTypeId
           ,'Test for Gliderpilots',
           SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType, 0)
    

PRINT 'INSERT PlanningDays'
SET @planningDayId = NEWID()
SET @locationId = (SELECT TOP 1 LocationId FROM Locations WHERE IcaoCode = 'LSZK')
INSERT INTO [dbo].[PlanningDays]
           ([PlanningDayId], [ClubId],[Day],[LocationId],[Remarks],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
VALUES	(@planningDayId, @insertClubId, (GETDATE()+1), @locationId, 'Test2', SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType, 0)

PRINT 'INSERT PlanningDay Assignments'
INSERT INTO [dbo].[PlanningDayAssignments]
           ([PlanningDayAssignmentId],[AssignedPlanningDayId],[AssignedPersonId],[AssignmentTypeId],[Remarks]
           ,[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@planningDayId
           ,(SELECT TOP 1 Persons.PersonId FROM Persons LEFT OUTER JOIN
                      PersonClub ON Persons.PersonId = PersonClub.PersonId 
					  where HasTowPilotLicence = 1 and PersonClub.ClubId = @insertClubId)
           ,@towingPilotAssignmentTypeId
           ,'Test for Towing pilot',
           SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType, 0)

INSERT INTO [dbo].[PlanningDayAssignments]
           ([PlanningDayAssignmentId],[AssignedPlanningDayId],[AssignedPersonId],[AssignmentTypeId],[Remarks]
           ,[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@planningDayId
           ,(SELECT TOP 1 Persons.PersonId FROM Persons LEFT OUTER JOIN
                      PersonClub ON Persons.PersonId = PersonClub.PersonId 
					  where HasGliderPilotLicence = 1 and PersonClub.ClubId = @insertClubId)
           ,@flightOperatorAssignmentTypeId
           ,'Test for Gliderpilots',
           SYSDATETIME(),		 @insertUserId, @recordState, @ownerId, @OwnershipType, 0)
    
GO
PRINT 'INSERT Test Datas Finished'