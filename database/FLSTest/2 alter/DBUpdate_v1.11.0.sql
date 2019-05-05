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
           (47,1,11,0,0
           ,'1.10.7.0'
           ,SYSUTCDATETIME())
GO

PRINT 'Insert Accounting Rule Filter Types'
-- INSERT INTO [dbo].[AccountingRuleFilterTypes] ([AccountingRuleFilterTypeId],[AccountingRuleFilterTypeName],[AccountingRuleFilterTypeKeyName],[CreatedOn],[ModifiedOn])
--      VALUES (5,'Ignore flight rule filter', 'IgnoreFlightRuleFilter', SYSDATETIME(), null)
INSERT INTO [dbo].[AccountingRuleFilterTypes] ([AccountingRuleFilterTypeId],[AccountingRuleFilterTypeName],[AccountingRuleFilterTypeKeyName],[CreatedOn],[ModifiedOn])
     VALUES (55,'Start tax invoice rule filter', 'StartTaxInvoiceRuleFilter', SYSDATETIME(), null)
GO
PRINT 'Finished update to Version 1.10.7'