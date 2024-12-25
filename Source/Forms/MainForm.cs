using System;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using Npgsql;
using System.Configuration;
using DatabaseMigration.Forms;

namespace DatabaseMigration
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                using (var oracleConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString))
                {
                    oracleConnection.Open();
                    MessageBox.Show("Connexion à Oracle réussie!");
                }

                using (var pgConnection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString))
                {
                    pgConnection.Open();
                    MessageBox.Show("Connexion à PostgreSQL réussie!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de connexion: {ex.Message}");
            }
        }

        private void InitializeComponent()
        {
            Button btnTestConnections = new Button
            {
                Text = "Tester les connexions",
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(150, 30)
            };
            btnTestConnections.Click += BtnTestConnections_Click;
            
            this.Controls.Add(btnTestConnections);
        }

        private void BtnTestConnections_Click(object sender, EventArgs e)
        {
            var testForm = new ConnectionTestForm();
            testForm.ShowDialog();
        }
    }
} 