using SqlQueryStress.DbProviders.MSSQL;
using SqlQueryStressEngine.Parameters;
using SqlQueryStressGUI.DbProviders;
using System.Collections.Generic;

namespace SqlQueryStressGUI.Parameters
{
    public class MssqlQueryParameterSettings : ParameterSettingsViewModel
    {
        public MssqlQueryParameterSettings(IConnectionProvider connectionProvider)
        {
            DatabaseConnections = connectionProvider.GetConnections();
        }

        public IEnumerable<DatabaseConnection> DatabaseConnections { get; }

        private DatabaseConnection _dbConnection;
        public DatabaseConnection DatabaseConnection
        {
            get => _dbConnection;
            set => SetProperty(value, ref _dbConnection);
        }

        private string _parameterQuery;
        public string ParameterQuery
        {
            get => _parameterQuery;
            set => SetProperty(value, ref _parameterQuery);
        }

        public override IParameterValueBuilder GetParameterValueBuilder()
        {
            return new MssqlParameterValueBuilder(Name, DatabaseConnection.ConnectionString, ParameterQuery);
        }
    }
}
