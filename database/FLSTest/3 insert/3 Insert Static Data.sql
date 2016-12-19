
USE [FLSTest]


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

DELETE FROM InvoiceRuleFilters
DELETE FROM InvoiceRuleFilterTypes

DELETE FROM Articles
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
DELETE FROM FlightAirStates
DELETE FROM FlightValidationStates
DELETE FROM FlightProcessStates
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
DELETE FROM ExtensionValues
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
DELETE FROM CounterUnitTypes

PRINT 'Insert CounterUnitTypes'
INSERT INTO [dbo].[CounterUnitTypes] ([CounterUnitTypeId], [CounterUnitTypeName], [CounterUnitTypeKeyName],[Comment],[IsActive],[CreatedOn])
     VALUES (1, 'Minutes','Min',NULL,1,SYSUTCDATETIME())

INSERT INTO [dbo].[CounterUnitTypes] ([CounterUnitTypeId], [CounterUnitTypeName], [CounterUnitTypeKeyName],[Comment],[IsActive],[CreatedOn])
     VALUES (2, '2 decimals per hour','2decimalsperhour',NULL,1,SYSUTCDATETIME())

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

-- countries

PRINT 'INSERT Countries'
INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'f4b8d4c7-10d1-4b10-9551-0090c518a1cb', 288, N'Ghana', NULL, N'GH', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'6bca5151-e0ee-4ae0-acd3-01bffd2c84d0', 262, N'Dschibuti', NULL, N'DJ', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'98ca8a9b-cf84-45b9-8a7f-0213c198d65d', 148, N'Tschad', NULL, N'TD', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'a7bebefb-5062-45df-a8c9-035c85b6f234', 246, N'Finnland', NULL, N'FI', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'6ddfd8a8-870b-4f41-978b-042da6498adf', 498, N'Moldau', NULL, N'MD', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'8b7b4394-95db-11e0-9bcd-06f14724019b', 276, N'Deutschland', NULL, N'DE', CAST(N'2014-06-02 11:17:44.4587500' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'e9e81d88-ba4b-4090-b34a-07c5ace42f2e', 56, N'Belgien', NULL, N'BE', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'0f0f4f33-2b6e-4648-8a93-07d9c45f2aa3', 616, N'Polen', NULL, N'PL', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9022f82e-95db-11e0-8d18-07f14724019b', 250, N'Frankreich', NULL, N'FR', CAST(N'2014-06-02 11:17:44.4587500' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'95c48d24-95db-11e0-a5e5-08f14724019b', 40, N'Österreich', NULL, N'AT', CAST(N'2014-06-02 11:17:44.4743750' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'249c2ab2-cfe3-4cb5-9679-095d9ac7bddc', 124, N'Kanada', NULL, N'CA', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'1d865573-e533-4d91-954e-09862210dceb', 558, N'Nicaragua', NULL, N'NI', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'd7c78070-0040-4916-90d7-0af72bb72c19', 336, N'Vatikan Stadt', NULL, N'VA', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'cfb25aad-5be1-4ea7-95b8-0b251a714f82', 862, N'Venezuela', NULL, N'VE', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'97333db0-c9ac-49bb-a8f9-0cd9f1029c7b', 320, N'Guatemala', NULL, N'GT', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3f0c69fe-3e3a-465c-ba4d-0d3f8abc08e8', 231, N'Äthiopien', NULL, N'ET', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'7691a9d6-f475-4ac1-9092-0d8eafb497ee', 499, N'Montenegro', NULL, N'ME', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'025780b0-b6e0-4a07-8386-1250b6e27f73', 68, N'Bolivien', NULL, N'BO', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'2e3aa37c-c601-413d-bf69-12b69e6353d5', 64, N'Bhutan', NULL, N'BT', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9b2f555a-95db-11e0-8e8c-15f14724019b', 380, N'Italien', NULL, N'IT', CAST(N'2014-06-02 11:17:44.4743750' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'48e235cd-2552-4dbf-9b87-16713898f407', 410, N'Korea (Republik Korea)', NULL, N'KR', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9f5b2be0-95db-11e0-89e5-19f14724019b', 438, N'Liechtenstein', NULL, N'LI', CAST(N'2014-06-02 11:17:44.4743750' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'17730a36-db7c-4057-9747-1dc32264449c', 784, N'Vereinigte Arabische Emirate', NULL, N'AE', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'bb8fde99-c35c-4376-8ac3-1de07197e878', 196, N'Zypern', NULL, N'CY', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'1c124b30-7d08-497c-bf6a-1e335cbc8156', 624, N'Guinea-Bissau', NULL, N'GW', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'2a5e5f7f-f953-4aa0-ba40-222afa203024', 328, N'Guyana', NULL, N'GY', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'441ed355-dee7-4e9a-947e-2282a8ef4e02', 324, N'Guinea', NULL, N'GN', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9b55e56f-bfc1-43c9-9af0-24111b589062', 8, N'Albanien', NULL, N'AL', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'11b0c76e-c342-4e46-b080-24b44838c3ab', 764, N'Thailand', NULL, N'TH', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'80bf6e31-f2c1-44e3-8b84-25822d3fac30', 524, N'Nepal', NULL, N'NP', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'09a1ef36-9eb3-4f07-ba0d-258e7290c0cd', 882, N'Samoa', NULL, N'WS', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'75a1d74c-4110-4bda-a7c4-262ab406a392', 104, N'Myanmar', NULL, N'MM', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'df19829a-fa56-4d85-9ff0-26eda1242234', 426, N'Lesotho', NULL, N'LS', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'0ee34449-c425-4b1c-8083-283fcf3fe58f', 31, N'Aserbaidschan', NULL, N'AZ', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9a6d9e73-4d01-4e53-b71a-28ee5fab168d', 662, N'St. Lucia', NULL, N'LC', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'64102753-f0c4-4815-a4b5-2aca0f466d3b', 780, N'Trinidad und Tobago', NULL, N'TT', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'21bc87c8-d901-459b-bc84-2b2a1b0519c4', 100, N'Bulgarien', NULL, N'BG', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'b55c6181-f4cf-46d9-b97e-2cef59ec93b2', 344, N'Hongkong', NULL, N'HK', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'd3a1382b-9d53-43b1-b58b-2d704561bd2f', 578, N'Norwegen', NULL, N'NO', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3e600524-7aa9-4d65-92c9-32bb19f44f75', 192, N'Kuba', NULL, N'CU', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'f34a5ab9-bdda-4e61-bed1-33c8380341fa', 626, N'Timor-Leste', NULL, N'TL', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'8aa8f214-0db6-4572-901c-34a8e01c7850', 84, N'Belize', NULL, N'BZ', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'c0e05ec9-656d-4d39-a3d2-35ac03f98868', 642, N'Rumänien', NULL, N'RO', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'f9886b0e-e314-4fdc-8b63-35b4421293e6', 643, N'Russische Föderation', NULL, N'RU', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'a508bc2a-5d54-480b-93a7-378873e94e01', 300, N'Griechenland', NULL, N'GR', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'a46bfbc4-f736-4503-ad31-3d615a038799', 894, N'Sambia', NULL, N'ZM', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'b2a1af0f-8193-4326-bab4-3def13e084d8', 36, N'Australien', NULL, N'AU', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'2fe5b07e-6747-48bb-9b83-3e5150c309e1', 458, N'Malaysia', NULL, N'MY', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'6f7a3dee-6ffd-412e-bffa-3f404ec1bcc0', 414, N'Kuwait', NULL, N'KW', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'33008451-81a3-4fe0-ac61-3f64c1fa1b8a', 740, N'Surinam', NULL, N'SR', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'faf478cc-168c-4206-8f9f-3f9655b43e10', 736, N'Sudan', NULL, N'SD', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'e50f8099-2aca-461e-a798-3ff5ecebb2f0', 24, N'Angola', NULL, N'AO', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'381727b5-e431-47c6-837c-409edf6fc1c4', 72, N'Botsuana', NULL, N'BW', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'288e3f48-5caa-4c5b-bb45-4159dd47dd15', 226, N'Äquatorialguinea', NULL, N'GQ', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'2c7116e1-17ac-42f7-a14a-41931a0da2cf', 48, N'Bahrain', NULL, N'BH', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'448eb197-a78f-45d9-aebd-41ce22225197', 585, N'Palau', NULL, N'PW', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'b1732b06-d0aa-400a-a566-41e46e87b0bb', 44, N'Bahamas', NULL, N'BS', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'ee06b75d-8a21-49f5-8f11-422fb0179c49', 670, N'St. Vincent u. Grenadinen', NULL, N'VC', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9f027152-3113-4419-92e3-479fd649f224', 554, N'Neuseeland', NULL, N'NZ', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'0b53da11-95f3-4e2a-8f1b-483d4c939f59', 646, N'Ruanda', NULL, N'RW', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'8ecdcf12-e38d-409d-9a49-4a48fd36f662', 376, N'Israel', NULL, N'IL', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'55690e5c-70d1-432a-b46d-4be42a8ac958', 484, N'Mexiko', NULL, N'MX', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'bf0132c7-a639-4bf7-8e0a-4d5e6647bac0', 352, N'Island', NULL, N'IS', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'233784f6-ced4-4b8a-bccc-4e8f9041d73d', 795, N'Turkmenistan', NULL, N'TM', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3cb8c6bd-af9d-4826-bbcf-4f4fe5fd260d', 690, N'Seychellen', NULL, N'SC', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'815f43ef-6e3f-49ee-8634-502c3c1c199f', 170, N'Kolumbien', NULL, N'CO', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'c8ee991b-9f37-4e56-aa83-54f478860038', 28, N'Antigua und Barbuda', NULL, N'AG', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'13d7e5c4-6f48-4a61-b3de-55ca3603e6fc', 583, N'Mikronesien', NULL, N'FM', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'be76179e-74ee-45fe-84a0-5a768743f01e', 528, N'Niederlande', NULL, N'NL', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3f75c565-2b68-4837-8061-5ac56ebdbfdd', 470, N'Malta', NULL, N'MT', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'0ac3a3e6-5a5d-41ce-9cd8-5b2dbc1df363', 516, N'Namibia', NULL, N'NA', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'80dc6993-1028-46e1-8595-5d7250c2550c', 840, N'Vereinigte Staaten von Amerika', NULL, N'US', CAST(N'2016-06-06 22:54:16.3073082' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'24cbac02-9fd7-4f25-83d6-5f6ca8c189ea', 52, N'Barbados', NULL, N'BB', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'bac1f0f3-2341-405a-b2b1-60a223df0d82', 776, N'Tonga', NULL, N'TO', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'07d8efe4-ff96-4253-9f26-613647e559d9', 332, N'Haiti', NULL, N'HT', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'84c22af9-e30e-47ae-9b84-6393431bb098', 408, N'Korea (Demokratische Volksrepublik)', NULL, N'KP', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3805a4a8-ae8e-4ed6-868d-63d28a5013d4', 204, N'Benin', NULL, N'BJ', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'f5108330-be96-4ab2-a21f-657c87d2f7b9', 428, N'Lettland', NULL, N'LV', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'827c0333-db81-43dd-beac-6647cf736ffd', 608, N'Philippinen', NULL, N'PH', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'f5d87cb7-fad8-4f9c-a21a-6695a2b809e0', 854, N'Burkina Faso', NULL, N'BF', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'8142372f-e909-4dd4-8cc3-67597a2f1b3b', 232, N'Eritrea', NULL, N'ER', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'45ece737-9af0-4080-b121-67a76e7ea9d4', 430, N'Liberia', NULL, N'LR', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9740665c-1e68-4d4f-90aa-694b86ba99e5', 174, N'Komoren', NULL, N'KM', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'6493f7ea-61f0-4f22-8eee-6b138403e72b', 584, N'Marshall-Inseln', NULL, N'MH', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'c76cd52b-fa70-4443-9e2d-6c5d98547e39', 702, N'Singapur', NULL, N'SG', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'58c4fb47-c142-41df-9d3c-70e3ab97aa0f', 442, N'Luxemburg', NULL, N'LU', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'50d7674a-1781-4779-8cb6-72eb28f47bc7', 520, N'Nauru', NULL, N'NR', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9f92762a-002d-4dd9-9806-73e29b5a248f', 400, N'Jordanien', NULL, N'JO', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'8803eb3c-c84e-4515-ab5a-74682fb52afb', 4, N'Afghanistan', NULL, N'AF', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'f7d1e91f-6f13-42b5-bd79-756f9c1495e0', 364, N'Iran', NULL, N'IR', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3b4de4a9-e69c-48f0-9177-76008a6045d8', 203, N'Tschechische Republik', NULL, N'CZ', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'2d499cb2-1ea0-4ba5-b42e-78628757cc7b', 710, N'Südafrika', NULL, N'ZA', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'd5087551-b3cc-43e4-a560-79d70ddccca7', 858, N'Uruguay', NULL, N'UY', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'06994195-7a38-4ab4-a767-7b863a5f340a', 51, N'Armenien', NULL, N'AM', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'6bca78d6-134f-4f9a-8364-7be9371db767', 222, N'El Salvador', NULL, N'SV', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'b654ec3c-80aa-4904-b238-7d25c0784889', 512, N'Oman', NULL, N'OM', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9c1c34d3-ca00-40be-a53b-7e427cb940a9', 76, N'Brasilien', NULL, N'BR', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'd43e6910-47e2-4226-8137-7ec81fcb2554', 604, N'Peru', NULL, N'PE', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'7d3466ec-ba42-4f61-ba8b-7ed40a133be8', 620, N'Portugal', NULL, N'PT', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'26a02792-87d0-41c4-ad48-8078f6ddb056', 208, N'Dänemark', NULL, N'DK', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'4761f4e8-f584-4526-af3f-80fef47ab1a9', 212, N'Dominica', NULL, N'DM', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'8cc9b57c-79bd-41dc-9a1a-819f1d9017a3', 418, N'Laos', NULL, N'LA', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'baa85619-df7a-4aec-88e5-85bd08be452f', 450, N'Madagaskar', NULL, N'MG', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9c1a31e3-119f-49ed-8e50-87ae8863d13f', 504, N'Marokko', NULL, N'MA', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'66605991-e0e8-4afe-ae6d-893fe2403fc8', 586, N'Pakistan', NULL, N'PK', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'e82101b9-b159-41f8-952c-8a01843ada57', 600, N'Paraguay', NULL, N'PY', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'a6a113e6-07e5-4e9c-a571-8ae50549eed9', 462, N'Malediven', NULL, N'MV', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'ee482ba1-aa16-4d70-8165-8b109a6817ef', 152, N'Chile', NULL, N'CL', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'ebd0ce4a-2865-49b2-bb02-8b90a14d2d9f', 398, N'Kasachstan', NULL, N'KZ', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'b8aa42ba-b9fb-4e88-b0bb-8de4c658e627', 308, N'Grenada', NULL, N'GD', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'04fc9e8a-ddd4-45ee-9d35-8f78ce807e03', 678, N'Sao Tomé und Principe', NULL, N'ST', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'c2cae6ba-b059-44db-a4c9-8f9133e82bfa', 492, N'Monaco', NULL, N'MC', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'240f2251-42e6-4996-9508-8f9de205f84b', 90, N'Salomonen', NULL, N'SB', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3419ef06-a21a-4844-8e6f-95855139b106', 50, N'Bangladesch', NULL, N'BD', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'f2ba9a18-0bc1-4118-86a9-958695802db9', 694, N'Sierra Leone', NULL, N'SL', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'0c13e846-9c81-4ef6-bc2d-958923810e07', 132, N'Kap Verde', NULL, N'CV', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'93e2e7c7-8531-4ddc-a7d7-988a8d266f60', 372, N'Irland', NULL, N'IE', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'2d73b7f2-b9fe-482e-896a-994b29ec3f7f', 804, N'Ukraine', NULL, N'UA', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'b3f3cddf-6d14-4b59-af64-9a0c1c244e68', 242, N'Fidschi', NULL, N'FJ', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'73523c73-33e7-4b23-9632-9e2281f29a84', 686, N'Senegal', NULL, N'SN', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9c3077e6-64f4-49b5-b75d-9efbe79d7aeb', 218, N'Ecuador', NULL, N'EC', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'b8f41430-5518-4df8-8176-a11658a48832', 360, N'Indonesien', NULL, N'ID', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'780ea22d-deab-4445-b81a-a1376b9735b9', 798, N'Tuvalu', NULL, N'TV', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'94f60836-d633-4209-ba01-a55fee64118f', 724, N'Spanien', NULL, N'ES', CAST(N'2016-06-07 15:09:50.7186659' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'cfc42001-4708-4add-b013-a78f80bef6d8', 70, N'Bosnien-Herzegowina', NULL, N'BA', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'758bcc63-cbc3-4c4f-81ce-a9b91cfd8934', 807, N'Mazedonien', NULL, N'MK', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'930b59d1-cd7c-435c-a4fd-aa748ab13e4d', 446, N'Macau', NULL, N'MO', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3f519aa4-c23a-46f9-9658-abeb88b81b61', 466, N'Mali', NULL, N'ML', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'd7a55395-12dd-404a-8b21-ac10e65c95c7', 752, N'Schweden', NULL, N'SE', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'0de09a94-b327-4bc4-bf73-b0d54fe128ab', 180, N'Demokratische Republik Kongo', NULL, N'CD', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3a148ec0-1e72-4d24-bb85-b0efe8a12121', 188, N'Costa Rica', NULL, N'CR', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'cdb15347-28d5-43c3-8602-b55ab087390b', 268, N'Georgien', NULL, N'GE', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'a7efbde2-2590-4bb6-ba4e-b6bad0fb09dc', 860, N'Usbekistan', NULL, N'UZ', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'1713d313-22e0-4b10-9767-b7abe349a454', 178, N'Kongo', NULL, N'CG', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3d958c47-ddaa-421b-bdca-b936e607fcb9', 434, N'Libyen', NULL, N'LY', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3beef29e-4f07-4e60-974d-bb227d8c465e', 417, N'Kirgisistan', NULL, N'KG', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'0335cb71-00fc-48ce-b0ec-be6ee07b1188', 158, N'Taiwan', NULL, N'TW', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'd6660831-2295-4140-b6dc-be9b4eee1005', 762, N'Tadschikistan', NULL, N'TJ', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'22912d26-26cb-4a83-8553-c168dec0bbfc', 356, N'Indien', NULL, N'IN', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'feda4536-37d6-40b1-928f-c189d8497ae9', 108, N'Burundi', NULL, N'BI', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'2f6814f2-f4ab-4763-8536-c2853c011b0b', 760, N'Syrien', NULL, N'SY', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'8c3b8f88-0ccb-4343-8f70-c433d8ab63fb', 706, N'Somalia', NULL, N'SO', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'90eb2310-6e4b-454c-8720-c48f8b480e2e', 548, N'Vanuatu', NULL, N'VU', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'b5f8e986-8ecb-481e-a998-c56f7cf7dfce', 496, N'Mongolei', NULL, N'MN', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'4093f07e-59aa-4180-9fbd-c7550c9af165', 887, N'Jemen', NULL, N'YE', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'7d0d1008-92a3-406a-a4f5-c7a8b42ee7be', 348, N'Ungarn', NULL, N'HU', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'da88a1c8-7331-45cc-80fc-cb1ac634bd6c', 340, N'Honduras', NULL, N'HN', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'e3b3f66d-67ff-4c57-977a-cb7af6265307', 716, N'Simbabwe', NULL, N'ZW', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'fccb7767-3199-4fe4-bcca-ccf28e1caf1c', 566, N'Nigeria', NULL, N'NG', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'97fbb6af-4e6c-4c31-b190-cdb3ffbcba10', 296, N'Kiribati', NULL, N'KI', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'416dd235-135a-40cc-b490-ce6528ecfa34', 266, N'Gabun', NULL, N'GA', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'6aa6c73d-84f0-4d6d-9022-ce912d34c16c', 440, N'Litauen', NULL, N'LT', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'78d5ee52-c6d7-4f1f-a9d5-d05e3a8b7892', 214, N'Dominikanische Republik', NULL, N'DO', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'55054ebe-adcd-4c75-8f78-d0d67fadecdd', 598, N'Papua-Neuguinea', NULL, N'PG', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'59d0d412-ed80-42ce-bed3-d15c48603b12', 96, N'Brunei Darussalam', NULL, N'BN', CAST(N'2016-06-07 22:53:46.8395236' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'6d3dbf5f-1dac-41a8-8ca1-d2d31f5a08e0', 140, N'Zentralafrikanische Republik', NULL, N'CF', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9b48f936-fff9-48a3-815e-d2e4836fcbd5', 788, N'Tunesien', NULL, N'TN', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'8ab09b4b-df48-45ad-a46b-d4346549a75d', 480, N'Mauritius', NULL, N'MU', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'21fc8a20-7282-4a6f-b439-d453585ab8ca', 659, N'St. Kitts und Nevis', NULL, N'KN', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'cc63345c-ad03-4d65-bb58-d4a6c73688bf', 233, N'Estland', NULL, N'EE', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'68aeb0e4-a411-4403-82c7-d55a191ac0b5', 392, N'Japan', NULL, N'JP', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'bf1ea542-8239-4ad4-88f0-d59f7861617d', 562, N'Niger', NULL, N'NE', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'b94b205d-9cbf-4838-8a01-d7652d59485c', 705, N'Slowenien', NULL, N'SI', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'19336607-3433-4e84-89d1-d7fdf7a4981f', 191, N'Kroatien', NULL, N'HR', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'bc999595-1dff-4303-b201-d81fed498065', 834, N'Tansania', NULL, N'TZ', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'c9e168ad-02f3-4c5e-ba54-d873996735c9', 508, N'Mosambik', NULL, N'MZ', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'7fe3f6c8-0bae-4796-b2a9-da333305e36b', 388, N'Jamaika', NULL, N'JM', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'dea41e0d-410c-44bd-935d-dab1d0e5449d', 32, N'Argentinien', NULL, N'AR', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'882f6da3-19f7-4edf-9006-dcbd2815fd47', 688, N'Serbien', NULL, N'RS', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'12507b0a-36fd-4d55-b8b0-dd1d028fdb29', 144, N'Sri Lanka', NULL, N'LK', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'98800033-7d36-4925-8778-de70d15ce5aa', 591, N'Panama', NULL, N'PA', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'6c3d605e-4312-40d1-bc51-e0ab493acedc', 818, N'Ägypten', NULL, N'EG', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'4aa74c11-b362-4a15-a173-e22d08b1bdb4', 768, N'Togo', NULL, N'TG', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'ac9dbc6a-6c45-4204-8d3b-e231714eeb53', 800, N'Uganda', NULL, N'UG', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'4b2939dd-9ca7-4885-b5a9-e50ef4455c4f', 116, N'Kambodscha', NULL, N'KH', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3006e026-e141-4be6-af28-e531391b5dda', 20, N'Andorra', NULL, N'AD', CAST(N'2016-06-07 22:53:46.8238944' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'ec1059fd-1986-43bc-bcec-e567ec36f630', 478, N'Mauretanien', NULL, N'MR', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'77cc3be6-95db-11e0-b104-e7f04724019b', 756, N'Schweiz', NULL, N'CH', CAST(N'2014-06-02 11:17:44.4587500' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'c7b01798-0250-40a1-bfd5-e903be41905e', 404, N'Kenia', NULL, N'KE', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'340000e2-0f68-4670-b375-ed052a75c468', 748, N'Swasiland', NULL, N'SZ', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'9a73c060-9785-481a-8e75-edd37b67d6bc', 422, N'Libanon', NULL, N'LB', CAST(N'2016-06-07 22:53:46.8863938' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'62294a63-2bd0-4488-be63-edf2fc3b50ee', 682, N'Saudi-Arabien', NULL, N'SA', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'8a8bbb22-60e1-41b9-ab97-f099b60094d8', 156, N'China', NULL, N'CN', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'aff3d2b3-6038-457f-9736-f16623e43ee5', 112, N'Belarus', NULL, N'BY', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'd7726c11-27b9-4a65-ab06-f2ffb5cfed07', 703, N'Slowakische Republik', NULL, N'SK', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'81859e95-5456-4eb9-bdc0-f5105c65fddd', 634, N'Katar', NULL, N'QA', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'727cf4ba-b641-469b-80cc-f571135a7508', 792, N'Türkei', NULL, N'TR', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'685898a9-1079-4567-b4c1-f66a141e6f7e', 368, N'Irak', NULL, N'IQ', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'5403d1e8-1135-4536-918f-f688ab9591df', 270, N'Gambia', NULL, N'GM', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'149f80ee-5d01-4a8d-9e12-f7b5ed05eede', 384, N'Côte d''Ivoire', NULL, N'CI', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'3db2066d-b99a-4c10-8868-f864b8356a8d', 826, N'Vereinigtes Königreich Großbritannien und Nordirland', NULL, N'GB', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'c92cfc1c-3532-4556-9a9a-fa058b4a5fca', 704, N'Vietnam', NULL, N'VN', CAST(N'2016-06-07 22:53:47.0114076' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'130c6966-52e3-4595-ba84-fb6db8cf3cf9', 454, N'Malawi', NULL, N'MW', CAST(N'2016-06-07 22:53:46.9020133' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'a96137cb-85ea-404b-ab22-fe5b7fcca840', 674, N'San Marino', NULL, N'SM', CAST(N'2016-06-07 22:53:46.9176510' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'b3e7a4d8-c79b-4b2b-a4d1-ff4b83efcb53', 12, N'Algerien', NULL, N'DZ', CAST(N'2016-06-07 22:53:46.8707785' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)

INSERT [dbo].[Countries] ([CountryId], [CountryIdIso], [CountryName], [CountryFullName], [CountryCodeIso2], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted]) VALUES (N'477da672-e9df-4114-b6d5-ff7045a62d10', 120, N'Kamerun', NULL, N'CM', CAST(N'2016-06-07 22:53:46.8552074' AS DateTime2), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0)



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
INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn], [HasEngine], [RequiresTowingInfo], [MayBeTowingAircraft])
     VALUES (0, 'Unbekannt', 'Flugzeugtyp ist nicht bekannt / nicht gesetzt', SYSDATETIME(), null, null, null)

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn], [HasEngine], [RequiresTowingInfo], [MayBeTowingAircraft])
     VALUES (1, 'Segelflugzeug', 'Reines Segelflugzeug ohne Motor oder Turbo', SYSDATETIME(), 0, 1, 0)

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn], [HasEngine], [RequiresTowingInfo], [MayBeTowingAircraft])
     VALUES (2, 'Segelflugzeug mit Motor', 'Segelflugzeug mit Motor oder Turbo', SYSDATETIME(), 1, 1, 0)

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn], [HasEngine], [RequiresTowingInfo], [MayBeTowingAircraft])
     VALUES (4, 'Motorsegelflugzeug', 'Motorsegelflugzeug', SYSDATETIME(), 1, 0, 1)

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn], [HasEngine], [RequiresTowingInfo], [MayBeTowingAircraft])
     VALUES (8, 'Motorflugzeug', 'Motorflugzeug oder Schleppflugzeug', SYSDATETIME(), 1, 0, 1)

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn], [HasEngine], [RequiresTowingInfo], [MayBeTowingAircraft])
     VALUES (16, 'Multi-Motorflugzeug', 'Motorflugzeug mit mehr als einem Motor', SYSDATETIME(), 1, 0, 0)

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn], [HasEngine], [RequiresTowingInfo], [MayBeTowingAircraft])
     VALUES (32, 'Jet', 'Jet', SYSDATETIME(), 1, 0, 0)

