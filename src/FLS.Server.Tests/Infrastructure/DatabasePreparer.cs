using System;
using FLS.Server.Data;
using FLS.Server.Service;
using NLog;

namespace FLS.Server.Tests.Infrastructure
{
    public class DatabasePreparer
    {
        private Logger _logger = LogManager.GetCurrentClassLogger();

        private static DatabasePreparer _instance;

        private DatabasePreparer()
        {
        }

        public static DatabasePreparer Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DatabasePreparer();
                }

                return _instance;
            }
        }

        public void PrepareDatabaseForTests()
        {
            return;

            _logger.Info("Preparing database...");
            var scriptName = "3 Insert Static Data.sql";

            try
            {
                using (var context = new FLSDataEntities(new IdentityService()))
                {
                    _logger.Info(string.Format("Executing SQL script: '{0}'", scriptName));
                    var sqlcommand = ResourceExtractor.Instance.ExtractSqlScript(scriptName);
                    context.Database.ExecuteSqlCommand(sqlcommand);
                    
                    scriptName = "4 or 5 Insert Test Data.sql";
                    _logger.Info(string.Format("Executing SQL script: '{0}'", scriptName));
                    sqlcommand = ResourceExtractor.Instance.ExtractSqlScript(scriptName);
                    context.Database.ExecuteSqlCommand(sqlcommand);

                    scriptName = "6 Insert Test Flights.sql";
                    _logger.Info(string.Format("Executing SQL script: '{0}'", scriptName));
                    sqlcommand = ResourceExtractor.Instance.ExtractSqlScript(scriptName);
                    context.Database.ExecuteSqlCommand(sqlcommand);

                    scriptName = "10 insert internationalisation values.sql";
                    _logger.Info(string.Format("Executing SQL script: '{0}'", scriptName));
                    sqlcommand = ResourceExtractor.Instance.ExtractSqlScript(scriptName);
                    context.Database.ExecuteSqlCommand(sqlcommand);

                    scriptName = "99 Insert SystemData.sql";
                    _logger.Info(string.Format("Executing SQL script: '{0}'", scriptName));
                    sqlcommand = ResourceExtractor.Instance.ExtractSqlScript(scriptName);
                    context.Database.ExecuteSqlCommand(sqlcommand);

                    scriptName = "90 Insert EmailTemplates.sql";
                    _logger.Info(string.Format("Executing SQL script: '{0}'", scriptName));
                    sqlcommand = ResourceExtractor.Instance.ExtractSqlScript(scriptName);
                    context.Database.ExecuteSqlCommand(sqlcommand);

                }

                _logger.Info("Database preparation finished.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error while executing SQL script: {scriptName}. Error-Message: {ex.Message}");
                _logger.Info("Database preparation finished, but with errors. Please check database and log files.");
                throw;
            }
        }
    }
}
