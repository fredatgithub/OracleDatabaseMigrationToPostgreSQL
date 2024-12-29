using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
namespace HelperLibrary
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
      using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_oracleConnectionString))
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
      using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_oracleConnectionString))
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

    public List<string> GetOracleFunctions()
    {
        var functions = new List<string>();
        using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_oracleConnectionString))
        {
            connection.Open();
            using (var command = new OracleCommand())
            {
                command.Connection = connection;
                command.CommandText = @"
                    SELECT object_name 
                    FROM user_objects 
                    WHERE object_type = 'FUNCTION'
                    ORDER BY object_name";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        functions.Add(reader.GetString(0));
                    }
                }
            }
        }
        return functions;
    }

    public List<string> GetPostgresFunctions()
    {
        var functions = new List<string>();
        using (var connection = new NpgsqlConnection(_postgresConnectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = @"
                    SELECT routines.routine_name
                    FROM information_schema.routines
                    WHERE routines.specific_schema NOT IN ('pg_catalog', 'information_schema')
                    AND type_udt_name != 'trigger'
                    ORDER BY routines.routine_name";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        functions.Add(reader.GetString(0));
                    }
                }
            }
        }
        return functions;
    }

    public void MigrateFunction(string functionName)
    {
        // Récupérer le code source de la fonction Oracle
        string oracleSource = GetOracleFunctionSource(functionName);
        
        // Convertir le code Oracle en PostgreSQL
        string pgSource = ConvertOracleToPgFunction(oracleSource);
        
        // Créer la fonction dans PostgreSQL
        using (var connection = new NpgsqlConnection(_postgresConnectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand(pgSource, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    private string GetOracleFunctionSource(string functionName)
    {
        using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_oracleConnectionString))
        {
            connection.Open();
            using (var command = new OracleCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT text FROM user_source WHERE name = :name AND type = 'FUNCTION' ORDER BY line";
                command.Parameters.Add(new OracleParameter("name", functionName.ToUpper()));

                var source = new System.Text.StringBuilder();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        source.Append(reader.GetString(0));
                    }
                }
                return source.ToString();
            }
        }
    }

    private string ConvertOracleToPgFunction(string oracleSource)
    {
        // Ici, vous devrez implémenter la logique de conversion
        // du code PL/SQL Oracle vers PL/pgSQL PostgreSQL
        // C'est une tâche complexe qui nécessite une analyse approfondie
        // et la gestion de nombreux cas particuliers
        
        // Pour l'instant, retournons simplement le code source avec quelques modifications basiques
        return oracleSource
            .Replace("BEGIN", "BEGIN")
            .Replace("END;", "END;")
            .Replace("VARCHAR2", "VARCHAR")
            .Replace("NUMBER", "NUMERIC");
    }

    public class User
    {
        public string Username { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public string DefaultTablespace { get; set; }
        public string TemporaryTablespace { get; set; }
        public string Profile { get; set; }
        public DateTime? AccountLockDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public List<User> GetOracleUsers()
    {
        var users = new List<User>();
        using (var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(_oracleConnectionString))
        {
            connection.Open();
            using (var command = new OracleCommand())
            {
                command.Connection = connection;
                command.CommandText = @"
                    SELECT username, 
                           account_status,
                           default_tablespace,
                           temporary_tablespace,
                           profile,
                           lock_date,
                           expiry_date
                    FROM dba_users 
                    WHERE username NOT IN ('SYS','SYSTEM','ANONYMOUS','APEX_PUBLIC_USER',
                        'AURORA$JIS$UTILITY$','AURORA$ORB$UNAUTHENTICATED','CTXSYS',
                        'DBSNMP','DIP','DMSYS','EXFSYS','LBACSYS','MDDATA','MDSYS',
                        'MGMT_VIEW','OLAPSYS','ORACLE_OCM','ORDDATA','ORDPLUGINS',
                        'ORDSYS','OUTLN','OWBSYS','SI_INFORMTN_SCHEMA','SPATIAL_CSW_ADMIN_USR',
                        'SPATIAL_WFS_ADMIN_USR','SYSMAN','TSMSYS','WK_TEST','WKPROXY',
                        'WKSYS','WMSYS','XDB','APEX_040000','APEX_040100','APEX_040200')
                    ORDER BY username";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var status = reader.GetString(1);
                        users.Add(new User
                        {
                            Username = reader.GetString(0),
                            Status = status,
                            StatusColor = status == "OPEN" ? "Green" : "Red",
                            DefaultTablespace = reader.GetString(2),
                            TemporaryTablespace = reader.GetString(3),
                            Profile = reader.GetString(4),
                            AccountLockDate = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                            ExpiryDate = reader.IsDBNull(6) ? null : reader.GetDateTime(6)
                        });
                    }
                }
            }
        }
        return users;
    }

    public List<User> GetPostgresUsers()
    {
        var users = new List<User>();
        using (var connection = new NpgsqlConnection(_postgresConnectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                command.CommandText = @"
                    SELECT rolname, 
                           CASE WHEN rolvaliduntil IS NULL THEN 'OPEN' ELSE 
                                CASE WHEN rolvaliduntil > CURRENT_TIMESTAMP THEN 'OPEN' 
                                ELSE 'EXPIRED' END 
                           END as status,
                           rolvaliduntil
                    FROM pg_roles 
                    WHERE rolname NOT IN ('postgres','pg_signal_backend','pg_read_all_settings',
                        'pg_read_all_stats','pg_stat_scan_tables','pg_monitor','pg_database_owner',
                        'pg_read_server_files','pg_write_server_files','pg_execute_server_program')
                    ORDER BY rolname";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var status = reader.GetString(1);
                        users.Add(new User
                        {
                            Username = reader.GetString(0),
                            Status = status,
                            StatusColor = status == "OPEN" ? "Green" : "Red",
                            ExpiryDate = reader.IsDBNull(2) ? null : reader.GetDateTime(2)
                        });
                    }
                }
            }
        }
        return users;
    }

    public void MigrateUser(User user)
    {
        using (var connection = new NpgsqlConnection(_postgresConnectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand())
            {
                command.Connection = connection;
                
                // Créer l'utilisateur
                command.CommandText = $"CREATE USER {user.Username} WITH PASSWORD 'ChangeMe123!'";
                command.ExecuteNonQuery();

                // Définir la date d'expiration si nécessaire
                if (user.ExpiryDate.HasValue)
                {
                    command.CommandText = $"ALTER USER {user.Username} VALID UNTIL '{user.ExpiryDate.Value:yyyy-MM-dd HH:mm:ss}'";
                    command.ExecuteNonQuery();
                }
            }
        }
    }
  }
}