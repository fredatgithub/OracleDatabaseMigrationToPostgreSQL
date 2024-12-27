using System;
using System.Data;
using System.Collections.Generic;
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

        public List<string> GetOracleTables()
        {
            var tables = new List<string>();
            using (var connection = new OracleConnection(_oracleConnectionString))
            {
                connection.Open();
                using (var command = new OracleCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        SELECT table_name 
                        FROM user_tables 
                        ORDER BY table_name";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return tables;
        }

        public List<string> GetPostgresTables()
        {
            var tables = new List<string>();
            using (var connection = new NpgsqlConnection(_postgresConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        SELECT tablename 
                        FROM pg_catalog.pg_tables 
                        WHERE schemaname != 'pg_catalog' 
                        AND schemaname != 'information_schema'
                        ORDER BY tablename";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return tables;
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
            var columnDefinitions = new List<string>();
            foreach (DataColumn column in schema.Columns)
            {
                string postgresType = MapOracleToPostgresType(column);
                columnDefinitions.Add($"{column.ColumnName} {postgresType}");
            }

            return $"CREATE TABLE IF NOT EXISTS {tableName} ({string.Join(", ", columnDefinitions)})";
        }

        private string MapOracleToPostgresType(DataColumn column)
        {
            switch (column.DataType.Name.ToLower())
            {
                case "string":
                    return column.MaxLength == -1 ? "text" : $"varchar({column.MaxLength})";
                case "int32":
                    return "integer";
                case "int64":
                    return "bigint";
                case "decimal":
                    return "numeric";
                case "datetime":
                    return "timestamp";
                case "boolean":
                    return "boolean";
                case "byte[]":
                    return "bytea";
                default:
                    return "text";
            }
        }
    }
} 