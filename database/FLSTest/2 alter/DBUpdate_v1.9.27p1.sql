USE [FLSTest]
GO

-- set IsActive = 1 for persons with member number, otherwise Proffix interface will update it which results in an AuditLog entry
UPDATE [dbo].[PersonClub]
   SET [IsActive] = 1
 WHERE MemberNumber is not null and IsDeleted = 0 and OwnerId = '0FA7B76F-47BA-4138-8F96-671400E37C83'
GO


