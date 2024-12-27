using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Npgsql;
using System.Configuration;
using DatabaseMigration.Utils;
using System.IO;
using System.Text.Json;

namespace DatabaseMigration
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadConnectionStrings();
            LoadCredentials();
        }

        private void LoadConnectionStrings()
        {
            try
            {
                var oracleConnStr = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                var pgConnStr = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                // Parse Oracle connection string
                var oracleBuilder = new OracleConnectionStringBuilder(oracleConnStr);
                txtOracleHost.Text = oracleBuilder.DataSource;
                txtOracleUser.Text = oracleBuilder.UserID;
                txtOraclePassword.Text = oracleBuilder.Password;

                // Parse Postgres connection string
                var pgBuilder = new NpgsqlConnectionStringBuilder(pgConnStr);
                txtPgHost.Text = pgBuilder.Host;
                txtPgPort.Text = pgBuilder.Port.ToString();
                txtPgDatabase.Text = pgBuilder.Database;
                txtPgUser.Text = pgBuilder.Username;
                txtPgPassword.Text = pgBuilder.Password;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des paramètres: {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTestOracle_Click(object sender, EventArgs e)
        {
            try
            {
                var connStr = BuildOracleConnectionString();
                using (var conn = new OracleConnection(connStr))
                {
                    conn.Open();
                    MessageBox.Show("Connexion Oracle réussie!", "Succès", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de connexion Oracle: {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnTestPg_Click(object sender, EventArgs e)
        {
            try
            {
                var connStr = BuildPostgresConnectionString();
                using (var conn = new NpgsqlConnection(connStr))
                {
                    conn.Open();
                    MessageBox.Show("Connexion PostgreSQL réussie!", "Succès", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de connexion PostgreSQL: {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveConnectionStrings();
                SaveCredentials();
                MessageBox.Show("Paramètres sauvegardés avec succès!", "Succès", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde: {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string BuildOracleConnectionString()
        {
            return $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={txtOracleHost.Text})(PORT={txtOraclePort.Text}))" +
                   $"(CONNECT_DATA=(SERVICE_NAME={txtOracleService.Text})));User Id={txtOracleUser.Text};Password={txtOraclePassword.Text}";
        }

        private string BuildPostgresConnectionString()
        {
            return $"Host={txtPgHost.Text};Port={txtPgPort.Text};Database={txtPgDatabase.Text};" +
                   $"Username={txtPgUser.Text};Password={txtPgPassword.Text}";
        }

        private void SaveConnectionStrings()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.ConnectionStrings.ConnectionStrings["OracleConnection"].ConnectionString = BuildOracleConnectionString();
            config.ConnectionStrings.ConnectionStrings["PostgresConnection"].ConnectionString = BuildPostgresConnectionString();
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        private void SaveCredentials()
        {
            var credentials = new
            {
                Oracle = new
                {
                    Host = txtOracleHost.Text,
                    Port = txtOraclePort.Text,
                    Service = txtOracleService.Text,
                    User = txtOracleUser.Text,
                    Password = txtOraclePassword.Text
                },
                Postgres = new
                {
                    Host = txtPgHost.Text,
                    Port = txtPgPort.Text,
                    Database = txtPgDatabase.Text,
                    User = txtPgUser.Text,
                    Password = txtPgPassword.Text
                }
            };

            string json = JsonSerializer.Serialize(credentials);
            string encrypted = SecurityHelper.Encrypt(json);
            string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            string credentialsPath = Path.Combine(appPath, "id.txt");
            File.WriteAllText(credentialsPath, encrypted);
        }

        private void LoadCredentials()
        {
            try
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                string credentialsPath = Path.Combine(appPath, "id.txt");

                if (File.Exists(credentialsPath))
                {
                    string encrypted = File.ReadAllText(credentialsPath);
                    string json = SecurityHelper.Decrypt(encrypted);
                    var credentials = JsonSerializer.Deserialize<dynamic>(json);

                    // Oracle
                    txtOracleHost.Text = credentials.Oracle.Host.GetString();
                    txtOraclePort.Text = credentials.Oracle.Port.GetString();
                    txtOracleService.Text = credentials.Oracle.Service.GetString();
                    txtOracleUser.Text = credentials.Oracle.User.GetString();
                    txtOraclePassword.Text = credentials.Oracle.Password.GetString();

                    // PostgreSQL
                    txtPgHost.Text = credentials.Postgres.Host.GetString();
                    txtPgPort.Text = credentials.Postgres.Port.GetString();
                    txtPgDatabase.Text = credentials.Postgres.Database.GetString();
                    txtPgUser.Text = credentials.Postgres.User.GetString();
                    txtPgPassword.Text = credentials.Postgres.Password.GetString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des identifiants: {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}