USE [master]
GO

-- You should change this password here and in the web.config for production release
CREATE LOGIN [flstestdbuser] WITH PASSWORD=N'Test#1234', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

CREATE LOGIN [flstestdbadmin] WITH PASSWORD=N'Test#1234', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

CREATE LOGIN [flstestsyslogwriter] WITH PASSWORD=N'Test#1234', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

CREATE LOGIN [flstestreportinguser] WITH PASSWORD=N'Test#1234', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

USE [FLSTest]
GO
CREATE SCHEMA [flstest_user]
GO
CREATE USER [flstestdbuser] FOR LOGIN [flstestdbuser] WITH DEFAULT_SCHEMA=[flstest_user]
GO
CREATE ROLE [flstest_user]
GO
GRANT EXECUTE, SELECT, UPDATE, INSERT, DELETE TO [flstest_user]
GO
ALTER ROLE [flstest_user] ADD MEMBER [flstestdbuser]
GO

CREATE USER [flstestdbadmin] FOR LOGIN [flstestdbadmin] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [flstestdbadmin]
GO

CREATE SCHEMA [flstest_logwriter]
GO
CREATE USER [flstestsyslogwriter] FOR LOGIN [flstestsyslogwriter] WITH DEFAULT_SCHEMA=[flstest_logwriter]
GO
CREATE ROLE [flstest_logwriter]
GO
GRANT INSERT ON [SystemLogs] TO [flstest_logwriter]
GO
ALTER ROLE [flstest_logwriter] ADD MEMBER [flstestsyslogwriter]
GO

CREATE SCHEMA [flstest_reporting_user]
GO
CREATE USER [flstestreportinguser] FOR LOGIN [flstestreportinguser] WITH DEFAULT_SCHEMA=[flstest_reporting_user]
GO
CREATE ROLE [flstest_reporting_user]
GO
GRANT EXECUTE, SELECT, UPDATE TO [flstest_reporting_user]
GO
ALTER ROLE [flstest_reporting_user] ADD MEMBER [flstestreportinguser]
GO

-- USE [ReportServer]
-- CREATE USER [flsreportinguser] FOR LOGIN [flsreportinguser] WITH DEFAULT_SCHEMA=[RSExecRole]
-- GO
