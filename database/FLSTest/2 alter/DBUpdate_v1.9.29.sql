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
           (43,1,9,29,0
           ,'1.9.28.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Modify Clubs table'
ALTER TABLE [dbo].[Clubs] 
	ADD [SendPassengerFlightRegistrationOperatorEmailTo] [nvarchar](250) NULL
GO


PRINT 'Insert new email templates for passenger flight registration'

INSERT [dbo].[EmailTemplates] ([EmailTemplateId], [ClubId], [EmailTemplateName], [EmailTemplateKeyName], [Description], [FromAddress], [ReplyToAddresses], [Subject], [IsSystemTemplate], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted], [HtmlBody], [TextBody], [IsCustomizable]) 
VALUES (NEWID(), NULL, N'Passenger flight registration confirmation email for passenger', N'PassengerFlightRegistrationEmailForPassenger', N'Sends a registration confirmation email to the passenger.', N'fls@glider-fls.ch', N'noreply@glider-fls.ch', N'Bestätigung für Passagierflug-Registrierung', 1, SYSUTCDATETIME(), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0, N'<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
<meta content="en-gb" http-equiv="Content-Language" />
<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
<title>Bestätigung für Passagierflug-Registrierung</title>
<style type="text/css">

body {
    font-family: Arial, Helvetica, Sans-Serif;
    font-size: 14px;
}
</style>
</head>

<body>
<p>
Hallo $PassengerFlightRegistrationModel.RecipientName
</p>
<p>
Danke für Ihre Anmeldung zu einem Passagierflug.
</p>
<p>
Dies ist eine Bestätigung für die Registrierung zu einem Passagierflug.</p>
<p>
Angaben zum Passagier:<br>
$PassengerFlightRegistrationModel.Lastname $PassengerFlightRegistrationModel.Firstname<br>
$PassengerFlightRegistrationModel.AddressLine1<br>
$PassengerFlightRegistrationModel.ZipCode $PassengerFlightRegistrationModel.City<br>
$!TrialFlightRegistrationModel.PrivateEmail<br>
Tel. Mobil: $!TrialFlightRegistrationModel.MobilePhoneNumber<br>
Tel. Privat: $!TrialFlightRegistrationModel.PrivatePhoneNumber<br>
Tel. Geschäft: $!TrialFlightRegistrationModel.BusinessPhoneNumber<br>
</p>
<p>
Bemerkungen: $!TrialFlightRegistrationModel.Remarks
</p>

<p>
Die Rechnung wird an folgende Adresse gesendet:<br>
$PassengerFlightRegistrationModel.InvoiceToLastname $PassengerFlightRegistrationModel.InvoiceToFirstname<br>
$PassengerFlightRegistrationModel.InvoiceToAddressLine1<br>
$PassengerFlightRegistrationModel.InvoiceToZipCode $PassengerFlightRegistrationModel.InvoiceToCity<br>
</p>

<p>
Der Gutschein wird gesendet an: $PassengerFlightRegistrationModel.SendCouponToInformation
</p>

<p>Herzliche Grüsse<br>
Flugsportgruppe Zürcher Oberland</p>

</body>

</html>
', NULL, 1)
GO


INSERT [dbo].[EmailTemplates] ([EmailTemplateId], [ClubId], [EmailTemplateName], [EmailTemplateKeyName], [Description], [FromAddress], [ReplyToAddresses], [Subject], [IsSystemTemplate], [CreatedOn], [CreatedByUserId], [ModifiedOn], [ModifiedByUserId], [DeletedOn], [DeletedByUserId], [RecordState], [OwnerId], [OwnershipType], [IsDeleted], [HtmlBody], [TextBody], [IsCustomizable]) 
VALUES (NEWID(), NULL, N'New passenger flight registration', N'NewPassengerFlightRegistrationEmail', N'Sends a passenger registration information to the organisator.', N'fls@glider-fls.ch', N'noreply@glider-fls.ch', N'Neue Passagierflug-Registrierung', 1, SYSUTCDATETIME(), N'13731ee2-c1d8-455c-8ad1-c39399893fff', NULL, NULL, NULL, NULL, 1, N'a1dde2cb-6326-4bb2-897d-7cfc118e842b', 2, 0, N'<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
<meta content="en-gb" http-equiv="Content-Language" />
<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
<title>Neue Passagierflug-Registrierung</title>
<style type="text/css">

body {
    font-family: Arial, Helvetica, Sans-Serif;
    font-size: 14px;
}
</style>
</head>

<body>
<p>
Hallo
</p>
<p>
Es hat eine neue Registrierung zu einem Passagierflug gegeben.</p>

<p>
Angaben zum Passagier:<br>
$PassengerFlightRegistrationModel.Lastname $PassengerFlightRegistrationModel.Firstname<br>
$PassengerFlightRegistrationModel.AddressLine1<br>
$PassengerFlightRegistrationModel.ZipCode $PassengerFlightRegistrationModel.City<br>
$!TrialFlightRegistrationModel.PrivateEmail<br>
Tel. Mobil: $!TrialFlightRegistrationModel.MobilePhoneNumber<br>
Tel. Privat: $!TrialFlightRegistrationModel.PrivatePhoneNumber<br>
Tel. Geschäft: $!TrialFlightRegistrationModel.BusinessPhoneNumber<br>
</p>

<p>
Rechnung an:<br>
$PassengerFlightRegistrationModel.InvoiceToLastname $PassengerFlightRegistrationModel.InvoiceToFirstname<br>
$PassengerFlightRegistrationModel.InvoiceToAddressLine1<br>
$PassengerFlightRegistrationModel.InvoiceToZipCode $PassengerFlightRegistrationModel.InvoiceToCity<br>
</p>

<p>
Gutschein an:<br>
$PassengerFlightRegistrationModel.SendCouponToInformation
</p>

<p>
Bemerkungen: $!TrialFlightRegistrationModel.Remarks
</p>

<p>
Bitte Passagierflug-Gutschein und Rechnung entsprechend ausstellen. Die Adressen sind bereits im FLS gespeichert und müssen nur noch ins Proffix synchronisiert werden, um die Rechnung auszustellen.
</p>

<p>Herzliche Grüsse</p>
<p>Flight Logging System</p>

</body>

</html>
', NULL, 1)
PRINT 'Finished update to Version 1.9.29'