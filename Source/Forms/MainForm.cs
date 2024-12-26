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

    private void BtnConnect_Click(object sender, EventArgs e)
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
      catch (Exception exception)
      {
        MessageBox.Show($"Erreur de connexion: {exception.Message}");
      }
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      // 
      // MainForm
      // 
      this.ClientSize = new System.Drawing.Size(282, 253);
      this.Name = "MainForm";
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.ResumeLayout(false);

    }

    private void BtnTestConnections_Click(object sender, EventArgs e)
    {
      var testForm = new ConnectionTestForm();
      testForm.ShowDialog();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {

    }
  }
}