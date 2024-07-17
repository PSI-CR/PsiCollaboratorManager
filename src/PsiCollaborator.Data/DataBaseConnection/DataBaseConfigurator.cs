using System;
using System.Configuration;
using System.Data.Common;

namespace PsiCollaborator.Data.DataBaseConnection
{
    internal class DataBaseConfigurator
    {
        public static DbConnection CreateDbConnection()
        {
            var settings = GetConnectionStringByProvider("Npgsql");
            var factory = DbProviderFactories.GetFactory(settings.ProviderName);
            var dbConnection = factory.CreateConnection();
            dbConnection.ConnectionString = settings.ConnectionString;
            return dbConnection;
        }

        public static ConnectionStringSettings GetConnectionStringByName(string connectionStringName)
        {
            if (string.IsNullOrEmpty(connectionStringName))
                throw new ArgumentException("connectionStringName is null or empty.", "connectionStringName");
            var settings = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
                throw new ConfigurationErrorsException(string.Format("Could not find connection string with name: {0}.", connectionStringName));
            return settings;
        }

        public static ConnectionStringSettings GetConnectionStringByProvider(string providerName)
        {
            if (string.IsNullOrEmpty(providerName))
                throw new ArgumentException("providerName is null or empty.", "providerName");
            ConnectionStringSettings returnSetting = null;
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.ProviderName == providerName)
                    {
                        returnSetting = cs;
                        break;
                    }
                }
            }
            if (returnSetting == null || string.IsNullOrEmpty(returnSetting.ConnectionString))
                throw new ConfigurationErrorsException(string.Format("Could not find connection string with provider name: {0}.", providerName));
            return returnSetting;
        }
    }
}
