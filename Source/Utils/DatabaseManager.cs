using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Npgsql;

namespace DatabaseMigration.Utils
{
    public class DatabaseManager
    {
        private readonly string _oracleConnectionString;
        private readonly string _postgresConnectionString;

        public DatabaseManager(string oracleConnectionString, string postgresConnectionString)
        {
            _oracleConnectionString = oracleConnectionString;
            _postgresConnectionString = postgresConnectionString;
        }

        public DataTable ReadOracleData(string query)
        {
            using (var connection = new OracleConnection(_oracleConnectionString))
            {
                connection.Open();
                using (var command = new OracleCommand(query, connection))
                {
                    var adapter = new OracleDataAdapter(command);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        public void CreatePostgresTable(string tableName, DataTable schema)
        {
            using (var connection = new NpgsqlConnection(_postgresConnectionString))
            {
                connection.Open();
                string createTableQuery = GenerateCreateTableQuery(tableName, schema);
                using (var command = new NpgsqlCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private string GenerateCreateTableQuery(string tableName, DataTable schema)
        {
            // Logique pour générer la requête CREATE TABLE
            // À implémenter selon vos besoins
            return "";
        }
    }
} 