INSERT INTO [AircraftTypes] ([AircraftTypeId], [AircraftTypeName], [Comment], [CreatedOn], [HasEngine], [RequiresTowingInfo], [MayBeTowingAircraft])
     VALUES (64, 'Helikopter', 'Helikopter', SYSDATETIME(), 1, 0, 0)
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
           ,0, 0, 0, 0
           ,SYSDATETIME())


INSERT INTO [FlightCostBalanceTypes] ([FlightCostBalanceTypeId],[FlightCostBalanceTypeName]
           ,[Comment],
		   [PersonForInvoiceRequired],[IsForGliderFlights],[IsForTowFlights],[IsForMotorFlights],[CreatedOn])
     VALUES
           (3,'Schlepppilot übernimmt Schleppkosten',
		   'Rechnung mit den Segelflugkosten inkl. Landetaxen geht an den Segelflugpiloten, Schleppkosten werden dem Schlepppiloten in Rechnung gestellt.'
           ,0, 0, 0, 0
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

INSERT [dbo].[FlightAirStates] ([FlightAirStateId], [FlightAirStateName], [Comment], [CreatedOn]) VALUES (0, N'Neu', N'Neuer Flug / Noch nicht gestartet', SYSUTCDATETIME())

INSERT [dbo].[FlightAirStates] ([FlightAirStateId], [FlightAirStateName], [Comment], [CreatedOn]) VALUES (5, N'Flugplan eröffnet', N'Flugplan eröffnet', SYSUTCDATETIME())

INSERT [dbo].[FlightAirStates] ([FlightAirStateId], [FlightAirStateName], [Comment], [CreatedOn]) VALUES (10, N'Gestartet', N'Flugzeug gestartet / in der Luft', SYSUTCDATETIME())

INSERT [dbo].[FlightAirStates] ([FlightAirStateId], [FlightAirStateName], [Comment], [CreatedOn]) VALUES (20, N'Gelandet', N'Flugzeug gelandet', SYSUTCDATETIME())

INSERT [dbo].[FlightAirStates] ([FlightAirStateId], [FlightAirStateName], [Comment], [CreatedOn]) VALUES (25, N'Geschlossen', N'Flug/Flugplan geschlossen', SYSUTCDATETIME())


INSERT [dbo].[FlightValidationStates] ([FlightValidationStateId], [FlightValidationStateName], [Comment], [CreatedOn]) VALUES (0, N'Nicht validiert', N'Flug wurde noch nicht validiert', SYSUTCDATETIME())

INSERT [dbo].[FlightValidationStates] ([FlightValidationStateId], [FlightValidationStateName], [Comment], [CreatedOn]) VALUES (28, N'Ungültig', N'Flug wurde validiert, Angaben sind aber ungültig oder nicht plausibel', SYSUTCDATETIME())

INSERT [dbo].[FlightValidationStates] ([FlightValidationStateId], [FlightValidationStateName], [Comment], [CreatedOn]) VALUES (30, N'Gültig', N'Flug wurde validiert und Angaben zum Flug sind gültig', SYSUTCDATETIME())


INSERT [dbo].[FlightProcessStates] ([FlightProcessStateId], [FlightProcessStateName], [Comment], [CreatedOn]) VALUES (0, N'Kein Prozess gelaufen', N'Für diesen Flug war noch kein Prozess gelaufen', SYSUTCDATETIME())

INSERT [dbo].[FlightProcessStates] ([FlightProcessStateId], [FlightProcessStateName], [Comment], [CreatedOn]) VALUES (40, N'Gesperrt', N'Flug kann nicht mehr editiert werden und ist für Verrechnung bereit', SYSUTCDATETIME())

INSERT [dbo].[FlightProcessStates] ([FlightProcessStateId], [FlightProcessStateName], [Comment], [CreatedOn]) VALUES (45, N'Geliefert', N'Flug wurde mit Lieferschein abgebucht und kann nicht mehr editiert werden', SYSUTCDATETIME())

INSERT [dbo].[FlightProcessStates] ([FlightProcessStateId], [FlightProcessStateName], [Comment], [CreatedOn]) VALUES (50, N'Verrechnet', N'Flug wurde verrechnet und kann nicht mehr editiert werden', SYSUTCDATETIME())

INSERT [dbo].[FlightProcessStates] ([FlightProcessStateId], [FlightProcessStateName], [Comment], [CreatedOn]) VALUES (55, N'Teilweise bezahlt', N'Flug wurde verrechnet und einen Teil der Rechnung(en) wurde bezahlt.', SYSUTCDATETIME())

INSERT [dbo].[FlightProcessStates] ([FlightProcessStateId], [FlightProcessStateName], [Comment], [CreatedOn]) VALUES (60, N'Bezahlt', N'Flug wurde bezahlt.', SYSUTCDATETIME())



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

-- Password for User workflow is "w"
SET @workflowUserId = NEWID()
INSERT INTO Users (UserId, ClubId, Username, FriendlyName, PasswordHash, EmailConfirmed, SecurityStamp, PersonId, NotificationEmail, AccountState, CreatedOn, CreatedByUserId, RecordState, OwnerId, OwnershipType)
VALUES	(@workflowUserId, @insertClubId, 'workflow', 'Workflow User', 'APa6nEYE2Cd4IaqSyrVMJK4uEiuT8wf9QBubmgWGRs6myd2aPkrwLRyHSZfTM+OS0A==', 1, NEWID(), null, 'test@glider-fls.ch', 1,  SYSDATETIME(),		 @insertUserId, @recordState, @insertUserId, @ownershipType)




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


PRINT 'INSERT InvoiceRuleFilterTypes'
--SET IDENTITY_INSERT [InvoiceRuleFilterTypes] ON
INSERT INTO [dbo].[InvoiceRuleFilterTypes]
           ([InvoiceRuleFilterTypeId],[InvoiceRuleFilterTypeName], [InvoiceRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (1,'Recipient invoice rule filter', 'RecipientInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[InvoiceRuleFilterTypes]
           ([InvoiceRuleFilterTypeId],[InvoiceRuleFilterTypeName], [InvoiceRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (2,'Aircraft invoice rule filter', 'AircraftInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[InvoiceRuleFilterTypes]
           ([InvoiceRuleFilterTypeId],[InvoiceRuleFilterTypeName], [InvoiceRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (3,'Additional fuel fee invoice rule filter', 'AdditionalFuelFeeInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[InvoiceRuleFilterTypes]
           ([InvoiceRuleFilterTypeId],[InvoiceRuleFilterTypeName], [InvoiceRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (4,'Instructor fee invoice rule filter', 'InstructorFeeInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[InvoiceRuleFilterTypes]
           ([InvoiceRuleFilterTypeId],[InvoiceRuleFilterTypeName], [InvoiceRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (5,'Landing tax invoice rule filter', 'LandingTaxInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[InvoiceRuleFilterTypes]
           ([InvoiceRuleFilterTypeId],[InvoiceRuleFilterTypeName], [InvoiceRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (6,'No landing tax invoice rule filter', 'NoLandingTaxInvoiceRuleFilter', SYSDATETIME(), null)

INSERT INTO [dbo].[InvoiceRuleFilterTypes]
           ([InvoiceRuleFilterTypeId],[InvoiceRuleFilterTypeName], [InvoiceRuleFilterTypeKeyName]
           ,[CreatedOn],[ModifiedOn])
     VALUES
           (7,'VSF fee invoice rule filter', 'VsfFeeInvoiceRuleFilter', SYSDATETIME(), null)

--SET IDENTITY_INSERT [InvoiceRuleFilterTypes] OFF

PRINT 'INSERT Static Data Finished'
