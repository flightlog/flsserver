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
           (43,1,10,5,0
           ,'1.9.28.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify Deliveries table'
ALTER TABLE [dbo].[Deliveries] 
	ADD [RecipientName] [nvarchar](250) NULL,
	[RecipientLastname] [nvarchar](100) NULL,
	[RecipientFirstname] [nvarchar](100) NULL,
	[RecipientAddressLine1] [nvarchar](200) NULL,
	[RecipientAddressLine2] [nvarchar](200) NULL,
	[RecipientZipCode] [nvarchar](10) NULL,
	[RecipientCity] [nvarchar](100) NULL,
	[RecipientCountryName] [nvarchar](100) NULL,
	[RecipientPersonId] [uniqueidentifier] NULL,
	[RecipientPersonClubMemberNumber] [nvarchar](20) NULL
GO

UPDATE [dbo].[Deliveries]
   SET [RecipientName] = (SELECT REPLACE(SUBSTRING([RecipientDetails], CHARINDEX('RecipientName":',[RecipientDetails],0)+15, CHARINDEX(',"Firstname":',[RecipientDetails],0) - CHARINDEX('RecipientName":',[RecipientDetails],0)-15), '"','')),
   [RecipientLastname] = (SELECT REPLACE(SUBSTRING([RecipientDetails], CHARINDEX('Lastname":',[RecipientDetails],0)+10, CHARINDEX(',"AddressLine1":',[RecipientDetails],0) - CHARINDEX('Lastname":',[RecipientDetails],0)-10), '"','')),
[RecipientFirstname] = (SELECT REPLACE(SUBSTRING([RecipientDetails], CHARINDEX('Firstname":',[RecipientDetails],0)+11, CHARINDEX(',"Lastname":',[RecipientDetails],0) - CHARINDEX('Firstname":',[RecipientDetails],0)-11), '"','')),
[RecipientAddressLine1] = (SELECT REPLACE(SUBSTRING([RecipientDetails], CHARINDEX('AddressLine1":',[RecipientDetails],0)+14, CHARINDEX(',"AddressLine2":',[RecipientDetails],0) - CHARINDEX('AddressLine1":',[RecipientDetails],0)-14), '"','')),
[RecipientAddressLine2] = (SELECT REPLACE(SUBSTRING([RecipientDetails], CHARINDEX('AddressLine2":',[RecipientDetails],0)+14, CHARINDEX(',"ZipCode":',[RecipientDetails],0) - CHARINDEX('AddressLine2":',[RecipientDetails],0)-14), '"','')),
[RecipientZipCode] = (SELECT REPLACE(SUBSTRING([RecipientDetails], CHARINDEX('ZipCode":',[RecipientDetails],0)+9, CHARINDEX(',"City":',[RecipientDetails],0) - CHARINDEX('ZipCode":',[RecipientDetails],0)-9), '"','')),
[RecipientCity] = (SELECT REPLACE(SUBSTRING([RecipientDetails], CHARINDEX('City":',[RecipientDetails],0)+6, CHARINDEX(',"CountryName":',[RecipientDetails],0) - CHARINDEX('City":',[RecipientDetails],0)-6), '"','')),
[RecipientCountryName] = (SELECT REPLACE(SUBSTRING([RecipientDetails], CHARINDEX('CountryName":',[RecipientDetails],0)+13, CHARINDEX(',"PersonClubMemberNumber":',[RecipientDetails],0) - CHARINDEX('CountryName":',[RecipientDetails],0)-13), '"','')),
[RecipientPersonClubMemberNumber] = (SELECT REPLACE(SUBSTRING([RecipientDetails], CHARINDEX('PersonClubMemberNumber":',[RecipientDetails],0)+24, CHARINDEX('}',[RecipientDetails],0) - CHARINDEX('PersonClubMemberNumber":',[RecipientDetails],0)-24), '"','')),
[RecipientPersonId] = 
CASE WHEN 
(SELECT REPLACE(SUBSTRING([RecipientDetails], CHARINDEX('PersonId":',[RecipientDetails],0)+10, CHARINDEX(',"RecipientName":',[RecipientDetails],0) - CHARINDEX('PersonId":',[RecipientDetails],0)-10), '"','')) = 'null'
THEN NULL 
ELSE
(CONVERT(uniqueidentifier, (SELECT REPLACE(SUBSTRING([RecipientDetails], CHARINDEX('PersonId":',[RecipientDetails],0)+10, CHARINDEX(',"RecipientName":',[RecipientDetails],0) - CHARINDEX('PersonId":',[RecipientDetails],0)-10), '"',''))))
END
GO

UPDATE [dbo].[Deliveries]
   SET [RecipientLastname] = NULL 
   WHERE [RecipientLastname] = 'null'

UPDATE [dbo].[Deliveries]
   SET [RecipientFirstname] = NULL 
   WHERE [RecipientFirstname] = 'null'

UPDATE [dbo].[Deliveries]
   SET [RecipientAddressLine1] = NULL 
   WHERE [RecipientAddressLine1] = 'null'

UPDATE [dbo].[Deliveries]
   SET [RecipientAddressLine2] = NULL 
   WHERE [RecipientAddressLine2] = 'null'

UPDATE [dbo].[Deliveries]
   SET [RecipientZip] = NULL 
   WHERE [RecipientZip] = 'null'

UPDATE [dbo].[Deliveries]
   SET [RecipientCity] = NULL 
   WHERE [RecipientCity] = 'null'

UPDATE [dbo].[Deliveries]
   SET [RecipientCountryName] = NULL 
   WHERE [RecipientCountryName] = 'null'

UPDATE [dbo].[Deliveries]
   SET [RecipientPersonClubMemberNumber] = NULL 
   WHERE [RecipientPersonClubMemberNumber] = 'null'

UPDATE [dbo].[Deliveries]
   SET [RecipientName] = NULL 
   WHERE [RecipientName] = 'null'

PRINT 'Finished update to Version 1.10.5'