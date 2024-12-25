using System;
using System.Configuration;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Npgsql;

namespace DatabaseMigration.Forms
{
    public partial class ConnectionTestForm : Form
    {
        private TextBox txtOracleHost;
        private TextBox txtOraclePort;
        private TextBox txtOracleService;
        private TextBox txtOracleUser;
        private TextBox txtOraclePassword;
        
        private TextBox txtPgHost;
        private TextBox txtPgPort;
        private TextBox txtPgDatabase;
        private TextBox txtPgUser;
        private TextBox txtPgPassword;

        public ConnectionTestForm()
        {
            //InitializeComponent();
            InitializeControls();
            LoadConnectionStrings();
        }

        private void InitializeControls()
        {
            this.Text = "Test des Connexions";
            this.Size = new System.Drawing.Size(600, 400);

            // Oracle Controls
            var grpOracle = new GroupBox
            {
                Text = "Connexion Oracle",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(280, 300)
            };

            txtOracleHost = CreateTextBoxWithLabel("Hôte:", 20, 30, grpOracle);
            txtOraclePort = CreateTextBoxWithLabel("Port:", 20, 80, grpOracle);
            txtOracleService = CreateTextBoxWithLabel("Service:", 20, 130, grpOracle);
            txtOracleUser = CreateTextBoxWithLabel("Utilisateur:", 20, 180, grpOracle);
            txtOraclePassword = CreateTextBoxWithLabel("Mot de passe:", 20, 230, grpOracle);
            txtOraclePassword.PasswordChar = '*';

            var btnTestOracle = new Button
            {
                Text = "Tester Oracle",
                Location = new System.Drawing.Point(90, 260),
                Size = new System.Drawing.Size(100, 30)
            };
            btnTestOracle.Click += BtnTestOracle_Click;
            grpOracle.Controls.Add(btnTestOracle);

            // PostgreSQL Controls
            var grpPostgres = new GroupBox
            {
                Text = "Connexion PostgreSQL",
                Location = new System.Drawing.Point(300, 10),
                Size = new System.Drawing.Size(280, 300)
            };

            txtPgHost = CreateTextBoxWithLabel("Hôte:", 20, 30, grpPostgres);
            txtPgPort = CreateTextBoxWithLabel("Port:", 20, 80, grpPostgres);
            txtPgDatabase = CreateTextBoxWithLabel("Base:", 20, 130, grpPostgres);
            txtPgUser = CreateTextBoxWithLabel("Utilisateur:", 20, 180, grpPostgres);
            txtPgPassword = CreateTextBoxWithLabel("Mot de passe:", 20, 230, grpPostgres);
            txtPgPassword.PasswordChar = '*';

            var btnTestPg = new Button
            {
                Text = "Tester PostgreSQL",
                Location = new System.Drawing.Point(90, 260),
                Size = new System.Drawing.Size(100, 30)
            };
            btnTestPg.Click += BtnTestPg_Click;
            grpPostgres.Controls.Add(btnTestPg);

            // Save Button
            var btnSave = new Button
            {
                Text = "Sauvegarder",
                Location = new System.Drawing.Point(250, 320),
                Size = new System.Drawing.Size(100, 30)
            };
            btnSave.Click += BtnSave_Click;

            this.Controls.AddRange(new Control[] { grpOracle, grpPostgres, btnSave });
        }

        private TextBox CreateTextBoxWithLabel(string labelText, int x, int y, GroupBox parent)
        {
            var label = new Label
            {
                Text = labelText,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(80, 20)
            };

            var textBox = new TextBox
            {
                Location = new System.Drawing.Point(x + 90, y),
                Size = new System.Drawing.Size(150, 20)
            };

            parent.Controls.AddRange(new Control[] { label, textBox });
            return textBox;
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
                MessageBox.Show($"Erreur lors du chargement des paramètres: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Connexion Oracle réussie!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de connexion Oracle: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Connexion PostgreSQL réussie!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de connexion PostgreSQL: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
                config.ConnectionStrings.ConnectionStrings["OracleConnection"].ConnectionString = BuildOracleConnectionString();
                config.ConnectionStrings.ConnectionStrings["PostgresConnection"].ConnectionString = BuildPostgresConnectionString();
                
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");

                MessageBox.Show("Paramètres sauvegardés avec succès!", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    private void InitializeComponent()
    {
      this.SuspendLayout();
      // 
      // ConnectionTestForm
      // 
      this.ClientSize = new System.Drawing.Size(282, 253);
      this.Name = "ConnectionTestForm";
      this.Load += new System.EventHandler(this.ConnectionTestForm_Load);
      this.ResumeLayout(false);

    }

    private void ConnectionTestForm_Load(object sender, EventArgs e)
    {

    }
  }
} 