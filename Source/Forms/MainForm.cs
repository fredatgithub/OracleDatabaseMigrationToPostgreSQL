using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Npgsql;
using System.Configuration;
using DatabaseMigration.Utils;
using System.IO;
using System.Text.Json;
using System.Drawing;

namespace DatabaseMigration
{
    public partial class MainForm : Form
    {
        private const string WINDOW_SETTINGS_FILE = "window_settings.json";
        private bool isOracleConnectionValid = false;
        private bool isPgConnectionValid = false;

        public MainForm()
        {
            InitializeComponent();
            LoadConnectionStrings();
            LoadCredentials();
            LoadWindowSettings();
            ValidateConnections();
            UpdateUIState();

            // Ajouter un gestionnaire pour l'événement FormClosing
            this.FormClosing += MainForm_FormClosing;
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

        private void ValidateConnections()
        {
            try
            {
                // Test connexion Oracle
                using (var conn = new OracleConnection(BuildOracleConnectionString()))
                {
                    conn.Open();
                    isOracleConnectionValid = true;
                }
            }
            catch
            {
                isOracleConnectionValid = false;
            }

            try
            {
                // Test connexion PostgreSQL
                using (var conn = new NpgsqlConnection(BuildPostgresConnectionString()))
                {
                    conn.Open();
                    isPgConnectionValid = true;
                }
            }
            catch
            {
                isPgConnectionValid = false;
            }
        }

        private void UpdateUIState()
        {
            bool areConnectionsValid = isOracleConnectionValid && isPgConnectionValid;

            // Colorer l'onglet Connexion
            tabConnexion.BackColor = areConnectionsValid ? Color.LightGreen : Color.LightPink;

            // Activer/désactiver l'onglet Tables
            tabTables.Enabled = areConnectionsValid;
            
            // Activer/désactiver les contrôles dans l'onglet Tables
            grpOracleTables.Enabled = areConnectionsValid;
            grpPgTables.Enabled = areConnectionsValid;
            btnLoadTables.Enabled = areConnectionsValid;
            btnMigrate.Enabled = areConnectionsValid;
            progressMigration.Enabled = areConnectionsValid;

            // Mettre à jour le texte de l'onglet Connexion pour indiquer le statut
            tabConnexion.Text = areConnectionsValid ? 
                "Connexion OK" : 
                "Connexion KO";
        }

        private void BtnTestOracle_Click(object sender, EventArgs e)
        {
            try
            {
                var connStr = BuildOracleConnectionString();
                using (var conn = new OracleConnection(connStr))
                {
                    conn.Open();
                    isOracleConnectionValid = true;
                    MessageBox.Show("Connexion Oracle réussie!", "Succès", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                isOracleConnectionValid = false;
                MessageBox.Show($"Erreur de connexion Oracle: {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateUIState();
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
                    isPgConnectionValid = true;
                    MessageBox.Show("Connexion PostgreSQL réussie!", "Succès", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                isPgConnectionValid = false;
                MessageBox.Show($"Erreur de connexion PostgreSQL: {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UpdateUIState();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveConnectionStrings();
                SaveCredentials();
                ValidateConnections();
                UpdateUIState();
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

        private void BtnLoadTables_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                lstOracleTables.Items.Clear();
                lstPgTables.Items.Clear();

                var dbManager = new DatabaseManager(
                    ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString,
                    ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString);

                // Charger les tables Oracle
                var oracleTables = dbManager.GetOracleTables();
                lstOracleTables.Items.AddRange(oracleTables.ToArray());

                // Charger les tables PostgreSQL
                var pgTables = dbManager.GetPostgresTables();
                lstPgTables.Items.AddRange(pgTables.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des tables : {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void BtnMigrate_Click(object sender, EventArgs e)
        {
            if (lstOracleTables.SelectedItems.Count == 0)
            {
                MessageBox.Show("Veuillez sélectionner au moins une table à migrer.", 
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                progressMigration.Minimum = 0;
                progressMigration.Maximum = lstOracleTables.SelectedItems.Count;
                progressMigration.Value = 0;

                var dbManager = new DatabaseManager(
                    ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString,
                    ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString);

                foreach (var table in lstOracleTables.SelectedItems)
                {
                    var tableName = table.ToString();
                    var schema = dbManager.ReadOracleData($"SELECT * FROM {tableName} WHERE 1=0");
                    dbManager.CreatePostgresTable(tableName, schema);
                    progressMigration.Value++;
                }

                MessageBox.Show("Migration terminée avec succès!", "Succès", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la migration : {ex.Message}", 
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void LoadWindowSettings()
        {
            try
            {
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                string settingsPath = Path.Combine(appPath, WINDOW_SETTINGS_FILE);

                if (File.Exists(settingsPath))
                {
                    string json = File.ReadAllText(settingsPath);
                    var settings = JsonSerializer.Deserialize<WindowSettings>(json);

                    // Restaurer la position et la taille
                    this.Location = new Point(settings.X, settings.Y);
                    this.Size = new Size(settings.Width, settings.Height);

                    // Vérifier que la fenêtre est visible sur un écran
                    bool isVisible = false;
                    foreach (Screen screen in Screen.AllScreens)
                    {
                        if (screen.WorkingArea.IntersectsWith(this.Bounds))
                        {
                            isVisible = true;
                            break;
                        }
                    }

                    // Si la fenêtre n'est pas visible, la replacer au centre de l'écran principal
                    if (!isVisible)
                    {
                        this.StartPosition = FormStartPosition.CenterScreen;
                    }
                }
            }
            catch
            {
                // En cas d'erreur, utiliser la position par défaut
                this.StartPosition = FormStartPosition.CenterScreen;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveWindowSettings();
        }

        private void SaveWindowSettings()
        {
            try
            {
                var settings = new WindowSettings
                {
                    X = this.Location.X,
                    Y = this.Location.Y,
                    Width = this.Size.Width,
                    Height = this.Size.Height
                };

                string json = JsonSerializer.Serialize(settings);
                string appPath = Path.GetDirectoryName(Application.ExecutablePath);
                string settingsPath = Path.Combine(appPath, WINDOW_SETTINGS_FILE);
                File.WriteAllText(settingsPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde des paramètres de fenêtre : {ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Classe pour stocker les paramètres de la fenêtre
        private class WindowSettings
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}