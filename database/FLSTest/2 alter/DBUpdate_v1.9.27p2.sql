USE [FLSTest]
GO

-- set IsActive = 1 for persons with member number, otherwise Proffix interface will update it which results in an AuditLog entry
UPDATE [dbo].[EmailTemplates]
SET [HtmlBody] = REPLACE([HtmlBody], 'Eingeteilter Segel2. Flugbesatzung: $!PlanningDayInfoModel.SecondCrewName<br><br>', 'Eingeteilter Segelfluglehrer: $!PlanningDayInfoModel.InstructorName<br><br>')
WHERE [EmailTemplateKeyName] in ('planningday-ok', 'planningday-cancel')

GO


