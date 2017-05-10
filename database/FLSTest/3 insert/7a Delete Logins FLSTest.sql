USE [FLSTest]
GO

/****** Object:  User [flsdbadmin]    Script Date: 10.05.2017 09:06:46 ******/
DROP USER [flsdbadmin]
DROP USER [flsdbuser]
DROP USER [flsreportinguser]
DROP USER [flssyslogwriter]

DROP ROLE [fls_logwriter]
DROP ROLE [fls_reporting_user]
DROP ROLE [fls_user]

DROP SCHEMA [fls_logwriter]
DROP SCHEMA [fls_reporting_user]
DROP SCHEMA [fls_user]
GO

USE [master]
DROP LOGIN [flstestdbadmin]
DROP LOGIN [flstestdbuser]
DROP LOGIN [flstestreportinguser]
DROP LOGIN [flstestsyslogwriter]
GO